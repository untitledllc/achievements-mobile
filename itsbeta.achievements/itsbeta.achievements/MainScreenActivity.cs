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
using Android.Views.InputMethods;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public partial class MainScreenActivity : Activity
    {
        static Animation _buttonClickAnimation;
        static Animation _fadeoutClickAnimation;
        static Button _inactiveListButton;
        static Button _inactiveAllButton;
        static Vibrator _vibe;
        static TextView _refreshAchTextView;
        public static MainScreenActivity _context;

        public static ListView _achievementsListView;

        public static bool _isAchListItemClicked = false;
        public static TextView _foundActionTextView;
        public static bool _isLogout = false;
        TextView _badgesCountDisplay;

        Typeface _font;

        ProgressDialog _progressDialog;
        AlertDialog.Builder _activateMessageBadgeDialogBuilder;
        AlertDialog _activateMessageBadgeDialog;

        Dialog _wrongCodeDialog;
        TextView _wrongCodeDialogTitle;
        TextView _wrongCodeDialogMessage;
        TextView _progressDialogMessage;
        Button _wrongCodeDialogReadyButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _context = this;
            SetContentView(Resource.Layout.secondscreenactivitylayout);
            _font = Typeface.CreateFromAsset(this.Assets, "Roboto-Light.ttf");  

            _achievementsListView = (ListView)FindViewById(Resource.Id.secondscr_listView);


            _foundActionTextView = new TextView(this);
            _refreshProjectsAndAchTextView = new TextView(this);

            _vibe = (Vibrator)this.GetSystemService(Context.VibratorService);
            _badgesCountDisplay = FindViewById<TextView>(Resource.Id.NavBar_AchievesCountTextView);


            //WrongCodeDialog
            RelativeLayout wrongCodeDialogRelativeLayout = new RelativeLayout(this);
            LayoutInflater wrongCodeDialoglayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            View wrongCodeDialogView = wrongCodeDialoglayoutInflater.Inflate(Resource.Layout.wrongcodedialoglayout, null);
            _wrongCodeDialogReadyButton = (Button)wrongCodeDialogView.FindViewById(Resource.Id.readyButton);
            _wrongCodeDialogTitle = (TextView)wrongCodeDialogView.FindViewById(Resource.Id.textView1);
            _wrongCodeDialogMessage = (TextView)wrongCodeDialogView.FindViewById(Resource.Id.textView2);


            wrongCodeDialogRelativeLayout.AddView(wrongCodeDialogView);
            _wrongCodeDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            _wrongCodeDialog.SetTitle("");
            _wrongCodeDialog.SetContentView(wrongCodeDialogRelativeLayout);

            _wrongCodeDialogReadyButton.Click += delegate { _wrongCodeDialog.Dismiss(); };
            //

            //ProgressDialog
            RelativeLayout progressDialogRelativeLayout = new RelativeLayout(this);
            LayoutInflater progressDialoglayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            View progressDialogView = progressDialoglayoutInflater.Inflate(Resource.Layout.progressdialoglayout, null);
            _progressDialogMessage = (TextView)progressDialogView.FindViewById(Resource.Id.progressDialogMessageTextView);
            progressDialogRelativeLayout.AddView(progressDialogView);
            _progressDialog = new ProgressDialog(this, Resource.Style.FullHeightDialog);
            _progressDialog.Show();
            _progressDialog.SetContentView(progressDialogRelativeLayout);
            _progressDialog.Dismiss();
            _progressDialog.SetCanceledOnTouchOutside(false);
            //
            

            ImageButton profileImageButton = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_ProfileScreenImageButton);
            ImageButton profileImageButtonFake = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_ProfileScreenImageButtonFake);

            ImageButton refreshImageButton = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_RefreshImageButton);
            ImageButton refreshImageButtonFake = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_RefreshImageButtonFake);
            

            _badgesCountDisplay.Text = AppInfo._badgesCount.ToString();
            profileImageButtonFake.Click += delegate
            {
                if (!_badgePopupWindow.IsShowing)
                {
                    _vibe.Vibrate(50); profileImageButton.StartAnimation(_buttonClickAnimation); StartActivity(typeof(ProfileActivity));
                }
            };

            refreshImageButtonFake.Click += delegate
            {
                if (!_badgePopupWindow.IsShowing)
                {
                    _vibe.Vibrate(50);
                    if (AppInfo.IsLocaleRu)
                    {
                        _progressDialogMessage.Text = "Обновление...";
                    } 
                    else
                    {
                        _progressDialogMessage.Text = "Refreshing...";
                    }
                    _progressDialog.Show();
                    refreshImageButton.StartAnimation(_buttonClickAnimation);
                    OnBadgeListRefresh();
                }
            };



            //.............................................................................
            _refreshAchTextView = new TextView(this);
            _badgePopupWindow = new PopupWindow(this);
            _vibe = (Vibrator)this.GetSystemService(Context.VibratorService);
            _inactiveListButton = FindViewById<Button>(Resource.Id.secondscr_inactiveListButton);
            _inactiveAllButton = FindViewById<Button>(Resource.Id.secondscr_inactiveAllButton);


            _inactiveAllButton.Visibility = ViewStates.Gone;
            _inactiveListButton.Visibility = ViewStates.Gone;

            _buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            _fadeoutClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeOut);
            _categoryViewRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.secondscr_categrowrelativeLayout);
            _subcategoryViewRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.secondscr_projectsrelativeLayout);

            GetCategoryView();
            GetProjectsView();
            GetAchievementsView();
            GetActivationDialog();

            _refreshProjectsAndAchTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(_refreshProjectsAndAchTextView_TextChanged);
            _refreshAchTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(_refreshAchTextView_TextChanged);
            _subcategoryViewRelativeLayout.Click += new EventHandler(_subcategoryViewRelativeLayout_Click);
            _inactiveListButton.Click += new EventHandler(_inactiveListButton_Click);
            _inactiveAllButton.Click += new EventHandler(_inactiveAllButton_Click);
            _achievementsListView.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(achievementsListView_ItemClick);
            _categoriesListView.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(_categoriesListView_ItemClick);
            _subcategoriesListView.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(_subcategoriesListView_ItemClick);

            _categoryViewRelativeLayout.Click += new EventHandler(_categoryViewRelativeLayout_Click);
        }


        void _inactiveAllButton_Click(object sender, EventArgs e)
        {
            _inactiveAllButton.Visibility = ViewStates.Gone;
        }

        void _inactiveListButton_Click(object sender, EventArgs e)
        {
            if (_isProjectsListOpen)    
            {
                _subcategoryViewRelativeLayout_Click(new object(),new EventArgs());
            }
            if (isCategoriesListOpen)
            {
                _categoryViewRelativeLayout_Click(new object(), new EventArgs());
            }
        }

        public override void Finish()
        {
            base.Finish();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (_isLogout)
            {
                Finish();
                _isLogout = false;
                StartActivity(typeof(LoginActivity));
            }
        }
        protected override void OnPause()
        {
            _categoriesListView.Visibility = ViewStates.Gone;
            _categoriesshadowImageView.Visibility = ViewStates.Gone;
            _inactiveListButton.Visibility = ViewStates.Gone;
            _subcategoriesListView.Visibility = ViewStates.Gone;
            _subcategoriesshadowImageView.Visibility = ViewStates.Gone;
            _isProjectsListOpen = false;
            isCategoriesListOpen = false;
            base.OnPause();
        }

        public override void OnBackPressed()
        {
            if (_categoriesListView.Visibility == ViewStates.Visible)
            {
                _categoriesListView.Visibility = ViewStates.Gone;
                _categoriesshadowImageView.Visibility = ViewStates.Gone;
                _inactiveListButton.Visibility = ViewStates.Gone;
                isCategoriesListOpen = false;
                return;
            }
            if (_subcategoriesListView.Visibility == ViewStates.Visible)
            {
                _subcategoriesListView.Visibility = ViewStates.Gone;
                _subcategoriesshadowImageView.Visibility = ViewStates.Gone;
                _inactiveListButton.Visibility = ViewStates.Gone;
                _isProjectsListOpen = false;
                return;
            }
            if (_badgePopupWindow.IsShowing)
            {
                _badgePopupWindow.Dismiss(); return;
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}