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
        public static List<CategoriesListData> _categoriesList;
        static ListView _categoriesListView;
        CategoriesListItemAdapter _categoriesListAdapter;
        static RelativeLayout _categoryViewRelativeLayout;
        static bool isCategoriesListOpen = false;
        public static string _selectedCategoryId;
        static TextView _refreshProjectsTextView;

        void GetCategoryView()
        {
            _categoriesList = new List<CategoriesListData>();
            _categoriesListView = FindViewById<ListView>(Resource.Id.categorieslistView);
            _categoriesListView.Visibility = ViewStates.Gone;
            _categoriesListView.DividerHeight = 0;
            _categoryViewRelativeLayout.Click += new EventHandler(_categoryViewRelativeLayout_Click);

            _refreshProjectsTextView = new TextView(this);
             #region Create List Fields
            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                if (i == 0)
                {
                    _categoriesList.Add(new CategoriesListData()
                    {
                        CategoryNameText = AppInfo._achievesInfo.CategoryArray[i].DisplayName
                    });
                    _selectedCategoryId = _categoriesList[0].CategoryNameText;
                    _categoryViewRelativeLayout.FindViewById<TextView>(Resource.Id.secondscr_CategNameRowTextView).Text = _categoriesList[0].CategoryNameText;
                }
                else
                {
                    _categoriesList.Add(new CategoriesListData()
                    {
                        CategoryNameText = AppInfo._achievesInfo.CategoryArray[i].DisplayName
                    });
                }
            }
            #endregion

            _categoriesListAdapter = new CategoriesListItemAdapter(this, Resource.Layout.SecondScreenDropDownListRow, _categoriesList);
            _categoriesListView.Adapter = _categoriesListAdapter;
        }

        void _categoryViewRelativeLayout_Click(object sender, EventArgs e)
        {
            if (!isCategoriesListOpen)
            {
                //_categoriesListView.StartAnimation(_buttonClickAnimation);
                _inactiveListButton.Visibility = ViewStates.Visible;
                _categoriesListView.Visibility = ViewStates.Visible;
                isCategoriesListOpen = true;
            }
            else
            {
                //_categoryViewRelativeLayout.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent) { Weight = 1 };
                _inactiveListButton.Visibility = ViewStates.Gone;
                _categoriesListView.Visibility = ViewStates.Gone;
                isCategoriesListOpen = false;
            }
        }
        
        public static void _categoriesListView_ItemClick(int pos)
        {
            AppInfo._selectedCategoriesDictionary.All(x => false);
            AppInfo._selectedCategoriesDictionary[_categoriesList[pos].CategoryNameText] = true;
            //_categoriesListView.StartAnimation(_fadeoutClickAnimation);
            _inactiveListButton.Visibility = ViewStates.Gone;
            _categoriesListView.Visibility = ViewStates.Gone;
            _categoryViewRelativeLayout.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent) { Weight = 1 };
            isCategoriesListOpen = false;
            _categoryViewRelativeLayout.FindViewById<TextView>(Resource.Id.secondscr_CategNameRowTextView).Text = _categoriesList[pos].CategoryNameText;
            _selectedCategoryId = _categoriesList[pos].CategoryNameText;
            _categoriesListView.InvalidateViews();
            _refreshProjectsTextView.Text = "ref";
        }

        void _refreshProjectsTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            GetProjectsView();
        }
    }
}
