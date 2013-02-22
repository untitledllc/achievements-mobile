using Microsoft.Phone.Controls;
using System;
using itsbeta_wp7.ViewModel;

namespace itsbeta_wp7
{
    /// <summary>
    /// Description for LoginPage.
    /// </summary>
    public partial class LoginPage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the LoginPage class.
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/FacebookPages/FacebookLoginPage.xaml", UriKind.Relative));
            }
            catch
            {
            };
        }

        private void PhoneApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                try
                {
                    if (NavigationService.CanGoBack)
                    {
                        while (NavigationService.RemoveBackEntry() != null)
                        {
                            NavigationService.RemoveBackEntry();
                        };
                    };
                }
                catch { };
            }
            catch { };
        }
    }
}