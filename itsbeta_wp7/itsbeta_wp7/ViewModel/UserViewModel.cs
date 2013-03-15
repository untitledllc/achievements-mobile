using GalaSoft.MvvmLight;
using System;
using Newtonsoft.Json.Linq;
using RestSharp;
using Facebook;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Tasks;
using itsbeta_wp7.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using MSPToolkit.Utilities;
using Microsoft.Phone.Net.NetworkInformation;
using ImageTools.IO.Png;
using ImageTools;
using ImageTools.Filtering;


namespace itsbeta_wp7.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class UserViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the UserViewModel class.
        /// </summary>
        public UserViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real": Connect to service, etc...
            ////}            
        }

        public override void Cleanup()
        {
            FacebookId = "";
            FacebookToken = "";
            // Clean up if needed
            base.Cleanup();
        }

        public void GetPlayerId() {
            try
            {
                var client_player = new RestClient("http://www.itsbeta.com");
                var request_player = new RestRequest("s/info/playerid.json", Method.GET);
                request_player.Parameters.Clear();
                request_player.AddParameter("access_token", App.ACCESS_TOKEN);
                request_player.AddParameter("type", "fb_user_id");
                request_player.AddParameter("id", FacebookId);

                client_player.ExecuteAsync(request_player, response_player =>
                {
                    try
                    {
                        JObject o_player = JObject.Parse(response_player.Content.ToString());
                        PlayerId = o_player["player_id"].ToString();
                        RaisePropertyChanged("UserProfilePicture");

                        ViewModelLocator.UserStatic.LogOut = false;
                        ViewModelLocator.MainStatic.LoadAchievements();

                        try
                        {
                            if ((Application.Current.RootVisual as PhoneApplicationFrame).CanGoBack)
                            {
                                while ((Application.Current.RootVisual as PhoneApplicationFrame).RemoveBackEntry() != null)
                                {
                                    (Application.Current.RootVisual as PhoneApplicationFrame).RemoveBackEntry();
                                }
                            };
                        }
                        catch { };
                    }
                    catch { };
                });
            }
            catch { };
        }

        /*www.itsbeta.com/s/activate.json?activation_code=.....&user_id=....&user_token=......*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activation_code"></param>
        public void ActivateAchieve(string activation_code)
        {
            ViewModelLocator.MainStatic.Loading = true;
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                var client = new RestClient("http://www.itsbeta.com");
                var request = new RestRequest("s/activate.json", Method.GET);
                request.Parameters.Clear();
                request.AddParameter("access_token", App.ACCESS_TOKEN);
                request.AddParameter("user_id", FacebookId);
                request.AddParameter("user_token", FacebookToken);
                request.AddParameter("activation_code", activation_code);

                client.ExecuteAsync(request, response =>
                {
                    try
                    {
                        JObject o = JObject.Parse(response.Content.ToString());
                        if (o["id"].ToString() != "")
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {                                
                                ViewModelLocator.MainStatic.Loading = false;
                                ViewModelLocator.MainStatic.LoadAchievements(activation_code);
                            });
                        }
                        else
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                ViewModelLocator.UserStatic.AchievedEarnedMessage("Не удалось активировать достижение!");
                                ViewModelLocator.MainStatic.Loading = false;
                            });
                        };
                    }
                    catch {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            ViewModelLocator.UserStatic.AchievedEarnedMessage("Не удалось активировать достижение!");
                            ViewModelLocator.MainStatic.Loading = false;
                        });
                    };
                });
            };
            bw.RunWorkerAsync();
        }


        public void AchievedEarnedMessage(string message, string title = "", string api_name="")
        {
            try
            {
                ToastPrompt toast = new ToastPrompt();
                toast.MillisecondsUntilHidden = 6000;
                toast.Background = new SolidColorBrush(Color.FromArgb(255, 83, 83, 83));
                toast.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

                if (api_name == "")
                {
                    toast.Title = title;
                    toast.Message = message;

                    if (title == "Пользователь itsbeta")
                    {
                        toast.Completed += toast_Completed;
                        BitmapImage img = new BitmapImage(new Uri("/images/Achive-itsbeta.png", UriKind.Relative));
                        img.CreateOptions = BitmapCreateOptions.None;
                        img.ImageOpened += (s, e) =>
                        {
                            WriteableBitmap wBitmap = new WriteableBitmap((BitmapImage)s);
                 
                            MemoryStream ms = new MemoryStream();
                            //wBitmap.SaveJpeg(ms, 50, 50, 0, 100);
                            var encoder = new PngEncoder();
                            BitmapImage bmp = new BitmapImage();                      
                            encoder.Encode(ExtendedImage.Resize(wBitmap.ToImage(), 50, new NearestNeighborResizer()), ms);
                            bmp.SetSource(ms);

                            toast.ImageSource = bmp;

                            ViewModelLocator.MainStatic.CurrentAchieve = ViewModelLocator.MainStatic.Achieves.FirstOrDefault(c => c.Badge_name == "itsbeta");
                            toast.Completed += toast_Completed;
                            toast.Show();
                        };
                    }
                    else
                    {
                        toast.Show();
                    };                    
                }
                else
                {
                    AchievesItem achieve = new AchievesItem();
                    achieve = ViewModelLocator.MainStatic.Achieves.FirstOrDefault(c => c.Api_name == api_name);
                    ViewModelLocator.MainStatic.CurrentAchieve = achieve;
                    toast.Title = achieve.Display_name;
                    toast.Message = achieve.Desc;

                    BitmapImage img = new BitmapImage(new Uri(achieve.Pic, UriKind.RelativeOrAbsolute));
                    img.CreateOptions = BitmapCreateOptions.None;
                    img.ImageOpened += (s, e) =>
                    {
                        WriteableBitmap wBitmap = new WriteableBitmap((BitmapImage)s);
                        MemoryStream ms = new MemoryStream();
                        wBitmap.SaveJpeg(ms, 50, 50, 0, 100);
                        BitmapImage bmp = new BitmapImage();
                        bmp.SetSource(ms);
                        toast.ImageSource = bmp;

                        toast.Completed += toast_Completed;
                        toast.Show();
                    };
                };
            }
            catch { };
        }
        void toast_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
            {
                if ((ViewModelLocator.MainStatic.Achieves.FirstOrDefault(c => c.Badge_name == "itsbeta") != null) || (ViewModelLocator.MainStatic.CurrentAchieve != null))
                {
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/BadgePage.xaml", UriKind.Relative));
                };
            };
        }

        public MessagePrompt messagePrompt;
        public string messageprompt_fb_id = "";
        public void GetItsbetaAchieve()
        {
            ViewModelLocator.MainStatic.Loading = true;
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                var client = new RestClient("http://www.itsbeta.com");
                var request = new RestRequest("s/other/itsbeta/achieves/posttofbonce.json", Method.POST);
                request.Parameters.Clear();
                request.AddParameter("access_token", App.ACCESS_TOKEN);
                request.AddParameter("user_id", FacebookId);
                request.AddParameter("user_token", FacebookToken);
                request.AddParameter("badge_name", "itsbeta");
                //for test
                //request.AddParameter("unique", "f");

                client.ExecuteAsync(request, response =>
                {
                    try
                    {
                        JObject o = JObject.Parse(response.Content.ToString());
                        if (o["id"].ToString() != "")
                        {                            
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                ViewModelLocator.UserStatic.GetPlayerId();
                                messagePrompt = new MessagePrompt();
                                try
                                {
                                    AchievedEarnedMessage("Установил приложение itsbeta.", "Пользователь itsbeta");
                                    /*messageprompt_fb_id = o["fb_id"].ToString();
                                    messagePrompt.Body = new BadgeControl();

                                    Button closeButton = new Button() { Content = "Закрыть" };
                                    Button moreButton = new Button() { Content = "Подробнее" };

                                    closeButton.Click += new RoutedEventHandler(closeButton_Click);
                                    moreButton.Click += new RoutedEventHandler(moreButton_Click);

                                    messagePrompt.ActionPopUpButtons.Clear();
                                    messagePrompt.ActionPopUpButtons.Add(closeButton);
                                    messagePrompt.ActionPopUpButtons.Add(moreButton);*/
                                }
                                catch
                                {
                                };

                                //messagePrompt.Show();
                            });
                        }
                        else
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                ViewModelLocator.UserStatic.GetPlayerId();
                            });
                        };
                    }
                    catch {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            ViewModelLocator.UserStatic.GetPlayerId();
                        });
                    };
                });
            };
            bw.RunWorkerAsync();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            messagePrompt.Hide();
        }

        private void moreButton_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webTask = new WebBrowserTask();
            webTask.Uri = new Uri("http://www.itsbeta.com/s/other/itsbeta/achieves/fb?locale=ru&name=itsbeta&fb_action_ids=" + messageprompt_fb_id);
            webTask.Show();
        }

        public void GetFBUserInfo()
        {
            var fb = new FacebookClient(FacebookToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                    });
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                Dictionary<string, object> d_result = new Dictionary<string,object>();
                foreach (var item in result) {
                    d_result.Add(item.Key, item.Value.ToString());
                };
                d_result.Add("fb_id", FacebookId);
                d_result.Add("fb_token", FacebookToken);

                SaveToIsolatedStorage(d_result);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    LoadUserData(d_result);
                });
            };
            fb.GetAsync("me");
        }

        public void LoadUserData(Dictionary<string, object> result)
        {
            try
            {
                ViewModelLocator.UserStatic.Name = (string)result["name"];
            }
            catch { };
            try
            {
                ViewModelLocator.UserStatic.First_name = (string)result["first_name"];
            }
            catch { };
            try
            {
                ViewModelLocator.UserStatic.Last_name = (string)result["last_name"];
            }
            catch { };
            try
            {
                ViewModelLocator.UserStatic.Birthday = (string)result["birthday"];
            }
            catch { };
            try
            {
                var item = JObject.Parse(result["location"].ToString());
                ViewModelLocator.UserStatic.Location = item["name"].ToString();// item["name"].ToString();
            }
            catch { };

            try
            {
                ViewModelLocator.UserStatic.FacebookId = (string)result["fb_id"];
            }
            catch { };

            try
            {
                ViewModelLocator.UserStatic.FacebookToken = (string)result["fb_token"];
            }
            catch { };
        }

        public void LoadFromIsolatedStorage()
        {
            try
            {
                Dictionary<string, object> result = IsolatedStorageHelper.LoadSerializableObject<Dictionary<string, object>>("user.xml");
                LoadUserData(result);
                this.UserLoaded = true;

                bool hasNetworkConnection = NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None;
                if (hasNetworkConnection)
                {
                    ViewModelLocator.UserStatic.GetItsbetaAchieve();
                    ViewModelLocator.UserStatic.GetFBUserInfo();
                }
                else
                {
                    ViewModelLocator.MainStatic.LoadFromIsolatedStorage();
                };
            }
            catch { };
        }

        public void SaveToIsolatedStorage(Dictionary<string, object> json)
        {
            IsolatedStorageHelper.SaveSerializableObject<Dictionary<string, object>>(json, "user.xml");
        }

        private string _location = "";
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                RaisePropertyChanged("Location");
            }
        }

        private bool _logOut = true;
        public bool LogOut
        {
            get
            {
                return _logOut;
            }
            set
            {
                _logOut = value;
                RaisePropertyChanged("LogOut");
            }
        }

        private bool _userLoaded = false;
        public bool UserLoaded
        {
            get
            {
                return _userLoaded;
            }
            set
            {                
                if (value == false)
                {
                    Dictionary<string, object> empty = new Dictionary<string, object>();
                    SaveToIsolatedStorage(empty);
                }
                else {
                    if ((_userLoaded==false) && (value==true)) {
                        (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    };
                };
                _userLoaded = value;
                RaisePropertyChanged("UserLoaded");
            }
        }


        private string _birthday;
        /// <summary>
        /// Деньрождения пользователя в формате MM/dd/yyyy
        /// </summary>
        public string Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                _birthday = value;
                try
                {
                    if (_birthday != "")
                    {
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        string format = "MM/dd/yyyy";
                        DateBirthday = DateTime.ParseExact(_birthday.ToString(), format, provider);
                    }
                    else
                    {
                        DateBirthday = DateTime.Today;
                        _birthday = DateBirthday.ToShortDateString();
                    };
                }
                catch { };
                RaisePropertyChanged("Birthday");
                RaisePropertyChanged("DateBirthday");                
            }
        }

        public string Age
        {
            get
            {
                string outstr="";
                DateTime today = DateTime.Today;
                int age = today.Year - DateBirthday.Year;
                if (DateBirthday > today.AddYears(-age)) age--;

                outstr+=age.ToString()+" ";
                switch (age%10) {
                    case 1:
                        outstr += AppResources.years1;
                        break;
                    case 2:
                    case 3:
                    case 4:
                        outstr += AppResources.years234;
                        break;
                    default: outstr += AppResources.years; break;
                };

                return outstr;
            }
            private set
            {
            }
        }

        private DateTime _dateBirthday = DateTime.Today;
        public DateTime DateBirthday
        {
            get
            {
                return _dateBirthday;
            }
            set
            {
                _dateBirthday = value;
                _birthday = DateBirthday.ToShortDateString();
                RaisePropertyChanged("Birthday");
                RaisePropertyChanged("DateBirthday");
                RaisePropertyChanged("Age");
            }
        }

        private string _first_name = "";
        public string First_name
        {
            get
            {
                return _first_name;
            }
            set
            {
                _first_name = value;
                RaisePropertyChanged("First_name");
            }
        }

        private string _last_name = "";
        public string Last_name
        {
            get
            {
                return _last_name;
            }
            set
            {
                _last_name = value;
                RaisePropertyChanged("Last_name");
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
                RaisePropertyChanged("Name");
            }
        }

        private string _playerId = "";
        public string PlayerId
        {
            get
            {
                return _playerId;
            }
            set
            {
                _playerId = value;
                RaisePropertyChanged("PlayerId");
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
                RaisePropertyChanged("FacebookId");
            }
        }

        private string _facebookToken = "";
        public string FacebookToken
        {
            get
            {
                return _facebookToken;
            }
            set
            {
                _facebookToken = value;
                RaisePropertyChanged("FacebookToken");
            }
        }

        public string UserProfilePicture
        {
            get
            {
                // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                // for more info visit http://developers.facebook.com/docs/reference/api
                return string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", FacebookId, "square", FacebookToken);
            }
            private set
            {
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}