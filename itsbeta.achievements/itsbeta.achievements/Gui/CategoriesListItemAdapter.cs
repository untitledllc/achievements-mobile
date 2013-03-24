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
    public class CategoriesListItemAdapter : ArrayAdapter<CategoriesListData>
    {
        int refCount = 0;
        private List<CategoriesListData> Items;
        ListView _listView;
        Button _checkButton;

        public CategoriesListItemAdapter(Context context, int textViewResourceId, List<CategoriesListData> items)
            : base(context, textViewResourceId, items)
        {
            
            Items = items;
            _listView = new ListView(context);
            _checkButton = new Button(context);
        }



        bool isChecked = false;
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            CategoriesListData _item = new CategoriesListData();
            foreach (var myitem in Items)
            {
                if (myitem.CategoryNameText == MainScreenActivity._selectedCategoryId)
                {
                    _item = myitem;
                }
            }
            Items.Remove(_item);
            Items.Insert(0, _item);

            MainScreenActivity._categoriesList = Items;


            View view = convertView;
            if (view == null)
            {
                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                //выбираем разметку, которую будем наполнять данными.
                view = inflater.Inflate(Resource.Layout.secondscreendropdownlistrow, null);
            }

            //получаем текущий элемент
            CategoriesListData item = Items[position];
            
            TextView categoryNameTextView = (TextView)view.FindViewById(Resource.Id.CategNameTextView);
            ImageView checkImageView = (ImageView)view.FindViewById(Resource.Id.CheckImageView);
            _checkButton = (Button)view.FindViewById(Resource.Id.check_button);
            _checkButton.Visibility = ViewStates.Gone;

            categoryNameTextView.Text = item.CategoryNameText;
            categoryNameTextView.SetTextColor(Android.Graphics.Color.DarkGray);
            checkImageView.Visibility = ViewStates.Invisible;

            if (item.CategoryNameText == MainScreenActivity._selectedCategoryId)
            {
                isChecked = true;
            }

            else
            {
                isChecked = false;
            }

            if (!isChecked)
            {
                categoryNameTextView.SetTextColor(Android.Graphics.Color.DarkGray);
                checkImageView.Visibility = ViewStates.Invisible;
            }

            else
            {
                categoryNameTextView.SetTextColor(Color.ParseColor("#6abada"));//new Android.Graphics.Color(105, 216, 248));
                checkImageView.Visibility = ViewStates.Visible;
            }

            if (refCount == 0)
            {
                //_checkButton.Click += delegate
                //{
                //    MainScreenActivity._categoriesListView_ItemClick(position);
                //    refCount++;
                //};
            }      

            return view;
        }
    }
}
