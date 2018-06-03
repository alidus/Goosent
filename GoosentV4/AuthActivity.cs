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
using Android.Support.Design.Widget;
using Android.Text.Method;
using System.Threading.Tasks;

namespace Goosent
{
    [Activity(Label = "AuthActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AuthActivity : Android.Support.V7.App.AppCompatActivity
    {
        Button buttonSignIn;
        Button buttonSignUp;
        Button buttonSkip;
        TextInputEditText emailInputField;
        TextInputEditText passwordInputField;
        ServerManager sm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Authorization);
            sm = new ServerManager();
            buttonSignIn = (Button)FindViewById(Resource.Id.authSignInButton);
            buttonSignUp = (Button)FindViewById(Resource.Id.authSignUpButton);
            buttonSkip = (Button)FindViewById(Resource.Id.authSkipButton);

            emailInputField = (TextInputEditText)FindViewById(Resource.Id.authEmailField);
            passwordInputField = (TextInputEditText)FindViewById(Resource.Id.authPasswordField);
            passwordInputField.TransformationMethod = new PasswordTransformationMethod();
            passwordInputField.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationPassword;

            //Debug
            emailInputField.Text = "test@test.com";
            passwordInputField.Text = "testtest";

            buttonSkip.Click += ButtonSkip_Click;
            buttonSignIn.Click += async delegate {
                sm.SetEmail(emailInputField.Text);
                sm.SetPassword(passwordInputField.Text);
                ServerManager.SignInResponseContainer response = await sm.SignIn();
                switch (response.response.answer)
                {
                    case "invalid_email":
                        Toast.MakeText(this, "Неправильно введен email", ToastLength.Short).Show();
                        break;
                    case "incorrect_password":
                        Toast.MakeText(this, "Неправильно введен пароль", ToastLength.Short).Show();
                        break;
                    case "authorization_completed":
                        Toast.MakeText(this, "Успешная авторизация", ToastLength.Short).Show();
                        Intent i = Intent;
                        i.PutExtra("uid", response.response.uid);
                        SetResult(Result.Ok, i);
                        Finish();
                        break;
                    default:
                        break;
                }
            };
            buttonSignUp.Click += async delegate {
                sm.SetEmail(emailInputField.Text);
                sm.SetPassword(passwordInputField.Text);
                ServerManager.SignInResponseContainer response = await sm.SignUp();
                switch (response.response.answer)
                {
                    case "bad_email":
                        Toast.MakeText(this, "Неправильно введен email", ToastLength.Short).Show();
                        break;
                    case "email_used":
                        Toast.MakeText(this, "Email уже используется", ToastLength.Short).Show();
                        break;
                    case "bad_pass":
                        Toast.MakeText(this, "Неподходящий пароль", ToastLength.Short).Show();
                        break;
                    case "registration_completed":
                        Toast.MakeText(this, "Успешная регистрация", ToastLength.Short).Show();
                        Intent i = Intent;
                        i.PutExtra("uid", response.response.uid);
                        SetResult(Result.Ok, i);
                        Finish();
                        break;
                    default:
                        break;
                }
            };
        }


        private void ButtonSkip_Click(object sender, EventArgs e)
        {
            
        }
    }
}