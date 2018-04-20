using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ScavengerServer
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static HttpResponseMessage Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="Function2/{id}")]HttpRequestMessage req,
			//string id,
			[DocumentDB("ScavengerDB", "Items", ConnectionStringSetting = "ConnectionString", Id = "{id}" /*SqlQuery = "SELECT * FROM Items i where i.id = {id}"*/)] Item documents,
			TraceWriter log
)
        {
			//log.Info($"Id: {id}");

			return req.CreateResponse(HttpStatusCode.OK, documents);
        }
    }
}
