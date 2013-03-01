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
        public static List<SubCategoriesListData> _subcategoriesList;
        static ListView _subcategoriesListView;
        SubCategoriesListItemAdapter _subcategoriesListAdapter;
        static RelativeLayout _subcategoryViewRelativeLayout;
        static bool _isProjectsListOpen = false;
        public static string _selectedsubCategoryId;

        void GetProjectsView()
        {
            _subcategoriesList = new List<SubCategoriesListData>();

            _subcategoriesListView = FindViewById<ListView>(Resource.Id.projectslistView);
            _subcategoriesListView.Visibility = ViewStates.Invisible;
            _subcategoriesListView.DividerHeight = 0;
            _subcategoriesList.Add(new SubCategoriesListData() { SubCategoryNameText = "Все проекты" });
            _subcategoryViewRelativeLayout.FindViewById<TextView>(Resource.Id.secondscr_projectNameRowTextView).Text = _subcategoriesList[0].SubCategoryNameText;
            _selectedsubCategoryId = _subcategoriesList[0].SubCategoryNameText;

            foreach (var category in AppInfo._achievesInfo.CategoryArray)
            {
                if (category.DisplayName == _selectedCategoryId)    
                {
                    foreach (var project in category.Projects)
                    {
                        _subcategoriesList.Add(new SubCategoriesListData() { SubCategoryNameText = project.DisplayName });
                    }
                }
            }

            _subcategoriesListAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.SecondScreenDropDownListRow, _subcategoriesList);
            _subcategoriesListView.Adapter = _subcategoriesListAdapter;

        }

        void _subcategoryViewRelativeLayout_Click(object sender, EventArgs e)
        {
            if (!_isProjectsListOpen)
            {
                //_subcategoriesListView.StartAnimation(_buttonClickAnimation);
                _subcategoriesListView.Visibility = ViewStates.Visible; 
                _inactiveListButton.Visibility = ViewStates.Visible;
                _isProjectsListOpen = true;
            }
            else
            {
                _inactiveListButton.Visibility = ViewStates.Gone;
                _subcategoriesListView.Visibility = ViewStates.Gone;
                _isProjectsListOpen = false;
            }
        }

        public static void _subcategoriesListView_ItemClick(int pos)
        {
            AppInfo._selectedSubCategoriesDictionary.All(x => false);
            if (AppInfo._selectedSubCategoriesDictionary.ContainsKey(_subcategoriesList[pos].SubCategoryNameText))
            {
                AppInfo._selectedSubCategoriesDictionary[_subcategoriesList[pos].SubCategoryNameText] = true;
            }

            _subcategoriesListView.Visibility = ViewStates.Gone;
            _inactiveListButton.Visibility = ViewStates.Gone;

            _isProjectsListOpen = false;
            _subcategoryViewRelativeLayout.FindViewById<TextView>(Resource.Id.secondscr_projectNameRowTextView).Text = _subcategoriesList[pos].SubCategoryNameText;
            _selectedsubCategoryId = _subcategoriesList[pos].SubCategoryNameText;
            _subcategoriesListView.InvalidateViews();
        }
    }
}
