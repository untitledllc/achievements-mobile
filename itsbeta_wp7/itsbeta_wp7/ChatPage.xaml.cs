using Microsoft.Phone.Controls;
using itsbeta_wp7.ViewModel;
using PubNubMessaging.Core;
using System.Windows;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace itsbeta_wp7
{
    /// <summary>
    /// Description for ActivatePage.
    /// </summary>
    public partial class ChatPage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the ActivatePage class.
        /// </summary>
        public ChatPage()
        {
            InitializeComponent();
        }

        private void SendTextButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string publishedMessage = this.TestOutput.Text;
            pubnub.Publish<string>(ViewModelLocator.MainStatic.CurrentAchieve.Api_name, publishedMessage, PubnubCallbackResult);
            Messages.Add(string.Format("messaage: {0}", publishedMessage));
        }

        Pubnub pubnub;
        string channel = "";
        bool ssl = false;
        string SubscribeKey = "sub-c-fc890f04-a2a8-11e2-a387-12313f022c90";
        string PublishKey = "pub-c-a920b269-83ea-4c11-bd99-726d17dd1966";
        string secretKey = "sec-c-ZjEyOTZiZjQtOTI2ZC00NzA1LWI3N2MtMThhYzRiZjdlYzA1";
        string cipherKey = "";
        string uuid = ViewModelLocator.UserStatic.FacebookId;
        bool resumeOnReconnect = false;

        int subscribeTimeoutInSeconds;
        int operationTimeoutInSeconds;
        int networkMaxRetries;
        int networkRetryIntervalInSeconds;
        int heartbeatIntervalInSeconds;

        private ObservableCollection<string> _messages = new ObservableCollection<string>();
        public ObservableCollection<string> Messages
        {
            set
            {
                _messages = value;
            }
            get
            {
                return _messages;
            }
        }

        private void PhoneApplicationPage_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MessagesList.ItemsSource = Messages;

            pubnub = new Pubnub(PublishKey, SubscribeKey, secretKey, cipherKey, ssl);
            pubnub.SessionUUID = uuid;
            /*pubnub.SubscribeTimeout = subscribeTimeoutInSeconds;
            pubnub.NonSubscribeTimeout = operationTimeoutInSeconds;
            pubnub.NetworkCheckMaxRetries = networkMaxRetries;
            pubnub.NetworkCheckRetryInterval = networkRetryIntervalInSeconds;
            pubnub.HeartbeatInterval = heartbeatIntervalInSeconds;
            pubnub.EnableResumeOnReconnect = resumeOnReconnect;*/

            pubnub.Subscribe<string>(ViewModelLocator.MainStatic.CurrentAchieve.Api_name, PubnubCallbackResult, PubnubConnectCallbackResult);
        }

        private void PubnubCallbackResult(string result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    Messages.Add(result);
                }
                );
        }

        private void PubnubConnectCallbackResult(string result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    Messages.Add(result);
                }
                );
        }

    }
}