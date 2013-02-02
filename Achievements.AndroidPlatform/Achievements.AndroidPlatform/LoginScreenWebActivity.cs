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
//using Android.Webkit;
//using System.Net;
//using System.IO;
//using Android.Content.PM;
//using System.Diagnostics.Contracts;
//using Achievements.AndroidPlatform.ItsBeta.Core;
//using Com.Facebook.Android;

//namespace Achievements.AndroidPlatform
//{
//    [Activity(Label = "My Activity", 
//        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
//                ScreenOrientation = ScreenOrientation.Portrait)]
//    public class LoginScreenWebActivity : Activity
//    {
//        public static TextView textChanged;
//        bool _isCodeGotten = false;
//        public static string _urlWithCode = "Null";
//        string _code = "Null";
//        public static string _accessToken = "Null";
//        Facebook facebook;
//        AsyncFacebookRunner mAsyncRunner;

//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);


//            textChanged = new TextView(this);

//            //this is your Facebook App ID
//            string clientId = "470780276293377";
//            //this is your Secret Key
//            string clientSecret = "7fd53fc2f4a1529a076abaa5ccdbccf6";
//            string RedirectUri = "https://arcane-river-8945.herokuapp.com/";

//            facebook = new Facebook(clientId);

//            SetContentView(Resource.Layout.LoginActivityWebLayout);

//            string[] permissions = new string[]{
//                "user_about_me",
//                "user_activities",
//                "user_birthday",
//                "user_checkins",
//                "user_education_history",
//                "user_events",
//                "user_groups",
//                "user_hometown",
//                "user_interests",
//                "user_likes",
//                "user_location",
//                "user_notes",
//                "user_online_presence",
//                "user_photo_video_tags",
//                "user_photos",
//                "user_relationships",
//                "user_relationship_details",
//                "user_religion_politics",
//                "user_status",
//                "user_videos",
//                "user_website",
//                "user_work_history",
//                "email",

//                "read_friendlists",
//                "read_insights",
//                "read_mailbox",
//                "read_requests",
//                "read_stream",
//                "xmpp_login",
//                "ads_management",
//                "create_event",
//                "manage_friendlists",
//                "manage_notifications",
//                "offline_access",
//                "publish_checkins",
//                "publish_stream",
//                "rsvp_event",
//                "sms",
//                //"publish_actions",

//                "manage_pages"
//                };

//            LoginDialogListener loginDialogListener = new LoginDialogListener();

//            facebook.Authorize(this, permissions, loginDialogListener);

//            mAsyncRunner = new AsyncFacebookRunner(facebook);

//            SessionStore.Restore(facebook, this);
//            SessionEvents.AddAuthListener(new SampleAuthListener());
//            SessionEvents.AddLogoutListener(new SampleLogoutListener());

            


//        }

//        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
//        {
//            facebook.AuthorizeCallback(requestCode, (int)resultCode, data);
//        }

//        public class SampleAuthListener : SessionEvents.IAuthListener
//        {

//            public void OnAuthSucceed()
//            {
//            }

//            public void OnAuthFail(string error)
//            {
//            }
//        }

//        public class SampleLogoutListener : SessionEvents.ILogoutListener
//        {
//            public void OnLogoutBegin()
//            {
//            }

//            public void OnLogoutFinish()
//            {
//            }
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

//    public class SessionEvents
//    {

//        private static LinkedList<IAuthListener> mAuthListeners =
//        new LinkedList<IAuthListener>();
//        private static LinkedList<ILogoutListener> mLogoutListeners =
//        new LinkedList<ILogoutListener>();

//        /**
//     * Associate the given listener with this Facebook object. The listener's
//     * callback interface will be invoked when authentication events occur.
//     * 
//     * @param listener
//     *            The callback object for notifying the application when auth
//     *            events happen.
//     */
//        public static void AddAuthListener(IAuthListener listener)
//        {
//            mAuthListeners.AddLast(listener);
//        }

//        /**
//     * Remove the given listener from the list of those that will be notified
//     * when authentication events occur.
//     * 
//     * @param listener
//     *            The callback object for notifying the application when auth
//     *            events happen.
//     */
//        public static void RemoveAuthListener(IAuthListener listener)
//        {
//            mAuthListeners.Remove(listener);
//        }

//        /**
//     * Associate the given listener with this Facebook object. The listener's
//     * callback interface will be invoked when logout occurs.
//     * 
//     * @param listener
//     *            The callback object for notifying the application when log out
//     *            starts and finishes.
//     */
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

//    public class LoginDialogListener : Object, Facebook.IDialogListener
//    {
//        public void OnComplete(Bundle values)
//        {
//            LoginScreenWebActivity.textChanged.Text = "change";
//        }

//        public virtual void OnFacebookError(FacebookError error)
//        {
//            LoginScreenWebActivity.textChanged.Text = "changeer";
//        }

//        public virtual void OnError(DialogError error)
//        {
//            LoginScreenWebActivity.textChanged.Text = "changeer";
//        }

//        public virtual void OnCancel()
//        {
//            LoginScreenWebActivity.textChanged.Text = "changecan";
//        }z

//        public IntPtr Handle
//        {
//            get 
//            {
//                return new IntPtr();
//            }
//        }

//        public void Dispose()
//        {
//            LoginScreenWebActivity.textChanged.Text = "changedi";
//        }
//    }
//}
