using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;   //GetService<>()

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)  // next is what happens next after action is executed // executes the action and do something after
        {
            var resultContext = await next();           // we wait until after the user has completed their activity to execute next()

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return; // here we are checking to see if the user is authenticated, if not return nothing

           // var username = resultContext.HttpContext.User.GetUsername();      // if they are authenticated we want to update the actionResult property
            var userId = resultContext.HttpContext.User.GetUserId();      // if they are authenticated we want to update the actionResult property // added once we update our extension and services
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();   // accesses our repository
           //  var user = await repo.GetUserByUsernameAsync(username);
            var user = await repo.GetUserByIdAsync(userId); // updated to reflect extension and services changes
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync();
        }
    }
}