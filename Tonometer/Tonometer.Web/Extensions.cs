#region

using System.Security.Claims;
using System.Text;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Tonometer.Database;
using Tonometer.Database.Entities;
using Tonometer.Web.Data.Models.Dtos;

#endregion

namespace Tonometer.Web;

public static class Extensions
{
    public static int GetUserId(this ControllerBase controller)
    {
        return GetUserId(controller.HttpContext);
    }

    public static int GetUserId(this HttpContext context)
    {
        return Convert.ToInt32(context.User.FindFirstValue(ClaimTypes.Sid) ?? "-1");
    }

    public static void AddMapster(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<User, UserDto>();
        config.NewConfig<Patient, PatientDto>()
              .Map(x => x.Age, x => (DateTime.UtcNow - x.BirthDay).TotalDays / 365);
        config.NewConfig<PatientWarning, PatientWarningDto>();
        config.NewConfig<MeasureDevice, MeasureDeviceDto>();
        config.NewConfig<MeasureData, MeasureDataDto>();

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    public static void AddTonometerAuthentication(this IServiceCollection services,
                                                  IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Auth:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Auth:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:Key"]!)),
                        ValidateIssuerSigningKey = true
                    };
                });
    }

    public static void SeedDatabase(this IServiceCollection services, IHostEnvironment environment)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<TonometerContext>();

        if (!environment.IsDevelopment())
        {
            return;
        }

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var user = new User
        {
            UserName = "alexeyzavar",
            FirstName = "Alexey",
            LastName = "Zavar",
            PasswordHash = CryptoUtilities.HashPassword("1122"),
            Email = "alexeyzavar@gmail.com",
            Type = UserType.Admin
        };
        var device = new MeasureDevice
        {
            Manufacturer = "Zavarenko Inc.",
            Model = "Tea 3000",
            Serial = "DEBUGOS424242"
        };

        var patient = new Patient
        {
            FirstName = "Alexi",
            LastName = "Zavarenko",
            Gender = Gender.Male,
            BirthDay = new DateTime(2005, 12, 5),
            Weight = 65.5m
        };

        patient.Watchers.Add(user);
        patient.Devices.Add(device);

        context.Users.Add(user);
        context.MeasureDevices.Add(device);
        context.Patients.Add(patient);

        context.SaveChanges();
    }
}
