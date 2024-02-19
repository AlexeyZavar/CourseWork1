#region

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tonometer.Core.DataRetrievers;
using Tonometer.Core.DataRetrievers.Abstractions;
using Tonometer.Core.Models;
using Tonometer.Core.NotificationServices;
using Tonometer.Core.NotificationServices.Abstractions;
using Tonometer.Database;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Core;

public sealed class MeasurementManager : IHostedService
{
    public IServiceScopeFactory ScopeFactory { get; }

    private readonly List<IDataRetriever> _retrievers;
    private readonly List<INotificationService> _notificationServices;

    public MeasurementManager(IServiceScopeFactory scopeFactory)
    {
        ScopeFactory = scopeFactory;

        _retrievers = new List<IDataRetriever>();
        _notificationServices = new List<INotificationService>();
    }

    public void OnDataReceived(int deviceId, int patientId, RetrievedMeasureData data)
    {
        var scope = ScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TonometerContext>();

        var measureDevice = context.MeasureDevices.Find(deviceId);
        if (measureDevice == null)
        {
            throw new ArgumentException("Device not found", nameof(deviceId));
        }

        var patient = context.Patients
                             .Include(patient => patient.Devices)
                             .FirstOrDefault(x => x.Id == patientId);
        if (patient == null)
        {
            throw new ArgumentException("Patient not found", nameof(patientId));
        }

        if (!patient.Devices.Contains(measureDevice))
        {
            throw new ArgumentException("Device is not assigned to patient", nameof(deviceId));
        }

        var measureData = new MeasureData
        {
            Time = data.Time,
            Sys = data.Sys,
            Dia = data.Dia,
            Pulse = data.Pulse,
            MeasureDevice = measureDevice,
            Patient = patient
        };

        context.MeasureData.Add(measureData);

        var warningText = GenerateWarningMessage(measureData, patient);
        if (!string.IsNullOrWhiteSpace(warningText))
        {
            var warning = new PatientWarning
            {
                Patient = patient,
                MeasureData = measureData,
                Message = warningText,
                Time = DateTimeOffset.UtcNow
            };
            context.Warnings.Add(warning);

            foreach (var notificationService in _notificationServices)
            {
                notificationService.SendWarning(warning);
            }
        }

        context.SaveChanges();
    }

    private string? GenerateWarningMessage(MeasureData data, Patient patient)
    {
        var delta = DateTime.UtcNow - patient.BirthDay;
        var age = (int)delta.TotalDays / 365;

        // https://med-prof.ru/o-tsentre/novosti/puls-zhizni-znacheniya-serdechnogo-ritma-pri-fiznagruzkakh/
        Func<decimal> limit = patient.Gender == Gender.Male ? () => 214 - 0.8m * age : () => 209 - 0.9m * age;

        var res = limit();

        if (data.Pulse > res)
        {
            return $"Слишком высокий пульс! ({data.Pulse:F1} > {res:F1})";
        }

        return null;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = ScopeFactory.CreateScope();

        // ------
        var debugRetriever = new DebugRetriever();
        debugRetriever.Start(this);
        _retrievers.Add(debugRetriever);

        // ------
        var debugLogger = scope.ServiceProvider.GetRequiredService<ILogger<DebugNotificationService>>();
        var debugNotificationService = new DebugNotificationService(debugLogger);
        _notificationServices.Add(debugNotificationService);

        var mailNotificationService = new MailNotificationService();
        _notificationServices.Add(mailNotificationService);

        var telegramNotificationsService = new TelegramNotificationService();
        _notificationServices.Add(telegramNotificationsService);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var retriever in _retrievers)
        {
            retriever.Stop();
        }

        return Task.CompletedTask;
    }
}
