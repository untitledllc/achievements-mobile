using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Views.Animations;

using Achievements.AndroidPlatform.GUI;
using ItsBeta.Core;
using Android.Graphics;
using System.IO;
using Java.IO;

namespace Achievements.AndroidPlatform
{
    public partial class MainActivity : Activity
    {
        List<CategoriesListData> _categoriesList;
        PopupWindow _categoriesPopupWindow;

        private void CreateCategoriesViewObject()
        {
            
            _categoriesList = new List<CategoriesListData>();

            for (int i = 0; i < _achievesInfo.CategoriesCount; i++)
            {
                _categoriesList.Add(new CategoriesListData() { CategoryNameText = String.Format("{0}",
                    _achievesArray[i].DisplayName),
                    IsCategoryActive = true });

                if (!_selectedCategoriesDictionary.ContainsKey(_achievesArray[i].DisplayName))
                {
                    _selectedCategoriesDictionary.Add(_achievesArray[i].DisplayName, _categoriesList[i].IsCategoryActive);
                }
            }

            _categoriesListView = new ListView(this);

            CategoriesListItemAdapter categoriesAdapter = new CategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow,
                _categoriesList, _selectedCategoriesDictionary);

            _categoriesListView.Adapter = categoriesAdapter;
            _categoriesListView.DividerHeight = 0;

            LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            _categoriesListView.SetWillNotCacheDrawing(true);
            _categoriesPopupWindow = new PopupWindow(_categoriesListView,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
            
            _navigationBarImageButton.Click += delegate
            {
                if (!_isBarCategoriesListOpen)
                {
                    if (_isBarSubCategoriesListOpen)
                    {
                     //   subCategoriesPopupWindow.Dismiss();
                    }
                    _categoriesPopupWindow.ShowAsDropDown(FindViewById<ImageButton>(Resource.Id.NavigationBarImageButton), 0, 0);
                    _isBarCategoriesListOpen = true;
                    return;
                }
                if (_isBarCategoriesListOpen)
                {
                    _categoriesPopupWindow.Dismiss();
                    categoriesLinearLayout.StartAnimation(categoriesChosedAnimation);
                    _isBarCategoriesListOpen = false;
                }
            };
        }
        

        int checkedCategoriesCount;
        bool _isBarSubCategoriesListOpen = false;
        

        private void CreateSubCategoriesViewObject()
        {
            checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);
            
