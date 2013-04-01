using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Graphics;

namespace itsbeta.achievements.gui
{
    public class SubCategoriesListItemAdapter : ArrayAdapter<SubCategoriesListData>
    {
        private List<SubCategoriesListData> Items;
        Button _checkButton;
        int refCount =0;
        public SubCategoriesListItemAdapter(Context context, int textViewResourceId,
            List<SubCategoriesListData> items)
            : base(context, textViewResourceId, items)
        {
            Items = items;
        }

        bool isChecked;
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            SubCategoriesListData _item = new SubCategoriesListData();
            SubCategoriesListData _allitem = new SubCategoriesListData();
            foreach (var myitem in Items)
            {
                if (myitem.SubCategoryNameText == MainScreenActivity._selectedsubCategoryId)
                {
                    _item = myitem;
                }
            }
            Items.Remove(_item);
            Items.Insert(0, _item);

            foreach (var allActiveItem in Items)
            {
                if (allActiveItem.SubCategoryNameText == "Все подкатегории" || allActiveItem.SubCategoryNameText == "All subcategories")
                {
                    if (Items.IndexOf(allActiveItem) != 0)
                    {
                        _allitem = allActiveItem;
                    }
                }
            }
            if (_allitem.SubCategoryNameText == "Все подкатегории" || _allitem.SubCategoryNameText == "All subcategories")
            {
                if (Items.IndexOf(_allitem) != 0)
                {
                    Items.Remove(_allitem);
                    Items.Insert(1, _allitem);
                }
            }
            MainScreenActivity._subcategoriesList = Items;

            View view = convertView;
            if (view == null)
            {
                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.secondscreendropdownlistrow, null);
            }

            SubCategoriesListData item = Items[position];

            TextView subcategoryNameTextView = (TextView)view.FindViewById(Resource.Id.CategNameTextView);
            ImageView checkImageView = (ImageView)view.FindViewById(Resource.Id.CheckImageView);
            _checkButton = (Button)view.FindViewById(Resource.Id.check_button);
            _checkButton.Visibility = ViewStates.Gone;

            subcategoryNameTextView.Text = item.SubCategoryNameText;
            subcategoryNameTextView.SetTextColor(Android.Graphics.Color.DarkGray);
            checkImageView.Visibility = ViewStates.Invisible;

            if (item.SubCategoryNameText == MainScreenActivity._selectedsubCategoryId)
            {
                isChecked = true;
            }
            else
            {
                isChecked = false;
            }


            if (!isChecked)
            {
                subcategoryNameTextView.SetTextColor(Android.Graphics.Color.DarkGray);
                checkImageView.Visibility = ViewStates.Invisible;
            }
            else
            {
                subcategoryNameTextView.SetTextColor(Color.ParseColor("#6abada"));
                checkImageView.Visibility = ViewStates.Visible;
            }

            //if (refCount == 0)
            //{
            //    _checkButton.Click += delegate
            //    {
            //        MainScreenActivity._subcategoriesListView_ItemClick(position);
            //        refCount++;
            //    };
            //}
            return view;
        }
    }
    
}
