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
using System.Threading;
using Android.Util;

namespace FlurryLib
{
    public class FlurryClient
    {
        public const string ApiKeyValue = "T3HNH9JVJS5YBXBY7Q7C";

        private readonly IntPtr _flurryClass;

        // SESSIONS
        private readonly IntPtr _flurryOnStartSession;
        private readonly IntPtr _flurryOnEndSession;
        private readonly IntPtr _flurrySetContinueSessionMillis;

        // SIMPLE EVENTS
        private readonly IntPtr _flurryLogEvent;
        private readonly IntPtr _flurryLogEventMap;

        // TIMED EVENTS
        private readonly IntPtr _flurryLogTimedEvent;
        private readonly IntPtr _flurryLogTimedEventMap;
        private readonly IntPtr _flurryEndTimedEvent;

        // ADDITIONAL USER INFORMATION
        private readonly IntPtr _flurrySetUserId;
        private readonly IntPtr _flurrySetAge;
        private readonly IntPtr _flurrySetGender;

        // LOCATION
        private readonly IntPtr _flurrySetReportLocation;

        // ERROR / LOG
        private readonly IntPtr _flurrySetLogEnabled;
        private readonly IntPtr _flurryOnError;

        // CONTINUE SESSION MILLIS


        public FlurryClient()
        {
            _flurryClass = JNIEnv.FindClass("com/flurry/android/FlurryAgent");

            // SESSIONS
            _flurryOnStartSession = JNIEnv.GetStaticMethodID(_flurryClass, "onStartSession", "(Landroid/content/Context;Ljava/lang/String;)V");
            _flurryOnEndSession = JNIEnv.GetStaticMethodID(_flurryClass, "onEndSession", "(Landroid/content/Context;)V");
            _flurrySetContinueSessionMillis = JNIEnv.GetStaticMethodID(_flurryClass, "setContinueSessionMillis", "(J)V");

            // SIMPLE EVENTS
            _flurryLogEvent = JNIEnv.GetStaticMethodID(_flurryClass, "logEvent", "(Ljava/lang/String;)V");
            _flurryLogEventMap = JNIEnv.GetStaticMethodID(_flurryClass, "logEvent", "(Ljava/lang/String;Ljava/util/Map;)V");

            // TIMED EVENTS
            _flurryLogTimedEvent = JNIEnv.GetStaticMethodID(_flurryClass, "logEvent", "(Ljava/lang/String;Z)V");
            _flurryLogTimedEventMap = JNIEnv.GetStaticMethodID(_flurryClass, "logEvent", "(Ljava/lang/String;Ljava/util/Map;Z)V");
            _flurryEndTimedEvent = JNIEnv.GetStaticMethodID(_flurryClass, "endTimedEvent", "(Ljava/lang/String;)V");

            // ADDITIONAL USER INFORMATION
            _flurrySetUserId = JNIEnv.GetStaticMethodID(_flurryClass, "setUserId", "(Ljava/lang/String;)V");
            _flurrySetAge = JNIEnv.GetStaticMethodID(_flurryClass, "setAge", "(I)V");
            _flurrySetGender = JNIEnv.GetStaticMethodID(_flurryClass, "setGender", "(B)V");

            // LOCATION
            _flurrySetReportLocation = JNIEnv.GetStaticMethodID(_flurryClass, "setReportLocation", "(Z)V");

            // ERRROR / LOG
            _flurrySetLogEnabled = JNIEnv.GetStaticMethodID(_flurryClass, "setLogEnabled", "(Z)V");
            _flurryOnError = JNIEnv.GetStaticMethodID(_flurryClass, "onError", "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)V");
        }

        #region SESSIONS
        public void OnStartActivity(Activity activity)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryOnStartSession, new JValue(activity), new JValue(new Java.Lang.String(ApiKeyValue))));
        }

        public void OnStopActivity(Activity activity)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryOnEndSession, new JValue(activity)));
        }

        public void setContinueSessionMillis(long millis)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurrySetContinueSessionMillis, new JValue(millis)));
        }
        #endregion

        #region SIMPLE EVENTS
        public void LogEvent(string eventName)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryLogEvent, new JValue(new Java.Lang.String(eventName))));
        }

        public void LogEvent(string eventName, Dictionary<string, string> parameters)
        {
            JavaDictionary<string, string> actualParams = new JavaDictionary<string, string>(parameters);
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryLogEventMap, new JValue(new Java.Lang.String(eventName)), new JValue(actualParams)));
        }
        #endregion

        #region TIMED EVENTS
        public void LogTimedEvent(string eventName, bool timed)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryLogTimedEvent, new JValue(new Java.Lang.String(eventName)), new JValue(timed)));
        }

        public void LogTimedEvent(string eventName, Dictionary<string, string> parameters, bool timed)
        {
            JavaDictionary<string, string> actualParams = new JavaDictionary<string, string>(parameters);
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryLogTimedEventMap, new JValue(new Java.Lang.String(eventName)), new JValue(actualParams), new JValue(timed)));
        }

        public void EndTimedEvent(string eventName)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryEndTimedEvent, new JValue(new Java.Lang.String(eventName))));
        }
        #endregion

        #region ADDITIONAL USER INFORMATION
        public void setUserId(string userId)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurrySetUserId, new JValue(new Java.Lang.String(userId))));
        }

        public void setAge(int age)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurrySetAge, new JValue(age)));
        }

        public void setGender(byte gender)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurrySetGender, new JValue(gender)));
        }
        #endregion

        #region LOCATION
        public void setReportLocation(bool enabled)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurrySetReportLocation, new JValue(enabled)));
        }
        #endregion

        #region ERROR / LOG
        public void setLogEnabled(bool enabled)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurrySetLogEnabled, new JValue(enabled)));
        }

        public void onError(string errorId, string message, string errorClass)
        {
            ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryOnError, new JValue(new Java.Lang.String(errorId)), new JValue(new Java.Lang.String(message)), new JValue(new Java.Lang.String(errorClass))));
        }
        #endregion

        #region HELPERS
        private static void ExceptionSafe(Action action)
        {
            try
            {
                action();
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                Log.Info("FlurryClient", "Exception seen in calling Flurry through JNI {0}", exception.ToString());
            }
        }
        #endregion

        ~FlurryClient()
        {
            JNIEnv.DeleteGlobalRef(_flurryClass);
        }
    }
}   