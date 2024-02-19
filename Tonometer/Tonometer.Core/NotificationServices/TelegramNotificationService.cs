using Tonometer.Core.NotificationServices.Abstractions;
using Tonometer.Database.Entities;

namespace Tonometer.Core.NotificationServices;

public class TelegramNotificationService : INotificationService
{
    /// <inheritdoc />
    public void SendWarning(PatientWarning warning)
    {
        // nah
    }
}
