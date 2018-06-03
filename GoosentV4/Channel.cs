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

namespace Goosent
{
    public class Channel
    {
        private string _name;
        private DATA.Platforms _platform;

        public Channel(string name, DATA.Platforms platform)
        {
            _name = name;
            _platform = platform;
        }

        public string Name
        {
            get { return _name; }
        }

        public DATA.Platforms Platform
        {
            get { return _platform; }
        }
    }
}