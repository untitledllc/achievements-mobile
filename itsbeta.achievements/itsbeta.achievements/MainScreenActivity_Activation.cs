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
    partial class MainScreenActivity
    {

        MobileBarcodeScanner _scanner;

        AutoCompleteTextView _codeCompleteTextView;
        ProgressDialog _progressDialog;
        AlertDialog.Builder _activateMessageBadgeDialogBuilder;
        AlertDialog _activateMessageBadgeDialog;
        ServiceItsBeta _serviceItsBeta = new ServiceItsBeta();
        ImageView _enterCodeLineImageView;
        Dialog _wrongCodeDialog;
        TextView _wrongCodeDialogTitle;
        TextView _wrongCodeDialogMessage;
        TextView _progressDialogMessage;

        void GetActivationDialog()
        {
            //WrongCodeDialog
            RelativeLayout wrongCodeDialogRelativeLayout = new RelativeLayout(this);
            LayoutInflater wrongCodeDialoglayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            View wrongCodeDialogView = wrongCodeDialoglayoutInflater.Inflate(Resource.Layout.wrongcodedialoglayout, null);
            Button wrongCodeDialogReadyButton = (Button)wrongCodeDialogView.FindViewById(Resource.Id.readyButton);
            _wrongCodeDialogTitle = (TextView)wrongCodeDialogView.FindViewById(Resource.Id.textView1);
            _wrongCodeDialogMessage = (TextView)wrongCodeDialogView.FindViewById(Resource.Id.textView2);
            

            wrongCodeDialogRelativeLayout.AddView(wrongCodeDialogView);
            _wrongCodeDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            _wrongCodeDialog.SetTitle("");
            _wrongCodeDialog.SetContentView(wrongCodeDialogRelativeLayout);

            wrongCodeDialogReadyButton.Click += delegate { _wrongCodeDialog.Dismiss(); };

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



            ImageButton addCodeImageButtonFake = FindViewById<ImageButton>(Resource.Id.NavBar_addcodeImageButtonFake);
            ImageButton addCodeImageButton = FindViewById<ImageButton>(Resource.Id.NavBar_addcodeImageButton);
            _activateMessageBadgeDialogBuilder = new AlertDialog.Builder(this);
            LayoutInflater addBadgeMenulayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            RelativeLayout addBadgeRelativeLayout = new RelativeLayout(this);
            View addBadgeView = addBadgeMenulayoutInflater.Inflate(Resource.Layout.addbadgemenulayoutlayout, null);


            Button addBadgeCancelButton = (Button)addBadgeView.FindViewById(Resource.Id.addbadge_cancelButton);
            Button readQRCodeButton = (Button)addBadgeView.FindViewById(Resource.Id.addbadge_readQRButton);
            Button addCodeButton = (Button)addBadgeView.FindViewById(Resource.Id.addbadge_addcodeButton);

            TextView addBadgeTitleTextView = (TextView)addBadgeView.FindViewById(Resource.Id.textView1);
            addBadgeRelativeLayout.AddView(addBadgeView);

            LayoutInflater addCodeMenulayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            RelativeLayout addCodeRelativeLayout = new RelativeLayout(this);
            View addCodeView = addCodeMenulayoutInflater.Inflate(Resource.Layout.entercodelayout, null);

            Button addCodeCancelButton = (Button)addCodeView.FindViewById(Resource.Id.addcode_cancelButton);
            Button addCodeReadyButton = (Button)addCodeView.FindViewById(Resource.Id.addcode_readyButton);
            _enterCodeLineImageView = (ImageView)addCodeView.FindViewById(Resource.Id.imageView1);

            TextView addCodeDescrTextView = (TextView)addCodeView.FindViewById(Resource.Id.textView2);
            TextView addCodeTitleTextView = (TextView)addCodeView.FindViewById(Resource.Id.textView1);

            _codeCompleteTextView = (AutoCompleteTextView)addCodeView.FindViewById(Resource.Id.addcode_autoCompleteTextView);

            if (!AppInfo.IsLocaleRu)
            {
                addBadgeTitleTextView.Text = "Activate Badge";
                addCodeButton.Text = "   Via Entering code";
                readQRCodeButton.Text = "   Via QR-reader";
                addCodeDescrTextView.Text = "Confirmation a code is a process for which is given a badge.";
                addCodeTitleTextView.Text = "Enter code";
                wrongCodeDialogReadyButton.Text = "Ok";
                addCodeCancelButton.Text = "Cancel";
                addCodeReadyButton.Text = "Ok";
            }


            _codeCompleteTextView.Click += delegate 
            {
                InputMethodManager im = (InputMethodManager)this.GetSystemService(InputMethodService);
                if (im.IsAcceptingText)
                {
                    _enterCodeLineImageView.SetBackgroundResource(Resource.Drawable.line_blue);
                }
                else
                {
                    _enterCodeLineImageView.SetBackgroundResource(Resource.Drawable.line);
                }
            };
            //codeCompleteTextView.SetDropDownBackgroundDrawable();
            addCodeRelativeLayout.AddView(addCodeView);

            Dialog addBadgeDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            addBadgeDialog.SetTitle("");
            addBadgeDialog.SetContentView(addBadgeRelativeLayout);

            Dialog addCodeDialog = new Dialog(this, Resource.Style.FullHeightDialog);
            addCodeDialog.SetTitle("");
            addCodeDialog.SetContentView(addCodeRelativeLayout);

            addCodeImageButtonFake.Click += delegate {
                if (!_badgePopupWindow.IsShowing)
                {
                    
                _categoriesListView.Visibility = ViewStates.Gone;
                _categoriesshadowImageView.Visibility = ViewStates.Gone;
                _inactiveListButton.Visibility = ViewStates.Gone;
                _subcategoriesListView.Visibility = ViewStates.Gone;
                _subcategoriesshadowImageView.Visibility = ViewStates.Gone;
                _isProjectsListOpen = false;
                isCategoriesListOpen = false;
                _vibe.Vibrate(50); 
                _codeCompleteTextView.Text = ""; 
                addCodeImageButton.StartAnimation(_buttonClickAnimation); 
                addBadgeDialog.Show(); 
                addBadgeRelativeLayout.StartAnimation(AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn));

                }
            };


          
            addBadgeCancelButton.Click += delegate { _vibe.Vibrate(50); addBadgeCancelButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Dismiss(); };
            addCodeButton.Click += delegate { _vibe.Vibrate(50); addCodeButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Dismiss(); addCodeDialog.Show(); addCodeRelativeLayout.StartAnimation(AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn)); };
            addCodeCancelButton.Click += delegate { _vibe.Vibrate(50); addCodeCancelButton.StartAnimation(_buttonClickAnimation); addCodeDialog.Dismiss(); };
            readQRCodeButton.Click += delegate
            {
                _vibe.Vibrate(50);
                readQRCodeButton.StartAnimation(_buttonClickAnimation); addBadgeDialog.Dismiss();
                _scanner = new MobileBarcodeScanner(this);
                _scanner.UseCustomOverlay = true;
                var zxingOverlay = LayoutInflater.FromContext(this).Inflate(Resource.Layout.qrreaderlayout, null);
                TextView qrTitleContent = (TextView)zxingOverlay.FindViewById(Resource.Id.qrreader_codetextView);
                if (!AppInfo.IsLocaleRu)
                {
                    qrTitleContent.Text = "Check QR-Code";
                }

                _scanner.CustomOverlay = zxingOverlay;

                _scanner.Scan().ContinueWith((t) =>
                {
                    if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        HandleScanResult(t.Result);
                });
            };


            _foundActionTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(_foundActionTextView_TextChanged);

            addCodeReadyButton.Click += delegate
            {
                _vibe.Vibrate(50);
                if (_codeCompleteTextView.Text.Replace(" ", "") != "")
                {
                    addCodeReadyButton.StartAnimation(_buttonClickAnimation);
                    addCodeDialog.Dismiss();



                    _progressDialogMessage.Text = "Активация достижения..."; 
                    if (!AppInfo.IsLocaleRu)
                    {
                        _progressDialogMessage.Text = "Badge activation..."; 
                    }
                    
                    _progressDialog.SetCancelable(false);
                    _progressDialog.Show();

                    ThreadStart threadStart = new ThreadStart(AsyncActivizationViaEntering);
                    Thread loadThread = new Thread(threadStart);
                    loadThread.Start();
                }
                else
                {
                    string toastStr = "Введите код активации";
                    if (!AppInfo.IsLocaleRu)
                    {
                        toastStr = "Enter activation code";
                    }

                    Toast.MakeText(this, toastStr, ToastLength.Short).Show();
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
                string qrtoastStr = "QR Code несоответствует формату";
                if (!AppInfo.IsLocaleRu)
                {
                    qrtoastStr = "Wrong QR-Code format mismatch";
                }
                RunOnUiThread(() => Toast.MakeText(this, qrtoastStr, ToastLength.Short).Show());
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
            _progressDialogMessage.Text = "Активация достижения...";
            if (!AppInfo.IsLocaleRu)
            {
                _progressDialogMessage.Text = "Badge activation...";
            }
            _progressDialog.SetCancelable(false);
            _progressDialog.Show();
        }

        string activatedBadgeFbId = "null";
        string errorDescr = "null";

        public void AsyncActivizationViaQR(string badgeApi_name)
        {
            ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve activatedAchieve;
            string response = "null";
            try
            {
                response = _serviceItsBeta.ActivateBadge(badgeApi_name, AppInfo._appaccess_token, AppInfo._user.FacebookUserID);
            }
            catch 
            {
            }

                if (response.StartsWith("badgefbId="))
                {
                    activatedBadgeFbId = response.Replace("badgefbId=", "");
                    try
                    {
                        AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId, AppInfo.IsLocaleRu);
                    }
                    catch
                    {
                        response = "null";
                        return;
                    }
                    
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
                    RunOnUiThread(() => _progressDialog.Dismiss());
                    //RunOnUiThread(() =>CreateAchievementsViewObject());

                    if (errorDescr == "obj not found")
                    {
                        errorDescr = "Неверный код активации";
                        if (!AppInfo.IsLocaleRu)
                        {
                            errorDescr = "Wrong activation code";
                        }
                    }
                    if (errorDescr == "activation code is used")
                    {
                        errorDescr = "Код уже активирован";
                        if (!AppInfo.IsLocaleRu)
                        {
                            errorDescr = "Code is already activated";
                        }

                    }

                    _wrongCodeDialogTitle.Text = "Информация";
                    if (!AppInfo.IsLocaleRu)
                    {
                        _wrongCodeDialogTitle.Text = "Information";
                    }
                    _wrongCodeDialogMessage.Text = errorDescr;

                    RunOnUiThread(() => _wrongCodeDialog.Show());
                }
                if (response == "null")
                {
                    RunOnUiThread(() => _progressDialog.Dismiss());
                    errorDescr = "Неудалось активировать. Проверьте настройки интернет соединения";
                    _wrongCodeDialogTitle.Text = "Ошибка";
                    if (!AppInfo.IsLocaleRu)
                    {
                        errorDescr = "Activation error. Internet connection is missing";
                        _wrongCodeDialogTitle.Text = "Error";
                    }
                    _wrongCodeDialogMessage.Text = errorDescr;

                    RunOnUiThread(() => _wrongCodeDialog.Show());
                }
        }

        public void AsyncActivizationViaEntering()
        {
            ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve activatedAchieve;
            string response = "null";
            try
            {
                response = _serviceItsBeta.ActivateBadge(_codeCompleteTextView.Text, AppInfo._appaccess_token, AppInfo._user.FacebookUserID);
            }
            catch
            {
            }

            if (response.StartsWith("badgefbId="))
            {
                activatedBadgeFbId = response.Replace("badgefbId=", "");
                try
                {
                    AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId, AppInfo.IsLocaleRu);
                }
                catch
                {
                    response = "null";
                    return;
                }
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
                RunOnUiThread(() => _progressDialog.Dismiss());
                //RunOnUiThread(() =>CreateAchievementsViewObject());

                if (errorDescr == "obj not found")
                {
                    errorDescr = "Неверный код активации";
                    if (!AppInfo.IsLocaleRu)
                    {
                        errorDescr = "Wrong activation code";
                    }
                }
                if (errorDescr == "activation code is used")
                {
                    errorDescr = "Код уже активирован";
                    if (!AppInfo.IsLocaleRu)
                    {
                        errorDescr = "Code is already activated";
                    }
                }

                _wrongCodeDialogTitle.Text = "Информация";
                if (!AppInfo.IsLocaleRu)
                {
                    _wrongCodeDialogTitle.Text = "Information";
                }
                _wrongCodeDialogMessage.Text = errorDescr;

                RunOnUiThread(() => _wrongCodeDialog.Show());
                return;
            }
            if (response == "null")
            {
                RunOnUiThread(() => _progressDialog.Dismiss());
                errorDescr = "Неудалось активировать. Проверьте настройки интернет соединения";
                _wrongCodeDialogTitle.Text = "Ошибка";
                if (!AppInfo.IsLocaleRu)
                {
                    errorDescr = "Activation error. Internet connection is missing";
                    _wrongCodeDialogTitle.Text = "Error";
                }
                _wrongCodeDialogMessage.Text = errorDescr;

                RunOnUiThread(() => _wrongCodeDialog.Show());
            }
        }

        void CompleteActivation(ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve activatedBadge)
        {
            _progressDialog.Dismiss();

            GetCategoryView();
            GetProjectsView();

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(LayoutInflaterService);
            ViewGroup relativeAgedSummary = new RelativeLayout(this);
            View layout = inflater.Inflate(Resource.Layout.receivebadgelayount, relativeAgedSummary);

            ImageView badgeImage = (ImageView)layout.FindViewById(Resource.Id.recbadgewin_BadgeImageView);
            Button inactiveButton = (Button)layout.FindViewById(Resource.Id.recbadgewin_inactiveButton);
            ImageButton badgeReadyButton = (ImageButton)layout.FindViewById(Resource.Id.recbadgewin_CloseImageButton);
            ImageButton badgeReadyButtonFake = (ImageButton)layout.FindViewById(Resource.Id.recbadgewin_CloseImageButtonFake);

            TextView profileName = (TextView)layout.FindViewById(Resource.Id.recbadgewin_badgeTextView);
            TextView badgeDescr = (TextView)layout.FindViewById(Resource.Id.recbadgewin_wonderdescrTextView);
            TextView badgeHowGetted = (TextView)layout.FindViewById(Resource.Id.recbadgewin_howwonderTextView);
            
            if (!AppInfo.IsLocaleRu)
            {
                badgeHowGetted.Text = "You got a new Badge";
            }


            //recbadgewin_howwonderTextView
            profileName.Text = AppInfo._user.Fullname;
            badgeDescr.Text = activatedBadge.Description;
            AppInfo._badgesCount += 1;
            _badgesCountDisplay.Text = AppInfo._badgesCount.ToString();
            Bitmap bitmap = BitmapFactory.DecodeFile(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" + activatedBadge.ApiName + ".PNG");
            badgeImage.SetImageBitmap(bitmap);
            bitmap.Dispose();

            LinearLayout bonusPaperListLinearLayout = (LinearLayout)layout.FindViewById(Resource.Id.bonuspaperlist_linearLayout);
            //
            bonusPaperListLinearLayout.RemoveAllViews();

            if (activatedBadge.Bonuses.Count() == 0)
            {
                bonusPaperListLinearLayout.Visibility = ViewStates.Gone;
            }
            if (activatedBadge.Bonuses.Count() == 1)
                {
                    var bonus = activatedBadge.Bonuses.First();
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
            if (activatedBadge.Bonuses.Count() > 1)
            {
                foreach (var bonus in activatedBadge.Bonuses)
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
                if (!AppInfo.IsLocaleRu)
                {
                    msg = "QR-code detected";// + result.Text;
                }
                this.RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Short).Show());
            }

            if (found)
            {
                this.RunOnUiThread(() => _foundActionTextView.Text = result.Text);
            }
        }
        #endregion
    }
}
