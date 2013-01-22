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

using Achievements.AndroidPlatform.GUI;
using ItsBeta.Core;

namespace Achievements.AndroidPlatform
{
    public partial class MainActivity : Activity
    {
        List<CategoriesListData> _categoriesList;
        PopupWindow _categoriesPopupWindow;

        private void CreateCategoriesViewObject()
        {
            
            _categoriesList = new List<CategoriesListData>();

            for (int i = 0; i < _achievesInfo.CategoriesCount; i++)
            {
                _categoriesList.Add(new CategoriesListData() { CategoryNameText = String.Format("{0}",
                    _achievesArray[i].DisplayName),
                    IsCategoryActive = true });

                _selectedCategoriesDictionary.Add(_achievesArray[i].DisplayName, _categoriesList[i].IsCategoryActive);
            }

            _categoriesListView = new ListView(this);

            CategoriesListItemAdapter categoriesAdapter = new CategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow,
                _categoriesList, _selectedCategoriesDictionary);

            _categoriesListView.Adapter = categoriesAdapter;

            LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            _categoriesListView.SetWillNotCacheDrawing(true);
            _categoriesPopupWindow = new PopupWindow(_categoriesListView,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
            
            _navigationBarImageButton.Click += delegate
            {
                if (!_isBarCategoriesListOpen)
                {
                    _categoriesPopupWindow.ShowAsDropDown(FindViewById<ImageButton>(Resource.Id.NavigationBarImageButton), 0, -5);
                    _isBarCategoriesListOpen = true;
                    return;
                }
                if (_isBarCategoriesListOpen)
                {
                    _categoriesPopupWindow.Dismiss();
                    categoriesLinearLayout.StartAnimation(categoriesChosedAnimation);
                    _isBarCategoriesListOpen = false;
                }
            };
        }
        

        int checkedCategoriesCount;
        bool _isBarSubCategoriesListOpen = false;
         PopupWindow subCategoriesPopupWindow;
        private void CreateSubCategoriesViewObject()
        {
            checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);
            
            foreach (var item in _selectedCategoriesDictionary)
            {
                if (item.Value == true)
                {
                    var categoryButton = new Button(this);
                    var cornerImageView = new ImageView(this);
                    cornerImageView.LayoutParameters = new ViewGroup.LayoutParams(20, 20);
                    cornerImageView.SetBackgroundResource(Resource.Drawable.Categories_btn_arrow);

                    categoryButton.Text = item.Key;
                    categoriesLinearLayout.AddView(categoryButton);
                    categoryButton.SetBackgroundColor(global::Android.Graphics.Color.Argb(2, 0, 0, 0));
                    categoryButton.Click += delegate { categoryButton.StartAnimation(buttonClickAnimation); };
                    categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
                    categoryButton.Gravity = GravityFlags.Left;
                    categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;

                    //middle
                    
                    List<SubCategoriesListData> subCategoriesList = new List<SubCategoriesListData>();
                    for (int i = 0; i < _achievesInfo.CategoriesCount; i++) 
                    {
                        if (item.Key == _achievesInfo.ParentCategoryArray()[i].DisplayName) 
                        {
                            for (int j  = 0; j < _achievesInfo.ParentCategoryArray()[i].Projects.Count(); j++)
                            {
                                subCategoriesList.Add(new SubCategoriesListData()
                                {
                                    SubCategoryNameText = String.Format("{0}", _achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName),
                                    IsSubCategoryActive = true
                                });
                                _selectedSubCategoriesDictionary.Add(_achievesInfo.ParentCategoryArray()[i].Projects[j].DisplayName, 
                                    subCategoriesList[j].IsSubCategoryActive);
                            }

                            _subCategoriesListView = new ListView(this);

                            var subCategoriesAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow,
                                subCategoriesList, _selectedSubCategoriesDictionary, i);

                            _subCategoriesListView.Adapter = subCategoriesAdapter;

                            LayoutInflater sublayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
                            _subCategoriesListView.SetWillNotCacheDrawing(true);

                            subCategoriesPopupWindow = new PopupWindow(_subCategoriesListView,
                                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
                        }
                        categoryButton.Click += delegate
                        {
                            if (!_isBarSubCategoriesListOpen)
                            {
                                _categoriesPopupWindow.Dismiss();
                                subCategoriesPopupWindow.ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout), 0, -12);
                                _isBarSubCategoriesListOpen = true;
                                return;
                            }
                            if (_isBarSubCategoriesListOpen)
                            {
                                _categoriesPopupWindow.Dismiss();
                                subCategoriesPopupWindow.Dismiss();
                                _isBarSubCategoriesListOpen = false;

                            }
                        };
                    }

                    //middleend

                    //end
                }
            }

            _navigationBarImageButton.Click += delegate
            {

                _navigationBarImageButton.StartAnimation(buttonClickAnimation);
                categoriesLinearLayout.RemoveAllViewsInLayout();
                categoriesLinearLayout.RemoveAllViews();
                //исправить нумурацию на ключи

                checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);
                foreach (var item in _selectedCategoriesDictionary)
                {
                    if (item.Value == true)
                    {
                        var categoryButton = new Button(this);

                        var cornerImageView = new ImageView(this);
                        cornerImageView.LayoutParameters = new ViewGroup.LayoutParams(20, 20);
                        cornerImageView.SetBackgroundResource(Resource.Drawable.Categories_btn_arrow);


                        categoryButton.Text = item.Key;
                        categoriesLinearLayout.AddView(categoryButton);
                        categoryButton.Click += delegate { categoryButton.StartAnimation(buttonClickAnimation); };
                        categoryButton.SetBackgroundColor(global::Android.Graphics.Color.Argb(2, 0, 0, 0));

                        //categoryButton.SetBackgroundResource(Resource.Drawable.Categories_btn_norm);

                        categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
                        categoryButton.Gravity = GravityFlags.Left;
                        categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;

                        categoryButton.Click += delegate
                        {
                            if (!_isBarSubCategoriesListOpen)
                            {
                                subCategoriesPopupWindow.ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout), 0, -12);
                                _isBarSubCategoriesListOpen = true;
                                return;
                            }
                            if (_isBarSubCategoriesListOpen)
                            {
                                subCategoriesPopupWindow.Dismiss();
                                _isBarSubCategoriesListOpen = false;
                            }
                        };
                    }
                }
            };
        } //доделать...

        

        private void CreateAchievementsViewObject()
        {
            List<AchievementsListData> achievementsList = new List<AchievementsListData>();


            for (int i = 0; i < _achievesInfo.CategoriesCount; i++) 
            {
                for (int j = 0; j < _achievesInfo.ParentCategoryArray()[i].Projects.Count(); j++)   
                {
                    for (int k = 0; k < _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements.Count(); k++)   
                    {
                        achievementsList.Add(new AchievementsListData() {
                            AchieveNameText = String.Format("{0}", 
                            _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].DisplayName),
                            AchieveDescriptionText = String.Format("{0}",
                            _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].Description),
                            AchievePicUrl = String.Format("{0}",
                            _achievesInfo.ParentCategoryArray()[i].Projects[j].Achievements[k].PicUrl)
                        });
                    }
                }
            }


            ListView achievementsListView = FindViewById<ListView>(Resource.Id.AchivementsListView);

            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.MainLayoutListRow, achievementsList);
            achievementsListView.Adapter = adapter;
        }
    }
}
