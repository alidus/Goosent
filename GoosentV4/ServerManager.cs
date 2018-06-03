using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Goosent
{
    /// <summary>
    /// Класс для работы с удаленным сервером
    /// </summary>
    public class ServerManager
    {
        private string _email;
        private string _password;
        private string _uid;

        public async Task<SignInResponseContainer> SignIn()
        {
            string url = "http://artkholl.pythonanywhere.com/authorization?email=" + _email + "&pass=" + _password;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    object jsonDoc = await Task.Run(() =>
                    {
                        var serializer = new JsonSerializer();
                        using (var sr = new StreamReader(stream))
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            return serializer.Deserialize(jsonTextReader);
                        }
                    });
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                    SignInResponseContainer dsResponse = JsonConvert.DeserializeObject<SignInResponseContainer>(jsonDoc.ToString());

                    // Return the JSON document:
                    return dsResponse;
                }
            }
        }

        public class SignInResponseContainer
        {
            public SignInResponse response { get; set; }
        }
        
        public class SignInResponse
        {
            public string answer { get; set; }
            public string uid { get; set; }
        }

        public async Task<SignInResponseContainer> SignUp()
        {
            string url = "http://artkholl.pythonanywhere.com/registration?email=" + _email + "&pass=" + _password;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    object jsonDoc = await Task.Run(() =>
                    {
                        var serializer = new JsonSerializer();
                        using (var sr = new StreamReader(stream))
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            return serializer.Deserialize(jsonTextReader);
                        }
                    });
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                    SignInResponseContainer dsResponse = JsonConvert.DeserializeObject<SignInResponseContainer>(jsonDoc.ToString());

                    // Return the JSON document:
                    return dsResponse;
                }
            }
        }

        public async Task<AddChatToListeningResponseContainer> AddChatToListening(string channelName)
        {
            string url = "http://artkholl.pythonanywhere.com/twitch_add?chat=" + channelName + "&uid=" + _uid;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    object jsonDoc = await Task.Run(() =>
                    {
                        var serializer = new JsonSerializer();
                        using (var sr = new StreamReader(stream))
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            return serializer.Deserialize(jsonTextReader);
                        }
                    });
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                    AddChatToListeningResponseContainer dsResponse = JsonConvert.DeserializeObject<AddChatToListeningResponseContainer>(jsonDoc.ToString());

                    // Return the JSON document:
                    return dsResponse;
                }
            }
        }

        public async Task<AddChatToListeningResponseContainer> DeleteListeningChat()
        {
            string url = "http://artkholl.pythonanywhere.com//twitch_del?uid=" + _uid;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    object jsonDoc = await Task.Run(() =>
                    {
                        var serializer = new JsonSerializer();
                        using (var sr = new StreamReader(stream))
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            return serializer.Deserialize(jsonTextReader);
                        }
                    });
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                    AddChatToListeningResponseContainer dsResponse = JsonConvert.DeserializeObject<AddChatToListeningResponseContainer>(jsonDoc.ToString());

                    // Return the JSON document:
                    return dsResponse;
                }
            }
        }

        public class AddChatToListeningResponseContainer
        {
            public AddChatResponse response { get; set; }
        }

        public class AddChatResponse
        {
            public string answer { get; set; }
        }

        public async Task<NewMessagesFromChatContainer> GetNewMessagesFromChat()
        {
            string url = "http://artkholl.pythonanywhere.com/get_messages?uid=" + _uid;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    object jsonDoc = await Task.Run(() =>
                    {
                        var serializer = new JsonSerializer();
                        using (var sr = new StreamReader(stream))
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            return serializer.Deserialize(jsonTextReader);
                        }
                    });
                    var jds = jsonDoc.ToString();
                    NewMessagesFromChatContainer dsResponse = JsonConvert.DeserializeObject<NewMessagesFromChatContainer>(jsonDoc.ToString());
                    
                    // Return the JSON document:
                    return dsResponse;
                }
            }
        }

        public class NewMessagesFromChatContainer
        {
            public string channel_name { get; set; }
            public MessageFromChat[] messages_array { get; set; }
        }

        public class MessageFromChat
        {
            public string message { get; set; }
            public string user { get; set; }
        }

        public void SetEmail(string email)
        {
            _email = email;
        }

        public string UID
        {
            get { return _uid; }
            set { _uid = value; }
        }

        public void SetPassword(string password)
        {
            _password = password;
        }

    }
}