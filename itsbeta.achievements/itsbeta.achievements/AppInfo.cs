using System;
using System.Collections.Generic;
using System.Text;
using ItsBeta.Core;
using Android.Views;

namespace itsbeta.achievements
{
    public static class AppInfo
    {
        public static User _user = new User();

        public static string _access_token = "059db4f010c5f40bf4a73a28222dd3e3";

        public static string _fbAppId = "264918200296425";
        public static string _fbScope = "publish_stream,publish_actions,user_birthday,user_location";
        public static string _loginRedirectUri = "https://www.facebook.com/connect/login_success.html";

        public static string _fbAccessToken;

        public static Achieves _achievesInfo;

        public static Dictionary<string, bool> _selectedCategoriesDictionary = new Dictionary<string, bool>();
        public static Dictionary<string, bool> _selectedSubCategoriesDictionary = new Dictionary<string, bool>();

        public static Display _display;

        public static int _badgesCount;
        public static int _bonusesCount;
        public static int _subcategCount;
    }
}
