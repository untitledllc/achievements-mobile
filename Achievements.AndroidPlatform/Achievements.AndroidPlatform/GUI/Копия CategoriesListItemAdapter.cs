//using System;
//using System.Collections.Generic;
//using System.Text;
//using Android.Widget;
//using Android.Content;
//using Android.Views;
//using Android.Views.Animations;

//namespace Achievements.AndroidPlatform.GUI
//{
//    public class CategoriesListItemAdapter : ArrayAdapter<CategoriesListData>
//    {
//        private IList<CategoriesListData> Items;
//        public Dictionary<string, bool> _selectedDictionary;

//        public CategoriesListItemAdapter(Context context, int textViewResourceId, IList<CategoriesListData> items, Dictionary<string,bool> selectedDictionary)
//            : base(context, textViewResourceId, items)
//        {
//            Items = items;
//            _selectedDictionary = selectedDictionary;
//        }

//        int cyclecount = 0;
//        public override View GetView(int position, View convertView, ViewGroup parent)
//        {
//            View view = convertView;
//            if (view == null)
//            {
//                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
//                //выбираем разметку, которую будем наполнять данными.
//                view = inflater.Inflate(Resource.Layout.MainLayoutCategoryDropDownListRow, null);
//            }

//            //получаем текущий элемент
//            CategoriesListData item = Items[position];

//            CheckBox categoriesCheckBox = (CheckBox)view.FindViewById(Resource.Id.checkBox1);
//            categoriesCheckBox.SetWillNotCacheDrawing(true);
//            categoriesCheckBox.Text = item.CategoryNameText;
//            //categoriesCheckBox.Checked = item.IsCategoryActive;
//            //categoriesCheckBox.Clickable = false;
//            //if (position == 0)
//            //{
//            //    cyclecount++;
//            //    if (cyclecount == 1)
//            //    {
//            //       categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + 0 + "]"];
//            //    }
//            //    if (cyclecount == 2)
//            //    {
//            //        cyclecount = 0;
//            //    }
//            //}
//            //if (position != 0)
//            //{
//            //    if (cyclecount == 0)
//            //    {
//            //       categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
//            //    }
//            //}
//            categoriesCheckBox.Checked = _selectedDictionary["Category [" + position + "]"];


//            categoriesCheckBox.Click += delegate
//            {
//                _selectedDictionary["Category [" + position + "]"] = !_selectedDictionary["Category [" + position + "]"];
//                categoriesCheckBox.Checked = _selectedDictionary["Category [" + position + "]"];

//                //if (position == 0)
//                //{
//                //    cyclecount++;
//                //    if(cyclecount == 1)
//                //    {
//                //        MainActivity._selectedCategoriesDictionary["Category [" + 0 + "]"] = !MainActivity._selectedCategoriesDictionary["Category [" + 0 + "]"];
//                //        categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + 0 + "]"];
//                //    }
//                //    if (cyclecount == 2)
//                //    {
//                //        cyclecount = 0;
//                //    }
//                //}
//                //if (position != 0)
//                //{
//                //    if (cyclecount == 0)
//                //    {
//                //        MainActivity._selectedCategoriesDictionary["Category [" + position + "]"] = !MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
//                //        categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
//                //    }
//                //} 
//            };

//            return view;
//        }


//    }

//}
