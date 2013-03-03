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
using Android.Webkit;
using System.IO;
using System.Collections;
using itsbeta.achievements.gui;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProfileActivity : Activity
    {
        Animation buttonClickAnimation;
        public static ProfileActivity _context;
        AlertDialog.Builder _messageDialogBuilder;
        AlertDialog _messageDialog;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            SetContentView(Resource.Layout.ProfileScreenActivityLayout);

            _context = this;
            _messageDialogBuilder = new AlertDialog.Builder(this);

            ImageButton logoutImageButton = FindViewById<ImageButton>(Resource.Id.profilescr_NavBar_LogoutImageButton);
            Button logoutButtonFake = FindViewById<Button>(Resource.Id.profilescr_logiutbuttonfake);
            Vibrator vibe = (Vibrator)_context.GetSystemService(Context.VibratorService);

            logoutButtonFake.Click += delegate
            {
                logoutImageButton.StartAnimation(buttonClickAnimation);
                vibe.Vibrate(50);
                ShowAlertDialog();
            };


            _messageDialogBuilder.SetTitle("�����������");
            _messageDialogBuilder.SetMessage("�� ������������� ������ ����� �� �������?");
            _messageDialogBuilder.SetNegativeButton("������", delegate { _messageDialog.Dismiss(); vibe.Vibrate(50); });
            _messageDialogBuilder.SetPositiveButton("��", delegate
            {
                vibe.Vibrate(150);
                CookieSyncManager.CreateInstance(this);
                CookieManager cookieManager = CookieManager.Instance;
                cookieManager.RemoveAllCookie();

                File.Delete(@"/data/data/ru.hintsolutions.itsbeta/data.txt");
                itsbeta.achievements.LoginWebActivity.ItsbetaLoginWebViewClient.loadPreviousState = false;
                MainScreenActivity._isLogout = true;
                Finish();
            });



            TextView userFullname = FindViewById<TextView>(Resource.Id.profilescr_usernameTextView);
            TextView userData = FindViewById<TextView>(Resource.Id.profilescr_userAgelocTextView);

            TextView badgesCount = FindViewById<TextView>(Resource.Id.profilescr_allbadgescountTextView);
            TextView bonusesCount = FindViewById<TextView>(Resource.Id.profilescr_bonusCountTextView);
            TextView subCategoriesCount = FindViewById<TextView>(Resource.Id.profilescr_subcategCountTextView);

            userFullname.Text = AppInfo._user.Fullname;
            userData.Text = GetUserAge(AppInfo._user.BirthDate) + ", " + AppInfo._user.City;

            badgesCount.Text = AppInfo._badgesCount.ToString();
            bonusesCount.Text = AppInfo._bonusesCount.ToString();
            subCategoriesCount.Text = AppInfo._subcategCount.ToString();

            LinearLayout profilescrLinearLayout = FindViewById<LinearLayout>(Resource.Id.profilescr_linearLayout);

            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                View view = layoutInflater.Inflate(Resource.Layout.ProfileScreenParentRow, null);
                TextView categoryName = (TextView)view.FindViewById(Resource.Id.profilescr_CategNameTextView);
                categoryName.Text = i+1 + ". " + AppInfo._achievesInfo.CategoryArray[i].DisplayName +":";
                
                profilescrLinearLayout.AddView(view);

                for (int j = 0; j < AppInfo._achievesInfo.CategoryArray[i].Projects.Length; j++)    
                {
                    LayoutInflater layoutInflater2 = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                    View view2 = layoutInflater2.Inflate(Resource.Layout.ProfileScreenChildRow, null);
                    TextView projectName = (TextView)view2.FindViewById(Resource.Id.profilescr_ProjectNameTextView);
                    ImageView statBarActive = (ImageView)view2.FindViewById(Resource.Id.profilescr_statbarImageViewwActive);
                    ImageView statBarPassive = (ImageView)view2.FindViewById(Resource.Id.profilescr_statbarImageViewNorm);
                    LinearLayout statbarlinearlayout = (LinearLayout)view2.FindViewById(Resource.Id.statbarlinearLayout);


                    for (int k = 0; k < AppInfo._achievesInfo.CategoryArray[i].Projects[j].TotalBadges - AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Length; k++)
                    {
                        TableRow fakebut = new TableRow(this);
                        //fakebut.LayoutParameters.Width = AppInfo._display.Width;
                        fakebut.LayoutParameters = new LinearLayout.LayoutParams(Android.Widget.LinearLayout.LayoutParams.FillParent, Android.Widget.LinearLayout.LayoutParams.FillParent, 1f);
                        statbarlinearlayout.AddView(fakebut);
                    }

                    projectName.Text = AppInfo._achievesInfo.CategoryArray[i].Projects[j].DisplayName + "- " + AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Length;

                    if (projectName.Text.EndsWith(" 1"))
                    {
                        projectName.Text += " badge";
                    }
                    else
                    {
                        projectName.Text += " badges";
                    }

                    profilescrLinearLayout.AddView(view2);
                }

            }
        }

        void ShowAlertDialog()
        {
            _messageDialog = _messageDialogBuilder.Create();
            _messageDialog.Show();
        }
        string GetUserAge(string date)
        {
            int daynow = DateTime.Now.Day;
            int monthnow = DateTime.Now.Month;
            int yearnow = DateTime.Now.Year;

            var inputdate = date.Split('/');

            int inputday;
            int inputmonth;
            int inputyear;

            int.TryParse(inputdate[1], out inputday);
            int.TryParse(inputdate[0], out inputmonth);
            int.TryParse(inputdate[2], out inputyear);

            int age = yearnow - inputyear;
            if (inputmonth > monthnow)
            {
                age--;
            }
            if (inputmonth == monthnow && inputday > daynow)
            {
                age--;
            }

            return age.ToString();
        }
    }
}