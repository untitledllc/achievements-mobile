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
        static ProgressDialog mDialog;
        AlertDialog.Builder _activateMessageBadgeDialogBuilder;
        AlertDialog _activateMessageBadgeDialog;
        Vibrator _vibe;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle); 
            _vibe = (Vibrator)this.GetSystemService(Context.VibratorService);
            loadComplete = new TextView(this);

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

            SetContentView(Resource.Layout.SecondScreenActivityLayout);
            mDialog = new ProgressDialog(this);
            mDialog.SetMessage("Загрузка пользовательских данных...");
            mDialog.SetCancelable(false);
            mDialog.Show();


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
                    RunOnUiThread(()=> mDialog.Hide());
                    Finish();
                    StartActivity(typeof(MainScreenActivity));
                }
            };

        }

        void RunOnUiRistBadgeWin()
        {
            mDialog.Hide();
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            SetContentView(Resource.Layout.FirstBadgeActivityLayout);

            TextView userName = FindViewById<TextView>(Resource.Id.firstbadgewin_usernameTextView);
            TextView badgeDescr = FindViewById<TextView>(Resource.Id.firstbadgewin_wonderdescrTextView);

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
            foreach (var bonus in _achieve.Bonuses)
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

        void treadStartVoid()
        {
            try
            {
                AppInfo._selectedCategoriesDictionary = new Dictionary<string, bool>();
                AppInfo._selectedSubCategoriesDictionary = new Dictionary<string, bool>();
                AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId);
            }
            catch
            {
                string msg = "Не удалось подключиться. Проверьте интернет подключение";
                RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Long).Show());
                RunOnUiThread(()=> Finish());
                return;
            }
            RunOnUiThread(()=> mDialog.SetMessage("Загрузка контента..."));
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

                            Bitmap bitmap = GetImageBitmap(AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].PicUrl);

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

            loadComplete.Text = "complete";
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