using Newtonsoft.Json;
using RobloxNET.Utilities.Users;
using System.Net;
using System.Text;

namespace RobloxNET
{
    public static class Roblox
    {
        public static async Task<int> GetIdFromUsername(string username, bool excludeBannedUsers = true)
        {
            string[] usernameArray = [username];

            UsernamePost usernamePost = new UsernamePost()
            {
                usernames = usernameArray,
                excludeBannedUsers = excludeBannedUsers
            };

            var usersAddress = new Uri("https://users.roblox.com");
            var client = new HttpClient();
            client.BaseAddress = usersAddress;

            var usernamePostJSON = JsonConvert.SerializeObject(usernamePost);
            var payload = new StringContent(usernamePostJSON, Encoding.UTF8, "application/json");

            var userResult = await client.PostAsync("/v1/usernames/users", payload);
            string userResultString = await userResult.Content.ReadAsStringAsync();

            //Remove the unwanted "data" content
            int index1 = userResultString.IndexOf("\"data\":[{");
            userResultString = userResultString.Replace("\"data\":[{", "");
            userResultString = userResultString.Replace("]}", "");

            try
            {
                UsernameResponse usernameResponse = JsonConvert.DeserializeObject<UsernameResponse>(userResultString);
                return usernameResponse.id;
            }
            catch (Newtonsoft.Json.JsonSerializationException e)
            {
                throw new Exceptions.InvalidUsernameException("Username not found!");
            }

        }

        public static async Task<string> GetUsernameFromId(int userId, bool excludeBannedUsers = true)
        {
            int[] idArray = [userId];

            UserIdPost userIdPost = new UserIdPost()
            {
                userIds = idArray,
                excludeBannedUsers = excludeBannedUsers
            };

            var usersAddress = new Uri("https://users.roblox.com");
            var client = new HttpClient();
            client.BaseAddress = usersAddress;

            var userIdPostJSON = JsonConvert.SerializeObject(userIdPost);
            var payload = new StringContent(userIdPostJSON, Encoding.UTF8, "application/json");

            var userResult = await client.PostAsync("/v1/users", payload);
            string userResultString = await userResult.Content.ReadAsStringAsync();

            //Remove the unwanted "data" content
            int index1 = userResultString.IndexOf("\"data\":[{");
            userResultString = userResultString.Replace("\"data\":[{", "");
            userResultString = userResultString.Replace("]}", "");

            try
            {
                UsernameResponse userIdResponse = JsonConvert.DeserializeObject<UsernameResponse>(userResultString);
                return userIdResponse.name;
            }
            catch (Newtonsoft.Json.JsonSerializationException e)
            {
                throw new Exceptions.InvalidUserIdException("The provided userId is invalid!");
            }
        }
    }
}
