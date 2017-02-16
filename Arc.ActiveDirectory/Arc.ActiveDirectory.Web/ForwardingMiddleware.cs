using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Arc.ActiveDirectory.Web
{
    public class ForwardingMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _appFunc;

        public ForwardingMiddleware(Func<IDictionary<string, object>, Task> appFunc)
        {
            _appFunc = appFunc;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var owinContext = new OwinContext(environment);

            //var request = await CreateHttpWebRequest(owinContext);
            //await ProcessResponse(request, owinContext.Response);

            await _appFunc(environment);
        }

        public async Task ProcessResponse(HttpWebRequest request, IOwinResponse owinResponse)
        {
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

            var response = await request.GetResponseAsync() as HttpWebResponse;
            await MapOwinResponse(response as HttpWebResponse, owinResponse);
        }

        public async Task MapOwinResponse(HttpWebResponse httpWebResponse, IOwinResponse owinResponse)
        {
            owinResponse.StatusCode = (int)httpWebResponse.StatusCode;

            var responseStream = httpWebResponse.GetResponseStream();

            if (responseStream != null)
            {
                await responseStream.CopyToAsync(owinResponse.Body);
            }

            foreach (var key in httpWebResponse.Headers.AllKeys)
            {
                if (owinResponse.Headers.ContainsKey(key))
                {
                    owinResponse.Headers.Remove(key);
                }

                if (!owinResponse.Headers.ContainsKey(key))
                {
                    owinResponse.Headers.Add(key, httpWebResponse.Headers.GetValues(key));
                }
            }
        }

        public async Task<HttpWebRequest> CreateHttpWebRequest(IOwinContext owinContext)
        {
            return null;

            //var url = Constants.CisApiUrl + owinContext.Request.Path.Value + owinContext.Request.QueryString;

            //var request = WebRequest.CreateHttp(url);

            //foreach (var key in owinContext.Request.Headers.Keys)
            //{
            //    if (key.ToLower() != "cookie")
            //    {
            //        AddHeader(key, owinContext.Request.Headers[key], request);
            //    }
            //}

            //request.Method = owinContext.Request.Method;

            //foreach (var httpHeaderBuilder in _httpHeaderBuilders)
            //{
            //    await httpHeaderBuilder.AddHeader(owinContext, request);
            //}

            //if (request.Method == "GET")
            //{
            //    return request;
            //}

            //CopyBody(request, owinContext.Request.Body);

            //return request;
        }

        private void CopyBody(WebRequest webRequest, Stream sourceStream)
        {
            Stream webStream = null;

            try
            {
                //copy incoming request body to outgoing request
                if (sourceStream != null && sourceStream.Length > 0)
                {
                    long length = sourceStream.Length;
                    webRequest.ContentLength = length;
                    webStream = webRequest.GetRequestStream();
                    sourceStream.CopyTo(webStream);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (null != webStream)
                {
                    webStream.Flush();
                    webStream.Close();
                }
            }
        }
    }
}