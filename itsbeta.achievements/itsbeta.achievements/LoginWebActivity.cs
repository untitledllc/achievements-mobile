using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Webkit;
using System.Text.RegularExpressions;
using ItsBeta.WebControls;
using ItsBeta.Core;
using System.Threading;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginWebActivity : Activity
    {
        static ProgressDialog mDialog;
        public static bool isPlayerExist;
        public static bool isAppBadgeEarned;
        public static bool isRelogin = true;

        static TextView endlogin;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.LoginWebLayout);
            endlogin = new TextView(this);

            WebView loginWebView = FindViewById<WebView>(Resource.Id.loginWebView);
            //loginWebView.Settings.JavaScriptEnabled = true;

            loginWebView.SetWebViewClient(new ItsbetaLoginWebViewClient(this));
            loginWebView.SetWebChromeClient(new ItsbetaLoginWebViewChromeClient());
            // "https://www.facebook.com/dialog/oauth?response_type=token&display=popup&client_id={0}&redirect_uri={1}&scope={2}",
            loginWebView.LoadUrl(String.Format(
                "https://m.facebook.com/dialog/oauth/?response_type=token&" +
                                                    "client_id={0}"+
                                                    "&redirect_uri={1}" +
                                                    "&scope={2}",
                    //"https://www.facebook.com/dialog/oauth/?response_type=token&display=popup&client_id={0}&redirect_uri={1}&scope={2}",
                    AppInfo._fbAppId, AppInfo._loginRedirectUri, AppInfo._fbScope));

            mDialog = new ProgressDialog(this);
            mDialog.SetMessage("Загрузка...");
            mDialog.SetCancelable(false);
            mDialog.Show();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            endlogin.TextChanged += delegate //здесь инициализировать все необходимое перед запуском...
            {
                Finish();
                StartActivity(typeof(FirstBadgeActivity));
            };
        }

        public class ItsbetaLoginWebViewClient : WebViewClient
        {
            LoginWebActivity _parent;
            public ItsbetaLoginWebViewClient(LoginWebActivity parent)
            {
                _parent = parent;
            }

            public static bool loadPreviousState = false;
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                if (url.StartsWith(AppInfo._loginRedirectUri))
                {
                    isRelogin = false;
                    //mDialog.Show();
                    Regex access_tokenRegex = new Regex("access_token=(.*)&");
                    var v = access_tokenRegex.Match(url);
                    AppInfo._fbAccessToken = v.Groups[1].ToString();

                    ThreadStart threadStart = new ThreadStart(AsyncAuth);
                    Thread loadThread = new Thread(threadStart);
                    loadThread.Start();
                    loadPreviousState = true;
                }
                view.LoadUrl(url);
                return true;
            }

            public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);

                if (url.StartsWith(AppInfo._loginRedirectUri))
                {
                    mDialog.SetMessage("Авторизация пользователя...");
                }
                else
                {
                    mDialog.SetMessage("Загрузка...");
                }
                mDialog.Show();
            }


            public void  AsyncAuth()
            {
                var jSonResponse = WebControls.GetMethod2("https://graph.facebook.com/me?fields=id,name,birthday,locale,location&access_token=" + AppInfo._fbAccessToken);
                var jSonUserFb = JsonArray.Parse(jSonResponse);

                try
                {
                    AppInfo._user.FacebookUserID = jSonUserFb["id"];
                }
                catch
                {

                }
                try
                {
                    AppInfo._user.Fullname = jSonUserFb["name"];
                }
                catch
                {

                }
                try
                {
                    AppInfo._user.BirthDate = jSonUserFb["birthday"];
                }
                catch
                {
                    AppInfo._user.BirthDate = "null";
                }
                try
                {
                    AppInfo._user.City = jSonUserFb["location"]["name"];
                }
                catch
                {
                    AppInfo._user.City = "Unknown";
                }

                ServiceItsBeta itsbetaService = new ServiceItsBeta();

                isPlayerExist = itsbetaService.GetPlayerExistBool(AppInfo._user.FacebookUserID);

                isAppBadgeEarned = itsbetaService.IsPostToFbOnce("059db4f010c5f40bf4a73a28222dd3e3", "other", "itsbeta", "itsbeta",
                            AppInfo._user.FacebookUserID, AppInfo._fbAccessToken);

                AppInfo._user.ItsBetaUserId = itsbetaService.GetItsBetaUserID(AppInfo._user.FacebookUserID);
                

                endlogin.Text = "change";
            }
        }

        class ItsbetaLoginWebViewChromeClient : WebChromeClient
        {
            public override void OnProgressChanged(WebView view, int newProgress)
            {
                base.OnProgressChanged(view, newProgress);
                if (newProgress == 100)
                {
                    if (ItsbetaLoginWebViewClient.loadPreviousState)
                    {

                    }
                    else
                    {
                        mDialog.Hide();
                    }
                }
            }
        }
    }
}