#region

using Reinforced.Typings;
using Reinforced.Typings.Fluent;
using Tonometer.Web.Data.Models.Dtos;

#endregion

namespace Tonometer.Web.Data.Typings;

public static class TypingsConfiguration
{
    public static void Configure(ConfigurationBuilder builder)
    {
        var types = typeof(UserDto)
                    .Assembly
                    .GetExportedTypes()
                    .Where(x => x.Namespace?.Contains("Tonometer.Web.Data.Models") == true);

        builder.ExportAsInterfaces(types,
                                   exportBuilder =>
                                   {
                                       exportBuilder.FlattenHierarchy();

                                       exportBuilder.WithPublicProperties(propertyExportBuilder =>
                                       {
                                           propertyExportBuilder.CamelCase();

                                           if (propertyExportBuilder.Member.PropertyType == typeof(Guid) ||
                                               propertyExportBuilder.Member.PropertyType == typeof(Guid?))
                                           {
                                               propertyExportBuilder.Type<string>();
                                           }

                                           if (propertyExportBuilder.Member.PropertyType == typeof(DateTimeOffset) ||
                                               propertyExportBuilder.Member.PropertyType == typeof(DateTimeOffset?))
                                           {
                                               propertyExportBuilder.Type<string>();
                                           }

                                           if (propertyExportBuilder.Member.PropertyType.IsNullable())
                                           {
                                               propertyExportBuilder.ForceNullable();
                                           }
                                       });

                                       exportBuilder.AutoI(false);
                                       exportBuilder.DontIncludeToNamespace();
                                       exportBuilder.ExportTo("dtos.d.ts");
                                   });
        builder.Context.Global.TabSymbol = "    ";
    }
}
