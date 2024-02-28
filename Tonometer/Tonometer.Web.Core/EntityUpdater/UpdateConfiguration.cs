#region

using System.Linq.Expressions;
using System.Reflection;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Core.EntityUpdater;

public sealed class UpdateConfiguration<T>
{
    private readonly Dictionary<string, UpdateProperty> _properties = new();

    public UpdateConfiguration<T> AddProperty<TProperty>(Expression<Func<T, TProperty>> accessor,
                                                         Predicate<TProperty> validator,
                                                         UserType minimumAuthority)
    {
        var propertyInfo = (PropertyInfo)((MemberExpression)accessor.Body).Member;
        var property = propertyInfo.Name;
        var name = char.ToLower(property[0]) + property[1..];

        _properties.Add(name,
                        new UpdateProperty(x => validator((TProperty)x), typeof(TProperty), propertyInfo,
                                           minimumAuthority));

        return this;
    }

    public UpdateProperty? GetProperty(string name)
    {
        return _properties.GetValueOrDefault(name);
    }

    public sealed record UpdateProperty(
        Predicate<object> Validator,
        Type Type,
        PropertyInfo Property,
        UserType MinimumAuthority);
}
