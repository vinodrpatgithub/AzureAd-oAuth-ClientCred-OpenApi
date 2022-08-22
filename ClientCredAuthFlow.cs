using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;
using System;

namespace SecureAzureFunctionMiW
{
    public class ClientCredAuthFlow : OpenApiOAuthSecurityFlows
    {
        private string _authTokenRefreshUrl = "https://login.microsoftonline.com/{0}/oauth2/v2.0/token";
        private string _tenantId = Environment.GetEnvironmentVariable("TenantId");
        private string _clientId = Environment.GetEnvironmentVariable("ClientId");
        public const string SchemeName = "ClientCredAuthFlow";

        public ClientCredAuthFlow()
        {
            var scope = string.Concat("api://", _clientId, "/.default");

            this.ClientCredentials = new OpenApiOAuthFlow()
            {
                Scopes = { { scope, "Default scope defined in the app" } },
                TokenUrl = new Uri(string.Format(_authTokenRefreshUrl, _tenantId))
            };
        }
    }
}
