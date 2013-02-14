using GalaSoft.MvvmLight;
using System;
using Newtonsoft.Json.Linq;
using RestSharp;

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
                        ViewModelLocator.MainStatic.LoadAchievements();
                    }
                    catch { };
                });
            }
            catch { };
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

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}