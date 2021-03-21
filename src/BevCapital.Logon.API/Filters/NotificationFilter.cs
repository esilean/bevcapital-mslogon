using BevCapital.Logon.Domain.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BevCapital.Logon.API.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly IAppNotificationHandler _appNotificationHandler;

        public NotificationFilter(IAppNotificationHandler appNotificationHandler)
        {
            _appNotificationHandler = appNotificationHandler;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                foreach (var error in context.ModelState.Values.SelectMany(x => x.Errors))
                    _appNotificationHandler.AddNotification("ModelStateValidator", error.ErrorMessage);
            }

            if (_appNotificationHandler.HasNotifications)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.HttpContext.Response.ContentType = "application/json";

                var notifications = JsonConvert.SerializeObject(_appNotificationHandler.Notifications);
                await context.HttpContext.Response.WriteAsync(notifications);

                return;
            }

            await next();
        }
    }
}
