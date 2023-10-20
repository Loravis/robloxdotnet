//Class for authenticated sessions
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Robloxdotnet.Utilities.Authentication;
using Robloxdotnet.Utilities.Groups;

namespace Robloxdotnet
{
    public class RobloxSession
    {
        private string roblosecurity;
        public int id;
        public string name;
        public string displayName;

        public async Task LoginAsync(string roblosecurityCookie)
        {

            var usersAddress = new Uri("https://users.roblox.com");
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookieContainer;
            cookieContainer.Add(usersAddress, new Cookie(".ROBLOSECURITY", roblosecurityCookie));

            var client = new HttpClient(handler);
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
                roblosecurity = roblosecurityCookie;
            }
            else
            {
                Console.Write("Authorization error!");
            }
        }

        public string GetRoblosecurity()
        {
            return roblosecurity;
        }

    }
}
