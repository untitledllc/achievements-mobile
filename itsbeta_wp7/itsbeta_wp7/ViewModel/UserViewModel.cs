﻿using GalaSoft.MvvmLight;
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
                request_player.AddParameter("access_token", "059db4f010c5f40bf4a73a28222dd3e3");
                request_player.AddParameter("type", "fb_user_id");
                request_player.AddParameter("id", FacebookId);

                client_player.ExecuteAsync(request_player, response_player =>
                {
                    try
                    {
                        JObject o_player = JObject.Parse(response_player.Content.ToString());
                        PlayerId = o_player["player_id"].ToString();
                        RaisePropertyChanged("UserProfilePicture");

                        ViewModelLocator.MainStatic.LoadAchievements();
                        ViewModelLocator.UserStatic.GetItsbetaAchieve();
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
                request.AddParameter("access_token", "059db4f010c5f40bf4a73a28222dd3e3");
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
                                MessageBox.Show("Достижение активировано!");
                                ViewModelLocator.MainStatic.Loading = false;
                                ViewModelLocator.MainStatic.LoadAchievements();
                            });
                        }
                        else
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            {
                                MessageBox.Show("Не удалось активировать достижение!");
                                ViewModelLocator.MainStatic.Loading = false;
                            });
                        };
                    }
                    catch {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("Не удалось активировать достижение!");
                            ViewModelLocator.MainStatic.Loading = false;
                        });
                    };
                });
            };
            bw.RunWorkerAsync();
        }

        public MessagePrompt messagePrompt;
        private void GetItsbetaAchieve()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                var client = new RestClient("http://www.itsbeta.com");
                var request = new RestRequest("s/other/itsbeta/achieves/posttofbonce.json", Method.POST);
                request.Parameters.Clear();
                request.AddParameter("access_token", "059db4f010c5f40bf4a73a28222dd3e3");
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

                                messagePrompt = new MessagePrompt();
                                try
                                {
                                    messagePrompt.Body = new BadgeControl();

                                    Button closeButton = new Button() { Content = "Закрыть" };
                                    Button moreButton = new Button() { Content = "Подробнее" };

                                    closeButton.Click += new RoutedEventHandler(closeButton_Click);
                                    moreButton.Click += new RoutedEventHandler(moreButton_Click);

                                    messagePrompt.ActionPopUpButtons.Clear();
                                    messagePrompt.ActionPopUpButtons.Add(closeButton);
                                    messagePrompt.ActionPopUpButtons.Add(moreButton);
                                }
                                catch
                                {
                                };

                                messagePrompt.Show();
                            });
                        };
                    }
                    catch { };
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
            webTask.Uri = new Uri("http://www.itsbeta.com/s/other/itsbeta/achieves/fb?locale=ru&name=itsbeta&fb_action_ids=" + FacebookId);
            webTask.Show();
        }

        public void GetFBUserInfo()
        {
            var fb = new FacebookClient(FacebookToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ViewModelLocator.UserStatic.Name = (string)result["name"];
                    ViewModelLocator.UserStatic.First_name = (string)result["first_name"];
                    ViewModelLocator.UserStatic.Last_name = (string)result["last_name"];
                    ViewModelLocator.UserStatic.Birthday = (string)result["birthday"];
                    var item = JObject.Parse(result["location"].ToString());
                    ViewModelLocator.UserStatic.Location = item["name"].ToString();// item["name"].ToString();
                });
            };
            fb.GetAsync("me");
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

        private string _birthday;
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
                        DateBirthday = DateTime.Parse(_birthday.ToString());
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