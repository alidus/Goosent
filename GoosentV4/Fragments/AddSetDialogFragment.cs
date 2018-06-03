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
    public class AddSetDialogFragment : DialogFragment
    {
        Spinner spinner;
        Button submitButton;
        EditText setNameEditText;
        string _setTitle;

        Context _context;
        public static AddSetDialogFragment GetInstance()
        {
            AddSetDialogFragment fragment = new AddSetDialogFragment();
            //Bundle args = new Bundle();
            //fragment.Arguments = args;
            

            return fragment;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            _context = Activity;
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            LayoutInflater inflater = Activity.LayoutInflater;
            View view = inflater.Inflate(Resource.Layout.AddSetDialogLayout, null);

            submitButton = (Button)view.FindViewById(Resource.Id.addSet_submit_button);
            setNameEditText = (EditText)view.FindViewById(Resource.Id.addSet_setName_editText);

            submitButton.Click += SubmitButton_Click;

            builder.SetView(view);

            TextView customTitle = new TextView(_context);
            customTitle.Text = "Добавить сет";
            customTitle.SetTextSize(ComplexUnitType.Dip, 20);
            customTitle.Gravity = GravityFlags.Center;

            builder.SetCustomTitle(customTitle);

            return builder.Create();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // Попробовать добавить сет
            string setName = setNameEditText.Text;
            if (!((MainActivity)_context).IsSetExist(setName))
            {
                ((MainActivity)_context).AddSet(new ChannelsSet(setName));
                Dismiss();
            }
            else
            {
                Toast.MakeText(_context, "Unable to add set", ToastLength.Short).Show();
            }
        }
    }
}