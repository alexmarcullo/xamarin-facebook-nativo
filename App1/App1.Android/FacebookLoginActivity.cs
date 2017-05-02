using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Places;
using Xamarin.Facebook.Places.Model;
using static Xamarin.Facebook.GraphRequest;

[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
[assembly: Permission(Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]
namespace App1.Droid
{
    [Activity(Label = "FacebookLoginActivity")]
    public class FacebookLoginActivity : Activity
    {
        ICallbackManager callbackManager;
        IFacebookCallback loginCallback;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FacebookSdk.SdkInitialize(this);
            callbackManager = CallbackManagerFactory.Create();

            var loginCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = loginResult =>
                {

                    var graphRequestCallback = new FacebookGraphCallback<GraphResponse>
                    {
                        HandleCompleted = graph =>
                        {
                            // TODO : SUCCESS
                            Intent mainIntent = new Intent(this, typeof(MainActivity));
                            mainIntent.PutExtra("facebookToken", loginResult.AccessToken.Token);
                            SetResult(Result.FirstUser, mainIntent);
                            Finish();
                        }
                    };
                    var gr = GraphRequest.NewMeRequest(loginResult.AccessToken, graphRequestCallback);
                    Bundle parameters = new Bundle();
                    parameters.PutString("fields", "id,name,link,email,gender,birthday");
                    gr.Parameters = parameters;
                    gr.ExecuteAsync();
                },
                HandleCancel = () =>
                {
                    // TODO : CANCEL
                },
                HandleError = loginError =>
                {
                    // TODO : ERROR
                }
            };

            if (AccessToken.CurrentAccessToken != null)
                LoginManager.Instance.LogOut();

            LoginManager.Instance.RegisterCallback(callbackManager, loginCallback);
            LoginManager.Instance.LogInWithReadPermissions(this, new[] { "email", "public_profile", "user_birthday" });
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }

    class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
    {
        public Action HandleCancel { get; set; }
        public Action<FacebookException> HandleError { get; set; }
        public Action<TResult> HandleSuccess { get; set; }

        public void OnCancel()
        {
            var c = HandleCancel;
            if (c != null)
                c();
        }

        public void OnError(FacebookException error)
        {
            var c = HandleError;
            if (c != null)
                c(error);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var c = HandleSuccess;
            if (c != null)
                c(result.JavaCast<TResult>());
        }
    }

    class FacebookGraphCallback<TResult> : Java.Lang.Object, IGraphJSONObjectCallback where TResult : Java.Lang.Object
    {
        public Action<TResult> HandleCompleted { get; set; }

        public void OnCompleted(JSONObject result, GraphResponse result2)
        {
            var c = HandleCompleted;
            if (c != null)
                c(result2.JavaCast<TResult>());
        }
    }
}