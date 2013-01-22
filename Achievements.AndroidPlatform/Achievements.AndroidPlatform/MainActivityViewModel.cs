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

        private void CreateCategoriesViewObject()
        {
            Categories categories = new Categories(access_token);

            _categoriesList = new List<CategoriesListData>();

            for (int i = 0; i < categories.Count; i++)
            {
                _categoriesList.Add(new CategoriesListData() { CategoryNameText = String.Format(i + " {0}",
                    categories.CategoriesArray()[i].Title),
                    IsCategoryActive = true });
                _selectedCategoriesDictionary.Add(i.ToString(), _categoriesList[i].IsCategoryActive);
            }


            _categoriesListView = new ListView(this);

            CategoriesListItemAdapter categoriesAdapter = new CategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow,
                _categoriesList, _selectedCategoriesDictionary);

            _categoriesListView.Adapter = categoriesAdapter;

            LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            _categoriesListView.SetWillNotCacheDrawing(true);
            PopupWindow categoriesPopupWindow = new PopupWindow(_categoriesListView,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);
            
            _navigationBarImageButton.Click += delegate
            {
                if (!_isBarCategoriesListOpen)
                {
                    categoriesPopupWindow.ShowAsDropDown(FindViewById<ImageButton>(Resource.Id.NavigationBarImageButton), 0, -5);
                    _isBarCategoriesListOpen = true;
                    return;
                }
                if (_isBarCategoriesListOpen)
                {
                    categoriesPopupWindow.Dismiss();
                    categoriesLinearLayout.StartAnimation(categoriesChosedAnimation);
                    _isBarCategoriesListOpen = false;
                }
            };
        }

        private void CreateSubCategoriesViewObject()
        {
            //int checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);

            //List<SubCategoriesListData> subCategoriesList = new List<SubCategoriesListData>();

            //for (int i = 0; i < 2; i++)
            //{
            //    subCategoriesList.Add(new SubCategoriesListData() { SubCategoryNameText = String.Format("SubCategory [{0}]", i), IsSubCategoryActive = true });
            //    _selectedSubCategoriesDictionary.Add(subCategoriesList[i].SubCategoryNameText, subCategoriesList[i].IsSubCategoryActive);
            //}

            //_subCategoriesListView = new ListView(this);

            //var subCategoriesAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow, subCategoriesList, _selectedSubCategoriesDictionary);

            //_subCategoriesListView.Adapter = subCategoriesAdapter;

            //LayoutInflater sublayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            //_subCategoriesListView.SetWillNotCacheDrawing(true);
            //PopupWindow subCategoriesPopupWindow = new PopupWindow(_subCategoriesListView,
            //    LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);


            //bool _isBarSubCategoriesListOpen = false;
            //foreach (var item in _selectedCategoriesDictionary)
            //{
            //    if (item.Value == true)
            //    {
            //        var categoryButton = new Button(this);
            //        var cornerImageView = new ImageView(this);
            //        cornerImageView.LayoutParameters = new ViewGroup.LayoutParams(20, 20);
            //        cornerImageView.SetBackgroundResource(Resource.Drawable.Categories_btn_arrow);

            //        categoryButton.Text = item.Key;
            //        categoriesLinearLayout.AddView(categoryButton);
            //        categoryButton.SetBackgroundColor(global::Android.Graphics.Color.Argb(2,0,0,0));
            //        categoryButton.Click += delegate { categoryButton.StartAnimation(buttonClickAnimation); };
            //        categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
            //        categoryButton.Gravity = GravityFlags.Left;
            //        categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;

            //        categoryButton.Click += delegate
            //        {
            //            if (!_isBarSubCategoriesListOpen)
            //            {
            //                subCategoriesPopupWindow.ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout), 0, -12);
            //                _isBarSubCategoriesListOpen = true;
            //                return;
            //            }
            //            if (_isBarSubCategoriesListOpen)
            //            {
            //                subCategoriesPopupWindow.Dismiss();
            //                _isBarSubCategoriesListOpen = false;
            //            }
            //        };
            //    }
            //}

            //_navigationBarImageButton.Click += delegate
            //{
            //    _navigationBarImageButton.StartAnimation(buttonClickAnimation);
            //    categoriesLinearLayout.RemoveAllViewsInLayout();
            //    categoriesLinearLayout.RemoveAllViews();
            //    //исправить нумурацию на ключи

            //    checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);
            //    foreach (var item in _selectedCategoriesDictionary)
            //    {
            //        if (item.Value == true)
            //        {
            //            var categoryButton = new Button(this);

            //            var cornerImageView = new ImageView(this);
            //            cornerImageView.LayoutParameters = new ViewGroup.LayoutParams(20, 20);
            //            cornerImageView.SetBackgroundResource(Resource.Drawable.Categories_btn_arrow);


            //            categoryButton.Text = item.Key;
            //            categoriesLinearLayout.AddView(categoryButton);
            //            categoryButton.Click += delegate { categoryButton.StartAnimation(buttonClickAnimation); };
            //            categoryButton.SetBackgroundColor(global::Android.Graphics.Color.Argb(2, 0, 0, 0));

            //            //categoryButton.SetBackgroundResource(Resource.Drawable.Categories_btn_norm);

            //            categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
            //            categoryButton.Gravity = GravityFlags.Left;
            //            categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;

            //            categoryButton.Click += delegate
            //            {
            //                if (!_isBarSubCategoriesListOpen)
            //                {
            //                    subCategoriesPopupWindow.ShowAsDropDown(FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout), 0, -12);
            //                    _isBarSubCategoriesListOpen = true;
            //                    return;
            //                }
            //                if (_isBarSubCategoriesListOpen)
            //                {
            //                    subCategoriesPopupWindow.Dismiss();
            //                    _isBarSubCategoriesListOpen = false;
            //                }
            //            };
            //        }
            //    }
            //};

        }

        private void CreateAchievementsViewObject()
        {
            //List<AchievementsListData> achievementsList = new List<AchievementsListData>();

            //for (int i = 0; i < 40; i++)
            //{
            //    achievementsList.Add(new AchievementsListData() { AchieveNameText = String.Format("Achieve [{0}]", i) });
            //}

            //ListView achievementsListView = FindViewById<ListView>(Resource.Id.AchivementsListView);

            //var adapter = new AchievementsListItemAdapter(this, Resource.Layout.MainLayoutListRow, achievementsList);
            //achievementsListView.Adapter = adapter;
        }
    }
}
