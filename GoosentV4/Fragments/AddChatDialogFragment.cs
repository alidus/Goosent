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

namespace Goosent.Fragments
{
    public class AddChatDialogFragment : DialogFragment
    {
        Spinner spinner;
        Button submitButton;
        EditText channelNameEditText;
        string _setTitle;
        int _setIndex;

        Context _context;
        public static AddChatDialogFragment GetInstance(int setindex)
        {
            AddChatDialogFragment fragment = new AddChatDialogFragment();
            Bundle args = new Bundle();
            args.PutInt("set_index", setindex);
            fragment.Arguments = args;

            return fragment;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            _context = ((MainActivity)Activity);
            _setIndex = Arguments.GetInt("set_index");
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            LayoutInflater inflater = Activity.LayoutInflater;
            View view = inflater.Inflate(Resource.Layout.AddChatDialogLayout, null);

            spinner = (Spinner)view.FindViewById(Resource.Id.addChat_platform_spinner);
            submitButton = (Button)view.FindViewById(Resource.Id.addChannel_submit_button);
            channelNameEditText = (EditText)view.FindViewById(Resource.Id.addChat_channelName_editText);

            submitButton.Click += SubmitButton_Click;

            Adapters.AvalibleStreamingPlatformsArrayAdapter spinnerAdapter = new Adapters.AvalibleStreamingPlatformsArrayAdapter(_context, _context.Resources.GetStringArray(Resource.Array.avalible_steaming_platforms).ToList<string>());
            spinner.Adapter = spinnerAdapter;

            builder.SetView(view);

            TextView customTitle = new TextView(_context);
            customTitle.Text = "Добавить чат в сет '" + ((MainActivity)_context).SetsList.GetSetsList[_setIndex].Name + "'";
            customTitle.SetTextSize(ComplexUnitType.Dip, 20);
            customTitle.Gravity = GravityFlags.Center;

            builder.SetCustomTitle(customTitle);

            return builder.Create();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // Попробовать добавить чат
            string channelName = channelNameEditText.Text;
            DATA.Platforms channelPlatform = (DATA.Platforms)spinner.SelectedItemId;
            if (IsChannelNotInSet(channelName) && IsChannelExist(channelName))
            {
                ((MainActivity)_context).AddChannel(_setIndex, new Channel(channelName, channelPlatform));
                Dismiss();
            }
        }

        private bool IsChannelNotInSet(string channelName)
        {
            foreach (Channel channel in ((MainActivity)_context).SelectedSet.Channels)
            {
                if (channel.Name == channelName)
                {

                    return false;
                }
            }

            return true;
        }

        private bool IsChannelExist(string channelName)
        {
            //TODO: Проверка с помощью API платформы существует ли канал с таким именем
            return true;
        }
    }
}