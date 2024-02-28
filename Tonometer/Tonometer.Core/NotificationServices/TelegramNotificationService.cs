#region

using Tonometer.Core.NotificationServices.Abstractions;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Core.NotificationServices;

public sealed class TelegramNotificationService : INotificationService
{
    /// <inheritdoc />
    public void SendWarning(PatientWarning warning)
    {
        // nah
    }
}
