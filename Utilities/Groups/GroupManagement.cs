﻿using Robloxdotnet.Exceptions;
using System.Net;
using System.Text;

namespace Robloxdotnet.Utilities.Groups
{
    public class GroupManagement
    {
        private RobloxSession session;

        public GroupManagement(RobloxSession session)
        {
            this.session = session;
        }

        public async Task SetGroupShout(ulong groupId, string message)
        {
            using (HttpClient client = session.GetClient(new Uri("https://groups.roblox.com")))
            {
                var messageJSON = "{ \"message\": \"" + message + "\" }";
                var payload = new StringContent(messageJSON, Encoding.UTF8, "application/json");
                var initialResponse = await client.PatchAsync("/v1/groups/" + groupId + "/status", null);
                client.DefaultRequestHeaders.Add("x-csrf-token", initialResponse.Headers.GetValues("x-csrf-token").First());
                var result = await client.PatchAsync("/v1/groups/" + groupId + "/status", payload);

                // resultString not neccesary?
                // var resultString = await result.Content.ReadAsStringAsync();

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
        }
    }
}
