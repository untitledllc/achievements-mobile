using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Views.Animations;

namespace itsbeta.achievements.gui
{
    public class SubCategoriesListItemAdapter : ArrayAdapter<SubCategoriesListData>
    {
        private IList<SubCategoriesListData> Items;
        public Dictionary<string, bool> _selectedDictionary;
        List<string> _subCategoryListName;

        public SubCategoriesListItemAdapter(Context context, int textViewResourceId,
            IList<SubCategoriesListData> items, Dictionary<string, bool> selectedDictionary, List<string> subCategoryListName)
            : base(context, textViewResourceId, items)
        {
            Items = items;
            _selectedDictionary = selectedDictionary;
            _subCategoryListName = subCategoryListName;
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
            SubCategoriesListData item = Items[position];

            bool isChecked;

            TextView categoryNameTextView = (TextView)view.FindViewById(Resource.Id.CategNameTextView);
            ImageView checkImageView = (ImageView)view.FindViewById(Resource.Id.CheckImageView);
            Button checkButton = (Button)view.FindViewById(Resource.Id.check_button);
            categoryNameTextView.Text = item.SubCategoryNameText;

            isChecked = _selectedDictionary[_subCategoryListName[position]];

            checkButton.Click += delegate
            {
                
                if (position == 0)
                {
                    tada = true;
                    isChecked = !isChecked;
                    _selectedDictionary[_subCategoryListName[position]] = isChecked;

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
                    _selectedDictionary[_subCategoryListName[position]] = isChecked;

                    if (isChecked)
                    {
                        checkImageView.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        checkImageView.Visibility = ViewStates.Invisible;
                    }
                }

                SecondScreenActivity.RefreshEventListTextView.Text = "changed";
            };



            tada = false;
            return view;
        }
    }
    
}
