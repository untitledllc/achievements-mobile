using GalaSoft.MvvmLight;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Telerik.Windows.Data;
using System.Linq;
using System.Collections.Generic;

namespace itsbeta_wp7.ViewModel
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
        {
            return new ObservableCollection<T>(col);
        }
    }

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

        //private ObservableCollection<BadgeItem> _categoryBadges = new ObservableCollection<BadgeItem>();
        public ObservableCollection<BadgeItem> CategoryBadges
        {
            get
            {
                var items = (from item in Badges
                             where item.Category_api_name == CurrentCategory.Api_name
                             select item);
                return items.ToObservableCollection();
            }
            private set
            {
                //_categoryBadges = value;
                //RaisePropertyChanged("CategoryBadges");
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

        private ProjectItem _currentProject = new ProjectItem();
        public ProjectItem CurrentProject 
        {
            set
            {
                _currentProject = value;
                RaisePropertyChanged("CurrentProject");
            }
            get
            {
                return _currentProject;
            }
        }

        private CategoryItem _currentCategory = new CategoryItem();
        public CategoryItem CurrentCategory
        {
            set
            {
                _currentCategory = value;
                RaisePropertyChanged("CurrentCategory");
                RaisePropertyChanged("CategoryBadges");
            }
            get
            {
                return _currentCategory;
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
                                    project.Category_api_name = category.Api_name;
                                    Projects.Add(project);
                                    foreach (var badgeJson in projectJson["achievements"])
                                    {
                                        BadgeItem badge = JsonConvert.DeserializeObject<BadgeItem>(badgeJson.ToString());
                                        badge.Project_api_name = project.Display_name;
                                        badge.Category_api_name = category.Api_name;
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