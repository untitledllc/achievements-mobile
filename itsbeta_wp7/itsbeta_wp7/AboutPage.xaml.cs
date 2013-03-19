using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace itsbeta_wp7
{
    /// <summary>
    /// Description for AboutPage.
    /// </summary>
    public partial class AboutPage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the AboutPage class.
        /// </summary>
        public AboutPage()
        {
            InitializeComponent();
        }

        private void rate_app_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                MarketplaceReviewTask task = new MarketplaceReviewTask();
                task.Show();
            }
            catch { };
        }

        private void send_email_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                EmailComposeTask emailcomposer = new EmailComposeTask();
                emailcomposer.To = "info@itsbeta.com";
                emailcomposer.Subject = "Itsbeta - Windows Phone application";
                emailcomposer.Body = "";
                emailcomposer.Show();
            }
            catch { };
        }
    }
}