using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Net;

namespace MIP.WebApp.Controllers
{
    public class TokenAcquisitionController : Controller
    {

        public TokenAcquisitionController(ITokenAcquisition tokenAcquisition, IHttpClientFactory client)
        {
            TokenAcquisition = tokenAcquisition;
            ClientFactory = client;
        }

        public ITokenAcquisition TokenAcquisition { get; }
        public IHttpClientFactory ClientFactory { get; }

        //assign the app role to the app.. since UserAssignment = true, this wont work unless a role is assigned to a user.
        // ./default when using client credentials or when requesting token on behalf of the app 
        public async Task<IActionResult> TokenAcquisitionForApp()
        {
            string data = string.Empty;
            var token = await TokenAcquisition.GetAccessTokenForAppAsync("{web api's client id}/.default");
            // var token = await TokenAcquisition.GetAccessTokenForAppAsync("api://{{web api's client id}}/.default");

            HttpClient httpClient = ClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync($"https://localhost:7053/WeatherForecast");

            if (response.StatusCode == HttpStatusCode.OK)
                data = await response.Content.ReadAsStringAsync();

            return View();
        }

        //this scope has to be granted to the app and the user has to provide consent while signing in
        [AuthorizeForScopes(ScopeKeySection = "api://{web api's client id}/webapi1.user")]
        public async Task<IActionResult> TokenAcquisitionForUser()
        {
            string data = string.Empty;
            var scopes = new List<string> { "api://{web api's client id}/webapi1.user" };
            var token2 = await TokenAcquisition.GetAccessTokenForUserAsync(scopes);

            HttpClient httpClient = ClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token2);

            var response = await httpClient.GetAsync($"https://localhost:7053/WeatherForecast");

            if (response.StatusCode == HttpStatusCode.OK)
                data = await response.Content.ReadAsStringAsync();

            return View();
        }

        //this scope has to be granted to the app and only the admin can provide consent
        [AuthorizeForScopes(ScopeKeySection = "api://{web api's client id}/onlyadmin")]
        public async Task<IActionResult> TokenAcquisitionForUserOnlyAdminScope()
        {
            var scopes = new List<string> { "api://{web api's client id}/onlyadmin" };
            var token2 = await TokenAcquisition.GetAccessTokenForUserAsync(scopes);

            return View();
        }

        public async Task<IActionResult> UsingDownStreamApi([FromServices] IDownstreamWebApi  downstreamWebApi)
        {

            HttpResponseMessage value = await downstreamWebApi.CallWebApiForAppAsync(
                  "WebApi1",
                  options =>
                  {
                      options.HttpMethod = HttpMethod.Get;
                      options.RelativePath = "WeatherForecast";
                  });

            return View();
        }
    }
}
