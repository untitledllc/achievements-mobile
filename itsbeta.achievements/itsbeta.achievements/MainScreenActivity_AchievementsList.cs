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
using Android.Graphics.Drawables;

namespace itsbeta.achievements
{
    public partial class MainScreenActivity
    {
        List<AchievementsListData> _achievementsList;
        List<string> _achievementsListBadgesImagePath;

        PopupWindow _badgePopupWindow;

        void GetAchievementsView()
        {
            GC.Collect();
            isItemClicked = false;
            
            if (_achievementsListView!=null)
            {
                //_achievementsListView.Dispose();
            }


            _achievementsList = new List<AchievementsListData>();
            _achievementsListBadgesImagePath = new List<string>();

            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                for (int j = 0; j < AppInfo._achievesInfo.CategoryArray[i].Projects.Count(); j++)
                {
                    for (int k = 0; k < AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements.Count(); k++)
                    {
                        if (AppInfo._achievesInfo.CategoryArray[i].Projects[j].DisplayName == _selectedsubCategoryId)
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
                                Bonuses = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Bonuses,
                                HexColor = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Color
                            });
                        }

                        if (_selectedCategoryId == "All categories" || _selectedCategoryId == "Все категории")
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
                                Bonuses = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Bonuses,
                                HexColor = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Color
                            });
                        }
                        
                        if (!AppInfo.IsLocaleRu)
                        {
                            if (AppInfo._achievesInfo.CategoryArray[i].DisplayName == _selectedCategoryId && _selectedsubCategoryId== "All subcategories")
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
                                    Bonuses = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Bonuses,
                                    HexColor = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Color
                                });
                            }
                        }
                        else
                        {
                            if (AppInfo._achievesInfo.CategoryArray[i].DisplayName == _selectedCategoryId && _selectedsubCategoryId == "Все подкатегории")
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
                                    Bonuses = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Bonuses,
                                    HexColor = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Color
                                });
                            }
                        }
                    }
                }
            }
            
            _achievementsListView.DividerHeight = 0;

            _achievementsList = _achievementsList.OrderByDescending(x => x.AchieveReceivedDateTime).ToList();
            
            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.secondscreenlistrow, _achievementsList);


            _achievementsListView.DrawingCacheEnabled = true;
            _achievementsListView.Adapter = adapter;
            //adapter.Dispose();
            _achievementsListView.Focusable = false;
        }


        public static bool isItemClicked = false;
        ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve achieve;
        void achievementsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GC.Collect();
            isItemClicked = true;
            if (!_badgePopupWindow.IsShowing && _subcategoriesListView.Visibility == ViewStates.Gone && _categoriesListView.Visibility == ViewStates.Gone)
            {
                int iID = 0;
                int jID = 0;

                _vibe.Vibrate(50);

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
                View layout = inflater.Inflate(Resource.Layout.badgewindowactivitylayout, relativeAgedSummary);
                ImageView badgeImage = (ImageView)layout.FindViewById(Resource.Id.badgewin_BadgeImageView);
                ImageView badgeImageShape = (ImageView)layout.FindViewById(Resource.Id.badgewin_BadgeImageViewShadow);

                ImageView badgeSheet = (ImageView)layout.FindViewById(Resource.Id.BadgeSheetImageView);
                TextView badgeName = (TextView)layout.FindViewById(Resource.Id.badgewin_badgeTextView);

                TextView categoryNameProjectName = (TextView)layout.FindViewById(Resource.Id.badgewin_categ_projectTextView);
                TextView badgeHowWonderDescr = (TextView)layout.FindViewById(Resource.Id.badgewin_wonderdescrTextView);
                TextView badgeHowDescr = (TextView)layout.FindViewById(Resource.Id.badgewin_howwonderTextView);
                TextView badgeDetails = (TextView)layout.FindViewById(Resource.Id.badgewin_detailsTextView);


                if (!AppInfo.IsLocaleRu)
                {
                    badgeHowDescr.Text = "Wonder how this Badge was earned?";
                }

                //badgewin_howwonderTextView
                ImageButton badgeReadyButton = (ImageButton)layout.FindViewById(Resource.Id.badgewin_CloseImageButton);
                ImageButton badgeReadyButtonfake = (ImageButton)layout.FindViewById(Resource.Id.badgewin_CloseImageButtonFake);

                //Button badgeInactiveBackgroundButton = (Button)layout.FindViewById(Resource.Id.badgewin_inactiveButton);
                //badgeInactiveBackgroundButton.Click += new EventHandler(badgeInactiveBackgroundButton_Click);
                
                badgeName.Text = achieve.DisplayName;
                //using (var bitmap = BitmapFactory.DecodeFile(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" + achieve.ApiName + ".PNG"))
                //{
                //    badgeImage.SetImageBitmap(bitmap);
                //}
                using (var d = Drawable.CreateFromPath(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" + achieve.ApiName + ".PNG"))
                {
                    badgeImage.SetImageDrawable(d);
                }

                //badgeImage.SetScaleType(ImageView.ScaleType.FitStart);
                //badgeImageShape.SetImageResource(Resource.Drawable.Paper_BadgeShape);

                categoryNameProjectName.Text = AppInfo._achievesInfo.CategoryArray[iID].DisplayName + ", " + AppInfo._achievesInfo.CategoryArray[iID].Projects[jID].DisplayName;

                badgeDetails.SetText(Android.Text.Html.FromHtml(achieve.Details), TextView.BufferType.Normal);
                badgeHowWonderDescr.SetText(Android.Text.Html.FromHtml(achieve.Description), TextView.BufferType.Spannable);
                badgeHowWonderDescr.Append("\n");
                badgeHowWonderDescr.Append(Android.Text.Html.FromHtml(achieve.Advertisments));
                badgeHowWonderDescr.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;

                categoryNameProjectName.SetTypeface(_font, TypefaceStyle.Normal);
                badgeHowWonderDescr.SetTypeface(_font, TypefaceStyle.Normal);
                badgeHowDescr.SetTypeface(_font, TypefaceStyle.Normal);
                badgeDetails.SetTypeface(_font, TypefaceStyle.Normal);
                badgeName.SetTypeface(_font, TypefaceStyle.Normal);


                //badgeHowWonderDescr.MovementMethod.
                
                LinearLayout bonusPaperListLinearLayout = (LinearLayout)layout.FindViewById(Resource.Id.bonuspaperlist_linearLayout);
                //
                bonusPaperListLinearLayout.RemoveAllViews();
                if (achieve.Bonuses.Count() == 0)
                {
                    bonusPaperListLinearLayout.Visibility = ViewStates.Gone;
                }
                if (achieve.Bonuses.Count() == 1)
                {
                    var bonus = achieve.Bonuses.First();
                        LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                        View bonusView = layoutInflater.Inflate(Resource.Layout.bonusonlistrowlayout, null);

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
                        bonusName.SetTypeface(_font, TypefaceStyle.Normal);
                        bonusDescr.SetTypeface(_font, TypefaceStyle.Normal);

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
                if (achieve.Bonuses.Count() > 1)
                {
                    foreach (var bonus in achieve.Bonuses)
                    {
                        LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                        View bonusView = layoutInflater.Inflate(Resource.Layout.bonusonlistrowlayout, null);

                        ImageView bonusLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_GreenBonusImageView);
                        ImageView discountLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_BlueBonusImageView);
                        ImageView giftLineImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_VioletBonusImageView);

                        ImageView bonusDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_greendescbackgroundImageView);
                        ImageView discountDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_bluedescbackgroundImageView);
                        ImageView giftDescrBackgroundImage = (ImageView)bonusView.FindViewById(Resource.Id.badgewin_violetdescbackgroundImageView);

                        TextView bonusName = (TextView)bonusView.FindViewById(Resource.Id.badgewin_bonusTextView);
                        TextView bonusDescr = (TextView)bonusView.FindViewById(Resource.Id.badgewin_bonusdescrTextView);
                        bonusName.SetTypeface(_font, TypefaceStyle.Normal);
                        bonusDescr.SetTypeface(_font, TypefaceStyle.Normal);
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
                


                _badgePopupWindow = new PopupWindow(layout,
                    LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
                _badgePopupWindow.ShowAsDropDown(FindViewById<TextView>(Resource.Id.secaondscr_faketextView), 0, 0);


                badgeReadyButton.Click += delegate
                {
                    _vibe.Vibrate(50);
                    badgeReadyButtonfake.StartAnimation(_buttonClickAnimation);
                    _badgePopupWindow.Dismiss();
                };
                badgeSheet.Click += delegate{ };
                badgeImage.Click += delegate { };
            }
        }

        void badgeInactiveBackgroundButton_Click(object sender, EventArgs e)
        {
            if (_badgePopupWindow.IsShowing)    
            {
                _badgePopupWindow.Dismiss();
            }
        }

        public void OnBadgeListRefresh()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId, AppInfo.IsLocaleRu);

                    AppInfo._subcategCount = 0;
                    AppInfo._badgesCount = 0;
                    AppInfo._bonusesCount = 0;

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
                                    bitmap = GetImageBitmap(AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].PicUrl);
                                    
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

                    RunOnUiThread(() =>
                    _badgesCountDisplay.Text = AppInfo._badgesCount.ToString()
                    );
                    RunOnUiThread(() => 
                    _progressDialog.Dismiss());

                    RunOnUiThread(() => _context.GetAchievementsView());
                }
                catch (Exception)
                {
                    _context.RunOnUiThread(() =>
                    _progressDialog.Dismiss());
                    string errorMessage = "Error while refreshing";
                    if (AppInfo.IsLocaleRu)
                    {
                        errorMessage = "Ошибка обновления";
                    }
                    _context.RunOnUiThread(() => Toast.MakeText(_context, errorMessage, ToastLength.Short).Show());
                }
            });
                
        }
    }

    
}