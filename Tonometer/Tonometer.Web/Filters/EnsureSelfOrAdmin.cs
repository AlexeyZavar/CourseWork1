#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Filters;

public sealed class EnsureSelfOrAdminAttribute : ActionFilterAttribute
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

        var userIdExtracted = context.ActionArguments.TryGetValue("userId", out var userIdParsed);

        if (!userIdExtracted)
        {
            // will return 400 anyway
            // or maybe placed on wrong method
            await base.OnActionExecutionAsync(context, next);
            return;
        }

        if ((int)(userIdParsed ?? -1) == userId)
        {
            context.ActionArguments["userType"] = UserType.Watcher;

            await base.OnActionExecutionAsync(context, next);
            return;
        }

        var dbContext = context.HttpContext.RequestServices.GetRequiredService<TonometerContext>();
        var hasAccess = await dbContext.Users.Where(x => x.Id == userId)
                                       .Select(x => x.Type == UserType.Admin)
                                       .FirstOrDefaultAsync();

        if (hasAccess)
        {
            context.ActionArguments["userType"] = UserType.Admin;

            await base.OnActionExecutionAsync(context, next);
            return;
        }

        context.Result = new ForbidResult();
    }
}
