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
        CategoriesListItemAdapter _categoriesListAdapter;

        public static List<CategoriesListData> _categoriesList;
        public static string _selectedCategoryId;
        public static string _previousSelectedCategoryId;

        ImageView _categoriesshadowImageView;
        ListView _categoriesListView;
        RelativeLayout _categoryViewRelativeLayout;
        bool isCategoriesListOpen = false;
        TextView _refreshProjectsAndAchTextView;

        void GetCategoryView()
        {
            _categoriesList = new List<CategoriesListData>();
            _categoriesListView = FindViewById<ListView>(Resource.Id.categorieslistView);
            _categoriesshadowImageView = FindViewById<ImageView>(Resource.Id.categoriesListDownShadowImageView);
            _categoriesListView.Visibility = ViewStates.Gone;
            _categoriesListView.DividerHeight = 0;

            
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
                if (_previousSelectedCategoryId == null)
                {
                    _previousSelectedCategoryId = _selectedCategoryId;
                }
            }
            #endregion

            _categoriesListAdapter = new CategoriesListItemAdapter(this, Resource.Layout.SecondScreenDropDownListRow, _categoriesList);
            _categoriesListView.Adapter = _categoriesListAdapter;
            _categoriesshadowImageView.Visibility = ViewStates.Gone;
        }
        
        void _categoryViewRelativeLayout_Click(object sender, EventArgs e)
        {
            if (!isCategoriesListOpen && !_badgePopupWindow.IsShowing)
            {
                //_categoriesListView.StartAnimation(_buttonClickAnimation);
                _inactiveListButton.Visibility = ViewStates.Visible;
                _categoriesListView.Visibility = ViewStates.Visible;
                _categoriesshadowImageView.Visibility = ViewStates.Visible;
                isCategoriesListOpen = true;
            }
            else
            {
                //_categoryViewRelativeLayout.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent) { Weight = 1 };
                _inactiveListButton.Visibility = ViewStates.Gone;
                _categoriesListView.Visibility = ViewStates.Gone;
                _categoriesshadowImageView.Visibility = ViewStates.Gone;
                isCategoriesListOpen = false;
            }
        }


        void _categoriesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position == 0)
            {
                _inactiveListButton.Visibility = ViewStates.Gone;
                _categoriesListView.Visibility = ViewStates.Gone;
                _categoriesshadowImageView.Visibility = ViewStates.Gone;
                isCategoriesListOpen = false;
                return;
            }
            if (e.Position != 0)
            {
                _inactiveListButton.Visibility = ViewStates.Gone;
                _categoriesListView.Visibility = ViewStates.Gone;
                _categoriesshadowImageView.Visibility = ViewStates.Gone;

                _categoryViewRelativeLayout.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent) { Weight = 1 };
                isCategoriesListOpen = false;
                _categoryViewRelativeLayout.FindViewById<TextView>(Resource.Id.secondscr_CategNameRowTextView).Text = _categoriesList[e.Position].CategoryNameText;
                _selectedCategoryId = _categoriesList[e.Position].CategoryNameText;
                _categoriesListView.InvalidateViews();
                _refreshProjectsAndAchTextView.Text = "ref";
                _previousSelectedCategoryId = _selectedCategoryId;
            }
        }


        void _refreshProjectsAndAchTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
                GetProjectsView();
                GetAchievementsView();
        }
    }
}
