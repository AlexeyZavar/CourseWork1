#region

using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Core.EntityUpdater;

public static class UpdateConfigurationsStorage
{
    static UpdateConfigurationsStorage()
    {
        UserConfiguration = new UpdateConfiguration<User>()
                            .AddProperty(x => x.Email, x => x is null || x.Length < 100, UserType.Watcher)
                            .AddProperty(x => x.Telegram, x => x is null || x.Length <= 64, UserType.Watcher)
                            .AddProperty(x => x.UserName, x => x.Length is >= 3 and <= 32, UserType.Admin)
                            .AddProperty(x => x.FirstName, x => x.Length is >= 3 and <= 32, UserType.Admin)
                            .AddProperty(x => x.LastName, x => x.Length is >= 3 and <= 32, UserType.Admin);
    }

    public static UpdateConfiguration<User> UserConfiguration { get; }
}
