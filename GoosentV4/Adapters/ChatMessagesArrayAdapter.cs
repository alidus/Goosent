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
using Java.Lang;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;

namespace Goosent.Adapters
{
    class ChatMessagesArrayAdapter : BaseAdapter<string>
    {
        List<UserMessage> _messages;
        private Context mContext;

        public ChatMessagesArrayAdapter(Context context, List<UserMessage> messages)
        {
            _messages = messages;
            mContext = context;
        }

        public override string this[int position]
        {
            get { return _messages[position].userName; }
        }

        public override int Count
        {
            get { return _messages.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.ChatListViewRow, null, false);
            }

            TextView chatMessageTextTextView = (TextView)row.FindViewById(Resource.Id.chatMessages_row_textView);

            string htmlChatRow = "";
            htmlChatRow += "<html><body><font color='" + _messages[position].userColorHex + "'>" + _messages[position].userName + "</font>" + "<font>: " + _messages[position].userMessageText + "</font></body></html>";
            //SpannableString chatMessageSpannableString = new SpannableString(new Java.Lang.String(_messages[position].userName + ": " +_messages[position].userMessageText));

            //chatMessageSpannableString.SetSpan(new ForegroundColorSpan(_messages[position].userColor), 0, _messages[position].userName.Length, SpanTypes.ExclusiveExclusive);
            //chatUserNameTextView.Text = _messages[position].userName;
            //chatUserNameTextView.SetTextColor(_messages[position].userColor); 
            //chatMessageTextTextView.TextFormatted = chatMessageSpannableString;
            chatMessageTextTextView.TextFormatted = Html.FromHtml(htmlChatRow);
            return row;
        }
    }
}