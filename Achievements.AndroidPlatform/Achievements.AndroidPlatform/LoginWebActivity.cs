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
using System.IO;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginWebActivity : Activity
    {
        static ProgressDialog mDialog;
        public static bool isPlayerExist;
        public static WebView loginWebView;
        static TextView endlogin;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.LoginWebLayout);
            endlogin = new TextView(this);

            loginWebView = FindViewById<WebView>(Resource.Id.loginWebView);
            //loginWebView.Settings.JavaScriptEnabled = true;

            loginWebView.SetWebViewClient(new ItsbetaLoginWebViewClient(this));

            loginWebView.LoadUrl(String.Format(
                    "https://www.facebook.com/dialog/oauth?response_type=token&display=popup&client_id={0}&redirect_uri={1}&scope={2}",
                    AppInfo._fbAppId, AppInfo._loginRedirectUri, AppInfo._fbScope));

            mDialog = new ProgressDialog(this);
            mDialog.SetMessage("Loading...");
            mDialog.SetCancelable(false);
            mDialog.Show();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            endlogin.TextChanged += delegate //здесь инициализировать все необходимое перед запуском...
            {
                AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId);


                if (!File.Exists(@"/data/data/itsbeta.achievements/data.txt"))
                {
                    List<string> config = new List<string>();

                    config.Add(AppInfo._user.Fullname);
                    config.Add(AppInfo._user.BirthDate);
                    config.Add(AppInfo._user.FacebookUserID);
                    config.Add(AppInfo._user.ItsBetaUserId);

                    File.WriteAllLines(@"/data/data/itsbeta.achievements/data.txt", config.ToArray(), Encoding.UTF8);
                }


                Finish();
                StartActivity(typeof(FirstBadgeActivity));
            };
        }

        class ItsbetaLoginWebViewClient : WebViewClient
        {
            LoginWebActivity _parent;
            public ItsbetaLoginWebViewClient(LoginWebActivity parent)
            {
                _parent = parent;
            }

            bool loadPreviousState = false;
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                if (url.StartsWith(AppInfo._loginRedirectUri))
                {
                    Regex access_tokenRegex = new Regex("access_token=(.*)&");
                    var v = access_tokenRegex.Match(url);
                    AppInfo._fbAccessToken = v.Groups[1].ToString();
                    
                    var jSonResponse = WebControls.GetMethod2("https://graph.facebook.com/me?fields=id,name,birthday&access_token=" + AppInfo._fbAccessToken);
                    var jSonUserFb = JsonArray.Parse(jSonResponse);

                    AppInfo._user.FacebookUserID = jSonUserFb["id"];
                    AppInfo._user.Fullname = jSonUserFb["name"];
                    AppInfo._user.BirthDate = jSonUserFb["birthday"];

                    ServiceItsBeta itsbetaService = new ServiceItsBeta();

                    isPlayerExist = itsbetaService.GetPlayerExistBool(AppInfo._user.FacebookUserID);

                    itsbetaService.PostToFbOnce("059db4f010c5f40bf4a73a28222dd3e3", "other", "itsbeta", "itsbeta",
                                AppInfo._user.FacebookUserID, AppInfo._fbAccessToken);

                    AppInfo._user.ItsBetaUserId = itsbetaService.GetItsBetaUserID(AppInfo._user.FacebookUserID);

                    endlogin.Text = "change";
                }
                view.LoadUrl(url);
                return true;
            }

            
            public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
            {
                loadPreviousState = true;
                base.OnPageStarted(view, url, favicon);
            }

            public override void OnPageFinished(WebView view, string url)
            {
                if (loadPreviousState == true)
                {
                    mDialog.Hide();
                }
                base.OnPageFinished(view, url);
            }
        }
    }
}