            foreach (var item in _selectedCategoriesDictionary)
            {
                if (item.Value == true)
                {
                    var categoryButton = new Button(this);
                    categoryButton.Text = item.Key;

                    categoriesLinearLayout.AddView(categoryButton);

                    categoryButton.SetBackgroundColor(global::Android.Graphics.Color.Argb(2, 0, 0, 0));
                    categoryButton.Click += delegate { categoryButton.StartAnimation(buttonClickAnimation); };
                    categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
                    categoryButton.Gravity = GravityFlags.Left;
                    categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;
                    categoryButton.SetSingleLine(true);

                    //middle

                    ListView _subCategoriesListView = new ListView(this);
                    _subCategoriesListView.DividerHeight = 0;

                    PopupWindow subCategoriesPopupWindow = new PopupWindow(_subCategoriesListView,
                                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
                    
                    List<string> subCategoriesArrayAdapterList = new List<string>();

                    List<SubCategoriesListData> subCategoriesList = new List<SubCategoriesListData>();
                    for (int i = 0; i < _achievesInfo.CategoriesCount; i++) 
                    {
                        if (item.Key == _achievesInfo.ParentCategoryArray()[i].DisplayName) 
                        {
                            for (int j  = 0; j < _achievesInfo.ParentCategoryArray()[i].Projects.Count(); j++)
                            {
                                subCategoriesList.Add(new SubCategoriesListData()
                                {
                                    SubCategoryNameText = String.Format("{0}", _achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName),
                                    IsSubCategoryActive = true
                                });

                                subCategoriesArrayAdapterList.Add(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName);

                                _selectedSubCategoriesDictionary.Add(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName,
                                subCategoriesList[j].IsSubCategoryActive);
                            }

                            var subCategoriesAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow,
                                subCategoriesList, _selectedSubCategoriesDictionary, subCategoriesArrayAdapterList);

                            _subCategoriesListView.Adapter = subCategoriesAdapter;

                            LayoutInflater sublayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                            _subCategoriesListView.SetWillNotCacheDrawing(true);

                            
                        }

                        categoryButton.Click += delegate
                        {
                            if (!_isBarSubCategoriesListOpen)
                            {
                                _categoriesPopupWindow.Dismiss();
                                subCategoriesPopupWindow.ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout), 0, 0);
                                _isBarSubCategoriesListOpen = true;

                                return;
                            }

                            if (_isBarSubCategoriesListOpen)
                            {
                                subCategoriesPopupWindow.Dismiss();
                                _categoriesPopupWindow.Dismiss();
                                _isBarSubCategoriesListOpen = false;

                            }
                        };
                    }
                }
            }

            _navigationBarImageButton.Click += delegate
            {

                _navigationBarImageButton.StartAnimation(buttonClickAnimation);
                categoriesLinearLayout.RemoveAllViewsInLayout();
                categoriesLinearLayout.RemoveAllViews();

                checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);

                foreach (var item in _selectedCategoriesDictionary)
                {
                    if (item.Value == true)
                    {
                        var categoryButton = new Button(this);
                        categoryButton.Text = item.Key;

                        categoriesLinearLayout.AddView(categoryButton);

                        categoryButton.SetBackgroundColor(global::Android.Graphics.Color.Argb(2, 0, 0, 0));
                        categoryButton.Click += delegate { categoryButton.StartAnimation(buttonClickAnimation); };
                        categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
                        categoryButton.Gravity = GravityFlags.Left;
                        categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;
                        categoryButton.SetSingleLine(true);

                        //middle


                        ListView _subCategoriesListView = new ListView(this);
                        _subCategoriesListView.DividerHeight = 0;

                        PopupWindow subCategoriesPopupWindow = new PopupWindow(_subCategoriesListView,
                                    LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);

                        List<string> subCategoriesArrayAdapterList = new List<string>();

                        List<SubCategoriesListData> subCategoriesList = new List<SubCategoriesListData>();
                        for (int i = 0; i < _achievesInfo.CategoriesCount; i++)
                        {
                            if (item.Key == _achievesInfo.ParentCategoryArray()[i].DisplayName)
                            {
                                for (int j = 0; j < _achievesInfo.ParentCategoryArray()[i].Projects.Count(); j++)
                                {
                                    subCategoriesList.Add(new SubCategoriesListData()
                                    {
                                        SubCategoryNameText = String.Format("{0}", _achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName),
                                        IsSubCategoryActive = true
                                    });

                                    subCategoriesArrayAdapterList.Add(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName);

                                    if (!_selectedSubCategoriesDictionary.ContainsKey(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName))
                                    {
                                        _selectedSubCategoriesDictionary.Add(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName,
                                            subCategoriesList[j].IsSubCategoryActive);
                                    }
                                }

                                var subCategoriesAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow,
                                    subCategoriesList, _selectedSubCategoriesDictionary, subCategoriesArrayAdapterList);

                                _subCategoriesListView.Adapter = subCategoriesAdapter;

                                LayoutInflater sublayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                                _subCategoriesListView.SetWillNotCacheDrawing(true);


                            }

                            categoryButton.Click += delegate
                            {
                                if (!_isBarSubCategoriesListOpen)
                                {
                                    subCategoriesPopupWindow.Dismiss();
                                    _categoriesPopupWindow.Dismiss();
                                    subCategoriesPopupWindow.ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout), 0, 0);
                                    _isBarSubCategoriesListOpen = true;

                                    return;
                                }

                                if (_isBarSubCategoriesListOpen)
                                {
                                    _categoriesPopupWindow.Dismiss();
                                    subCategoriesPopupWindow.Dismiss();
                                    _isBarSubCategoriesListOpen = false;

                                }
                            };
                        }
                    }
                }

            };

        }

        public static int achievesCount = 0;
        private void CreateAchievementsViewObject()
        {
            List<AchievementsListData> achievementsList = new List<AchievementsListData>();

            Directory.CreateDirectory(@"/data/data/Achievements.AndroidPlatform/cache/achPics/");

            for (int i = 0; i < _achievesInfo.CategoriesCount; i++) 
            {
                for (int j = 0; j < _achievesInfo.ParentCategoryArray()[i].Projects.Count(); j++)   
                {
                    for (int k = 0; k < _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements.Count(); k++)   
                    {
                        FileStream fs = new FileStream(@"/data/data/Achievements.AndroidPlatform/cache/achPics/" +
                            _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].ApiName + ".PNG", FileMode.OpenOrCreate,
                            FileAccess.ReadWrite, FileShare.ReadWrite
                            );

                        if (!System.IO.File.Exists(@"/data/data/Achievements.AndroidPlatform/cache/achPics/" + "achive" +
                            _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].ApiName + ".PNG"))
                        {
                            GetImageBitmap(_achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].PicUrl).Compress(
                            Bitmap.CompressFormat.Jpeg, 100, fs);

                            System.IO.File.Copy(@"/data/data/Achievements.AndroidPlatform/cache/achPics/" +
                            _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].ApiName + ".PNG",
                            @"/data/data/Achievements.AndroidPlatform/cache/achPics/" + "achive" +
                            _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].ApiName + ".PNG");

                            System.IO.File.Delete(@"/data/data/Achievements.AndroidPlatform/cache/achPics/" +
                            _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].ApiName + ".PNG");
                        }

                        if (_selectedCategoriesDictionary[_achievesInfo.ParentCategoryArray()[i].DisplayName] == true
                            &&
                            _selectedSubCategoriesDictionary[_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName] == true)  
                        {
                            achievementsList.Add(new AchievementsListData()
                            {
                                AchieveApiName = String.Format("{0}",
                                _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].ApiName),
                                AchieveNameText = String.Format("{0}",
                                _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].DisplayName),
                                AchieveDescriptionText = String.Format("{0}",
                                _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].Description),
                                AchievePicUrl = String.Format("{0}",
                                _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].PicUrl),
                                AchieveReceivedTime = String.Format("{0}",
                                _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].CreateTime)
                            });

                            achievesCount++;
                        }

                    }
                }
            }

            ListView achievementsListView = FindViewById<ListView>(Resource.Id.AchivementsListView);

            achievementsListView.DividerHeight = 0;

            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.MainLayoutListRow, achievementsList);
            achievementsListView.Adapter = adapter;


            
        }


        void achievementsListView_Click(object sender, Android.Text.TextChangedEventArgs e)
        {
            Achieves.ParentCategory.ParentProject.Achieve achive = new Achieves.ParentCategory.ParentProject.Achieve();
            int iID = 0;
            int jID = 0;


            for (int i = 0; i < _achievesInfo.ParentCategoryArray().Count(); i++)   
            {
                for (int j = 0; j < _achievesInfo.ParentCategoryArray()[i].Projects.Count(); j++)   
                {
                    for (int k = 0; k < _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements.Count(); k++)   
                    {
                        if (_achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].ApiName == e.Text.ToString())   
                        {
                            achive = _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k];
                            iID = i;
                            jID = j;
                        }
                    }
                }
            }

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(LayoutInflaterService);
            ViewGroup relativeAgedSummary = new RelativeLayout(this);

            View layout = inflater.Inflate(Resource.Layout.BadgeListFrameLayout, relativeAgedSummary);

            TextView badgeName = (TextView)layout.FindViewById(Resource.Id.badgenameTextView);
            badgeName.Text = achive.DisplayName;

            ImageView badgeImage = (ImageView)layout.FindViewById(Resource.Id.BadgeImageView);
            badgeImage.SetImageBitmap(BitmapFactory.DecodeFile(@"/data/data/Achievements.install/cache/achPics/" + "achive" +
                achive.ApiName +
                ".PNG"
                ));

            TextView categoryNameProjectName = (TextView)layout.FindViewById(Resource.Id.categoryProjectTextView);
            categoryNameProjectName.Text = _achievesInfo.ParentCategoryArray()[iID].DisplayName + ", " + _achievesInfo.ParentCategoryArray()[iID].Projects[jID].DisplayName;

            TextView badgeHowWonderDescr = (TextView)layout.FindViewById(Resource.Id.howWasEarnDescrTextView);
            badgeHowWonderDescr.Text = achive.Description;

            var badgePopupWindow = new PopupWindow(layout,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);

            badgePopupWindow.ShowAsDropDown(FindViewById<ImageButton>(Resource.Id.NavigationBarImageButton), 0, 0);

            Button badgeReadyButton = (Button)layout.FindViewById(Resource.Id.badgereadybutton);

            badgeReadyButton.Click += delegate
            {
                badgeReadyButton.StartAnimation(buttonClickAnimation);
                badgePopupWindow.Dismiss();
            };

        }


        private Bitmap GetImageBitmap(String url)
        {
            Bitmap bm = null;

            Java.Net.URL aURL = new Java.Net.URL(url);
            Java.Net.HttpURLConnection conn = (Java.Net.HttpURLConnection)aURL.OpenConnection();
            conn.Connect();

            Stream stream = conn.InputStream;
            BufferedStream bsteam = new BufferedStream(stream);

            bm = BitmapFactory.DecodeStream(bsteam);
            bsteam.Close();
            stream.Close();

            return bm;
        }
    }
}
