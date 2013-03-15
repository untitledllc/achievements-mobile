using GalaSoft.MvvmLight;
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
using Microsoft.Phone.Shell;
using System;
using System.Net;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone;
using System.Windows.Resources;
using Microsoft.Phone.Net.NetworkInformation;
using ImageTools.IO.Png;
using ImageTools;
using ImageTools.Filtering;
using System.Windows.Media;
using System.Globalization;

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
                             where (item.Bonuses.Count()>0)
                             select item);
                return items.ToObservableCollection();
            }
            private set
            {
            }
        }

        /// <summary>
        /// Количество бонусов во всех достижениях пользователя
        /// </summary>
        public int BonusCount
        {
            get
            {
                int count = 0;
                var items = (from item in Achieves
                             where (item.Bonuses.Count() > 0)
                             select item);
                foreach (var item in items) {
                    count += item.Bonuses.Count();
                };
                return count;
            }
            private set
            {
            }
        }


        public ObservableCollection<int> AchievesChartData
        {
            get
            {
                ObservableCollection<int> chartData = new ObservableCollection<int>();
                foreach (var item in Categories)
                {
                    chartData.Add(item.Total_badges);
                };
                return chartData;
            }
            private set
            {
            }
        }

        /// <summary>
        /// Последние 3 полученные достижения пользователя
        /// </summary>
        public ObservableCollection<AchievesItem> LastAchieves
        {
            get
            {
                var items = (from item in Achieves
                             where ((item.Fb_id != "") && (item.Fb_id != null))
                             group item by item.Display_name into grp
                             select grp.OrderBy(a => a.Create_time).Last()).OrderByDescending(a => a.Create_time).Take(3).ToObservableCollection();
                return items;
            }
            private set
            {
            }
        }

        private AchievesItem _currentAchieve = null;
        /// <summary>
        /// Текущее достижение (отображаемое на странице просмотра достижения)
        /// </summary>
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
        /// <summary>
        /// Текущий проект
        /// </summary>
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
        /// <summary>
        /// Флаг, означающий загрузку данных
        /// </summary>
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
        /// <summary>
        /// Текущая категория
        /// </summary>
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

        /// <summary>
        /// Загружаем из isolatedStorage и разбираем ранее сохраненный json
        /// </summary>
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

        /// <summary>
        /// Сохраняем ответ от itsbeta в isolatedstorage для работы оффлайн и кэширования
        /// </summary>
        /// <param name="json">json полученный от achievements.json</param>
        public void SaveToIsolatedStorage(string json="")
        {
            IsolatedStorageHelper.SaveSerializableObject<string>(json, "achieves.xml");            
        }

        /// <summary>
        /// Разбор ответа скатегориями, проеками, достижениями от itsbeta
        /// Описание формата - https://hackpad.com/itsbeta-API-version-1-aVSDkDTPbA1
        /// </summary>
        /// <param name="content">json полученный от achievements.json</param>
        public void ParseAcievesJson(string content="") {
            ObservableCollection<CategoryItem> tempCategories = new ObservableCollection<CategoryItem>();
            ObservableCollection<ProjectItem> tempProjects = new ObservableCollection<ProjectItem>();
            ObservableCollection<AchievesItem> tempAchieves = new ObservableCollection<AchievesItem>();
            int all_count = 0;
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
                                        project.Bonuses_count += badge.Bonuses.Count();
                                    }                                    
                                    count += pcount;
                                    project.Activated_badges_count = pcount;
                                    tempProjects.Add(project);

                                }
                                category.Activated_badges_count = count;
                                all_count += count;
                               tempCategories.Add(category);
                            };
                            Categories = tempCategories;
                            Projects = tempProjects;
                            Achieves = tempAchieves;

                            Loading = false;
                            RaisePropertyChanged("BonusAchieves");
                            RaisePropertyChanged("BonusCount");
                            RaisePropertyChanged("Categories");
                            RaisePropertyChanged("Projects");
                            RaisePropertyChanged("Achieves");
                            RaisePropertyChanged("LastAchieves");                                    
                        }
                        catch { };
            UpdateTile(all_count);
        }

        /// <summary>
        /// Обновляем главный tile приложения
        /// </summary>
        /// <param name="count"></param>
        private void UpdateTile(int count=0)
        {
            try
            {
                ShellTile appTile = ShellTile.ActiveTiles.First();
                if (appTile != null)
                {
                    if (count > 0)
                    {
                        WebClient client = new WebClient();
                        client.OpenReadCompleted += (s, e) =>
                        {
                            PicToIsoStore(e.Result);

                            string imageFolder = @"\Shared\ShellContent";
                            string shareJPEG = "backtile.jpg";
                            string filePath = System.IO.Path.Combine(imageFolder, shareJPEG);  
                            StandardTileData newTile = new StandardTileData
                            {
                                //Title = "Itsbeta",
                                //BackgroundImage = new Uri("tile_image.png", UriKind.Relative),
                                //Count = count,
                                //BackTitle = LastAchieves.First().Display_name,
                                BackBackgroundImage = new Uri(@"isostore:" + filePath, UriKind.Absolute)
                                //BackContent = "Content for back tile."
                            };
                            appTile.Update(newTile);
                        };
                        client.OpenReadAsync(new Uri(LastAchieves.First().Pic, UriKind.Absolute));
                    };
                };
            }
            catch { };
        }

        /// <summary>
        /// Сохраняем изображение в isolatedStorage
        /// </summary>
        /// <param name="pic"></param>
        private void PicToIsoStore(Stream pic)
        {
            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var bi = new BitmapImage();
                bi.SetSource(pic);
                var wb = new WriteableBitmap(bi);
                string imageFolder = @"\Shared\ShellContent";
                string shareJPEG = "backtile.jpg";
                string filePath = System.IO.Path.Combine(imageFolder, shareJPEG);
                using (var isoFileStream = isoStore.CreateFile(filePath))
                {
                    //var width = 140;//wb.PixelWidth;
                    //var height = 140;//wb.PixelHeight;
                    //wb.SaveJpeg(isoFileStream, width, height, 0, 100);
                    var encoder = new PngEncoder();
                    wb = new WriteableBitmap(bi);
                    //WriteableBitmap wb2 = new WriteableBitmap(210, 210);
                    //encoder.Encode(ExtendedImage.Resize(wb.ToImage(), 210, new NearestNeighborResizer()), isoFileStream);
                    encoder.Encode(wb.ToImage(), isoFileStream);
                }
            }
        }

        /// <summary>
        /// Загружаем данные о категориях, проектах, достижениях
        /// </summary>
        /// <param name="activation_code"></param>
        public void LoadAchievements(string activation_code="")
        {
            LoadFromIsolatedStorage();
            bool hasNetworkConnection =
            NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None;
            if (hasNetworkConnection)
            {
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
                        request.AddParameter("locale", ViewModelLocator.MainStatic.CurrentLanguage);

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
            };
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

        /// <summary>
        /// Возвращаем строку, с текщуим языком приложения, в формате, необходимом для itsbeta
        /// </summary>
        public string CurrentLanguage
        {
            get
            {
                switch (CultureInfo.CurrentCulture.Name)
                {
                    case "ru-RU": return "ru";
                    default: return "en";
                }
            }
            private set { } }
    }
}