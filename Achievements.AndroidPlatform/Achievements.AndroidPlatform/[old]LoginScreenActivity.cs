//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Android.Content.PM;
//using ItsBeta.Core;
//using Achievements.AndroidPlatform.ItsBeta.Core;
//using Com.Facebook.Android;
//using Java.Net;
//using Java.IO;
//using Android.Util;

//using System.Json;
//using Object = Java.Lang.Object;
//using Android.Graphics;

//namespace Achievements.AndroidPlatform
//{
    
//    [Activity(Label = "Achievements", MainLauncher = true,
//        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
//                ScreenOrientation = ScreenOrientation.Portrait)]
//    public class LoginScreenActivity : Activity
//    {
//        public static bool isPlayerExist = true;
//        public static User _user;
//        const String APP_ID = "264918200296425";

//        private LoginButton facebookSignInButton;
//        public static Facebook mFacebook;
//        public static AsyncFacebookRunner mAsyncRunner;
//        private ProgressDialog mDialog;

//        public static bool isNeedStoreClear = false;

//        public static Context loginContext;

//        public static TextView refreshOnCreate;

//        public static bool isCommingFromProfile = false;
        
//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);
            
//            SetContentView(Resource.Layout.LoginActivityLayout);

//            refreshOnCreate = new TextView(this);

//            loginContext = this;

//            mDialog = new ProgressDialog(this);
//            mDialog.SetMessage("Loading...");
//            mDialog.SetCancelable(false);


//            refreshOnCreate.TextChanged += delegate { OnCreate(bundle); mDialog.Dismiss(); };

//            facebookSignInButton = (LoginButton)FindViewById(Resource.Id.login);


//            mFacebook = new Facebook(APP_ID);

//            mAsyncRunner = new AsyncFacebookRunner(mFacebook);

//            if (isNeedStoreClear || isCommingFromProfile)
//            {
//                SessionStore.Clear(this);
                
//            }
//            else
//            {
//                SessionStore.Restore(mFacebook, this);
//            }
            
//            SessionEvents.AddAuthListener(new SampleAuthListener(this));
//            SessionEvents.AddLogoutListener(new SampleLogoutListener(this));

//            facebookSignInButton.Init(this, mFacebook, new string[] { "publish_actions", "publish_stream", "user_birthday" });

//            if (mFacebook.IsSessionValid && !isCommingFromProfile)
//            {
//                mDialog.Show();
//                mAsyncRunner.Request("me", new RequestUserInfo(this));
//            }

//        }

//        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
//        {
//            mFacebook.AuthorizeCallback(requestCode, (int)resultCode, data);
//        }

//        public class SampleAuthListener : SessionEvents.IAuthListener
//        {
//            public SampleAuthListener(LoginScreenActivity parent)
//            {
//                this.parent = parent;
//            }
//            LoginScreenActivity parent;

//            public void OnAuthSucceed()
//            {
//                if (isCommingFromProfile)
//                {
//                    parent.facebookSignInButton.Visibility = ViewStates.Visible;
//                    isCommingFromProfile = false;
//                }
//                else
//                {
//                    parent.RunOnUiThread(() =>
//                    {
//                        parent.facebookSignInButton.Visibility = ViewStates.Invisible;
//                        parent.mDialog.Show();
//                    });
//                }
//                mAsyncRunner.Request("me", new RequestUserInfo(parent));
//            }

//            public void OnAuthFail(String error)
//            {
//                Console.WriteLine("AuthFail");
//                //parent.mText.Text = ("Login Failed: " + error);
//            }
//        }

//        public class SampleLogoutListener : SessionEvents.ILogoutListener
//        {
//            public SampleLogoutListener(LoginScreenActivity parent)
//            {
//                this.parent = parent;
//            }
//            LoginScreenActivity parent;

//            public void OnLogoutBegin()
//            {
//                //parent.mText.Text = ("Logging out...");
//            }

//            public void OnLogoutFinish()
//            {
//                //parent.mText.Text = ("You have logged out! ");
//                //parent.mRequestButton.Visibility = (ViewStates.Invisible);
//                //parent.mUploadButton.Visibility = (ViewStates.Invisible);
//                //parent.mPostButton.Visibility = (ViewStates.Invisible);
//            }
//        }

//        protected override void  OnPause()
//        {
//             base.OnPause();

//             Finish();
//             OnDestroy();
//        }
//    }


//    public abstract class BaseRequestListener : Object, AsyncFacebookRunner.IRequestListener
//    {

//        public void OnFacebookError(FacebookError e, Object state)
//        {
//            Log.Error("Facebook", e.Message);
//            e.PrintStackTrace();
//        }

//        public void OnFileNotFoundException(FileNotFoundException e,
//                                        Object state)
//        {
//            Log.Error("Facebook", e.Message);
//            e.PrintStackTrace();
//        }

//        public void OnIOException(Java.IO.IOException e, Object state)
//        {
//            Log.Error("Facebook", e.Message);
//            e.PrintStackTrace();
//        }

