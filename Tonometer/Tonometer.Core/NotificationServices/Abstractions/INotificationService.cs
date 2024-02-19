#region

using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Core.NotificationServices.Abstractions;

public interface INotificationService
{
    public void SendWarning(PatientWarning warning);
}
