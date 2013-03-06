﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Facebook;
using itsbeta_wp7.ViewModel;

namespace facebook_windows_phone_sample.Pages
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        private const string AppId = "264918200296425";

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me, read_stream, publish_stream, publish_actions, email, user_birthday";

        private readonly FacebookClient _fb = new FacebookClient();

        public FacebookLoginPage()
        {
            InitializeComponent();
        }

        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                /*if (ViewModelLocator.UserStatic.LogOut == false)
                {
                    NavigationService.GoBack();
                }
                else
                {*/
                    if (ViewModelLocator.UserStatic.FacebookId != "")
                    {
                        webBrowser1.Navigate(new Uri("https://www.facebook.com/logout.php?access_token=" + ViewModelLocator.UserStatic.FacebookToken + "&confirm=1&next=http://itsbeta.com/ru/"));
                        //NavigationService.GoBack();
                    }
                    else
                    {
                        var loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
                        webBrowser1.Navigate(loginUrl);
                    };
                //};
            }
            catch { };


        }

        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return _fb.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (e.Uri.AbsolutePath == "/ru/")
            {
                try
                {
                    ViewModelLocator.UserStatic.Cleanup();
                    ViewModelLocator.MainStatic.Cleanup();
                    //MessageBox.Show("Выход осуществлен.");
                    NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                }
                catch { };                
            }
            else
            {
                if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
                {
                    return;
                }

                if (oauthResult.IsSuccess)
                {
                    var accessToken = oauthResult.AccessToken;
                    LoginSucceded(accessToken);
                }
                else
                {
                    // user cancelled
                    MessageBox.Show(oauthResult.ErrorDescription);
                }
            };
        }

        private void LoginSucceded(string accessToken)
        {
            var fb = new FacebookClient(accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var id = (string)result["id"];

                var url = string.Format("/FacebookPages/FacebookInfoPage.xaml?access_token={0}&id={1}", accessToken, id);
                
                Dispatcher.BeginInvoke(() => {
                    ViewModelLocator.UserStatic.FacebookId = id;
                    ViewModelLocator.UserStatic.FacebookToken = accessToken;
                    ViewModelLocator.UserStatic.GetFBUserInfo();
                    //ViewModelLocator.UserStatic.GetPlayerId();
                    ViewModelLocator.UserStatic.GetItsbetaAchieve();
                    

                    NavigationService.Navigate(new Uri("/PanoramaPage.xaml", UriKind.Relative));
                });
            };

            fb.GetAsync("me");

        }
    }
}