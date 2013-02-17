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
        LinearLayout _linearLayoutInactive;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            _context = this;
            SetContentView(Resource.Layout.SecondScreenActivityLayout);
            _navigationBarImageButton = FindViewById<ImageButton>(Resource.Id.NavBar_ImageButton);

            _linearLayoutInactive = FindViewById<LinearLayout>(Resource.Id.secondscr_linearLayoutInactive);
            _linearLayoutInactive.Visibility = ViewStates.Gone;

            RefreshEventListTextView = new TextView(this);
            ImageButton profileImageButton = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_ProfileScreenImageButton);
            ImageButton profileImageButtonFake = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_ProfileScreenImageButtonFake);
            ImageButton addCodeImageButton = FindViewById<ImageButton>(Resource.Id.NavBar_addcodeImageButton);
            ImageButton addCodeImageButtonFake = FindViewById<ImageButton>(Resource.Id.NavBar_addcodeImageButtonFake);
            TextView badgesCount = FindViewById<TextView>(Resource.Id.NavBar_AchievesCountTextView);


            LayoutInflater addBadgeMenulayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            RelativeLayout addBadgeRelativeLayout = new RelativeLayout(this);
            View addBadgeView = addBadgeMenulayoutInflater.Inflate(Resource.Layout.AddBadgeMenuLayoutLayout, null);
            Button addBadgeCancelButton = (Button)addBadgeView.FindViewById(Resource.Id.addbadge_cancelButton);
            Button readQRCodeButton = (Button)addBadgeView.FindViewById(Resource.Id.addbadge_readQRButton);
            Button addCodeButton = (Button)addBadgeView.FindViewById(Resource.Id.addbadge_addcodeButton);
            addBadgeRelativeLayout.AddView(addBadgeView);
            
            LayoutInflater addCodeMenulayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            RelativeLayout addCodeRelativeLayout = new RelativeLayout(this);
            View addCodeView = addCodeMenulayoutInflater.Inflate(Resource.Layout.EnterCodeLayout, null);
            Button addCodeCancelButton = (Button)addCodeView.FindViewById(Resource.Id.addcode_cancelButton);
            Button addCodeReadyButton = (Button)addCodeView.FindViewById(Resource.Id.addcode_readyButton);
            AutoCompleteTextView codeCompleteTextView = (AutoCompleteTextView)addCodeView.FindViewById(Resource.Id.addcode_autoCompleteTextView);
            //codeCompleteTextView.SetDropDownBackgroundDrawable();
            addCodeRelativeLayout.AddView(addCodeView);
            
            Dialog addBadgeDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            addBadgeDialog.SetTitle("");
            addBadgeDialog.SetContentView(addBadgeRelativeLayout);

            Dialog addCodeDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            addCodeDialog.SetTitle("");
            addCodeDialog.SetContentView(addCodeRelativeLayout);
            

            badgesCount.Text = AppInfo._badgesCount.ToString();
            profileImageButtonFake.Click += delegate { profileImageButton.StartAnimation(buttonClickAnimation); StartActivity(typeof(ProfileActivity)); };
            addCodeImageButtonFake.Click += delegate { addCodeImageButton.StartAnimation(buttonClickAnimation); addBadgeDialog.Show(); addBadgeRelativeLayout.StartAnimation(AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn)); };

            addBadgeCancelButton.Click += delegate { addBadgeCancelButton.StartAnimation(buttonClickAnimation); addBadgeDialog.Dismiss(); };
            addCodeButton.Click += delegate { addCodeButton.StartAnimation(buttonClickAnimation); addBadgeDialog.Dismiss(); addCodeDialog.Show(); addCodeRelativeLayout.StartAnimation(AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn));};
            addCodeCancelButton.Click += delegate { addCodeCancelButton.StartAnimation(buttonClickAnimation); addCodeDialog.Dismiss(); };
            readQRCodeButton.Click += delegate { readQRCodeButton.StartAnimation(buttonClickAnimation); addBadgeDialog.Dismiss();  StartActivity(typeof(QRReaderActivity)); };


            addCodeReadyButton.Click += delegate
            {
                addCodeReadyButton.StartAnimation(buttonClickAnimation);

            };


            CreateCategoriesViewObject();
            CreateSubCategoriesViewObject();
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
            //_categoriesListView.SetWillNotCacheDrawing(true);

            _categoriesPopupWindow = new PopupWindow(_categoriesListView,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);

            _navigationBarImageButton.Click += delegate
            {
                if (!_isBarCategoriesListOpen)
                {
                    _linearLayoutInactive.Visibility = ViewStates.Visible;
                    _categoriesPopupWindow.ShowAsDropDown(FindViewById<ImageButton>(Resource.Id.NavBar_ImageButton), 0, 0);
                    _isBarCategoriesListOpen = true;
                    return;
                }
                if (_isBarCategoriesListOpen)
                {
                    _linearLayoutInactive.Visibility = ViewStates.Gone;
                    _categoriesPopupWindow.Dismiss();
                    CreateSubCategoriesViewObject();
                    //categoriesLinearLayout.StartAnimation(categoriesChosedAnimation);
                    _isBarCategoriesListOpen = false;
                }
            };
        }
        #endregion


        private void CreateSubCategoriesViewObject()
        {
            LinearLayout secondscr_RowLinearLayout = FindViewById<LinearLayout>(Resource.Id.secondscr_categoriesbar_linearLayout);
            int trueDicValCount = AppInfo._selectedCategoriesDictionary.Where(e => e.Value == true).Count();
            bool isViewOpen = false;

            secondscr_RowLinearLayout.RemoveAllViews();

            foreach (var item in AppInfo._selectedCategoriesDictionary)
            {
                if (item.Value == true)
                {
                    LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                    View view = layoutInflater.Inflate(Resource.Layout.SecondScreenCategoryRow, null);
                    view.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent, 1f);

                    TextView categoryName = (TextView)view.FindViewById(Resource.Id.secondscr_CategNameRowTextView);
                    categoryName.Text = item.Key;

                    secondscr_RowLinearLayout.AddView(view);

                    view.Click += delegate 
                    {
                        view.StartAnimation(buttonClickAnimation);
                    };
                }
            }

            ////_navigationBarImageButton.Click += delegate
            ////{

            ////    _navigationBarImageButton.StartAnimation(buttonClickAnimation);
            ////    categoriesLinearLayout.RemoveAllViewsInLayout();
            ////    categoriesLinearLayout.RemoveAllViews();

            ////    checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);

            ////    foreach (var item in _selectedCategoriesDictionary)
            ////    {
            ////        if (item.Value == true)
            ////        {
            ////            var categoryButton = new Button(this);
            ////            categoryButton.Text = item.Key;

            ////            categoriesLinearLayout.AddView(categoryButton);

            ////            categoryButton.SetBackgroundColor(global::Android.Graphics.Color.Argb(2, 0, 0, 0));
            ////            categoryButton.Click += delegate { categoryButton.StartAnimation(buttonClickAnimation); };
            ////            categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
            ////            categoryButton.Gravity = GravityFlags.Left;
            ////            categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;
            ////            categoryButton.SetSingleLine(true);

            ////            //middle


            ////            ListView _subCategoriesListView = new ListView(this);
            ////            _subCategoriesListView.DividerHeight = 0;

            ////            PopupWindow subCategoriesPopupWindow = new PopupWindow(_subCategoriesListView,
            ////                        LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);

            ////            List<string> subCategoriesArrayAdapterList = new List<string>();

            ////            List<SubCategoriesListData> subCategoriesList = new List<SubCategoriesListData>();
            ////            for (int i = 0; i < _achievesInfo.CategoriesCount; i++)
            ////            {
            ////                if (item.Key == _achievesInfo.ParentCategoryArray()[i].DisplayName)
            ////                {
            ////                    for (int j = 0; j < _achievesInfo.ParentCategoryArray()[i].Projects.Count(); j++)
            ////                    {
            ////                        subCategoriesList.Add(new SubCategoriesListData()
            ////                        {
            ////                            SubCategoryNameText = String.Format("{0}", _achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName),
            ////                            IsSubCategoryActive = true
            ////                        });

            ////                        subCategoriesArrayAdapterList.Add(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName);

            ////                        if (!_selectedSubCategoriesDictionary.ContainsKey(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName))
            ////                        {
            ////                            _selectedSubCategoriesDictionary.Add(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName,
            ////                                subCategoriesList[j].IsSubCategoryActive);
            ////                        }
            ////                    }

            ////                    var subCategoriesAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow,
            ////                        subCategoriesList, _selectedSubCategoriesDictionary, subCategoriesArrayAdapterList);

            ////                    _subCategoriesListView.Adapter = subCategoriesAdapter;

            ////                    LayoutInflater sublayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            ////                    _subCategoriesListView.SetWillNotCacheDrawing(true);


            ////                }

            ////                categoryButton.Click += delegate
            ////                {
            ////                    if (!_isBarSubCategoriesListOpen)
            ////                    {
            ////                        subCategoriesPopupWindow.Dismiss();
            ////                        _categoriesPopupWindow.Dismiss();
            ////                        subCategoriesPopupWindow.ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout), 0, 0);
            ////                        _isBarSubCategoriesListOpen = true;

            ////                        return;
            ////                    }

            ////                    if (_isBarSubCategoriesListOpen)
            ////                    {
            ////                        _categoriesPopupWindow.Dismiss();
            ////                        subCategoriesPopupWindow.Dismiss();
            ////                        _isBarSubCategoriesListOpen = false;

            ////                    }
            ////                };
            ////            }
            ////        }
            ////    }

            ////};

        }



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

    internal class CategoryRowObjects
    {
        public bool IsActive { get; set; }


    }
}