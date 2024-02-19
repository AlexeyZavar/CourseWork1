#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Filters;

public sealed class EnsureAccessToPatientAttribute : ActionFilterAttribute
{
    /// <inheritdoc />
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.GetUserId();
        if (userId == -1)
        {
            // will return 401 anyway
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        var patientIdExtracted = context.ActionArguments.TryGetValue("patientId", out var patientIdParsed);

        if (!patientIdExtracted)
        {
            // will return 400 anyway
            // or maybe placed on wrong method
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        var patientId = patientIdParsed is int value ? value : 0;
        if (patientId == 0)
        {
            // shouldn't happen but still
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        var dbContext = context.HttpContext.RequestServices.GetRequiredService<TonometerContext>();
        var hasAccess = await dbContext.Patients.Where(x => x.Id == patientId)
                                       .Select(x => x.Watchers.Any(y => y.Id == userId))
                                       .FirstOrDefaultAsync();

        if (hasAccess)
        {
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        hasAccess = await dbContext.Users.Where(x => x.Id == userId)
                                   .Select(x => x.Type == UserType.Admin)
                                   .FirstOrDefaultAsync();

        if (hasAccess)
        {
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        context.Result = new ForbidResult();
    }
}
