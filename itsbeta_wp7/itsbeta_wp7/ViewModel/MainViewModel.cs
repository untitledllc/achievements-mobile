using GalaSoft.MvvmLight;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Telerik.Windows.Data;

namespace itsbeta_wp7.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
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
    public class MainViewModel : ViewModelBase
    {
        public string ApplicationTitle
        {
            get
            {
                return "itsbeta";
            }
        }

        public string PageName
        {
            get
            {
                return "My page:";
            }
        }

        private ObservableCollection<CategoryItem> _categories = new ObservableCollection<CategoryItem>();
        public ObservableCollection<CategoryItem> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                RaisePropertyChanged("Categories");
            }
        }

        private ObservableCollection<ProjectItem> _projects = new ObservableCollection<ProjectItem>();
        public ObservableCollection<ProjectItem> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                _projects = value;
                RaisePropertyChanged("Projects");
            }
        }

        private ObservableCollection<BadgeItem> _badges = new ObservableCollection<BadgeItem>();
        public ObservableCollection<BadgeItem> Badges
        {
            get
            {
                return _badges;
            }
            set
            {
                _badges = value;
                RaisePropertyChanged("Badges");
            }
        }

        private BadgeItem _currentBadge = new BadgeItem();
        public BadgeItem CurrentBadge
        {
            set
            {
                _currentBadge = value;
                RaisePropertyChanged("CurrentBadge");
            }
            get
            {
                return _currentBadge;
            }
        }

        public void LoadAchievements()
        {
            try
            {
                try
                {
                    var client = new RestClient("http://www.itsbeta.com");
                    var request = new RestRequest("s/info/achievements.json", Method.GET);
                    request.Parameters.Clear();
                    request.AddParameter("access_token", "059db4f010c5f40bf4a73a28222dd3e3");
                    request.AddParameter("player_id", ViewModelLocator.UserStatic.PlayerId);

                    client.ExecuteAsync(request, response =>
                    {
                        try
                        {
                            JArray o = JArray.Parse(response.Content.ToString());
                            foreach (var categoryJson in o)
                            {
                                CategoryItem category = new CategoryItem();
                                category = JsonConvert.DeserializeObject<CategoryItem>(categoryJson.ToString());
                                Categories.Add(category);
                                foreach (var projectJson in categoryJson["projects"])
                                {
                                    ProjectItem project = JsonConvert.DeserializeObject<ProjectItem>(projectJson.ToString());
                                    Projects.Add(project);
                                    foreach (var badgeJson in projectJson["achievements"])
                                    {
                                        BadgeItem badge = JsonConvert.DeserializeObject<BadgeItem>(badgeJson.ToString());
                                        badge.Project_api_name = project.Display_name;
                                        Badges.Add(badge);
                                    }
                                }
                            };
                            RaisePropertyChanged("Categories");
                            RaisePropertyChanged("Projects");
                            RaisePropertyChanged("Badges");                            
                        }
                        catch { };
                    });
                }
                catch { };
            }
            catch { };
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}