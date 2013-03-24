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

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class FirstBadgeActivity : Activity
    {
        public static Achieves.ParentCategory[] _achievesArray;
        public static TextView loadComplete;
        Animation buttonClickAnimation;
        static AlertDialog.Builder _messageDialogBuilder;
        static AlertDialog _messageDialog;
        Vibrator _vibe;


        static ProgressDialog _progressDialog;
        static TextView _progressDialogMessage;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var locale = Java.Util.Locale.Default.DisplayLanguage;
            if (locale == "русский")
            {
                AppInfo.IsLocaleRu = true;
            }
            else
            {
                AppInfo.IsLocaleRu = false;
            }

            _vibe = (Vibrator)this.GetSystemService(Context.VibratorService);
            loadComplete = new TextView(this);
            _messageDialogBuilder = new AlertDialog.Builder(this);
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

            SetContentView(Resource.Layout.firstbadgeactivityloadinglayout);


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

            _progressDialogMessage.Text = "Загрузка данных...";
            if (!AppInfo.IsLocaleRu)
            {
                _progressDialogMessage.Text = "Loading Data...";
            }
            _progressDialog.Show();

            ThreadStart threadStart = new ThreadStart(treadStartVoid);
            Thread loadThread = new Thread(threadStart);
            loadThread.Start();

            loadComplete.TextChanged += delegate
            {
                if (!LoginWebActivity.isAppBadgeEarned)
                {
                    RunOnUiThread(() => RunOnUiRistBadgeWin());
                }
                else
                {
                    RunOnUiThread(() => _progressDialog.Hide());
                    Finish();
                    StartActivity(typeof(MainScreenActivity));
                }
            };

        }

        void RunOnUiRistBadgeWin()
        {
            _progressDialog.Hide();
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            SetContentView(Resource.Layout.firstbadgeactivitylayout);

            TextView userName = FindViewById<TextView>(Resource.Id.firstbadgewin_usernameTextView);
            TextView badgeDescr = FindViewById<TextView>(Resource.Id.firstbadgewin_wonderdescrTextView);
            TextView howGetContent = FindViewById<TextView>(Resource.Id.firstbadgewin_howTextView);
            if (!AppInfo.IsLocaleRu)
            {
                howGetContent.Text = "You got a new Badge";
            }

            if (AppInfo._user.Fullname != null)
            {
                userName.Text = AppInfo._user.Fullname;
            }

            ImageView badgeImageView = FindViewById<ImageView>(Resource.Id.firstbadgewin_BadgeImageView);

            ImageButton badgeReadyButton = FindViewById<ImageButton>(Resource.Id.firstbadgewin_CloseImageButton);
            ImageButton badgeReadyButtonFake = FindViewById<ImageButton>(Resource.Id.firstbadgewin_CloseImageButtonFake);


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

            Bitmap bitmap = BitmapFactory.DecodeFile(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                _achieve.ApiName+ ".PNG"
                );

            LinearLayout bonusPaperListLinearLayout = FindViewById<LinearLayout>(Resource.Id.bonuspaperlist_linearLayout);


            if (_achieve.Bonuses.Count() == 0)
            {
                bonusPaperListLinearLayout.Visibility = ViewStates.Gone;
            }
                if (_achieve.Bonuses.Count() == 1)
                {
                    var bonus = _achieve.Bonuses.First();
                    {
                        LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                        View bonusView = layoutInflater.Inflate(Resource.Layout.bonusonlistrowlayout, null);
                        bonusView.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);
                        ImageView bonusLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_GreenBonusImageView);
                        ImageView discountLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_BlueBonusImageView);
                        ImageView giftLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_VioletBonusImageView);

                        ImageView bonusDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_greendescbackgroundImageView);
                        ImageView discountDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_bluedescbackgroundImageView);
                        ImageView giftDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_violetdescbackgroundImageView);

                        TextView bonusName = (TextView)bonusView.FindViewById(Resource.Id.badgewin_bonusTextView);
                        TextView bonusDescr = (TextView)bonusView.FindViewById(Resource.Id.badgewin_bonusdescrTextView);


                        bonusDescr.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;

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

                            bonusPaperListLinearLayout.SetBackgroundColor(new Color(201, 238, 255, 89));

                            bonusDescr.Visibility = ViewStates.Visible;
                            bonusName.Visibility = ViewStates.Visible;

                            bonusName.Text = "Скидка";
                            if (!AppInfo.IsLocaleRu)
                            {
                                bonusName.Text = "Discount";
                            }
                            bonusDescr.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                            bonusPaperListLinearLayout.AddView(bonusView);
                        }
                        if (bonus.Type == "bonus")
                        {
                            bonusLineImage.Visibility = ViewStates.Visible;
                            discountLineImage.Visibility = ViewStates.Invisible;
                            giftLineImage.Visibility = ViewStates.Invisible;


                            bonusPaperListLinearLayout.SetBackgroundColor(new Color(189, 255, 185, 127));

                            bonusDescr.Visibility = ViewStates.Visible;
                            bonusName.Visibility = ViewStates.Visible;

                            bonusName.Text = "Бонус";
                            if (!AppInfo.IsLocaleRu)
                            {
                                bonusName.Text = "Bonus";
                            }
                            bonusDescr.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                            bonusPaperListLinearLayout.AddView(bonusView);
                        }
                        if (bonus.Type == "present")
                        {
                            bonusLineImage.Visibility = ViewStates.Invisible;
                            discountLineImage.Visibility = ViewStates.Invisible;
                            giftLineImage.Visibility = ViewStates.Visible;

                            bonusPaperListLinearLayout.SetBackgroundColor(new Color(255, 185, 245, 127));

                            bonusDescr.Visibility = ViewStates.Visible;
                            bonusName.Visibility = ViewStates.Visible;

                            bonusName.Text = "Подарок";
                            if (!AppInfo.IsLocaleRu)
                            {
                                bonusName.Text = "Present";
                            }
                            bonusDescr.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                            bonusPaperListLinearLayout.AddView(bonusView);
                        }
                    }
                }
                if (_achieve.Bonuses.Count() > 1)
            {
                foreach (var bonus in _achieve.Bonuses)
                {
                    LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                    View bonusView = layoutInflater.Inflate(Resource.Layout.bonusonlistrowlayout, null);
                    bonusView.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);
                    ImageView bonusLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_GreenBonusImageView);
                    ImageView discountLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_BlueBonusImageView);
                    ImageView giftLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_VioletBonusImageView);

                    ImageView bonusDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_greendescbackgroundImageView);
                    ImageView discountDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_bluedescbackgroundImageView);
                    ImageView giftDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_violetdescbackgroundImageView);

                    TextView bonusName = (TextView)bonusView.FindViewById(Resource.Id.badgewin_bonusTextView);
                    TextView bonusDescr = (TextView)bonusView.FindViewById(Resource.Id.badgewin_bonusdescrTextView);
                    bonusDescr.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;

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
                        if (!AppInfo.IsLocaleRu)
                        {
                            bonusName.Text = "Discount";
                        }
                        bonusDescr.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

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
                        if (!AppInfo.IsLocaleRu)
                        {
                            bonusName.Text = "Bonus";
                        }
                        bonusDescr.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

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
                        if (!AppInfo.IsLocaleRu)
                        {
                            bonusName.Text = "Present";
                        }
                        bonusDescr.SetText(Android.Text.Html.FromHtml(bonus.Description), TextView.BufferType.Spannable);

                        bonusPaperListLinearLayout.AddView(bonusView);
                    }
                }
            }


            badgeImageView.SetImageBitmap(bitmap);
            badgeDescr.Text = _achieve.Description;

            badgeReadyButton.Click += delegate
            {
                _vibe.Vibrate(50);
                badgeReadyButtonFake.StartAnimation(buttonClickAnimation);
                Finish();
                StartActivity(typeof(MainScreenActivity));
            };
        }

        void ShowAlertDialog()
        {
            _messageDialog = _messageDialogBuilder.Create();
            _messageDialog.Show();
        }

        void treadStartVoid()
        {
            try
            {
                AppInfo._selectedCategoriesDictionary = new Dictionary<string, bool>();
                AppInfo._selectedSubCategoriesDictionary = new Dictionary<string, bool>();
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
                        AppInfo._badgesCount ++;

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
                                bitmap = GetImageBitmap(AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].PicUrl);
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

            RunOnUiThread(() => loadComplete.Text = "complete");
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

        public Achieves.ParentCategory.ParentProject.Achieve _achieve { get; set; }
    }
}