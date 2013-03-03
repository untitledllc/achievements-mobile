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
    public partial class MainScreenActivity
    {
        List<AchievementsListData> _achievementsList;
        ListView _achievementsListView;
        PopupWindow _badgePopupWindow;

        void GetAchievementsView()
        {
            _achievementsList = new List<AchievementsListData>();
            
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
                                Bonuses = AppInfo._achievesInfo.CategoryArray[i].Projects[j].Achievements[k].Bonuses
                            });
                        }
                        if (AppInfo._achievesInfo.CategoryArray[i].DisplayName == _selectedCategoryId && _selectedsubCategoryId== "Все проекты")
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

            _achievementsListView = FindViewById<ListView>(Resource.Id.secondscr_listView);

            _achievementsListView.DividerHeight = 0;

            _achievementsList = _achievementsList.OrderByDescending(x => x.AchieveReceivedDateTime).ToList();

            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.SecondScreenListRow, _achievementsList);
            _achievementsListView.Adapter = adapter;
            _achievementsListView.DrawingCacheEnabled = true;

            _achievementsListView.Focusable = false;
        }

        ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve achieve;
        void achievementsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
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
                View layout = inflater.Inflate(Resource.Layout.BadgeWindowActivityLayout, relativeAgedSummary);
                ImageView badgeImage = (ImageView)layout.FindViewById(Resource.Id.badgewin_BadgeImageView);
                ImageView badgeSheet = (ImageView)layout.FindViewById(Resource.Id.BadgeSheetImageView);
                TextView badgeName = (TextView)layout.FindViewById(Resource.Id.badgewin_badgeTextView);


                TextView categoryNameProjectName = (TextView)layout.FindViewById(Resource.Id.badgewin_categ_projectTextView);
                TextView badgeHowWonderDescr = (TextView)layout.FindViewById(Resource.Id.badgewin_wonderdescrTextView);
                //@+id/badgewin_inactiveButton
                ImageButton badgeReadyButton = (ImageButton)layout.FindViewById(Resource.Id.badgewin_CloseImageButton);
                ImageButton badgeReadyButtonfake = (ImageButton)layout.FindViewById(Resource.Id.badgewin_CloseImageButtonFake);

                Button badgeInactiveBackgroundButton = (Button)layout.FindViewById(Resource.Id.badgewin_inactiveButton);
                badgeInactiveBackgroundButton.Click += new EventHandler(badgeInactiveBackgroundButton_Click);
                badgeName.Text = achieve.DisplayName;
                badgeImage.SetImageBitmap(BitmapFactory.DecodeFile(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" + achieve.ApiName + ".PNG"));
                categoryNameProjectName.Text = AppInfo._achievesInfo.CategoryArray[iID].DisplayName + ", " + AppInfo._achievesInfo.CategoryArray[iID].Projects[jID].DisplayName;
                badgeHowWonderDescr.Text = achieve.Description;

                LinearLayout bonusPaperListLinearLayout = (LinearLayout)layout.FindViewById(Resource.Id.bonuspaperlist_linearLayout);
                //
                bonusPaperListLinearLayout.RemoveAllViews();
                if (achieve.Bonuses.Count() == 1)
                {
                    var bonus = achieve.Bonuses.First();
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
                if (achieve.Bonuses.Count() > 1)
                {
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
            
    }
}