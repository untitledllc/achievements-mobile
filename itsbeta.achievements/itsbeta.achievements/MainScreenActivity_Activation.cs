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
    partial class MainScreenActivity
    {

        MobileBarcodeScanner _scanner;

        AutoCompleteTextView _codeCompleteTextView;
        ProgressDialog _activationDialog;
        AlertDialog.Builder _activateMessageBadgeDialogBuilder;
        AlertDialog _activateMessageBadgeDialog;
        ServiceItsBeta _serviceItsBeta = new ServiceItsBeta();


        void GetActivationDialog()
        {
            ImageButton addCodeImageButtonFake = FindViewById<ImageButton>(Resource.Id.NavBar_addcodeImageButtonFake);
            ImageButton addCodeImageButton = FindViewById<ImageButton>(Resource.Id.NavBar_addcodeImageButton);
            _activateMessageBadgeDialogBuilder = new AlertDialog.Builder(this);
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

            addCodeImageButtonFake.Click += delegate { _vibe.Vibrate(50); _codeCompleteTextView.Text = ""; addCodeImageButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Show(); addBadgeRelativeLayout.StartAnimation(AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn)); };


          
            addBadgeCancelButton.Click += delegate { _vibe.Vibrate(50); addBadgeCancelButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Dismiss(); };
            addCodeButton.Click += delegate { _vibe.Vibrate(50); addCodeButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Dismiss(); addCodeDialog.Show(); addCodeRelativeLayout.StartAnimation(AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn)); };
            addCodeCancelButton.Click += delegate { _vibe.Vibrate(50); addCodeCancelButton.StartAnimation(_buttonClickAnimation); addCodeDialog.Dismiss(); };
            readQRCodeButton.Click += delegate
            {
                _vibe.Vibrate(50);
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

                _foundActionTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(_foundActionTextView_TextChanged);
            };

            addCodeReadyButton.Click += delegate
            {
                _vibe.Vibrate(50);
                if (_codeCompleteTextView.Text.Replace(" ", "") != "")
                {
                    addCodeReadyButton.StartAnimation(_buttonClickAnimation);
                    addCodeDialog.Dismiss();

                    _activationDialog = new ProgressDialog(this);
                    _activationDialog.SetMessage("Активация достижения...");
                    _activationDialog.SetCancelable(false);
                    _activationDialog.Show();

                    ThreadStart threadStart = new ThreadStart(AsyncActivizationViaEntering);
                    Thread loadThread = new Thread(threadStart);
                    loadThread.Start();
                }
                else
                {
                    Toast.MakeText(this, "Введите код активации", ToastLength.Short).Show();
                    addCodeReadyButton.StartAnimation(_buttonClickAnimation);
                }
            };

        }

        bool qrValid = false;

        void _foundActionTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            _scanner.Cancel();

            var senderTV = (TextView)sender;
            string activationCode = "null";

            if (senderTV.Text.StartsWith("http://www.itsbeta.com/activate?activation_code="))
            {
                activationCode = senderTV.Text.Replace("http://www.itsbeta.com/activate?activation_code=", "");
                qrValid = true;
            }
            else
            {
                Toast.MakeText(this, "QR Code несоответствует формату", ToastLength.Short).Show();
                qrValid = false;
            }

            if (qrValid)
            {
                RunOnUiThread(() => ShowQRDialog());
                ThreadStart threadStart = new ThreadStart(() => AsyncActivizationViaQR(activationCode));
                Thread loadThread = new Thread(threadStart);
                loadThread.Start();
                qrValid = false;
            }
        }


        public void ShowQRDialog()
        {
            _activationDialog = new ProgressDialog(this);
            _activationDialog.SetMessage("Активация достижения...");
            _activationDialog.SetCancelable(false);
            _activationDialog.Show();
        }

        string activatedBadgeFbId = "null";
        string errorDescr = "null";

        public void AsyncActivizationViaQR(string badgeApi_name)
        {
            ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve activatedAchieve;
                string response = _serviceItsBeta.ActivateBadge(badgeApi_name, AppInfo._appaccess_token, AppInfo._user.FacebookUserID);
                if (response.StartsWith("badgefbId="))
                {
                    activatedBadgeFbId = response.Replace("badgefbId=", "");
                    AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId);
                    activatedAchieve = new Achieves.ParentCategory.ParentProject.Achieve();

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

                    FileStream fs = new FileStream(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                                activatedAchieve.ApiName + ".PNG", FileMode.OpenOrCreate,
                                FileAccess.ReadWrite, FileShare.ReadWrite
                                );

                    if (!System.IO.File.Exists(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                        activatedAchieve.ApiName + ".PNG"))
                    {
                        Bitmap bitmap = GetImageBitmap(activatedAchieve.PicUrl);

                        bitmap.Compress(
                        Bitmap.CompressFormat.Png, 10, fs);
                        bitmap.Dispose();
                        fs.Flush();
                        fs.Close();

                        System.IO.File.Copy(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                        activatedAchieve.ApiName + ".PNG",
                        @"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                        activatedAchieve.ApiName + ".PNG");

                        System.IO.File.Delete(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                        activatedAchieve.ApiName + ".PNG");
                    }
                    RunOnUiThread(() => CompleteActivation(activatedAchieve));
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

        public void AsyncActivizationViaEntering()
        {
            ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve activatedAchieve;
            string response = _serviceItsBeta.ActivateBadge(_codeCompleteTextView.Text, AppInfo._appaccess_token, AppInfo._user.FacebookUserID);
            if (response.StartsWith("badgefbId="))
            {
                activatedBadgeFbId = response.Replace("badgefbId=", "");
                AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId);
                activatedAchieve = new Achieves.ParentCategory.ParentProject.Achieve();
                
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

                FileStream fs = new FileStream(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                            activatedAchieve.ApiName + ".PNG", FileMode.OpenOrCreate,
                            FileAccess.ReadWrite, FileShare.ReadWrite
                            );

                if (!System.IO.File.Exists(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                    activatedAchieve.ApiName + ".PNG"))
                {
                    Bitmap bitmap = GetImageBitmap(activatedAchieve.PicUrl);

                    bitmap.Compress(
                    Bitmap.CompressFormat.Png, 10, fs);
                    bitmap.Dispose();
                    fs.Flush();
                    fs.Close();

                    System.IO.File.Copy(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                    activatedAchieve.ApiName + ".PNG",
                    @"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                    activatedAchieve.ApiName + ".PNG");

                    System.IO.File.Delete(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" +
                    activatedAchieve.ApiName + ".PNG");
                }
                RunOnUiThread(() => CompleteActivation(activatedAchieve));
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

        void CompleteActivation(ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve activatedBadge)
        {
           // _activationDialog.Dismiss();
            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(LayoutInflaterService);
            ViewGroup relativeAgedSummary = new RelativeLayout(this);
            View layout = inflater.Inflate(Resource.Layout.ReceiveBadgeLayount, relativeAgedSummary);

            ImageView badgeImage = (ImageView)layout.FindViewById(Resource.Id.recbadgewin_BadgeImageView);
            Button inactiveButton = (Button)layout.FindViewById(Resource.Id.recbadgewin_inactiveButton);
            ImageButton badgeReadyButton = (ImageButton)layout.FindViewById(Resource.Id.recbadgewin_CloseImageButton);
            ImageButton badgeReadyButtonFake = (ImageButton)layout.FindViewById(Resource.Id.recbadgewin_CloseImageButtonFake);

            TextView profileName = (TextView)layout.FindViewById(Resource.Id.recbadgewin_badgeTextView);
            TextView badgeDescr = (TextView)layout.FindViewById(Resource.Id.recbadgewin_wonderdescrTextView);
            profileName.Text = AppInfo._user.Fullname;
            badgeDescr.Text = activatedBadge.Description;
            AppInfo._badgesCount ++;
            badgeImage.SetImageBitmap(BitmapFactory.DecodeFile(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" + activatedBadge.ApiName + ".PNG"));

            LinearLayout bonusPaperListLinearLayout = (LinearLayout)layout.FindViewById(Resource.Id.bonuspaperlist_linearLayout);
            //
            bonusPaperListLinearLayout.RemoveAllViews();
            if (achieve.Bonuses.Count() == 1)
                {
                    var bonus = activatedBadge.Bonuses.First();
                    {
                        LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                        View bonusView = layoutInflater.Inflate(Resource.Layout.BonusOnListRowLayout, null);
                        bonusView.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);
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

                            bonusPaperListLinearLayout.SetBackgroundColor(new Color(201, 238, 255, 89));

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


                            bonusPaperListLinearLayout.SetBackgroundColor(new Color(189, 255, 185, 127));

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

                            bonusPaperListLinearLayout.SetBackgroundColor(new Color(255, 185, 245, 127));

                            bonusDescr.Visibility = ViewStates.Visible;
                            bonusName.Visibility = ViewStates.Visible;

                            bonusName.Text = "Подарок";
                            bonusDescr.Text = bonus.Description;

                            bonusPaperListLinearLayout.AddView(bonusView);
                        }
                    }
                }
            if (achieve.Bonuses.Count() > 1)
            {
                foreach (var bonus in activatedBadge.Bonuses)
                {
                    LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                    View bonusView = layoutInflater.Inflate(Resource.Layout.BonusOnListRowLayout, null);
                    bonusView.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FillParent, RelativeLayout.LayoutParams.FillParent);
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
            }

            var badgePopupWindow = new PopupWindow(layout,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
            badgePopupWindow.ShowAsDropDown(FindViewById<TextView>(Resource.Id.secaondscr_faketextView), 0, 0);

            badgeReadyButton.Click += delegate
            {
                _vibe.Vibrate(50);
                badgeReadyButtonFake.StartAnimation(_buttonClickAnimation);
                GetAchievementsView();
                badgePopupWindow.Dismiss();
            };
            inactiveButton.Click += delegate
            {
                GetAchievementsView();
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
        
        #region QR Code Reader Region

        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";
            bool found = false;
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                found = true;
                msg = "QR код найден";// + result.Text;
            }
            else
                msg = "Сканирование отменено";

            this.RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Short).Show());
            if (found)
            {
                _foundActionTextView.Text = result.Text;
            }
        }
        #endregion
    }
}
