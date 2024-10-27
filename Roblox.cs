using Newtonsoft.Json;
using Robloxdotnet.Utilities.Users;
using System.Net;
using System.Text;

namespace Robloxdotnet
{
    public static class Roblox
    {
        public static async Task<ulong> GetIdFromUsername(string username, bool excludeBannedUsers = true) 
        {
            using (HttpClient client = new HttpClient())
            {
                string[] usernameArray = [username];

                UsernamePost usernamePost = new UsernamePost()
                {
                    usernames = usernameArray,
                    excludeBannedUsers = excludeBannedUsers
                };

                var usersAddress = new Uri("https://users.roblox.com");
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
                catch (JsonSerializationException)
                {
                    throw new Exceptions.InvalidUsernameException("Username not found!");
                }
            }
        }

        public static async Task<string> GetUsernameFromId(ulong userId, bool excludeBannedUsers = true)
        {
            using (HttpClient client = new HttpClient())
            {
                ulong[] idArray = [userId];

                UserIdPost userIdPost = new UserIdPost()
                {
                    userIds = idArray,
                    excludeBannedUsers = excludeBannedUsers
                };

                var usersAddress = new Uri("https://users.roblox.com");
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
                catch (JsonSerializationException)
                {
                    throw new Exceptions.InvalidUserIdException("The provided userId is invalid!");
                }
            }
        }

        public static async Task<UserInfo> GetUserInfo(ulong userId)
        {
            using (HttpClient client = new HttpClient())
            {
                var usersAddress = new Uri("https://users.roblox.com");
                client.BaseAddress = usersAddress;

                var userResult = await client.GetAsync("/v1/users/" + userId.ToString());
                string userResultString = await userResult.Content.ReadAsStringAsync();
                UserInfo userDetails = JsonConvert.DeserializeObject<UserInfo>(userResultString);

                return userDetails;
            }
        }

        public static async Task<string> GetUserThumbnail(ulong userId, string format, bool isCircular, string thumbnailType)
        {
            using (HttpClient client = new HttpClient())
            {
                var thumbAddress = new Uri("https://thumbnails.roblox.com");
                client.BaseAddress = thumbAddress;

                string size = "420x420";

                string requestString = "/v1/users/" + thumbnailType + "?userIds=" + userId.ToString() + "&size=" + size + "&format=" + format + "&isCircular=" + isCircular.ToString().ToLower();

                var response = await client.GetAsync(requestString);
                var userResultString = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Remove the unwanted "data" content
                    userResultString = userResultString.Replace("\"data\":[{", "");
                    userResultString = userResultString.Replace("]}", "");

                    string imageUrl = JsonConvert.DeserializeObject<ThumbnailResponse>(userResultString).imageUrl;
                    return imageUrl;
                }
                else
                {
                    throw new Exception("There was an error retrieving the requested user's " + thumbnailType + "!");
                }
            }
        }
    }

    public class ThumbnailResponse
    {
        public string imageUrl { get; set; }
    }
}
