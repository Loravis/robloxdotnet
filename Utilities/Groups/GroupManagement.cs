using Robloxdotnet.Exceptions;
using System.Net;
using System.Text;

namespace Robloxdotnet.Utilities.Groups
{
    public class GroupManagement
    {
        private static string roblosecurity = null;
        public static async Task SetGroupShout(RobloxSession session, ulong groupId, string message)
        {
            roblosecurity = session.GetRoblosecurity();

            if (roblosecurity == null)
            {
                throw new Exceptions.InvalidLoginException("The current RobloxSession is not logged into any account! Did you forget to run LoginAsync()?");
            }

            HttpClient client = CreateHttpClient();

            var messageJSON = "{ \"message\": \"" + message + "\" }";
            var payload = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var initialResponse = await client.PatchAsync("/v1/groups/" + groupId + "/status", null);
            client.DefaultRequestHeaders.Add("x-csrf-token", initialResponse.Headers.GetValues("x-csrf-token").First());
            var result = await client.PatchAsync("/v1/groups/" + groupId + "/status", payload);

            var resultString = await result.Content.ReadAsStringAsync();

            if (result.StatusCode != HttpStatusCode.OK)
            {
                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new GroupException("Failed to set the group shout! You either lack permission to set the group shout or entered an invalid group ID");
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Authorization has been denied for this request.");
                }
            }
        }
        private static HttpClient CreateHttpClient()
        {
            var baseAddress = new Uri("https://groups.roblox.com");
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
