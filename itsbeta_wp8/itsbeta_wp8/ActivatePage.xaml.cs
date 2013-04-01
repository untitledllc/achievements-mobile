using Microsoft.Phone.Controls;
using itsbeta_wp7.ViewModel;
using System.Diagnostics;
using Windows.Networking.Proximity;
using System.Windows;

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
                //NavigationService.GoBack();
                this.Focus();
                ViewModelLocator.UserStatic.NeedActivate = true;
                ViewModelLocator.UserStatic.ActivateCode = this.ActivationCode.Text;
                NavigationService.GoBack();
                //ViewModelLocator.UserStatic.ActivateAchieve(this.ActivationCode.Text);                
            }
            catch { };
        }

        private void PhoneApplicationPage_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
        {
            /*try
            {
                ProximityDevice device = ProximityDevice.GetDefault();
                // Make sure NFC is supported
                if (device != null)
                {
                    long Id = device.SubscribeForMessage("Windows.SampleMessageType", messageReceived);
                    //Debug.WriteLine("Published Message. ID is {0}", Id);
                    MessageBox.Show("Published Message. ID is " + Id.ToString());
                }
            }
            catch { };*/
        }

        private void messageReceived(ProximityDevice sender, ProximityMessage message)
        {
            //Debug.WriteLine("Received from {0}:'{1}'", sender.DeviceId, message.DataAsString);
            MessageBox.Show("Received from " + sender.DeviceId.ToString() + " " + message.DataAsString);
        }

    }
}