using Newtonsoft.Json;
using System.Net;
using System.Text;
using Robloxdotnet.Exceptions;

namespace Robloxdotnet.Utilities.Groups
{
    public class MemberManagement
    {
        private RobloxSession session;

        public MemberManagement(RobloxSession session) {
            this.session = session;
        }

        public async Task<bool> SetUserGroupRole(ulong groupId, ulong userId, int role)
        {
            using (HttpClient client = session.GetClient(new Uri("https://groups.roblox.com")))
            {

                //Get roleID from the inserted role number
                var rolesResponse = await client.GetAsync("/v1/groups/" + groupId + "/roles");
                var rolesString = await rolesResponse.Content.ReadAsStringAsync();

                string replace = "\"groupId\":" + Convert.ToString(groupId) + ",";
                rolesString = rolesString.Replace(replace, "");
                replace = "{\"roles\":";
                rolesString = rolesString.Replace(replace, "");
                rolesString = rolesString.Remove(rolesString.Length - 1, 1);
                List<RoleList> roleList;

                try
                {
                    roleList = JsonConvert.DeserializeObject<List<RoleList>>(rolesString);
                }
                catch (Exception ex)
                {
                    throw new InvalidUserIdException("The specified group does not exist!");
                };

                bool roleFound = false;
                ulong roleId = 0;
                foreach (RoleList currentRole in roleList)
                {
                    if (currentRole.rank == role)
                    {
                        roleFound = true;
                        roleId = currentRole.id;
                        break;
                    }
                }
                if (!roleFound)
                {
                    throw new Exceptions.InvalidRoleException("The specified role number could not be found! Ensure you enter a valid role number that's between 0-255!");
                }

                string patchContentJSON = "{ \"roleId\": " + roleId + " }";
                var payload = new StringContent(patchContentJSON, Encoding.UTF8, "application/json");
                var firstResponse = await client.PatchAsync("/v1/groups/" + groupId + "/users/" + userId, null);
                client.DefaultRequestHeaders.Add("x-csrf-token", firstResponse.Headers.GetValues("x-csrf-token").First());
                var secondResponse = await client.PatchAsync("/v1/groups/" + groupId + "/users/" + userId, payload);

                if (secondResponse.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else if (secondResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new GroupException("You do not have permission to manage this member.");
                }
                else
                {
                    throw new GroupException("There was an error executing your request! HTTP Status Code: " + secondResponse.StatusCode);
                }
            }
        }

        public async Task<UserGroupInfo> GetUserGroupInfo(ulong userId)
        {
            using (HttpClient client = session.GetClient(new Uri("https://groups.roblox.com")))
            {
                var rolesResult = await client.GetAsync("/v1/users/" + userId.ToString() + "/groups/roles");
                string userResultString = await rolesResult.Content.ReadAsStringAsync();
                UserGroupInfo root;

                try
                {
                    root = JsonConvert.DeserializeObject<UserGroupInfo>(userResultString);
                }
                catch (Exception ex)
                {
                    throw new InvalidUserIdException("The specified user does not exist!");
                }


                return root;
            }
        }
    }
}
