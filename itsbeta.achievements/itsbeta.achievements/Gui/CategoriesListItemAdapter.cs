using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Views.Animations;

namespace itsbeta.achievements.gui
{
    public class CategoriesListItemAdapter : ArrayAdapter<CategoriesListData>
    {
        private IList<CategoriesListData> Items;
        ListView _listView;

        public CategoriesListItemAdapter(Context context, int textViewResourceId, IList<CategoriesListData> items, ListView listView)
            : base(context, textViewResourceId, items)
        {
            Items = items;
            _listView = listView;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                //выбираем разметку, которую будем наполнять данными.
                view = inflater.Inflate(Resource.Layout.SecondScreenDropDownListRow, null);
            }

            //получаем текущий элемент
            CategoriesListData item = Items[position];
            
            TextView categoryNameTextView = (TextView)view.FindViewById(Resource.Id.CategNameTextView);
            ImageView checkImageView = (ImageView)view.FindViewById(Resource.Id.CheckImageView);
            categoryNameTextView.Text = item.CategoryNameText;
            categoryNameTextView.SetTextColor(Android.Graphics.Color.DarkGray);

            if (_listView.SelectedItemPosition==position)
            {
                categoryNameTextView.SetTextColor(new Android.Graphics.Color(105, 216, 248));
                checkImageView.Visibility = ViewStates.Visible;
            }

            else
            {
                categoryNameTextView.SetTextColor(Android.Graphics.Color.DarkGray);
                checkImageView.Visibility = ViewStates.Invisible;
            }
            return view;
        }


    }

}
