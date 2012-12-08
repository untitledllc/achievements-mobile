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
using Android.Webkit;
using Android.Content.PM;
using System.Net;
using System.IO;

namespace Achievements.Android
{
    [Activity(Label = "My Activity", Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginScreenActivity : Activity
    {
        public static TextView textChanged;
        bool _isCodeGotten = false;
        string _code = "Null";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.FacebookLoginLayout);

            textChanged = new TextView(this);

            //this is your Facebook App ID
            string clientId = "470780276293377";
            //this is your Secret Key
            string clientSecret = "7fd53fc2f4a1529a076abaa5ccdbccf6";
            string RedirectUri = "https://arcane-river-8945.herokuapp.com/";

            if (!_isCodeGotten)
            {
                WebView webView = FindViewById<WebView>(Resource.Id.webView1);
                textChanged.TextChanged += delegate
                {
                    if (webView.Url!=null)
                    {
                        _code = webView.Url.Replace("http://arcane-river-8945.herokuapp.com/?code=", "");
                        _isCodeGotten = true;
                        OnCreate(bundle);
                    }
                }; 
                string codeurl = string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=code&scope=publish_stream", clientId, RedirectUri);
                webView.SetWebViewClient(new MyWebViewClient());
                webView.Settings.BuiltInZoomControls = false;
                webView.Settings.SetSupportZoom(false);
                webView.Settings.SetSupportMultipleWindows(false);
                webView.Settings.JavaScriptEnabled = true;
                webView.Settings.PluginsEnabled = true;
                webView.Settings.SetPluginState(WebSettings.PluginState.On);
                webView.LoadUrl(codeurl);
            }

            if (_code != "Null")
            {
                //we have to request an access token from the following Uri
                string url = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}";
                //Create a webrequest to perform the request against the Uri
                WebRequest request = WebRequest.Create(string.Format(url, clientId, RedirectUri, clientSecret, _code));

                //read out the response as a utf-8 encoding and parse out the access_token
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader streamReader = new StreamReader(stream, encode);
                string accessToken = streamReader.ReadToEnd().Replace("access_token=", "");
                streamReader.Close();
                response.Close();

                //now that we have an access token, query the Graph Api for the JSON representation of the User
                string url2 = "https://graph.facebook.com/me?access_token={0}";

                //create the request to https://graph.facebook.com/me
                request = WebRequest.Create(string.Format(url2, accessToken));

                //Get the response
                response = request.GetResponse();
            }

        }

        class MyWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.LoadUrl(url);
                               
                return true;
            }

            public override void OnPageStarted(WebView view, string url, global::Android.Graphics.Bitmap favicon)
            {
                if (url.StartsWith("https://arcane-river-8945.herokuapp.com/?code="))
                {
                        LoginScreenActivity.textChanged.Text = "NewURL!";
                }
                base.OnPageStarted(view, url, favicon);
            }
        }
    }
}