﻿using Newtonsoft.Json;
using Robloxdotnet;
using System.Net;
using System.Text;

namespace Robloxdotnet.Utilities.Misc
{
    public class MessageManagement
    {
        private RobloxSession session;

        public MessageManagement(RobloxSession session) {
            this.session = session;
        }

        public async Task<bool> SendMessage(string subject, string body, ulong recipientId)
        {
            using (HttpClient client = session.GetClient(new Uri("https://privatemessages.roblox.com")))
            {

                if (subject == String.Empty)
                {
                    throw new Exception("The message subject can't be blank.");
                }

                if (body == String.Empty)
                {
                    throw new Exception("The message body can't be blank.");
                }

                Message message = new Message()
                {
                    subject = subject,
                    body = body,
                    recipientId = recipientId,
                    includePreviousMessage = false
                };

                string messageJSON = JsonConvert.SerializeObject(message);
                StringContent payload = new(messageJSON, Encoding.UTF8, "application/json");
                var firstResponse = await client.PostAsync("/v1/messages/send", null);
                client.DefaultRequestHeaders.Add("x-csrf-token", firstResponse.Headers.GetValues("x-csrf-token").First());
                var secondResponse = await client.PostAsync("v1/messages/send", payload);

                if (secondResponse.StatusCode == HttpStatusCode.OK)
                {
                    string content = await secondResponse.Content.ReadAsStringAsync();
                    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(content);
                    if (response.success)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception(response.message);
                    }
                }
                else
                {
                    throw new Exception("The server returned HTTP status code " + secondResponse.StatusCode);
                }
            }
        }
    }
    public class Message
    {
        public string subject { get; set; }
        public string body { get; set; }
        public ulong recipientId { get; set; }
        public bool includePreviousMessage { get; set; }
    }
    public class ApiResponse
    {
        public bool success { get; set; }
        public string shortMessage { get; set; }
        public string message { get; set; }
    }
}
