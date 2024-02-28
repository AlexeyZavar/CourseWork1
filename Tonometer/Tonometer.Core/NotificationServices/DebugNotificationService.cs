#region

using Microsoft.Extensions.Logging;
using Tonometer.Core.NotificationServices.Abstractions;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Core.NotificationServices;

public sealed class DebugNotificationService : INotificationService
{
    private readonly ILogger<DebugNotificationService> _logger;

    public DebugNotificationService(ILogger<DebugNotificationService> logger)
    {
        _logger = logger;
    }

    public void SendWarning(PatientWarning warning)
    {
        _logger.LogWarning("Warning: {Message}", warning.Message);
    }
}