//        public void OnMalformedURLException(MalformedURLException e,
//                                        Object state)
//        {
//            Log.Error("Facebook", e.Message);
//            e.PrintStackTrace();
//        }

//        public abstract void OnComplete(string response, Java.Lang.Object state);

//    }

//    public class LoginButton : ImageButton
//    {
//        private Facebook mFb;
//        private Handler mHandler;
//        private SessionListener mSessionListener;
//        private String[] mPermissions;
//        private Activity mActivity;

//        public LoginButton(Context context)
//            : base(context)
//        {
//            mSessionListener = new SessionListener(this);
//        }

//        public LoginButton(Context context, IAttributeSet attrs)
//            : base(context, attrs)
//        {
//            mSessionListener = new SessionListener(this);
//        }

//        public LoginButton(Context context, IAttributeSet attrs, int defStyle)
//            : base(context, attrs, defStyle)
//        {
//            mSessionListener = new SessionListener(this);
//        }

//        public void Init(LoginScreenActivity activity, Facebook fb)
//        {
//            Init(activity, fb, new String[] { });
//        }

//        public void Init(LoginScreenActivity activity, Facebook fb, String[] permissions)
//        {
//            mActivity = activity;
//            mFb = fb;
//            mPermissions = permissions;
//            mHandler = new Handler();

//            SetBackgroundColor(Color.Transparent);
//            SetAdjustViewBounds(true);

//            if (!fb.IsSessionValid)
//            {
//                SetImageResource(Resource.Drawable.singInFacebook);
//            }
//            else if(!LoginScreenActivity.isCommingFromProfile)
//            {
//                Visibility = ViewStates.Invisible;
//            }



//            DrawableStateChanged();

//            SessionEvents.AddAuthListener(mSessionListener);
//            SessionEvents.AddLogoutListener(mSessionListener);
//            SetOnClickListener(new ButtonOnClickListener(this));
//        }

//        class ButtonOnClickListener : Object, IOnClickListener
//        {
//            public ButtonOnClickListener(LoginButton parent)
//            {
//                this.parent = parent;
//            }
//            LoginButton parent;

//            public void OnClick(View arg0)
//            {
//                if (parent.mFb.IsSessionValid)
//                {
//                    SessionEvents.OnLogoutBegin();
//                    AsyncFacebookRunner asyncRunner = new AsyncFacebookRunner(parent.mFb);
//                    asyncRunner.Logout(parent.Context, new LogoutRequestListener(parent));
//                }
//                else
//                {
//                    parent.mFb.Authorize(parent.mActivity, parent.mPermissions,
//                              new LoginDialogListener());
//                }
//            }
//        }

//        class LoginDialogListener : Object, Facebook.IDialogListener
//        {
//            public void OnComplete(Bundle values)
//            {
//                SessionEvents.OnLoginSuccess();
//            }

//            public void OnFacebookError(FacebookError error)
//            {
//                SessionEvents.OnLoginError(error.Message);
//            }

//            public void OnError(DialogError error)
//            {
//                SessionEvents.OnLoginError(error.Message);
//            }

//            public void OnCancel()
//            {
//                SessionEvents.OnLoginError("Action Canceled");
//            }
//        }

//        public class LogoutRequestListener : BaseRequestListener
//        {
//            public LogoutRequestListener(Achievements.AndroidPlatform.LoginButton parent)
//            {
//                this.parent = parent;
//            }

//            LoginButton parent;

//            public override void OnComplete(String response, Object state)
//            {
//                // callback should be run in the original thread, 
//                // not the background thread
//                //parent.mHandler.Post(delegate
//                //{
//                //    SessionEvents.OnLogoutFinish();
//                //});
//            }
//        }

//        class SessionListener : Object, SessionEvents.IAuthListener, SessionEvents.ILogoutListener
//        {
//            public SessionListener(LoginButton parent)
//            {
//                this.parent = parent;
//            }

//            LoginButton parent;

//            public void OnAuthSucceed()
//            {
//                //parent.SetImageResource(Resource.Drawable.logout_button);
//                SessionStore.Save(parent.mFb, parent.Context);
//            }

//            public void OnAuthFail(String error)
//            {
//            }

//            public void OnLogoutBegin()
//            {

//            }

//            public void OnLogoutFinish()
//            {
//                SessionStore.Clear(parent.Context);
//                //parent.SetImageResource(Resource.Drawable.login_button);
//            }
//        }
//    }

//    public class SessionEvents
//    {

//        private static LinkedList<IAuthListener> mAuthListeners =
//        new LinkedList<IAuthListener>();
//        private static LinkedList<ILogoutListener> mLogoutListeners =
//        new LinkedList<ILogoutListener>();

//        public static void AddAuthListener(IAuthListener listener)
//        {
//            mAuthListeners.AddLast(listener);
//        }

//        public static void RemoveAuthListener(IAuthListener listener)
//        {
//            mAuthListeners.Remove(listener);
//        }

//        public static void AddLogoutListener(ILogoutListener listener)
//        {
//            mLogoutListeners.AddLast(listener);
//        }

