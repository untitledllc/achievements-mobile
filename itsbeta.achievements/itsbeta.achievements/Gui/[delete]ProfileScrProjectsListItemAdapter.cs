//using System;
//using System.Collections.Generic;
//using System.Text;
//using Android.Widget;
//using Android.Content;
//using Android.Views;
//using Android.Views.Animations;

//namespace itsbeta.achievements.gui
//{
//    public class ProfileScrProjectsListItemAdapter : ArrayAdapter<CategoriesListData>
//    {
//        private IList<CategoriesListData> Items;

//        public ProfileScrProjectsListItemAdapter(Context context, int textViewResourceId, IList<CategoriesListData> items)
//            : base(context, textViewResourceId, items)
//        {
//            Items = items;
//        }

//        public override View GetView(int position, View convertView, ViewGroup parent)
//        {
//            View view = convertView;
//            if (view == null)
//            {
//                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
//                //выбираем разметку, которую будем наполнять данными.
//                view = inflater.Inflate(Resource.Layout.ProfileScreenChildRow, null);
//            }

//            //получаем текущий элемент
//            CategoriesListData item = Items[position];
            
//            TextView projectNameTextView = (TextView)view.FindViewById(Resource.Id.profilescr_ProjectNameTextView);
//            projectNameTextView.Text = item.CategoryNameText;


//            return view;
//        }


//    }

//}
