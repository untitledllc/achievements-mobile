﻿using GalaSoft.MvvmLight;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Telerik.Windows.Data;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using MSPToolkit.Utilities;

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

        public ObservableCollection<AchievesItem> BonusAchieves
        {
            get
            {
                var items = (from item in Achieves
                             where ((item.Bonus != "") && (item.Bonus != null))
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
                             where ((item.Fb_id != "") && (item.Fb_id != null))
                             group item by item.Display_name into grp
                             select grp.OrderBy(a => a.Create_time).Last()).OrderByDescending(a => a.Create_time).ToObservableCollection();
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

        private bool _loading = false;
        public bool Loading
        {
            set
            {
                _loading = value;
                RaisePropertyChanged("Loading");
            }
            get
            {
                return _loading;
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

        public void LoadFromIsolatedStorage()
        {
            try
            {
                string json = IsolatedStorageHelper.LoadSerializableObject<string>("achieves.xml");
                Loading = true;
                ParseAcievesJson(json);
                Loading = false;
            }
            catch { };
        }

        public void SaveToIsolatedStorage(string json="")
        {
            IsolatedStorageHelper.SaveSerializableObject<string>(json, "achieves.xml");            
        }

        public void ParseAcievesJson(string content="") {
            ObservableCollection<CategoryItem> tempCategories = new ObservableCollection<CategoryItem>();
            ObservableCollection<ProjectItem> tempProjects = new ObservableCollection<ProjectItem>();
            ObservableCollection<AchievesItem> tempAchieves = new ObservableCollection<AchievesItem>();
                     try
                        {
                            JArray o = JArray.Parse(content.ToString());
                            
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
                                        badge.Project_api_name = project.Api_name;
                                        badge.Project_name = project.Display_name;
                                        badge.Category_api_name = category.Api_name;
                                        badge.Category_name = category.Display_name;
                                        if (tempAchieves.FirstOrDefault(c => c.Badge_name == badge.Badge_name) == null)
                                        {
                                            pcount++;
                                        };
                                            tempAchieves.Add(badge);

                                    }                                    
                                    count += pcount;
                                    project.Activated_badges_count = pcount;
                                    tempProjects.Add(project);

                                }
                                category.Activated_badges_count = count;
                                    tempCategories.Add(category);
                            };
                                    Categories = tempCategories;
                                    Projects = tempProjects;
                                    Achieves = tempAchieves;

                                    Loading = false;
                                    RaisePropertyChanged("BonusAchieves");
                                    RaisePropertyChanged("Categories");
                                    RaisePropertyChanged("Projects");
                                    RaisePropertyChanged("Achieves");
                                    RaisePropertyChanged("LastAchieves");                                    
                        }
                        catch { };
        }

        ///badges.json
        public void LoadAchievements(string activation_code="")
        {
            LoadFromIsolatedStorage();
            try
            {
                var bw = new BackgroundWorker();
                //bw.DoWork += delegate
                //{
                try
                {
                    Loading = true;

                    var client = new RestClient("http://www.itsbeta.com");
                    var request = new RestRequest("s/info/achievements.json", Method.GET);
                    request.Parameters.Clear();
                    request.AddParameter("access_token", App.ACCESS_TOKEN);
                    request.AddParameter("player_id", ViewModelLocator.UserStatic.PlayerId);

                    client.ExecuteAsync(request, response =>
                    {
                        try
                        {
                            //JArray o = JArray.Parse(response.Content.ToString());
                            ParseAcievesJson(response.Content.ToString());
                            SaveToIsolatedStorage(response.Content.ToString());
                            Loading = false;
                            
                            if (activation_code != "")
                            {
                                ViewModelLocator.UserStatic.AchievedEarnedMessage("Достижение активировано!", "", activation_code);
                            };
                        }
                        catch { };
                    });
                }
                catch { };
                //};
                //bw.RunWorkerAsync(); 
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

        public override void Cleanup()
        {
            // Clean up if needed
            base.Cleanup();
        }
    }
}