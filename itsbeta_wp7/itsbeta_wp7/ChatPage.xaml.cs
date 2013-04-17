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
using System.Linq;
using Microsoft.Devices;

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

         
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            pubnub.Unsubscribe<string>(channel, PubnubCallbackResult, PubnubConnectCallbackResult, PubnubDisconnectCallbackResult);
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.MessagesList.ItemsSource = Messages;

            pubnub = new Pubnub(PublishKey, SubscribeKey, secretKey, cipherKey, ssl);
            pubnub.SessionUUID = ViewModelLocator.UserStatic.FacebookId;
            pubnub.SubscribeTimeout = subscribeTimeoutInSeconds;
            pubnub.NonSubscribeTimeout = operationTimeoutInSeconds;
            pubnub.NetworkCheckMaxRetries = networkMaxRetries;
            pubnub.NetworkCheckRetryInterval = networkRetryIntervalInSeconds;
            pubnub.HeartbeatInterval = heartbeatIntervalInSeconds;
            pubnub.EnableResumeOnReconnect = resumeOnReconnect;
            pubnub.Subscribe<string>(channel, PubnubCallbackResult, PubnubConnectCallbackResult);
            pubnub.DetailedHistory<string>(channel, 20, PubnubHistoryCallbackResult);

            base.OnNavigatedTo(e);
        }

        private void PubnubDisconnectCallbackResult(string result)
        {
        }

        private void SendTextButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SendMessage();
        }

        private void SendMessage() {
            string publishedMessage = this.TestOutput.Text;
            this.TestOutput.Text = "";
            pubnub.Publish<string>(ViewModelLocator.MainStatic.CurrentAchieve.Badge_name, new Message()
            { 
                FacebookId = ViewModelLocator.UserStatic.FacebookId ,
                Text = publishedMessage,
                Name = ViewModelLocator.UserStatic.Name,
                MessageSendDate = DateTime.Now
            }, PubnubCallbackResult);
        }


        Pubnub pubnub;
        string channel = ViewModelLocator.MainStatic.CurrentAchieve.Badge_name;
        bool ssl = false;
        string SubscribeKey = "sub-c-fc890f04-a2a8-11e2-a387-12313f022c90";
        string PublishKey = "pub-c-a920b269-83ea-4c11-bd99-726d17dd1966";
        string secretKey = "sec-c-ZjEyOTZiZjQtOTI2ZC00NzA1LWI3N2MtMThhYzRiZjdlYzA1";
        string cipherKey = "";
        string uuid = ViewModelLocator.UserStatic.FacebookId;
        bool resumeOnReconnect = false;

        int subscribeTimeoutInSeconds = 10;
        int operationTimeoutInSeconds = 10;
        int networkMaxRetries = 10;
        int networkRetryIntervalInSeconds = 1;
        int heartbeatIntervalInSeconds = 1;

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
        public ObservableCollection<Message> MessagesReversed
        {
            get
            {
                return new ObservableCollection<Message>(_messages.Reverse());
            }
            private set
            {
            }
        }

        private void PhoneApplicationPage_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void PubnubHistoryCallbackResult(string result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        JArray o = JArray.Parse(result.ToString());
                        string messages_array = o[0].ToString();
                        JArray history = JArray.Parse(messages_array);
                        foreach (var item in history)
                        {
                            try
                            {
                                Message message_item = JsonConvert.DeserializeObject<Message>(item.ToString());
                                Messages.Add(message_item);
                                MessagesList.BringIntoView(message_item);
                            }
                            catch { };
                        };
                        VibrateController vibrate = VibrateController.Default;
                        vibrate.Start(TimeSpan.FromMilliseconds(100));
                    }
                    catch { };
                });
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
                            //ChatScroll.ScrollToVerticalOffset(ChatScroll.ScrollableHeight + ChatScroll.ExtentHeight + 40);
                            MessagesList.BringIntoView(item);

                            VibrateController vibrate = VibrateController.Default;
                            vibrate.Start(TimeSpan.FromMilliseconds(100));
                        };
                    }
                    catch(Exception ex) {
                        //MessageBox.Show(ex.Message);
                    };                   
                }
                );
        }

        private void PubnubConnectCallbackResult(string result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    //JObject o = JObject.Parse(result.ToString());
                    //MessageBox.Show(result.ToString());
                    //Messages.Add("Успешное подключение к чату.");
                }
                );
        }

        private void MessagesList_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {

        }

        private void TestOutput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==System.Windows.Input.Key.Enter) {
                SendMessage();
            };
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Message item = (this.MessagesList.SelectedItem as Message);
            WebBrowserTask webTask = new WebBrowserTask();
            webTask.Uri = new Uri("http://facebook.com/" + item.FacebookId.ToString());
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

        private DateTime _messageSendDate = DateTime.Now;
        public DateTime MessageSendDate
        {
            get
            {
                return _messageSendDate;
            }
            set
            {
                _messageSendDate = value;
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

        public string NameForList
        {
            get
            {
                return "[" + MessageSendDate.ToString("d.m HH:mm") + "] " + this.Name + ":";
            }
            private set
            {
            }
        }

        public string TextColor
        {
            get
            {
                string color = "#FF000000";
                if (FacebookId == ViewModelLocator.UserStatic.FacebookId)
                {
                    color = "#FF569db7";
                };
                return color;
            }
            private set
            {
            }
        }
    }
}