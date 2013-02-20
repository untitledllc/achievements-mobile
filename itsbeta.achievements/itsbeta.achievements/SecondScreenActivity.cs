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
using Android.Graphics;

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
        public static TextView AchieveListSelectedEventTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            _context = this;
            AchieveListSelectedEventTextView = new TextView(this);
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

        #region
        PopupWindow[] _projectsPopupWindows;
        IList<string> _projectsPopupWindowId;
        private void CreateSubCategoriesViewObject()
        {
            AppInfo._selectedSubCategoriesDictionary = new Dictionary<string, bool>();
            _projectsPopupWindowId = new List<string>();
            LinearLayout secondscr_RowLinearLayout = FindViewById<LinearLayout>(Resource.Id.secondscr_categoriesbar_linearLayout);
            int trueDicValCount = AppInfo._selectedCategoriesDictionary.Where(e => e.Value == true).Count();
            bool isViewOpen = false;
            int prevOpenedId = -1;
            View[] views = new View[trueDicValCount];


            if (_projectsPopupWindows != null)
            {
                foreach (var window in _projectsPopupWindows)
                {
                    window.Dismiss();
                }
            }
            _projectsPopupWindows = new PopupWindow[trueDicValCount];

            foreach (var category in AppInfo._selectedCategoriesDictionary.Where(e => e.Value == true))
	        {
                _projectsPopupWindowId.Add(category.Key);
	        }


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

                    int id = 0;
                    for (int i = 0; i < _projectsPopupWindowId.Count; i++)
                    {
                        if (_projectsPopupWindowId[i] == item.Key)
                        {
                            id = i;
                        }
                    }

                    views[id] = view;
                    secondscr_RowLinearLayout.AddView(views[id]);

                    //всё что сверху сами вьюхи. нижу будет все что связано с попапами:
                    ListView subCategoriesListView = new ListView(this);
                    List<SubCategoriesListData> subCategoriesList = new List<SubCategoriesListData>();
                    List<string> subCategoriesArrayAdapterList = new List<string>();
                    LayoutInflater sublayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);

                    var category = AppInfo._achievesInfo.CategoryArray.Where(x => x.DisplayName == item.Key).FirstOrDefault();
                    for (int j = 0; j < category.Projects.Count(); j++)
                    {
                        subCategoriesList.Add(new SubCategoriesListData()
                        {
                            SubCategoryNameText = category.Projects[j].DisplayName,
                            IsSubCategoryActive = true
                        });

                        subCategoriesArrayAdapterList.Add(category.Projects[j].DisplayName);

                        AppInfo._selectedSubCategoriesDictionary.Add(category.Projects[j].DisplayName,
                        subCategoriesList[j].IsSubCategoryActive);
                    }

                    var subCategoriesAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.SecondScreenDropDownListRow,
                        subCategoriesList, AppInfo._selectedSubCategoriesDictionary, subCategoriesArrayAdapterList);
                    subCategoriesListView.Adapter = subCategoriesAdapter;
                    subCategoriesListView.DividerHeight = 0;

                    

                    _projectsPopupWindows[id] = new PopupWindow(subCategoriesListView,
                        LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);

                    views[id].Click += delegate 
                    {
                        views[id].StartAnimation(buttonClickAnimation);

                        for (int i = 0; i < _projectsPopupWindows.Count(); i++)
                        {
                            if (i == id)
                            {
                                if (!isViewOpen)
                                {
                                    prevOpenedId = i;
                                    _projectsPopupWindows[id].ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.secondscr_categoriesbar_linearLayout), 0, 0);
                                    views[id].SetBackgroundResource(Resource.Drawable.Categories_btn_press);
                                    _linearLayoutInactive.Visibility = ViewStates.Visible;
                                    isViewOpen = true;
                                    return;
                                }
                            }
                        }
                        if (isViewOpen && prevOpenedId != -1)
                        {
                            _projectsPopupWindows[prevOpenedId].Dismiss();
                            _linearLayoutInactive.Visibility = ViewStates.Invisible;
                            views[prevOpenedId].SetBackgroundResource(Resource.Drawable.Categories_btn_norm);
                            isViewOpen = false;
                            if (id != prevOpenedId)
                            {
                                _projectsPopupWindows[id].ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.secondscr_categoriesbar_linearLayout), 0, 0);
                                views[id].SetBackgroundResource(Resource.Drawable.Categories_btn_press);
                                _linearLayoutInactive.Visibility = ViewStates.Visible;
                                prevOpenedId = id;
                                isViewOpen = true;
                            }
                        }
                    };
                }
            }
        }
        #endregion

        #region
        private void CreateAchievementsViewObject()
        {
            List<AchievementsListData> achievementsList = new List<AchievementsListData>();
            
            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                for (int j = 0; j < AppInfo._achievesInfo.CategoryArray[i].Projects.Count(); j++)
                {
                    for (int k = 0; k < AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Count(); k++)
                    {
                        if (AppInfo._selectedCategoriesDictionary.ContainsKey(AppInfo._achievesInfo.CategoryArray[i].DisplayName)
                            && AppInfo._selectedSubCategoriesDictionary.ContainsKey(AppInfo._achievesInfo.CategoryArray[i].Projects[j].DisplayName))
                        {
                            if (AppInfo._selectedCategoriesDictionary[AppInfo._achievesInfo.CategoryArray[i].DisplayName] == true
                            &&
                            AppInfo._selectedSubCategoriesDictionary[AppInfo._achievesInfo.CategoryArray[i].Projects[j].DisplayName] == true)
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
            }

            ListView achievementsListView = FindViewById<ListView>(Resource.Id.secondscr_listView);

            achievementsListView.DividerHeight = 0;

            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.SecondScreenListRow, achievementsList);
            achievementsListView.Adapter = adapter;



            AchieveListSelectedEventTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(AchieveListSelectedEventTextView_TextChanged);

        }


        ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve achive;
        void AchieveListSelectedEventTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            int iID = 0;
            int jID = 0;


            for (int i = 0; i < AppInfo._achievesInfo.CategoryArray.Count(); i++)
            {
                for (int j = 0; j < AppInfo._achievesInfo.CategoryArray[i].Projects.Count(); j++)
                {
                    for (int k = 0; k < AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Count(); k++)
                    {
                        if (AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName == e.Text.ToString())
                        {
                            achive = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k];
                            iID = i;
                            jID = j;
                        }
                    }
                }
            }


            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(LayoutInflaterService);
            ViewGroup relativeAgedSummary = new RelativeLayout(this);

            View layout = inflater.Inflate(Resource.Layout.BadgeWindowActivityLayout, relativeAgedSummary);

            TextView badgeName = (TextView)layout.FindViewById(Resource.Id.badgewin_badgeTextView);
            badgeName.Text = achive.DisplayName;

            ImageView badgeImage = (ImageView)layout.FindViewById(Resource.Id.badgewin_BadgeImageView);
            badgeImage.SetImageBitmap(BitmapFactory.DecodeFile(@"/data/data/itsbeta.achievements/cache/pictures/" + "achive" +
                achive.ApiName +
                ".PNG"
                ));

            TextView categoryNameProjectName = (TextView)layout.FindViewById(Resource.Id.badgewin_categ_projectTextView);
            categoryNameProjectName.Text = AppInfo._achievesInfo.CategoryArray[iID].DisplayName + ", " + AppInfo._achievesInfo.CategoryArray[iID].Projects[jID].DisplayName;

            TextView badgeHowWonderDescr = (TextView)layout.FindViewById(Resource.Id.badgewin_wonderdescrTextView);
            badgeHowWonderDescr.Text = achive.Description;


            var badgePopupWindow = new PopupWindow(layout,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);

            badgePopupWindow.ShowAsDropDown(FindViewById<TextView>(Resource.Id.secaondscr_faketextView), 0, 0);

            ImageButton badgeReadyButton = (ImageButton)layout.FindViewById(Resource.Id.badgewin_CloseImageButton);

            badgeReadyButton.Click += delegate
            {
                badgeReadyButton.StartAnimation(buttonClickAnimation);
                badgePopupWindow.Dismiss();
            };
        }

        #endregion


        
    }

}