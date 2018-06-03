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

namespace Goosent.Adapters
{
    class ChatsSpinnerArrayAdapter : BaseAdapter
    {
        private List<Channel> _channels;
        private Context mContext;

        public ChatsSpinnerArrayAdapter(Context context, List<Channel> channels)
        {
            mContext = context;
            _channels = channels;
        }

        public override int Count
        {
            get { return _channels.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.ChatsSpinnerRow, null, false);
            }

            TextView chatName = (TextView)row.FindViewById(Resource.Id.chatsSpinner_ChatName_TextView);
            chatName.Text = _channels[position].Name;

            return row;
        }


        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.ChatsSpinnerDropRow, null, false);
            }

            TextView chatName = (TextView)row.FindViewById(Resource.Id.chatsSpinner_ChatName_TextView);
            chatName.Text = _channels[position].Name;

            return row;
        }
    }
}