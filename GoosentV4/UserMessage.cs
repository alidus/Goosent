using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Goosent
{
    public class UserMessage
    {
        public string userName;
        public string userMessageText;
        public TimeSpan messageTime;
        public string userColorHex;

        public UserMessage(string userName, string userMessageText, TimeSpan messageTime, string userColorHex)
        {
            this.userName = userName;
            this.userMessageText = userMessageText;
            this.messageTime = messageTime;
            this.userColorHex = userColorHex;
        }
    }
}