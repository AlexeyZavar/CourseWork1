#region

using System.Text.Json;
using Tonometer.Database;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Core.EntityUpdater;

public sealed record UpdateIn<T>(
    JsonDocument Doc,
    UserType UserType,
    TonometerContext Context,
    T Entity,
    UpdateConfiguration<T> Configuration);

public static class UpdateProcessor
{
    public static async Task<UpdateResult> Process<T>(UpdateIn<T> updateIn)
    {
        var processed = new List<string>();
        var failed = new List<FailedUpdate>();

        foreach (var property in updateIn.Doc.RootElement.EnumerateObject().Take(100)) // limit to prevent hacking
        {
            var name = property.Name;
            var updater = updateIn.Configuration.GetProperty(name);

            if (updater is null)
            {
                failed.Add(new FailedUpdate(name, "Поле не найдено, или его нельзя редактировать"));
                continue;
            }

            if (processed.Contains(name))
            {
                failed.Add(new FailedUpdate(name, "Поле уже обработано"));
                continue;
            }

            if (updater.MinimumAuthority > updateIn.UserType)
            {
                failed.Add(new FailedUpdate(name, "Недостаточно прав"));
                continue;
            }

            var jsonValue = property.Value;
            object? value = null;

            try
            {
                value = jsonValue.Deserialize(updater.Type);
            }
            catch
            {
                // ignored
            }

            if (value is null && jsonValue.ValueKind != JsonValueKind.Null &&
                (jsonValue.ValueKind != JsonValueKind.String || !string.IsNullOrWhiteSpace(jsonValue.GetString())))
            {
                failed.Add(new FailedUpdate(name, "Неверный тип"));
                continue;
            }

            if (jsonValue.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(jsonValue.GetString()))
            {
                value = "";
            }

            if (!updater.Validator(value!))
            {
                failed.Add(new FailedUpdate(name, "Неверное значение"));
                continue;
            }

            try
            {
                updater.Property.SetValue(updateIn.Entity, value);
            }
            catch
            {
                failed.Add(new FailedUpdate(name, "Ошибка при установке значения"));
                continue;
            }

            processed.Add(name);
        }

        if (processed.Count > 0)
        {
            await updateIn.Context.SaveChangesAsync();
        }

        var res = new UpdateResult(processed, failed);

        return res;
    }

    public sealed record FailedUpdate(string Property, string Reason);

    public sealed record UpdateResult(List<string> Processed, List<FailedUpdate> Failed);
}
