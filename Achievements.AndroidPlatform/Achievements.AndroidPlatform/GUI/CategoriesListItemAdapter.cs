using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Views.Animations;

namespace Achievements.AndroidPlatform.GUI
{
    public class CategoriesListItemAdapter : ArrayAdapter<CategoriesListData>
    {
        private IList<CategoriesListData> Items;
        public Dictionary<string, bool> _selectedDictionary;

        public CategoriesListItemAdapter(Context context, int textViewResourceId, IList<CategoriesListData> items, Dictionary<string,bool> selectedDictionary)
            : base(context, textViewResourceId, items)
        {
            Items = items;
            _selectedDictionary = selectedDictionary;
        }

        bool tada = false;
        int cyclecount = 0;
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                //выбираем разметку, которую будем наполнять данными.
                view = inflater.Inflate(Resource.Layout.MainLayoutCategoryDropDownListRow, null);
            }

            //получаем текущий элемент
            CategoriesListData item = Items[position];

            bool isChecked;

            TextView categoryNameTextView = (TextView)view.FindViewById(Resource.Id.CategNameTextView);
            ImageView checkImageView = (ImageView)view.FindViewById(Resource.Id.CheckImageView);
            Button checkButton = (Button)view.FindViewById(Resource.Id.check_button);
            categoryNameTextView.Text = item.CategoryNameText;

            isChecked = _selectedDictionary[MainActivity._achievesArray[position].DisplayName];

            
            checkButton.Click += delegate
            {
                if (position == 0)
                {
                    tada = true;
                    isChecked = !isChecked;
                    _selectedDictionary[MainActivity._achievesArray[position].DisplayName] = isChecked;

                    if (isChecked)
                    {
                        checkImageView.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        checkImageView.Visibility = ViewStates.Invisible;
                    }
                }
                else if (tada == false)
                {
                    isChecked = !isChecked;
                    _selectedDictionary[MainActivity._achievesArray[position].DisplayName] = isChecked;

                    if (isChecked)
                    {
                        checkImageView.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        checkImageView.Visibility = ViewStates.Invisible;
                    }
                }
            };

            tada = false;
            return view;
        }


    }

}
