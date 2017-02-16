using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Arc.ActiveDirectory.Shared
{
    public class WebRequestsLogger : OwinMiddleware
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WebRequestsLogger(OwinMiddleware next)
            : base(next)
        { }

        public override async Task Invoke(IOwinContext context)
        {
            var currentId = Guid.NewGuid();

            LogRequest(context, currentId);
            await Next.Invoke(context);
            LogResponse(context, currentId);
        }

        private void LogRequest(IOwinContext context, Guid currentId)
        {
            var message = string.Format("{2} - Request {0} - {1}",
                context.Request.Path,
                DateTime.Now.ToString("s"),
                currentId);

            _logger.Info(message);
        }

        private void LogResponse(IOwinContext context, Guid currentId)
        {
            var message = string.Format("{2} - Response {0} - {1}",
                context.Response.StatusCode,
                DateTime.Now.ToString("s"),
                currentId);

            _logger.Info(message);
        }
    }
}