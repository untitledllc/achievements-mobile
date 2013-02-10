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
using itsbeta.achievements.gui;
using System.IO;
using Android.Views.Animations;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class SecondScreenActivity : Activity
    {
        ListView _categoriesListView;
        ImageButton _navigationBarImageButton;
        Animation buttonClickAnimation;
        public static TextView RefreshEventListTextView;
        bool _isBarCategoriesListOpen = false;
        public static SecondScreenActivity _context;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            _context = this;
            SetContentView(Resource.Layout.SecondScreenActivityLayout);
            _navigationBarImageButton = FindViewById<ImageButton>(Resource.Id.NavBar_ImageButton);

            RefreshEventListTextView = new TextView(this);
            ImageButton profileImageButton = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_ProfileScreenImageButton);
            ImageButton profileImageButtonFake = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_ProfileScreenImageButtonFake);
            ImageButton addCodeImageButton = FindViewById<ImageButton>(Resource.Id.NavBar_addcodeImageButton);
            ImageButton addCodeImageButtonFake = FindViewById<ImageButton>(Resource.Id.NavBar_addcodeImageButtonFake);
            TextView badgesCount = FindViewById<TextView>(Resource.Id.NavBar_AchievesCountTextView);

            badgesCount.Text = AppInfo._badgesCount.ToString();
            profileImageButtonFake.Click += delegate { profileImageButton.StartAnimation(buttonClickAnimation); StartActivity(typeof(ProfileActivity)); };
            addCodeImageButtonFake.Click += delegate { addCodeImageButton.StartAnimation(buttonClickAnimation); };

            CreateCategoriesViewObject();
            CreateAchievementsViewObject();
            RefreshEventListTextView.TextChanged += delegate { CreateAchievementsViewObject(); };
        }

        #region
        List<CategoriesListData> _categoriesList;
        PopupWindow _categoriesPopupWindow;
        private void CreateCategoriesViewObject()
        {

            _categoriesList = new List<CategoriesListData>();

            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                _categoriesList.Add(new CategoriesListData()
                {
                    CategoryNameText = String.Format("{0}",
                        AppInfo._achievesInfo.CategoryArray[i].DisplayName),
                    IsCategoryActive = true
                });

                if (!AppInfo._selectedCategoriesDictionary.ContainsKey(AppInfo._achievesInfo.CategoryArray[i].DisplayName))
                {
                    AppInfo._selectedCategoriesDictionary.Add(AppInfo._achievesInfo.CategoryArray[i].DisplayName, _categoriesList[i].IsCategoryActive);
                }
            }

            _categoriesListView = new ListView(this);

            CategoriesListItemAdapter categoriesAdapter = new CategoriesListItemAdapter(this, Resource.Layout.SecondScreenDropDownListRow,
                _categoriesList, AppInfo._selectedCategoriesDictionary);

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
                    _categoriesPopupWindow.ShowAsDropDown(FindViewById<ImageButton>(Resource.Id.NavBar_ImageButton), 0, 0);
                    _isBarCategoriesListOpen = true;
                    return;
                }
                if (_isBarCategoriesListOpen)
                {
                    _categoriesPopupWindow.Dismiss();
                    //categoriesLinearLayout.StartAnimation(categoriesChosedAnimation);
                    _isBarCategoriesListOpen = false;
                }
            };
        }
        #endregion

        #region
        private void CreateAchievementsViewObject()
        {
            List<AchievementsListData> achievementsList = new List<AchievementsListData>();

            //Directory.CreateDirectory(@"/data/data/Achievements.AndroidPlatform/cache/achPics/");

            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                for (int j = 0; j < AppInfo._achievesInfo.CategoryArray[i].Projects.Count(); j++)
                {
                    for (int k = 0; k < AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Count(); k++)
                    {
                        if (AppInfo._selectedCategoriesDictionary[AppInfo._achievesInfo.CategoryArray[i].DisplayName] == true
                            /*&&
                            AppInfo._selectedSubCategoriesDictionary[AppInfo._achievesInfo.CategoryArray[i].Projects[j].DisplayName] == true*/)
                        {
                            achievementsList.Add(new AchievementsListData()
                            {
                                AchieveApiName = String.Format("{0}",
                                AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName),
                                AchieveNameText = String.Format("{0}",
                                AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].DisplayName),
                                AchieveDescriptionText = String.Format("{0}",
                                AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Description),
                                AchievePicUrl = String.Format("{0}",
                                AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].PicUrl),
                                AchieveReceivedTime = String.Format("{0}",
                                AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].CreateTime),
                                BonusStatus = String.Format("{0}",
                                AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].BonusStatus)
                            });
                        }

                    }
                }
            }

            ListView achievementsListView = FindViewById<ListView>(Resource.Id.secondscr_listView);

            achievementsListView.DividerHeight = 0;

            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.SecondScreenListRow, achievementsList);
            achievementsListView.Adapter = adapter;
        }
        #endregion



    }
}