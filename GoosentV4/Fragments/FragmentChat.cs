using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using Android.Support.V4;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Goosent
{
    public class FragmentChat : BaseTabFragment
    {

        private List<UserMessage> chatMessages = new List<UserMessage>();
        private Adapters.ChatMessagesArrayAdapter chatAdapter;
        private ListView chatListView;
        private int _temp_messageCounter = 0;
        private string currentChannelName;
        private string prevChannelName;
        private ServerManager sm;

        //Temp
        private List<string> fakeNames = new List<string>();
        private List<string> fakeMessages = new List<string>();
        Random random = new Random();

        public static FragmentChat getInstance(Context context)
        {
            Bundle args = new Bundle();
            FragmentChat cFragment = new FragmentChat();
            cFragment.Arguments = args;
            cFragment.SetContext(context);
            cFragment.SetTitle(context.GetString(Resource.String.tab_item_chat));

            return cFragment;
        }

        //private void TempInitFake()
        //{
        //    fakeNames.Add("Ramzan");
        //    fakeNames.Add("Valera");
        //    fakeNames.Add("Micheal");
        //    fakeNames.Add("Denis");
        //    fakeNames.Add("Max");
        //    fakeNames.Add("Rose");
        //    fakeNames.Add("Gleb");
        //    fakeNames.Add("Artyom");
        //    fakeMessages.Add("Всем привет!");
        //    fakeMessages.Add("MLG!");
        //    fakeMessages.Add("So pro! Such skill! Wow!");
        //    fakeMessages.Add(")))))))))))))000)000)hrtklhrtklhjmklergmnrklgergjrkgjergkljrglk;ejgrjgkl");
        //    fakeMessages.Add("KappaPride");
        //    fakeMessages.Add("SimpleText");
        //    fakeMessages.Add("Hello everyone!");
        //    fakeMessages.Add("General Kenobi!");
        //}

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            chatAdapter = new Adapters.ChatMessagesArrayAdapter(context, chatMessages);
            chatListView = (ListView)view.FindViewById(Resource.Id.chat_list_view);
            sm = new ServerManager();

            chatListView.Adapter = chatAdapter;
            AsyncConsistantChatUpdating();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.FragmentChatLayout, container, false);
            //TempInitFake();

            

            return view;
        }

        public void SetContext(Context context)
        {
            this.context = context;
        }

        private async Task AsyncConsistantChatUpdating()
        {
            while (true)
            {

                Activity.RunOnUiThread(() =>
                {
                    Task.Run(async () =>
                    {
                        var newMessages = await ((MainActivity)context).sm.GetNewMessagesFromChat();
                        AddNewMessages(newMessages);
                        
                    });
                    
                });

                _temp_messageCounter += 1;
                chatAdapter.NotifyDataSetChanged();
                await Task.Delay(TimeSpan.FromSeconds(0.5));
            }
        }

        private void AutoScrollChatToTheBottom()
        {
            chatListView.SetSelection(chatAdapter.Count - 1);
        }

        private async Task AddNewMessages(ServerManager.NewMessagesFromChatContainer newMessages)
        {
            //chatMessages.Add(new UserMessage(fakeNames[random.Next(0, fakeNames.Count-1)], fakeMessages[random.Next(0, fakeMessages.Count - 1)], new TimeSpan(14, 10, 5), String.Format("#{0:X6}", random.Next(0x1000000))));

            if (((MainActivity)context).SelectedChannel.Name != "")
            {
                sm.UID = ((MainActivity)context).sm.UID;
                currentChannelName = newMessages.channel_name;
                if (currentChannelName != prevChannelName)
                {
                    chatMessages.Clear();
                    chatMessages.Add(new UserMessage("Чтение канала", currentChannelName, new TimeSpan(14, 10, 5), "#ffffff"));

                }
                prevChannelName = currentChannelName;
                foreach (ServerManager.MessageFromChat message in newMessages.messages_array)
                {
                    Console.WriteLine(message.message);
                    chatMessages.Add(new UserMessage(message.user, message.message, new TimeSpan(14, 10, 5), String.Format("#{0:X6}", random.Next(0x1000000))));
                }

                
                //if (newMessages.messages_array.Length > 0)
                //{
                    
                //}

                //if (chatListView.LastVisiblePosition == chatAdapter.Count - 2)
                //{
                //    AutoScrollChatToTheBottom();
                //}
            }
            
        }
    }
}