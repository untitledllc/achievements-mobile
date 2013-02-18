using Microsoft.Phone.Controls;
using itsbeta_wp7.ViewModel;

namespace itsbeta_wp7
{
    /// <summary>
    /// Description for ActivatePage.
    /// </summary>
    public partial class ActivatePage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the ActivatePage class.
        /// </summary>
        public ActivatePage()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                ViewModelLocator.UserStatic.ActivateAchieve(this.ActivationCode.Text);
                NavigationService.GoBack();
            }
            catch { };
        }
    }
}