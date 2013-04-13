using Microsoft.Phone.Controls;
using itsbeta_wp7.ViewModel;
using PubNubMessaging.Core;
using System.Windows;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Phone.Tasks;
using System;

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
            pubnub.Publish<string>(ViewModelLocator.MainStatic.CurrentAchieve.Badge_name, new Message()
            { 
                FacebookId = ViewModelLocator.UserStatic.FacebookId ,
                Text = publishedMessage,
                Name = ViewModelLocator.UserStatic.Name
            }, PubnubCallbackResult);
            //Messages.Add(string.Format("messaage: {0}", publishedMessage));
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

        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
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
            pubnub.SessionUUID = ViewModelLocator.MainStatic.CurrentAchieve.Badge_name;
            /*pubnub.SubscribeTimeout = subscribeTimeoutInSeconds;
            pubnub.NonSubscribeTimeout = operationTimeoutInSeconds;
            pubnub.NetworkCheckMaxRetries = networkMaxRetries;
            pubnub.NetworkCheckRetryInterval = networkRetryIntervalInSeconds;
            pubnub.HeartbeatInterval = heartbeatIntervalInSeconds;
            pubnub.EnableResumeOnReconnect = resumeOnReconnect;*/

            pubnub.Subscribe<string>(ViewModelLocator.MainStatic.CurrentAchieve.Badge_name, PubnubCallbackResult, PubnubConnectCallbackResult);
        }

        private void PubnubCallbackResult(string result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        JArray o = JArray.Parse(result.ToString());
                        if (o[1].ToString() != "Sent")
                        {
                            Message item = JsonConvert.DeserializeObject<Message>(o[0].ToString());
                            Messages.Add(item);
                        };
                    }
                    catch { };                   
                }
                );
        }

        private void PubnubConnectCallbackResult(string result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    //JObject o = JObject.Parse(result.ToString());
                    //Messages.Add(result.ToString());
                    //Messages.Add("Успешное подключение к чату.");
                }
                );
        }

        private void MessagesList_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            Message item = (e.Item.Content as Message);
            WebBrowserTask webTask = new WebBrowserTask();
            webTask.Uri = new Uri("http://facebook.com/"+item.FacebookId.ToString());
            webTask.Show();
        }

    }


    public class Message
    {
        public Message() {
        }

        private string _text = "";
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        private string _facebookId = "";
        public string FacebookId
        {
            get
            {
                return _facebookId;
            }
            set
            {
                _facebookId = value;
            }
        }

        private string _name = "";
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string TextWithName
        {
            get
            {
                return this.Name+": "+this.Text;
            }
            private set
            {
            }
        }
    }
}