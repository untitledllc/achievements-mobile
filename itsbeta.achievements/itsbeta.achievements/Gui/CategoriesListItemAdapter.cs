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
                view = inflater.Inflate(Resource.Layout.SecondScreenDropDownListRow, null);
            }

            //получаем текущий элемент
            CategoriesListData item = Items[position];

            bool isChecked;

            TextView categoryNameTextView = (TextView)view.FindViewById(Resource.Id.CategNameTextView);
            ImageView checkImageView = (ImageView)view.FindViewById(Resource.Id.CheckImageView);
            Button checkButton = (Button)view.FindViewById(Resource.Id.check_button);
            categoryNameTextView.Text = item.CategoryNameText;
            categoryNameTextView.SetTextColor(new Android.Graphics.Color(105, 216, 248));

            isChecked = _selectedDictionary[AppInfo._achievesInfo.CategoryArray[position].DisplayName];

            
            checkButton.Click += delegate
            {
                if (position == 0)
                {
                    tada = true;
                    isChecked = !isChecked;
                    _selectedDictionary[AppInfo._achievesInfo.CategoryArray[position].DisplayName] = isChecked;

                    if (isChecked)
                    {
                        checkImageView.Visibility = ViewStates.Visible;
                        //#69D8F8
                        categoryNameTextView.SetTextColor(new Android.Graphics.Color(105,216,248));
                    }
                    else
                    {
                        checkImageView.Visibility = ViewStates.Invisible;
                        categoryNameTextView.SetTextColor(Android.Graphics.Color.DarkGray);
                    }
                }
                else if (tada == false)
                {
                    isChecked = !isChecked;
                    _selectedDictionary[AppInfo._achievesInfo.CategoryArray[position].DisplayName] = isChecked;

                    if (isChecked)
                    {
                        checkImageView.Visibility = ViewStates.Visible;
                        categoryNameTextView.SetTextColor(new Android.Graphics.Color(105, 216, 248));
                    }
                    else
                    {
                        checkImageView.Visibility = ViewStates.Invisible;
                        categoryNameTextView.SetTextColor(Android.Graphics.Color.DarkGray);
                    }
                }

                SecondScreenActivity.RefreshEventListTextView.Text = "changed";
            };

            tada = false;
            return view;
        }


    }

}
