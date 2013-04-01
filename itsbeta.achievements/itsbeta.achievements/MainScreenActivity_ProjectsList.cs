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
        SubCategoriesListItemAdapter _subcategoriesListAdapter;

        public static List<SubCategoriesListData> _subcategoriesList;
        public static string _selectedsubCategoryId;

        RelativeLayout _subcategoryViewRelativeLayout;
        ListView _subcategoriesListView;
        ImageView _subcategoriesshadowImageView;
        bool _isProjectsListOpen = false;

        void GetProjectsView()
        {
            _subcategoriesList = new List<SubCategoriesListData>();
            _subcategoriesListView = FindViewById<ListView>(Resource.Id.projectslistView);
            _subcategoriesshadowImageView = FindViewById<ImageView>(Resource.Id.projectsListDownShadowImageView);
            _subcategoriesshadowImageView.Visibility = ViewStates.Gone;
            _subcategoriesListView.Visibility = ViewStates.Gone;
            _subcategoriesListView.DividerHeight = 0;
            if (!AppInfo.IsLocaleRu)
            {
                _subcategoriesList.Add(new SubCategoriesListData() { SubCategoryNameText = "All subcategories" });
            }
            else
            {
                _subcategoriesList.Add(new SubCategoriesListData() { SubCategoryNameText = "Все подкатегории" });
            }
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

            _subcategoriesListAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.secondscreendropdownlistrow, _subcategoriesList);
            _subcategoriesListView.Adapter = _subcategoriesListAdapter;
            

        }

        void _subcategoryViewRelativeLayout_Click(object sender, EventArgs e)
        {
            if (!_isProjectsListOpen && !_badgePopupWindow.IsShowing)
            {
                //_subcategoriesListView.StartAnimation(_buttonClickAnimation);
                _subcategoriesListView.Visibility = ViewStates.Visible; 
                _inactiveListButton.Visibility = ViewStates.Visible;
                _subcategoriesshadowImageView.Visibility = ViewStates.Visible;
                _isProjectsListOpen = true;
            }
            else
            {
                _subcategoriesshadowImageView.Visibility = ViewStates.Gone;
                _inactiveListButton.Visibility = ViewStates.Gone;
                _subcategoriesListView.Visibility = ViewStates.Gone;
                _isProjectsListOpen = false;
            }
        }



        void _subcategoriesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position == 0)
            {
                _subcategoriesshadowImageView.Visibility = ViewStates.Gone;
                _subcategoriesListView.Visibility = ViewStates.Gone;
                _inactiveListButton.Visibility = ViewStates.Gone;
                _isProjectsListOpen = false;
            }
            if (e.Position != 0)
            {
                _subcategoriesListView.Visibility = ViewStates.Gone;
                _inactiveListButton.Visibility = ViewStates.Gone;
                _subcategoriesshadowImageView.Visibility = ViewStates.Gone;

                _isProjectsListOpen = false;
                _subcategoryViewRelativeLayout.FindViewById<TextView>(Resource.Id.secondscr_projectNameRowTextView).Text = _subcategoriesList[e.Position].SubCategoryNameText;
                _selectedsubCategoryId = _subcategoriesList[e.Position].SubCategoryNameText;

                _subcategoriesListView.InvalidateViews();

                _refreshAchTextView.Text = "ref";
            }
        }

        void _refreshAchTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            GetAchievementsView();
        }
    }
}
