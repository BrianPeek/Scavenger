using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ScavengerServer
{
    public static class GetAdminPage
    {
        public static string CachedPage = null;

        [FunctionName("GetAdminPage")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "GetAdminPage")]HttpRequestMessage req,
            TraceWriter log, ExecutionContext context)
        {
            log.Info($"GetAdminPage begin");

            if (string.IsNullOrEmpty(CachedPage))
            {
                CachedPage = File.ReadAllText($"{context.FunctionAppDirectory}\\adminPage.html");
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new StringContent(CachedPage);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
