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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            loadComplete = new TextView(this);

            if (!File.Exists(@"/data/data/itsbeta.achievements/data.txt"))
            {
                List<string> config = new List<string>();

                config.Add(AppInfo._user.Fullname);
                config.Add(AppInfo._user.BirthDate);
                config.Add(AppInfo._user.FacebookUserID);
                config.Add(AppInfo._user.ItsBetaUserId);
                config.Add(AppInfo._user.City);

                File.WriteAllLines(@"/data/data/itsbeta.achievements/data.txt", config.ToArray(), Encoding.UTF8);
            }

            SetContentView(Resource.Layout.SecondScreenActivityLayout);
            mDialog = new ProgressDialog(this);
            mDialog.SetMessage("Loading User Data...");
            mDialog.SetCancelable(false);
            mDialog.Show();


            ThreadStart threadStart = new ThreadStart(treadStartVoid);
            Thread loadThread = new Thread(threadStart);
            loadThread.Start();


            loadComplete.TextChanged += delegate
            {
                if (!LoginWebActivity.isPlayerExist)
                {
                    RunOnUiThread(() => RunOnUiRistBadgeWin());
                }
                else
                {
                    RunOnUiThread(()=> mDialog.Hide());
                    Finish();
                    StartActivity(typeof(SecondScreenActivity));
                }
            };

        }

        void RunOnUiRistBadgeWin()
        {
            mDialog.Hide();
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            SetContentView(Resource.Layout.FirstBadgeActivityLayout);

            TextView userName = FindViewById<TextView>(Resource.Id.FirstBadge_ProfileNameTextView);
            if (AppInfo._user.Fullname != null)
            {
                userName.Text = AppInfo._user.Fullname;
            }

            ImageView badgeImageView = FindViewById<ImageView>(Resource.Id.paper_BadgeImageView);
            ImageButton badgeReadyButton = FindViewById<ImageButton>(Resource.Id.BadgeSheet_CloseImageButton);
            ImageButton badgeReadyButtonFake = FindViewById<ImageButton>(Resource.Id.BadgeSheet_CloseImageButtonFake);

            Bitmap bitmap = BitmapFactory.DecodeFile(@"/data/data/itsbeta.achievements/cache/pictures/" + "achive" +
                "egMGNg79ys.PNG"
                );

            badgeImageView.SetImageBitmap(bitmap);

            badgeReadyButton.Click += delegate
            {
                badgeReadyButtonFake.StartAnimation(buttonClickAnimation);
                Finish();
                StartActivity(typeof(SecondScreenActivity));
            };
        }

        void treadStartVoid()
        {
            AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId);
            RunOnUiThread(()=> mDialog.SetMessage("Loading Badge's pictures..."));
            Directory.CreateDirectory(@"/data/data/itsbeta.achievements/cache/pictures/");

            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                for (int j = 0; j < AppInfo._achievesInfo.CategoryArray[i].Projects.Count(); j++)
                {
                    AppInfo._subcategCount++;
                    for (int k = 0; k < AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Count(); k++)
                    {
                        AppInfo._badgesCount ++;

                        AppInfo._bonusesCount += AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Bonuses.Count();

                        FileStream fs = new FileStream(@"/data/data/itsbeta.achievements/cache/pictures/" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG", FileMode.OpenOrCreate,
                            FileAccess.ReadWrite, FileShare.ReadWrite
                            );

                        if (!System.IO.File.Exists(@"/data/data/itsbeta.achievements/cache/pictures/" + "achive" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG"))
                        {

                            Bitmap bitmap = GetImageBitmap(AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].PicUrl);

                            bitmap.Compress(
                            Bitmap.CompressFormat.Png, 10, fs);
                            bitmap.Dispose();
                            fs.Flush();
                            fs.Close();

                            System.IO.File.Copy(@"/data/data/itsbeta.achievements/cache/pictures/" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG",
                            @"/data/data/itsbeta.achievements/cache/pictures/" + "achive" +
                            AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].ApiName + ".PNG");

                            System.IO.File.Delete(@"/data/data/itsbeta.achievements/cache/pictures/" +
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
    }
}