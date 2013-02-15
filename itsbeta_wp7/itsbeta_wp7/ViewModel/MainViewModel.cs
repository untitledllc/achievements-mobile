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

        private ObservableCollection<AchievesItem> _achieves = new ObservableCollection<AchievesItem>();
        public ObservableCollection<AchievesItem> Achieves
        {
            get
            {
                return _achieves;
            }
            set
            {
                _achieves = value;
                RaisePropertyChanged("Achieves");
            }
        }

        //private ObservableCollection<BadgeItem> _categoryBadges = new ObservableCollection<BadgeItem>();
        public ObservableCollection<AchievesItem> CategoryAchieves
        {
            get
            {
                var items = (from item in Achieves
                             where item.Category_api_name == CurrentCategory.Api_name
                             select item);
                return items.ToObservableCollection();
            }
            private set
            {
            }
        }

        public ObservableCollection<AchievesItem> LastAchieves
        {
            get
            {
                var items = (from item in Achieves
                             group item by item.Display_name into grp
                             select grp.OrderBy(a => a.Create_time).Last()).ToObservableCollection();
                return items;
            }
            private set
            {
            }
        }

        private AchievesItem _currentAchieve = new AchievesItem();
        public AchievesItem CurrentAchieve
        {
            set
            {
                _currentAchieve = value;
                RaisePropertyChanged("CurrentAchieve");
            }
            get
            {
                return _currentAchieve;
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
                RaisePropertyChanged("CategoryAchieves");
            }
            get
            {
                return _currentCategory;
            }
        }

        ///badges.json

        public void LoadAchievements()
        {
            try
            {
                try
                {
                    Categories = new ObservableCollection<CategoryItem>();
                    Projects = new ObservableCollection<ProjectItem>();
                    Achieves = new ObservableCollection<AchievesItem>();

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
                                int count = 0;
                                foreach (var projectJson in categoryJson["projects"])
                                {
                                    ProjectItem project = JsonConvert.DeserializeObject<ProjectItem>(projectJson.ToString());
                                    project.Category_api_name = category.Api_name;
                                    category.Total_badges = category.Total_badges + project.Total_badges;
                                    int pcount = 0;
                                    foreach (var badgeJson in projectJson["achievements"])
                                    {

                                        
                                        AchievesItem badge = JsonConvert.DeserializeObject<AchievesItem>(badgeJson.ToString());
                                        badge.Project_api_name = project.Display_name;
                                        badge.Category_api_name = category.Api_name;
                                        if (Achieves.FirstOrDefault(c => c.Display_name == badge.Display_name) == null)
                                        {
                                            pcount++;
                                        };                                        
                                        Achieves.Add(badge);
                                    }                                    
                                    count += pcount;
                                    project.Activated_badges_count = pcount;
                                    Projects.Add(project);
                                }
                                category.Activated_badges_count = count;
                                Categories.Add(category);
                            };
                            RaisePropertyChanged("Categories");
                            RaisePropertyChanged("Projects");
                            RaisePropertyChanged("Achieves");
                            RaisePropertyChanged("LastAchieves");
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