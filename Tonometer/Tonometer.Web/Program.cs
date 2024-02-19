#region

using Microsoft.EntityFrameworkCore;
using Tonometer.Core;
using Tonometer.Database;
using Tonometer.Web;
using Tonometer.Web.Services;

#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TonometerContext>(x =>
                                                    x.UseSqlite("Data Source=tonometer.db")
                                               );

builder.Services.SeedDatabase(builder.Environment);

builder.Services
       .AddSingleton<TokenService>();

builder.Services
       .AddMapster();

builder.Services
       .AddTonometerAuthentication(builder.Configuration);

builder.Services
       .AddAuthorization();

builder.Services.AddHostedService<MeasurementManager>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    // todo: bind to domain
    options.AddDefaultPolicy(x =>
                                 x.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin()
                            );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
