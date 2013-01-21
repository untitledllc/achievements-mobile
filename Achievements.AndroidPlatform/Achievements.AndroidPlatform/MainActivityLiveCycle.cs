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

namespace Achievements.AndroidPlatform
{
    [Activity(Label = "Achievements", 
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public partial class MainActivity : Activity
    {
        string access_token = "059db4f010c5f40bf4a73a28222dd3e3";
        string player_id = "50f4603a947a06706e000002";


        public static Display _display;
        bool _isBarCategoriesListOpen = false;
        ListView _categoriesListView;
        ListView _subCategoriesListView;

        public static Dictionary<string,bool> _selectedCategoriesDictionary = new Dictionary<string,bool>();
        public static Dictionary<string, bool> _selectedSubCategoriesDictionary = new Dictionary<string, bool>();

        Animation categoriesChosedAnimation;
        Animation buttonClickAnimation;

        ImageButton _navigationBarImageButton;
        LinearLayout categoriesLinearLayout;
        ImageButton _navigationBarMenuImageButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _display = WindowManager.DefaultDisplay;

            SetContentView(Resource.Layout.MainActivityLayout);

            categoriesChosedAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.SlideInLeft);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);

            categoriesLinearLayout = FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout);
            _navigationBarImageButton = FindViewById<ImageButton>(Resource.Id.NavigationBarImageButton);
            _navigationBarMenuImageButton = FindViewById<ImageButton>(Resource.Id.NavigationBarMenuImageButton);

            _navigationBarMenuImageButton.Click += delegate { StartActivity(typeof(ProfileActivity)); };

            #region CategoriesList Local
            CreateCategoriesViewObject();
            #endregion

            #region SubCategories Local
            CreateSubCategoriesViewObject();
            #endregion

            #region AchievementsList Local
            CreateAchievementsViewObject();
            #endregion

        }

    }

}