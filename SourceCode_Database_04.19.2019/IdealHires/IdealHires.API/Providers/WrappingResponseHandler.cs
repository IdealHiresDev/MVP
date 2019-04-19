using IdealHires.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace IdealHires.API.Providers
{
    public class WrappingResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {            
            if (IsSwagger(request))
            {
              return await base.SendAsync(request, cancellationToken);
            }
            else
            {
                var response = await base.SendAsync(request, cancellationToken);
                try
                {
                    return GenerateResponse(request, response);
                }
                catch (Exception ex)
                {
                    ResponseDTO responseMetadata = new ResponseDTO();
                    responseMetadata = GetResponseMetadata(HttpStatusCode.InternalServerError, string.Empty, ex.Message);
                    return request.CreateResponse(response.StatusCode, responseMetadata);
                }
            }
        }

        private bool IsSwagger(HttpRequestMessage request)
        {
            return request.RequestUri.PathAndQuery.StartsWith("/swagger");
        }
        private HttpResponseMessage GenerateResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            string errorMessage = null;
            HttpStatusCode statusCode = response.StatusCode;
            ResponseDTO responseMetadata = new ResponseDTO();
            if (!IsResponseValid(response))
            {
                responseMetadata = GetResponseMetadata(HttpStatusCode.BadRequest, string.Empty, ApiResource.BadRequest);
                return request.CreateResponse(response.StatusCode, responseMetadata);
            }
            object responseContent;
            if (response.TryGetContentValue(out responseContent))
            {
                HttpError httpError = responseContent as HttpError;
                if (httpError != null)
                {
                    errorMessage = httpError.Message;
                    statusCode = HttpStatusCode.InternalServerError;
                    responseContent = null;
                }
            }
            responseMetadata = GetResponseMetadata(statusCode, responseContent, errorMessage);
            var result = request.CreateResponse(response.StatusCode, responseMetadata);
            return result;
        }
        private bool IsResponseValid(HttpResponseMessage response)
        {
            if ((response != null) && (response.StatusCode == HttpStatusCode.OK))
                return true;
            return false;
        }

        private ResponseDTO GetResponseMetadata(HttpStatusCode statusCode, object responseContent, string errorMessage)
        {
            ResponseDTO responseMetadata = new ResponseDTO();
            responseMetadata.Version = ApiResource.Version;
            responseMetadata.StatusCode = statusCode;
            responseMetadata.Result = JsonConvert.SerializeObject(responseContent);
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            responseMetadata.Timestamp = dt;
            responseMetadata.Error = errorMessage;
            responseMetadata.Size = (responseContent == null) ? 0 : responseContent.ToString().Length;
            return responseMetadata;
        }
    }
}