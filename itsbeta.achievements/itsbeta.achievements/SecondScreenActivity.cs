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
using ItsBeta.Core;
using System.Threading;
using ZXing.Mobile;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class SecondScreenActivity : Activity
    {
        public static TextView _refreshEventListTextView;
        public static TextView _achieveListSelectedEventTextView;
        public static TextView _foundActionTextView;
        public static SecondScreenActivity _context;

        MobileBarcodeScanner _scanner;
        ServiceItsBeta _serviceItsBeta = new ServiceItsBeta();

        bool _isBarCategoriesListOpen = false;

        ListView _categoriesListView;
        ListView _achievementsListView;

        ImageButton _navigationBarImageButton;
        Animation _buttonClickAnimation;
        LinearLayout _linearLayoutInactive;
        AutoCompleteTextView _codeCompleteTextView;
        ProgressDialog _activationDialog;
        AlertDialog.Builder _activateMessageBadgeDialogBuilder;
        AlertDialog _activateMessageBadgeDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            _context = this;
            _activateMessageBadgeDialogBuilder = new AlertDialog.Builder(this);
            _achieveListSelectedEventTextView = new TextView(this);
            _refreshEventListTextView = new TextView(this);
            _foundActionTextView = new TextView(this);

            SetContentView(Resource.Layout.SecondScreenActivityLayout);
            _navigationBarImageButton = FindViewById<ImageButton>(Resource.Id.NavBar_ImageButton);

            _linearLayoutInactive = FindViewById<LinearLayout>(Resource.Id.secondscr_linearLayoutInactive);
            _linearLayoutInactive.Visibility = ViewStates.Gone;

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
            _codeCompleteTextView = (AutoCompleteTextView)addCodeView.FindViewById(Resource.Id.addcode_autoCompleteTextView);
            //codeCompleteTextView.SetDropDownBackgroundDrawable();
            addCodeRelativeLayout.AddView(addCodeView);
            
            Dialog addBadgeDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            addBadgeDialog.SetTitle("");
            addBadgeDialog.SetContentView(addBadgeRelativeLayout);

            Dialog addCodeDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            addCodeDialog.SetTitle("");
            addCodeDialog.SetContentView(addCodeRelativeLayout);
            
            badgesCount.Text = AppInfo._badgesCount.ToString();
            profileImageButtonFake.Click += delegate { profileImageButton.StartAnimation(_buttonClickAnimation); StartActivity(typeof(ProfileActivity)); };
            addCodeImageButtonFake.Click += delegate { _codeCompleteTextView.Text = ""; addCodeImageButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Show(); addBadgeRelativeLayout.StartAnimation(AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn)); };

            addBadgeCancelButton.Click += delegate { addBadgeCancelButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Dismiss(); };
            addCodeButton.Click += delegate { addCodeButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Dismiss(); addCodeDialog.Show(); addCodeRelativeLayout.StartAnimation(AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn));};
            addCodeCancelButton.Click += delegate { addCodeCancelButton.StartAnimation(_buttonClickAnimation); addCodeDialog.Dismiss(); };
            readQRCodeButton.Click += delegate { 
                readQRCodeButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Dismiss();
                _scanner = new MobileBarcodeScanner(this);
                _scanner.UseCustomOverlay = true;
                var zxingOverlay = LayoutInflater.FromContext(this).Inflate(Resource.Layout.QRReaderLayout, null);

                _scanner.CustomOverlay = zxingOverlay;

                _scanner.Scan().ContinueWith((t) =>
                {
                    if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        HandleScanResult(t.Result);
                });
                _foundActionTextView.TextChanged += delegate
                {
                    _scanner.Cancel();
                };
            };

            addCodeReadyButton.Click += delegate
            {
                if (_codeCompleteTextView.Text.Replace(" ", "") != "")
                {
                    addCodeReadyButton.StartAnimation(_buttonClickAnimation);
                    addCodeDialog.Dismiss();

                    _activationDialog = new ProgressDialog(this);
                    _activationDialog.SetMessage("Активация достижения...");
                    _activationDialog.SetCancelable(false);
                    _activationDialog.Show();

                    ThreadStart threadStart = new ThreadStart(AsyncActivization);
                    Thread loadThread = new Thread(threadStart);
                    loadThread.Start();
                }
                else
                {
                    Toast.MakeText(this, "Введите код активации", ToastLength.Short).Show();
                    addCodeReadyButton.StartAnimation(_buttonClickAnimation);
                }
            };

            CreateCategoriesViewObject();
            CreateSubCategoriesViewObject();
            CreateAchievementsViewObject();
            _refreshEventListTextView.TextChanged += delegate { CreateAchievementsViewObject(); };
            _achievementsListView.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(achievementsListView_ItemClick);
        }

        #region Categories List Region
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

        #region Projects List Region
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
                        views[id].StartAnimation(_buttonClickAnimation);

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

        #region Achievements List Region
        List<AchievementsListData> _achievementsList;
        private void CreateAchievementsViewObject()
        {
            _achievementsList = new List<AchievementsListData>();
            
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
                                _achievementsList.Add(new AchievementsListData()
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
                                    Bonuses = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Bonuses
                                });
                            }
                        }
                        
                    }
                }
            }

            _achievementsListView = FindViewById<ListView>(Resource.Id.secondscr_listView);

            _achievementsListView.DividerHeight = 0;

            _achievementsList = _achievementsList.OrderByDescending(x => x.AchieveReceivedDateTime).ToList();

            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.SecondScreenListRow, _achievementsList);
            _achievementsListView.Adapter = adapter;

            _achievementsListView.Focusable = false;
        }

        
        ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve achieve;
        void achievementsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int iID = 0;
            int jID = 0;


            for (int i = 0; i < AppInfo._achievesInfo.CategoryArray.Count(); i++)
            {
                for (int j = 0; j < AppInfo._achievesInfo.CategoryArray[i].Projects.Count(); j++)
                {
                    for (int k = 0; k < AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Count(); k++)
                    {
                        if (AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName == _achievementsList[(int)e.Id].AchieveApiName)
                        {
                            achieve = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k];
                            iID = i;
                            jID = j;
                        }
                    }
                }
            }


            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(LayoutInflaterService);

            ViewGroup relativeAgedSummary = new RelativeLayout(this);
            View layout = inflater.Inflate(Resource.Layout.BadgeWindowActivityLayout, relativeAgedSummary);
            ImageView badgeImage = (ImageView)layout.FindViewById(Resource.Id.badgewin_BadgeImageView);
            TextView badgeName = (TextView)layout.FindViewById(Resource.Id.badgewin_badgeTextView);

            TextView categoryNameProjectName = (TextView)layout.FindViewById(Resource.Id.badgewin_categ_projectTextView);
            TextView badgeHowWonderDescr = (TextView)layout.FindViewById(Resource.Id.badgewin_wonderdescrTextView);

            ImageButton badgeReadyButton = (ImageButton)layout.FindViewById(Resource.Id.badgewin_CloseImageButton);
            
            badgeName.Text = achieve.DisplayName;
            badgeImage.SetImageBitmap(BitmapFactory.DecodeFile(@"/data/data/itsbeta.achievements/cache/pictures/" + "achive" +achieve.ApiName + ".PNG"));
            categoryNameProjectName.Text = AppInfo._achievesInfo.CategoryArray[iID].DisplayName + ", " + AppInfo._achievesInfo.CategoryArray[iID].Projects[jID].DisplayName;
            badgeHowWonderDescr.Text = achieve.Description;

            LinearLayout bonusPaperListLinearLayout = (LinearLayout)layout.FindViewById(Resource.Id.bonuspaperlist_linearLayout);
            //
            bonusPaperListLinearLayout.RemoveAllViews();
            foreach (var bonus in achieve.Bonuses)
            {
                LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                View bonusView = layoutInflater.Inflate(Resource.Layout.BonusOnListRowLayout, null);

                ImageView bonusLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_GreenBonusImageView);
                ImageView discountLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_BlueBonusImageView);
                ImageView giftLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_VioletBonusImageView);

                ImageView bonusDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_greendescbackgroundImageView);
                ImageView discountDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_bluedescbackgroundImageView);
                ImageView giftDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_violetdescbackgroundImageView);

                TextView bonusName = (TextView)bonusView.FindViewById(Resource.Id.badgewin_bonusTextView);
                TextView bonusDescr = (TextView)bonusView.FindViewById(Resource.Id.badgewin_bonusdescrTextView);

                bonusLineImage.Visibility = ViewStates.Invisible;
                discountLineImage.Visibility = ViewStates.Invisible;
                giftLineImage.Visibility = ViewStates.Invisible;

                bonusDescrBackgroundImage.Visibility = ViewStates.Invisible;
                discountDescrBackgroundImage.Visibility = ViewStates.Invisible;
                giftDescrBackgroundImage.Visibility = ViewStates.Invisible;

                bonusDescr.Visibility = ViewStates.Invisible;
                bonusName.Visibility = ViewStates.Invisible;

                if (bonus.Type == "discount")
                {
                    bonusLineImage.Visibility = ViewStates.Invisible;
                    discountLineImage.Visibility = ViewStates.Visible;
                    giftLineImage.Visibility = ViewStates.Invisible;

                    bonusDescrBackgroundImage.Visibility = ViewStates.Invisible;
                    discountDescrBackgroundImage.Visibility = ViewStates.Visible;
                    giftDescrBackgroundImage.Visibility = ViewStates.Invisible;

                    bonusDescr.Visibility = ViewStates.Visible;
                    bonusName.Visibility = ViewStates.Visible;

                    bonusName.Text = "Скидка";
                    bonusDescr.Text = bonus.Description;

                    bonusPaperListLinearLayout.AddView(bonusView);
                }
                if (bonus.Type == "bonus")
                {
                    bonusLineImage.Visibility = ViewStates.Visible;
                    discountLineImage.Visibility = ViewStates.Invisible;
                    giftLineImage.Visibility = ViewStates.Invisible;

                    bonusDescrBackgroundImage.Visibility = ViewStates.Visible;
                    discountDescrBackgroundImage.Visibility = ViewStates.Invisible;
                    giftDescrBackgroundImage.Visibility = ViewStates.Invisible;

                    bonusDescr.Visibility = ViewStates.Visible;
                    bonusName.Visibility = ViewStates.Visible;

                    bonusName.Text = "Бонус";
                    bonusDescr.Text = bonus.Description;

                    bonusPaperListLinearLayout.AddView(bonusView);
                }
                if (bonus.Type == "present")
                {
                    bonusLineImage.Visibility = ViewStates.Invisible;
                    discountLineImage.Visibility = ViewStates.Invisible;
                    giftLineImage.Visibility = ViewStates.Visible;

                    bonusDescrBackgroundImage.Visibility = ViewStates.Invisible;
                    discountDescrBackgroundImage.Visibility = ViewStates.Invisible;
                    giftDescrBackgroundImage.Visibility = ViewStates.Visible;

                    bonusDescr.Visibility = ViewStates.Visible;
                    bonusName.Visibility = ViewStates.Visible;

                    bonusName.Text = "Подарок";
                    bonusDescr.Text = bonus.Description;

                    bonusPaperListLinearLayout.AddView(bonusView);
                }
            }

            var badgePopupWindow = new PopupWindow(layout,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
            badgePopupWindow.ShowAsDropDown(FindViewById<TextView>(Resource.Id.secaondscr_faketextView), 0, 0);

            badgeReadyButton.Click += delegate
            {
                badgeReadyButton.StartAnimation(_buttonClickAnimation);
                badgePopupWindow.Dismiss();
            };
        }

        #endregion

        #region Activation Region
        string activatedBadgeFbId = "null";
        string errorDescr = "null";
        ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve activatedAchieve;
        public void AsyncActivization()
        {
            string response = _serviceItsBeta.ActivateBadge(_codeCompleteTextView.Text, AppInfo._appaccess_token, AppInfo._user.FacebookUserID);
            if (response.StartsWith("badgefbId="))
            {
                activatedBadgeFbId = response.Replace("badgefbId=", "");
                AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId);
                activatedAchieve  = new Achieves.ParentCategory.ParentProject.Achieve();
                #region Load Badge Pic
                foreach (var category in AppInfo._achievesInfo.CategoryArray)
                {
                    foreach (var project in category.Projects)
                    {
                        foreach (var achieve in project.Achievements)
                        {
                            if (achieve.FbId == activatedBadgeFbId)
                            {
                                activatedAchieve = achieve;
                            }
                        }
                    }
                }

                FileStream fs = new FileStream(@"/data/data/itsbeta.achievements/cache/pictures/" +
                            activatedAchieve.ApiName + ".PNG", FileMode.OpenOrCreate,
                            FileAccess.ReadWrite, FileShare.ReadWrite
                            );

                if (!System.IO.File.Exists(@"/data/data/itsbeta.achievements/cache/pictures/" + "achive" +
                    activatedAchieve.ApiName + ".PNG"))
                {
                    Bitmap bitmap = GetImageBitmap(activatedAchieve.PicUrl);

                    bitmap.Compress(
                    Bitmap.CompressFormat.Png, 10, fs);
                    bitmap.Dispose();
                    fs.Flush();
                    fs.Close();

                    System.IO.File.Copy(@"/data/data/itsbeta.achievements/cache/pictures/" +
                    activatedAchieve.ApiName + ".PNG",
                    @"/data/data/itsbeta.achievements/cache/pictures/" + "achive" +
                    activatedAchieve.ApiName + ".PNG");

                    System.IO.File.Delete(@"/data/data/itsbeta.achievements/cache/pictures/" +
                    activatedAchieve.ApiName + ".PNG");
                }
                #endregion
                RunOnUiThread(() => CompleteActivation());                
            }
            if (response.StartsWith("error="))
            {
                errorDescr = response.Replace("error=", "");
                RunOnUiThread(() => _activationDialog.Dismiss());
                //RunOnUiThread(() =>CreateAchievementsViewObject());

                if (errorDescr == "obj not found")
                {
                    errorDescr = "Неверный код активации";
                }
                if (errorDescr == "activation code is used")
                {
                    errorDescr = "Код уже активирован";
                }
                
                _activateMessageBadgeDialogBuilder.SetTitle("Информация");
                _activateMessageBadgeDialogBuilder.SetMessage(errorDescr);
                _activateMessageBadgeDialogBuilder.SetPositiveButton("Ок", delegate { });

                RunOnUiThread(() => ShowAlertDialog());
                
            }
        }
        void CompleteActivation()
        {
            _activationDialog.Dismiss();
            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(LayoutInflaterService);
            ViewGroup relativeAgedSummary = new RelativeLayout(this);
            View layout = inflater.Inflate(Resource.Layout.ReceiveBadgeLayount, relativeAgedSummary);

            ImageView badgeImage = (ImageView)layout.FindViewById(Resource.Id.receivebadge_BadgeImageView);
            ImageView bonusLineImage = (ImageView)layout.FindViewById(Resource.Id.receivebadge_GreenBonusImageView);
            ImageView discountLineImage = (ImageView)layout.FindViewById(Resource.Id.receivebadge_BlueBonusImageView);
            ImageView giftLineImage = (ImageView)layout.FindViewById(Resource.Id.receivebadge_VioletBonusImageView);
            ImageButton badgeReadyButton = (ImageButton)layout.FindViewById(Resource.Id.receivebadge_CloseImageButton);

            TextView profileName = (TextView)layout.FindViewById(Resource.Id.receivebadge_ProfileNameTextView);
            TextView bonusName = (TextView)layout.FindViewById(Resource.Id.receivebadge_bonustextView);
            TextView discountName = (TextView)layout.FindViewById(Resource.Id.receivebadge_discounttextView);
            TextView giftName = (TextView)layout.FindViewById(Resource.Id.receivebadge_presenttextView);
            TextView bonusDescr = (TextView)layout.FindViewById(Resource.Id.receivebadge_bonusdesctextView);
            profileName.Text = AppInfo._user.Fullname;

            badgeImage.SetImageBitmap(BitmapFactory.DecodeFile(@"/data/data/itsbeta.achievements/cache/pictures/" + "achive" + activatedAchieve.ApiName + ".PNG"));

            bonusLineImage.Visibility = ViewStates.Invisible;
            bonusDescr.Visibility = ViewStates.Invisible;
            bonusName.Visibility = ViewStates.Invisible;

            discountLineImage.Visibility = ViewStates.Invisible;
            discountName.Visibility = ViewStates.Invisible;

            giftLineImage.Visibility = ViewStates.Invisible;
            giftName.Visibility = ViewStates.Invisible;

            foreach (var bonus in activatedAchieve.Bonuses)
            {
                if (bonus.Type == "discount")
                {
                    discountLineImage.Visibility = ViewStates.Visible;
                    discountName.Visibility = ViewStates.Visible;
                }
                if (bonus.Type == "bonus")
                {
                    bonusLineImage.Visibility = ViewStates.Visible;
                    bonusDescr.Visibility = ViewStates.Visible;
                    bonusName.Visibility = ViewStates.Visible;
                }
                if (bonus.Type == "present")
                {
                    giftLineImage.Visibility = ViewStates.Visible;
                    giftName.Visibility = ViewStates.Visible;
                }
            }

            var badgePopupWindow = new PopupWindow(layout,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
            badgePopupWindow.ShowAsDropDown(FindViewById<TextView>(Resource.Id.secaondscr_faketextView), 0, 0);

            badgeReadyButton.Click += delegate
            {
                badgeReadyButton.StartAnimation(_buttonClickAnimation);
                CreateAchievementsViewObject();
                badgePopupWindow.Dismiss();
            };
        }
        void ShowAlertDialog()
        {
            _activateMessageBadgeDialog = _activateMessageBadgeDialogBuilder.Create();
            _activateMessageBadgeDialog.Show();
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
        #endregion

        #region QR Code Reader Region

        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                _foundActionTextView.Text = result.Text;
                msg = "QR код найден";// + result.Text;
            }
            else
                msg = "Сканирование отменено";

            this.RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Short).Show());
        }
        #endregion
    }

}