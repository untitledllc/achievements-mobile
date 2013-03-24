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
        //static ProgressDialog mDialog;
        public static bool isPlayerExist;
        public static bool isAppBadgeEarned;
        public static bool isRelogin = true;
        static WebView _loginWebView;
        //static AlertDialog.Builder _messageDialogBuilder;
        //static AlertDialog _messageDialog;
        static TextView _endlogin;
        static TextView _loginError; 
        static Context _context;

        static ProgressDialog _progressDialog;
        static TextView _progressDialogMessage;


        static Dialog _errorDialog;
        static TextView _errorDialogTitle;
        static TextView _errorDialogMessage;
        static Button _errorDialogReadyButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _context = this;
            _loginError = new TextView(this);

            SetContentView(Resource.Layout.loginweblayout);
            _endlogin = new TextView(this);
            //_messageDialogBuilder = new AlertDialog.Builder(this);
            _loginWebView = FindViewById<WebView>(Resource.Id.loginWebView);
            _loginWebView.Settings.SavePassword = false;
            _loginWebView.Settings.JavaScriptEnabled = true;
            _loginWebView.Settings.PluginsEnabled = true;
            _loginWebView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            _loginWebView.SaveEnabled = false;
            _loginWebView.SetWebViewClient(new ItsbetaLoginWebViewClient(this));
            _loginWebView.SetWebChromeClient(new ItsbetaLoginWebViewChromeClient());
            // "https://www.facebook.com/dialog/oauth?response_type=token&display=popup&client_id={0}&redirect_uri={1}&scope={2}",
            _loginWebView.LoadUrl(String.Format(
                "https://m.facebook.com/dialog/oauth/?response_type=token&" +
                                                    "client_id={0}"+
                                                    "&redirect_uri={1}" +
                                                    "&scope={2}",
                    //"https://www.facebook.com/dialog/oauth/?response_type=token&display=popup&client_id={0}&redirect_uri={1}&scope={2}",
                    AppInfo._fbAppId, AppInfo._loginRedirectUri, AppInfo._fbScope));


            RelativeLayout progressDialogRelativeLayout = new RelativeLayout(this);
            LayoutInflater progressDialoglayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            View progressDialogView = progressDialoglayoutInflater.Inflate(Resource.Layout.progressdialoglayout, null);
            _progressDialogMessage = (TextView)progressDialogView.FindViewById(Resource.Id.progressDialogMessageTextView);
            progressDialogRelativeLayout.AddView(progressDialogView);
            _progressDialog = new ProgressDialog(this, Resource.Style.FullHeightDialog);
            _progressDialog.Show();
            _progressDialog.SetContentView(progressDialogRelativeLayout);
            _progressDialog.Dismiss();

            
            _progressDialog.Show();
            _progressDialog.SetCanceledOnTouchOutside(false);


            /////

            RelativeLayout errorDialogRelativeLayout = new RelativeLayout(this);
            LayoutInflater errorDialoglayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            View errorDialogView = errorDialoglayoutInflater.Inflate(Resource.Layout.wrongcodedialoglayout, null);
            _errorDialogReadyButton = (Button)errorDialogView.FindViewById(Resource.Id.readyButton);
            _errorDialogTitle = (TextView)errorDialogView.FindViewById(Resource.Id.textView1);
            _errorDialogMessage = (TextView)errorDialogView.FindViewById(Resource.Id.textView2);

            errorDialogRelativeLayout.AddView(errorDialogView);
            _errorDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            _errorDialog.SetTitle("");
            _errorDialog.SetContentView(errorDialogRelativeLayout);
            
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _endlogin.TextChanged += delegate //здесь инициализировать все необходимое перед запуском...
            {
                Finish();
                StartActivity(typeof(FirstBadgeActivity));
            };
            _loginError.TextChanged += delegate { Finish(); StartActivity(typeof(LoginActivity)); };
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
            public override void OnReceivedError(WebView view, ClientError errorCode, string description, string failingUrl)
            {
               // base.OnReceivedError(view, errorCode, description, failingUrl);

                _errorDialogTitle.Text = "Ошибка";
                _errorDialogMessage.Text = "Не удалось подключиться. Проверьте состояние интернет подключения.";
                if (!AppInfo.IsLocaleRu)
                {
                    _errorDialogTitle.Text = "Error";
                    _errorDialogMessage.Text = "Internet connection is missing.";
                }


                _errorDialogReadyButton.Click += delegate { LoginWebActivity._loginError.Text = description; };

                ShowAlertDialog();
                _progressDialog.Dismiss();
            }

            void ShowAlertDialog()
            {
                _loginWebView.Visibility = ViewStates.Gone;
                _errorDialog.Show();
            }

            public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
            {
                base.OnPageStarted(view, url, favicon);

                if (url.StartsWith(AppInfo._loginRedirectUri))
                {
                    _progressDialogMessage.Text = "Авторизация...";
                    if (!AppInfo.IsLocaleRu)
                    {
                        _progressDialogMessage.Text = "Authorization...";
                    }
                    _loginWebView.Visibility = ViewStates.Gone;
                }
                if (url.Contains("//m.facebook.com/home.php?_rdr"))
                {
                    _loginWebView.Visibility = ViewStates.Invisible;
                    CookieSyncManager.CreateInstance(_parent);
                    CookieManager cookieManager = CookieManager.Instance;
                    cookieManager.RemoveAllCookie();

                    _loginWebView.ClearHistory();
                    _loginWebView.Dispose();
                    _parent.Finish();
                    _parent.StartActivity(typeof(LoginActivity));
                }
                else
                {
                    _progressDialogMessage.Text = "Загрузка...";
                    if (!AppInfo.IsLocaleRu)
                    {
                        _progressDialogMessage.Text = "Loading...";
                    }
                }
                
                _progressDialog.Show();
            }

            public void  AsyncAuth()
            {
                string jSonResponse = "null";
                try
                {
                    jSonResponse = WebControls.GetMethod2("https://graph.facebook.com/me?fields=id,name,birthday,locale,location&access_token=" + AppInfo._fbAccessToken);
                }
                catch
                {
                    _errorDialogTitle.Text = "Ошибка";
                    _errorDialogMessage.Text = "Необходима авторизация приложения.";
                    if (!AppInfo.IsLocaleRu)
                    {
                        _errorDialogTitle.Text = "Error";
                        _errorDialogMessage.Text = "App authorization required";
                    }
                    _errorDialogReadyButton.Click += delegate { ItsbetaLoginWebViewClient.loadPreviousState = false; _loginWebView.ClearHistory(); _parent.Finish(); _parent.StartActivity(typeof(LoginActivity)); };
                    
                    
                    _parent.RunOnUiThread (()=>ShowAlertDialog());
                    _progressDialog.Dismiss();
                    return;
                }
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
                    AppInfo._user.City = "Unknown city";
                }

                ServiceItsBeta itsbetaService = new ServiceItsBeta();

                isPlayerExist = itsbetaService.GetPlayerExistBool(AppInfo._user.FacebookUserID);

                isAppBadgeEarned = itsbetaService.IsPostToFbOnce("059db4f010c5f40bf4a73a28222dd3e3", "other", "itsbeta", "itsbeta",
                            AppInfo._user.FacebookUserID, AppInfo._fbAccessToken);


                try
                {
                    AppInfo._user.ItsBetaUserId = itsbetaService.GetItsBetaUserID(AppInfo._user.FacebookUserID);
                }
                catch (Exception)
                {

                }

                _parent.RunOnUiThread(()=> _endlogin.Text = "change");
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
                        _progressDialog.Hide();
                    }
                }
            }
        }
    }
}