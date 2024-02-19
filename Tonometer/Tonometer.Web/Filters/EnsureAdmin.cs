#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Tonometer.Database;
using Tonometer.Database.Entities;

#endregion

namespace Tonometer.Web.Filters;

public sealed class EnsureAdminAttribute : ActionFilterAttribute
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

        var dbContext = context.HttpContext.RequestServices.GetRequiredService<TonometerContext>();
        var hasAccess = await dbContext.Users.Where(x => x.Id == userId)
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
