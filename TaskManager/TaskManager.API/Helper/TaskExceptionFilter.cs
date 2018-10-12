using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace TaskManager.API.Helper
{
    public class TaskExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            String message = "Task Manager Service Exception : " + actionExecutedContext.Exception.Message;

            var exceptionType = actionExecutedContext.Exception.GetType();

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = status
            };
            base.OnException(actionExecutedContext);
        }
    }
}