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
    class AvalibleStreamingPlatformsArrayAdapter : BaseAdapter
    {
        private List<string> _platforms;
        private Context mContext;

        public AvalibleStreamingPlatformsArrayAdapter(Context context, List<string> platforms)
        {
            mContext = context;
            _platforms = platforms;
        }

        public override int Count
        {
            get { return _platforms.Count; }
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.PlatformsSpinnerRow, null, false);
            }

            TextView platformNameTextView = (TextView)row.FindViewById(Resource.Id.platformSpinner_platformName_TextView);
            platformNameTextView.Text = _platforms[position];

            return row;
        }


        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.PlatformsSpinnerDropRow, null, false);
            }

            TextView chatName = (TextView)row.FindViewById(Resource.Id.platformSpinner_platformName_TextView);
            chatName.Text = _platforms[position];

            return row;
        }
    }
}