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
using ItsBeta.Core;
using System.IO;
using System.Threading;
using Android.Graphics;
using FlurryLib;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class FirstBadgeActivity : Activity
    {

        FlurryClient _flurryClient;

        #region Base Activity Global Objects
        //Анимация и вибрация нажатия на кнопки
        Animation _fadeAnimation;
        Vibrator _vibe;

        //Диалоговые окна оповещения
        AlertDialog.Builder _messageDialogBuilder;
        AlertDialog _messageDialog;

        LayoutInflater _baseLayoutInflater;

        //Объекты создания прогрессДиалога
        ProgressDialog _progressDialog;
        RelativeLayout _progressDialogRelativeLayout;
        View _progressDialogView;
        TextView _progressDialogMessage;

        //Сторонний шрифт
        Typeface _robotoLightFont;

        //Второстепенный поток
        ThreadStart _asyncInitThreadStart;
        Thread _asyncInitThread;


        //Объекты листа Бейджа
        ImageView _badgeSheetBadgeImageView;
        ImageButton _badgeSheetCloseImageButton;
        ImageButton _badgeSheetCloseFakeImageButton;

        TextView _badgeSheetUserNameTextView;
        TextView _badgeSheetDescrTextView;
        TextView _badgeSheetAnounceTextView;

        LinearLayout _badgeSheetBonusListLinearLayout;
        View _badgeSheetBonusView;
        ImageView _badgeSheetBonusLineImageView;      
        ImageView _badgeSheetDiscountLineImageView;    
        ImageView _badgeSheetPresentLineImageView;     
        ImageView _badgeSheetBonusDescrBackgroundImageView;
        ImageView _badgeSheetDiscountDescrBackgroundImageView;
        ImageView _badgeSheetPresentDescrBackgroundImageView;

        TextView _badgeSheetBonusNameTextView;
        TextView _badgeSheetBonusDescrTextView;
        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _flurryClient = new FlurryClient();

            if (Java.Util.Locale.Default.DisplayLanguage == "русский")
            {
                AppInfo.IsLocaleRu = true;
            }
            else
            {
                AppInfo.IsLocaleRu = false;
            }

            _robotoLightFont = Typeface.CreateFromAsset(this.Assets, "Roboto-Light.ttf"); 
            _vibe = (Vibrator)this.GetSystemService(Context.VibratorService);

            _messageDialogBuilder = new AlertDialog.Builder(this);

            //Считываем пользовательские данные
            if (!File.Exists(@"/data/data/ru.hintsolutions.itsbeta/data.txt"))
            {
                List<string> config = new List<string>();

                config.Add(AppInfo._user.Fullname);
                config.Add(AppInfo._user.BirthDate);
                config.Add(AppInfo._user.FacebookUserID);
                config.Add(AppInfo._user.ItsBetaUserId);
                config.Add(AppInfo._user.City);
                config.Add(AppInfo._fbAccessToken);

                File.WriteAllLines(@"/data/data/ru.hintsolutions.itsbeta/data.txt", config.ToArray(), Encoding.UTF8);
            }

            //Binding на xmlразметку
            SetContentView(Resource.Layout.firstbadgeactivityloadinglayout);


            //Инициализируем костомный прогресс диалог
            _progressDialogRelativeLayout = new RelativeLayout(this);
            _baseLayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            _progressDialogView = _baseLayoutInflater.Inflate(Resource.Layout.progressdialoglayout, null);
            _progressDialogMessage = (TextView)_progressDialogView.FindViewById(Resource.Id.progressDialogMessageTextView);
            _progressDialogRelativeLayout.AddView(_progressDialogView);
            _progressDialog = new ProgressDialog(this, Resource.Style.FullHeightDialog);
            _progressDialog.Show();
            _progressDialog.SetContentView(_progressDialogRelativeLayout);
            _progressDialog.Dismiss();
            _progressDialog.SetCanceledOnTouchOutside(false);
            _progressDialogMessage.Text = "Загрузка данных...";
            if (!AppInfo.IsLocaleRu)
            {
                _progressDialogMessage.Text = "Loading Data...";
            }
            _progressDialog.Show();


            //Запуск параллельного потока для выполнения основных действий
            _asyncInitThreadStart = new ThreadStart(AsyncInitCallback);
            _asyncInitThread = new Thread(_asyncInitThreadStart);
            _asyncInitThread.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();


            //Анимация и вибрация нажатия на кнопки
            if (_fadeAnimation != null)
            {
                _fadeAnimation.Dispose();
            }
            if (_vibe != null)
            {
                _vibe.Dispose();
            }

            //Диалоговые окна оповещения
            if (_messageDialogBuilder != null)
            {
                _messageDialogBuilder.Dispose();
            }
            if (_messageDialog != null)
            {
                _messageDialog.Dispose();
            }
            if (_baseLayoutInflater != null)
            {
                _baseLayoutInflater.Dispose();
            }

            //Объекты создания прогрессДиалога
            if (_progressDialog != null)
            {
                _progressDialog.Dispose();
            }
            if (_progressDialogRelativeLayout != null)
            {
                _progressDialogRelativeLayout.Dispose();
            }
            if (_progressDialogView != null)
            {
                _progressDialogView.Dispose();
            }
            if (_progressDialogMessage != null)
            {
                _progressDialogMessage.Dispose();
            }

            //Сторонний шрифт
            if (_robotoLightFont != null)
            {
                _robotoLightFont.Dispose();
            }

            //Второстепенный поток
            if (_asyncInitThread.ThreadState == ThreadState.Running)
            {
                _asyncInitThread.Abort();
            }

            //Объекты листа Бейджа
            if (_badgeSheetBadgeImageView != null)
            {
                _badgeSheetBadgeImageView.Dispose();
            }
            if (_badgeSheetCloseImageButton != null)
            {
                _badgeSheetCloseImageButton.Dispose();
            }
            if (_badgeSheetCloseFakeImageButton != null)
            {
                _badgeSheetCloseFakeImageButton.Dispose();
            }
            if (_badgeSheetUserNameTextView != null)
            {
                _badgeSheetUserNameTextView.Dispose();
            }
            if (_badgeSheetDescrTextView != null)
            {
                _badgeSheetDescrTextView.Dispose();
            }
            if (_badgeSheetAnounceTextView != null)
            {
                _badgeSheetAnounceTextView.Dispose();
            }
            if (_badgeSheetBonusListLinearLayout != null)
            {
                _badgeSheetBonusListLinearLayout.Dispose();
            }
            if (_badgeSheetBonusView != null)
            {
                _badgeSheetBonusView.Dispose();
            }
            if (_badgeSheetBonusLineImageView != null)
            {
                _badgeSheetBonusLineImageView.Dispose();
            }
            if (_badgeSheetDiscountLineImageView != null)
            {
                _badgeSheetDiscountLineImageView.Dispose();
            }
            if (_badgeSheetPresentLineImageView != null)
            {
                _badgeSheetPresentLineImageView.Dispose();
            }
            if (_badgeSheetBonusDescrBackgroundImageView != null)
            {
                _badgeSheetBonusDescrBackgroundImageView.Dispose();
            }
            if (_badgeSheetDiscountDescrBackgroundImageView != null)
            {
                _badgeSheetDiscountDescrBackgroundImageView.Dispose();
            }
            if (_badgeSheetPresentDescrBackgroundImageView != null)
            {
                _badgeSheetPresentDescrBackgroundImageView.Dispose();
            }
            if (_badgeSheetBonusNameTextView != null)
            {
                _badgeSheetBonusNameTextView.Dispose();
            }
            if (_badgeSheetBonusDescrTextView != null)
            {
                _badgeSheetBonusDescrTextView.Dispose();
            }

        }

        private void AsyncInitCallback()
        {
            try
            {
                AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId, AppInfo.IsLocaleRu);
            }
            catch
            {
                _messageDialogBuilder.SetTitle("Ошибка");
                _messageDialogBuilder.SetMessage("Не удалось подключиться. Проверьте состояние интернет подключения.");
                if (!AppInfo.IsLocaleRu)
                {
                    _messageDialogBuilder.SetTitle("Error");
                    _messageDialogBuilder.SetMessage("Internet connection missing.");
                }

                if (AppInfo.IsLocaleRu)
                {
                    _messageDialogBuilder.SetPositiveButton("Ок", delegate { RunOnUiThread(() => Finish()); });
                }
                if (!AppInfo.IsLocaleRu)
                {
                    _messageDialogBuilder.SetPositiveButton("Оk", delegate { RunOnUiThread(() => Finish()); });
                }

                RunOnUiThread(() => ShowAlertDialog());
                RunOnUiThread(() => _progressDialog.Dismiss());
                return;
            }
            if (AppInfo.IsLocaleRu)
            {
                RunOnUiThread(() =>
                _progressDialogMessage.Text = "Загрузка данных..."
                );
            }
            if (!AppInfo.IsLocaleRu)
            {
                RunOnUiThread(() =>
                _progressDialogMessage.Text = "Loading Data..."
                );
            }

            Directory.CreateDirectory(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/");

            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                for (int j = 0; j < AppInfo._achievesInfo.CategoryArray[i].Projects.Count(); j++)
                {
                    AppInfo._subcategCount++;
                    for (int k = 0; k < AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Count(); k++)
                    {
                        AppInfo._badgesCount++;

                        AppInfo._bonusesCount += AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Bonuses.Count();

                        FileStream fs = new FileStream(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG", FileMode.OpenOrCreate,
                            FileAccess.ReadWrite, FileShare.ReadWrite
                            );

                        if (!System.IO.File.Exists(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG"))
                        {
                            Bitmap bitmap;
                            try
                            {
                                bitmap = GetImageBitmapFromUrl(AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].PicUrl);
                            }
                            catch
                            {
                                if (AppInfo.IsLocaleRu)
                                {
                                    _messageDialogBuilder.SetTitle("Ошибка");
                                    _messageDialogBuilder.SetMessage("Ошибка загрузки контента.");
                                    _messageDialogBuilder.SetPositiveButton("Ок", delegate { RunOnUiThread(() => Finish()); });
                                }
                                if (!AppInfo.IsLocaleRu)
                                {
                                    _messageDialogBuilder.SetTitle("Error");
                                    _messageDialogBuilder.SetMessage("Content load error.");
                                    _messageDialogBuilder.SetPositiveButton("Оk", delegate { RunOnUiThread(() => Finish()); });
                                }

                                RunOnUiThread(() => ShowAlertDialog());
                                RunOnUiThread(() => _progressDialog.Dismiss());
                                return;
                            }

                            bitmap.Compress(
                            Bitmap.CompressFormat.Png, 10, fs);
                            bitmap.Recycle();
                            bitmap.Dispose();
                            fs.Flush();
                            fs.Close();

                            System.IO.File.Copy(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG",
                            @"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG");

                            System.IO.File.Delete(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG");
                        }
                    }
                }
            }

            RunOnUiThread(() => OnDataLoadingComplete());
        }

        private void OnDataLoadingComplete()
        {
            if (!LoginWebActivity.isAppBadgeEarned)
            {
                RunOnUiThread(() => ShowBadgeSheet());
            }
            else
            {
                RunOnUiThread(() => _progressDialog.Hide());
                Finish();
                StartActivity(typeof(MainScreenActivity));
            }
        }

        private void ShowBadgeSheet()
        {
            _progressDialog.Hide();
            _fadeAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);

            SetContentView(Resource.Layout.firstbadgeactivitylayout);

            _badgeSheetUserNameTextView     = FindViewById<TextView>(Resource.Id.firstbadgewin_usernameTextView);
            _badgeSheetDescrTextView        = FindViewById<TextView>(Resource.Id.firstbadgewin_wonderdescrTextView);
            _badgeSheetAnounceTextView = FindViewById<TextView>(Resource.Id.firstbadgewin_howTextView);

            _badgeSheetBadgeImageView = FindViewById<ImageView>(Resource.Id.firstbadgewin_BadgeImageView);
            _badgeSheetCloseImageButton = FindViewById<ImageButton>(Resource.Id.firstbadgewin_CloseImageButton);
            _badgeSheetCloseFakeImageButton = FindViewById<ImageButton>(Resource.Id.firstbadgewin_CloseImageButtonFake);

            _badgeSheetBonusListLinearLayout = FindViewById<LinearLayout>(Resource.Id.bonuspaperlist_linearLayout);


            _badgeSheetUserNameTextView.SetTypeface(_robotoLightFont, TypefaceStyle.Normal);
            _badgeSheetDescrTextView.SetTypeface(_robotoLightFont, TypefaceStyle.Normal);
            _badgeSheetAnounceTextView.SetTypeface(_robotoLightFont, TypefaceStyle.Normal);

            if (!AppInfo.IsLocaleRu)
            {
                _badgeSheetAnounceTextView.Text = "You got a new Badge";
            }

            if (AppInfo._user.Fullname != null)
            {
                _badgeSheetUserNameTextView.Text = AppInfo._user.Fullname;
            }

            Achieves.ParentCategory.ParentProject.Achieve _achieve = new Achieves.ParentCategory.ParentProject.Achieve();

            foreach (var category in AppInfo._achievesInfo.CategoryArray)
            {
                foreach (var project in category.Projects)
                {
                    foreach (var achieve in project.Achievements)
                    {
                        if (achieve.FbId == ServiceItsBeta.PostOnFBBadgeFbId)
                        {
                            _achieve = achieve;
                        }
                    }
                }
            }

            using (var d = BitmapFactory.DecodeFile(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                    _achieve.ApiName + ".PNG"))
            {
            _badgeSheetBadgeImageView.SetImageBitmap(d);
            }

            _badgeSheetDescrTextView.Text = _achieve.Description;

            _badgeSheetCloseImageButton.Click += delegate
            {
                _vibe.Vibrate(50);
                _badgeSheetCloseFakeImageButton.StartAnimation(_fadeAnimation);
                Finish();
                StartActivity(typeof(MainScreenActivity));
            };



            //Не показываем лист с бонусами
            if (_achieve.Bonuses.Count() == 0)
            {
                _badgeSheetBonusListLinearLayout.Visibility = ViewStates.Gone;
            }
            #region Показываем лист с 1 бонусом
            if (_achieve.Bonuses.Count() == 1)
            {
                var bonus = _achieve.Bonuses.First();
                    
                _badgeSheetBonusView = _baseLayoutInflater.Inflate(Resource.Layout.bonusonlistrowlayout, null);
                _badgeSheetBonusView.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);
                        
                _badgeSheetBonusLineImageView                =  (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_GreenBonusImageView);
                _badgeSheetDiscountLineImageView             = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_BlueBonusImageView);
                _badgeSheetPresentLineImageView                 = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_VioletBonusImageView);

                _badgeSheetBonusDescrBackgroundImageView     = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_greendescbackgroundImageView);
                _badgeSheetDiscountDescrBackgroundImageView  = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_bluedescbackgroundImageView);
                _badgeSheetPresentDescrBackgroundImageView      = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_violetdescbackgroundImageView);

                _badgeSheetBonusNameTextView  = (TextView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_bonusTextView);
                _badgeSheetBonusDescrTextView = (TextView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_bonusdescrTextView);

                _badgeSheetBonusNameTextView.SetTypeface(_robotoLightFont, TypefaceStyle.Normal);
                _badgeSheetBonusDescrTextView.SetTypeface(_robotoLightFont, TypefaceStyle.Normal);

                _badgeSheetBonusDescrTextView.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;

                _badgeSheetBonusLineImageView.Visibility = ViewStates.Invisible;
                _badgeSheetDiscountLineImageView.Visibility = ViewStates.Invisible;
                _badgeSheetPresentLineImageView.Visibility = ViewStates.Invisible;

                _badgeSheetBonusDescrBackgroundImageView.Visibility = ViewStates.Invisible;
                _badgeSheetDiscountDescrBackgroundImageView.Visibility = ViewStates.Invisible;
                _badgeSheetPresentDescrBackgroundImageView.Visibility = ViewStates.Invisible;

                _badgeSheetBonusDescrTextView.Visibility = ViewStates.Invisible;
                _badgeSheetBonusNameTextView.Visibility = ViewStates.Invisible;

                if (bonus.Type == "discount")
                {
                    _badgeSheetBonusLineImageView.Visibility = ViewStates.Invisible;
                    _badgeSheetDiscountLineImageView.Visibility = ViewStates.Visible;
                    _badgeSheetPresentLineImageView.Visibility = ViewStates.Invisible;

                    _badgeSheetBonusListLinearLayout.SetBackgroundColor(new Color(201, 238, 255, 86));

                    _badgeSheetBonusDescrTextView.Visibility = ViewStates.Visible;
                    _badgeSheetBonusNameTextView.Visibility = ViewStates.Visible;

                    _badgeSheetBonusNameTextView.Text = "Скидка";
                    if (!AppInfo.IsLocaleRu)
                    {
                        _badgeSheetBonusNameTextView.Text = "Discount";
                    }
                    _badgeSheetBonusDescrTextView.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                    _badgeSheetBonusListLinearLayout.AddView(_badgeSheetBonusView);
                }
                if (bonus.Type == "bonus")
                {
                    _badgeSheetBonusLineImageView.Visibility = ViewStates.Visible;
                    _badgeSheetDiscountLineImageView.Visibility = ViewStates.Invisible;
                    _badgeSheetPresentLineImageView.Visibility = ViewStates.Invisible;

                    _badgeSheetBonusListLinearLayout.SetBackgroundColor(new Color(189, 255, 185, 120));

                    _badgeSheetBonusDescrTextView.Visibility = ViewStates.Visible;
                    _badgeSheetBonusNameTextView.Visibility = ViewStates.Visible;

                    _badgeSheetBonusNameTextView.Text = "Бонус";
                    if (!AppInfo.IsLocaleRu)
                    {
                        _badgeSheetBonusNameTextView.Text = "Bonus";
                    }
                    _badgeSheetBonusDescrTextView.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                    _badgeSheetBonusListLinearLayout.AddView(_badgeSheetBonusView);
                }
                if (bonus.Type == "present")
                {
                    _badgeSheetBonusLineImageView.Visibility = ViewStates.Invisible;
                    _badgeSheetDiscountLineImageView.Visibility = ViewStates.Invisible;
                    _badgeSheetPresentLineImageView.Visibility = ViewStates.Visible;

                    _badgeSheetBonusListLinearLayout.SetBackgroundColor(new Color(255, 185, 245, 120));

                    _badgeSheetBonusDescrTextView.Visibility = ViewStates.Visible;
                    _badgeSheetBonusNameTextView.Visibility = ViewStates.Visible;

                    _badgeSheetBonusNameTextView.Text = "Подарок";
                    if (!AppInfo.IsLocaleRu)
                    {
                        _badgeSheetBonusNameTextView.Text = "Present";
                    }
                    _badgeSheetBonusDescrTextView.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                    _badgeSheetBonusListLinearLayout.AddView(_badgeSheetBonusView);
                }
                    
            }
            #endregion
            
            if (_achieve.Bonuses.Count() > 1)
            {
                foreach (var bonus in _achieve.Bonuses)
                {
                    _badgeSheetBonusView = _baseLayoutInflater.Inflate(Resource.Layout.bonusonlistrowlayout, null);
                    _badgeSheetBonusView.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);

                    _badgeSheetBonusLineImageView = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_GreenBonusImageView);
                    _badgeSheetDiscountLineImageView = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_BlueBonusImageView);
                    _badgeSheetPresentLineImageView = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_VioletBonusImageView);

                    _badgeSheetBonusDescrBackgroundImageView = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_greendescbackgroundImageView);
                    _badgeSheetDiscountDescrBackgroundImageView = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_bluedescbackgroundImageView);
                    _badgeSheetPresentDescrBackgroundImageView = (ImageView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_violetdescbackgroundImageView);

                    _badgeSheetBonusNameTextView = (TextView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_bonusTextView);
                    _badgeSheetBonusDescrTextView = (TextView)_badgeSheetBonusView.FindViewById(Resource.Id.badgewin_bonusdescrTextView);

                    _badgeSheetBonusNameTextView.SetTypeface(_robotoLightFont, TypefaceStyle.Normal);
                    _badgeSheetBonusDescrTextView.SetTypeface(_robotoLightFont, TypefaceStyle.Normal);

                    _badgeSheetBonusDescrTextView.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;

                    _badgeSheetBonusLineImageView.Visibility = ViewStates.Invisible;
                    _badgeSheetDiscountLineImageView.Visibility = ViewStates.Invisible;
                    _badgeSheetPresentLineImageView.Visibility = ViewStates.Invisible;

                    _badgeSheetBonusDescrBackgroundImageView.Visibility = ViewStates.Invisible;
                    _badgeSheetDiscountDescrBackgroundImageView.Visibility = ViewStates.Invisible;
                    _badgeSheetPresentDescrBackgroundImageView.Visibility = ViewStates.Invisible;

                    _badgeSheetBonusDescrTextView.Visibility = ViewStates.Invisible;
                    _badgeSheetBonusNameTextView.Visibility = ViewStates.Invisible;

                    if (bonus.Type == "discount")
                    {
                        _badgeSheetBonusLineImageView.Visibility = ViewStates.Invisible;
                        _badgeSheetDiscountLineImageView.Visibility = ViewStates.Visible;
                        _badgeSheetPresentLineImageView.Visibility = ViewStates.Invisible;

                        _badgeSheetBonusDescrTextView.Visibility = ViewStates.Visible;
                        _badgeSheetBonusNameTextView.Visibility = ViewStates.Visible;

                        _badgeSheetBonusNameTextView.Text = "Скидка";
                        if (!AppInfo.IsLocaleRu)
                        {
                            _badgeSheetBonusNameTextView.Text = "Discount";
                        }
                        _badgeSheetBonusDescrTextView.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                        _badgeSheetBonusListLinearLayout.AddView(_badgeSheetBonusView);
                    }
                    if (bonus.Type == "bonus")
                    {
                        _badgeSheetBonusLineImageView.Visibility = ViewStates.Visible;
                        _badgeSheetDiscountLineImageView.Visibility = ViewStates.Invisible;
                        _badgeSheetPresentLineImageView.Visibility = ViewStates.Invisible;

                        _badgeSheetBonusDescrTextView.Visibility = ViewStates.Visible;
                        _badgeSheetBonusNameTextView.Visibility = ViewStates.Visible;

                        _badgeSheetBonusNameTextView.Text = "Бонус";
                        if (!AppInfo.IsLocaleRu)
                        {
                            _badgeSheetBonusNameTextView.Text = "Bonus";
                        }
                        _badgeSheetBonusDescrTextView.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                        _badgeSheetBonusListLinearLayout.AddView(_badgeSheetBonusView);
                    }
                    if (bonus.Type == "present")
                    {
                        _badgeSheetBonusLineImageView.Visibility = ViewStates.Invisible;
                        _badgeSheetDiscountLineImageView.Visibility = ViewStates.Invisible;
                        _badgeSheetPresentLineImageView.Visibility = ViewStates.Visible;

                        _badgeSheetBonusDescrTextView.Visibility = ViewStates.Visible;
                        _badgeSheetBonusNameTextView.Visibility = ViewStates.Visible;

                        _badgeSheetBonusNameTextView.Text = "Подарок";
                        if (!AppInfo.IsLocaleRu)
                        {
                            _badgeSheetBonusNameTextView.Text = "Present";
                        }
                        _badgeSheetBonusDescrTextView.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                        _badgeSheetBonusListLinearLayout.AddView(_badgeSheetBonusView);
                    }
                }
            }
        }

        private void ShowAlertDialog()
        {
            _messageDialog = _messageDialogBuilder.Create();
            _messageDialog.Show();
        }
        
        private Bitmap GetImageBitmapFromUrl(String url)
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


        protected override void OnStart()
        {
            base.OnStart();
            try
            {
                _flurryClient.OnStartActivity(this);
            }
            catch { }
        }

        protected override void OnStop()
        {
            base.OnStop();
            try
            {
                _flurryClient.OnStopActivity(this);
            }
            catch { }
        }
    }
}