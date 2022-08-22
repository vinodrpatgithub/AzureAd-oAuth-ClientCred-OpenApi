using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;

namespace SecureAzureFunctionMiW
{
    public class TestOpenApiClientCredFlow
    {
        private readonly ILogger<TestOpenApiClientCredFlow> _logger;
        readonly string[] appRoleRequiredByApi;

        public TestOpenApiClientCredFlow(ILogger<TestOpenApiClientCredFlow> logger)
        {
            _logger = logger;
            appRoleRequiredByApi = new string[] { Environment.GetEnvironmentVariable("ApplicationRole") };
        }

        [FunctionName(nameof(TestOpenApiClientCredFlow))]
        [OpenApiOperation(operationId: "oauth.flows.clientcredentials", tags: new[] { "oauth" }, Summary = "OAuth client credentials flows", Description = "This shows the OAuth client crendetails flows", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(ClientCredAuthFlow.SchemeName, SecuritySchemeType.OAuth2, Flows = typeof(ClientCredAuthFlow))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                var (authenticationStatus, authenticationResponse) =
                    await req.HttpContext.AuthenticateAzureFunctionAsync();
                if (!authenticationStatus) return authenticationResponse;

                req.HttpContext.ValidateAppRole(appRoleRequiredByApi);

                return new JsonResult("This HTTP triggered function executed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception : {0}", ex);
                throw;
            }
        }

    }
}
