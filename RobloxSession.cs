//Class for authenticated sessions
using System.Net;
using Newtonsoft.Json;
using Robloxdotnet.Utilities.Authentication;

namespace Robloxdotnet
{
    public class RobloxSession 
    {
        private string roblosecurity;
        public ulong id;
        public string? name;
        public string? displayName;

        public RobloxSession(string roblosecurityCookie) {
            roblosecurity = roblosecurityCookie;
        }

        public async Task LoginAsync()
        {
            var usersAddress = new Uri("https://users.roblox.com");
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookieContainer;
            cookieContainer.Add(usersAddress, new Cookie(".ROBLOSECURITY", roblosecurity));

            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = usersAddress;

                var authResult = await client.GetAsync("/v1/users/authenticated");

                if (authResult.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exceptions.InvalidLoginException("The login has failed!");
                }

                if (authResult.StatusCode == HttpStatusCode.OK)
                {
                    string resultString = await authResult.Content.ReadAsStringAsync();
                    RobloxLogin robloxLogin = JsonConvert.DeserializeObject<RobloxLogin>(resultString);
                    id = robloxLogin.id;
                    name = robloxLogin.name;
                    displayName = robloxLogin.displayName;
                }
                else
                {
                    throw new Exception("Authentication failed! The server returned the HTTP status code " + authResult.StatusCode.ToString());
                }
            }
        }

        public HttpClient GetClient(Uri baseAddress) {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookieContainer;
            cookieContainer.Add(baseAddress, new Cookie(".ROBLOSECURITY", roblosecurity));
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = baseAddress;

            return client;
        }
    }
}
