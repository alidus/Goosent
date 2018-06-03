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
using Android.Support.V4.App;
using Java.Lang;

namespace Goosent
{
    class TabsPagerFragmentAdapter : FragmentPagerAdapter
    {
        public Dictionary<int, BaseTabFragment> tabs;
        private Context context;

        public TabsPagerFragmentAdapter(Android.Support.V4.App.FragmentManager fm, Context context) : base(fm)
        {
            this.context = context;
            InitTabsDict(context);
        }

        
        public override int Count
        {
            get { return tabs.Count; }
        }

        // Show tab labels

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            ICharSequence convertedString = new Java.Lang.String(tabs[position].GetTitle());
            return convertedString;
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {

            return tabs[position];
        }

        private void InitTabsDict(Context context)
        {
            tabs = new Dictionary<int, BaseTabFragment>();
            tabs.Add(0, FragmentChat.getInstance(context));
            tabs.Add(1, FragmentEditSets.getInstance(context));
            //tabs.Add(2, FragmentEditSets.getInstance(context));
        }

    }
}