//        /**
//     * Remove the given listener from the list of those that will be notified
//     * when logout occurs.
//     * 
//     * @param listener
//     *            The callback object for notifying the application when log out
//     *            starts and finishes.
//     */
//        public static void RemoveLogoutListener(ILogoutListener listener)
//        {
//            mLogoutListeners.Remove(listener);
//        }

//        public static void OnLoginSuccess()
//        {
//            foreach (var listener in mAuthListeners)
//            {
//                listener.OnAuthSucceed();
//            }
//        }

//        public static void OnLoginError(String error)
//        {
//            foreach (var listener in mAuthListeners)
//            {
//                listener.OnAuthFail(error);
//            }
//        }

//        public static void OnLogoutBegin()
//        {
//            foreach (var l in mLogoutListeners)
//            {
//                l.OnLogoutBegin();
//            }
//        }

//        public static void OnLogoutFinish()
//        {
//            foreach (var l in mLogoutListeners)
//            {
//                l.OnLogoutFinish();
//            }
//        }

//        /**
//     * Callback interface for authorization events.
//     *
//     */
//        public interface IAuthListener
//        {

//            /**
//         * Called when a auth flow completes successfully and a valid OAuth 
//         * Token was received.
//         * 
//         * Executed by the thread that initiated the authentication.
//         * 
//         * API requests can now be made.
//         */
//            void OnAuthSucceed();

//            /**
//         * Called when a login completes unsuccessfully with an error. 
//         *  
//         * Executed by the thread that initiated the authentication.
//         */
//            void OnAuthFail(String error);
//        }

//        /**
//     * Callback interface for logout events.
//     *
//     */
//        public interface ILogoutListener
//        {
//            /**
//         * Called when logout begins, before session is invalidated.  
//         * Last chance to make an API call.  
//         * 
//         * Executed by the thread that initiated the logout.
//         */
//            void OnLogoutBegin();

//            /**
//         * Called when the session information has been cleared.
//         * UI should be updated to reflect logged-out state.
//         * 
//         * Executed by the thread that initiated the logout.
//         */
//            void OnLogoutFinish();
//        }

//    }

//    public class SessionStore
//    {
//        const string TOKEN = "access_token";
//        const string EXPIRES = "expires_in";
//        const string KEY = "facebook-session";

//        public static bool Save(Facebook session, Context context)
//        {
//            var editor = context.GetSharedPreferences(KEY, FileCreationMode.Private).Edit();
//            editor.PutString(TOKEN, session.AccessToken);
//            editor.PutLong(EXPIRES, session.AccessExpires);
//            return editor.Commit();
//        }

//        public static bool Restore(Facebook session, Context context)
//        {
//            var savedSession = context.GetSharedPreferences(KEY, FileCreationMode.Private);
//            session.AccessToken = savedSession.GetString(TOKEN, null);
//            session.AccessExpires = savedSession.GetLong(EXPIRES, 0);
//            return session.IsSessionValid;
//        }

//        public static void Clear(Context context)
//        {
//            var editor = context.GetSharedPreferences(KEY, FileCreationMode.Private).Edit();
//            editor.Clear();
//            editor.Commit();
//        }
//    }

//    public class RequestUserInfo : BaseRequestListener
//    {
//        Achievements.AndroidPlatform.LoginScreenActivity _parent;

//        public RequestUserInfo(Achievements.AndroidPlatform.LoginScreenActivity parent)
//        {
//            _parent = parent;
//        }

//        public override void OnComplete(string response, Object state)
//        {
//            try
//            {
//                var json = (JsonObject)JsonValue.Parse(response);
//                LoginScreenActivity._user = new User();

//                if (!json.ContainsKey("name"))
//                {
//                    LoginScreenActivity.isNeedStoreClear = true;
//                    _parent.RunOnUiThread(() => LoginScreenActivity.refreshOnCreate.Text = "refresh");
//                    return;
//                }

//                LoginScreenActivity._user.Fullname = json["name"];
//                LoginScreenActivity._user.FacebookUserID = json["id"];
//                LoginScreenActivity._user.BirthDate = json["birthday"];

                

//                ServiceItsBeta itsbetaService = new ServiceItsBeta();

//                LoginScreenActivity.isPlayerExist = itsbetaService.GetPlayerExistBool(LoginScreenActivity._user.FacebookUserID);
                
//                itsbetaService.PostToFbOnce("059db4f010c5f40bf4a73a28222dd3e3", "other", "itsbeta", "itsbeta",
//                            LoginScreenActivity._user.FacebookUserID, LoginScreenActivity.mFacebook.AccessToken);

//                LoginScreenActivity._user.ItsBetaUserId = itsbetaService.GetItsBetaUserID(LoginScreenActivity._user.FacebookUserID);

//                _parent.Finish();
//                _parent.StartActivity(typeof(FirstBadgeActivity));
//            } 
//            catch (FacebookError e)
//            {
//                Log.Warn ("Facebook-Example", "Facebook Error: " + e.Message);
//                SessionEvents.OnLogoutBegin();
//            }
//        }
//    }


//}