using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Views.Animations;

namespace itsbeta.achievements.gui
{
    public class ProfileScrCategoriesListItemAdapter : ArrayAdapter<CategoriesListData>
    {
        private IList<CategoriesListData> Items;

        public ProfileScrCategoriesListItemAdapter(Context context, int textViewResourceId, IList<CategoriesListData> items)
            : base(context, textViewResourceId, items)
        {
            Items = items;
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
                view = inflater.Inflate(Resource.Layout.ProfileScreenParentRow, null);
            }

            //получаем текущий элемент
            CategoriesListData item = Items[position];
            
            TextView categoryNameTextView = (TextView)view.FindViewById(Resource.Id.profilescr_CategNameTextView);
            categoryNameTextView.Text = position+1 + ". " + item.CategoryNameText;


            IList<CategoriesListData> projectsList = new List<CategoriesListData>();

            for (int i = 0; i < AppInfo._achievesInfo.CategoryArray[position].Projects.Length; i++)
            {
                projectsList.Add(new CategoriesListData()
                {
                    CategoryNameText = String.Format("{0}",
                        AppInfo._achievesInfo.CategoryArray[position].Projects[i].DisplayName),
                });
            }

            ListView projectListView = (ListView)view.FindViewById(Resource.Id.profilescr_projectsListView);

            ProfileScrProjectsListItemAdapter projectsAdapter = new ProfileScrProjectsListItemAdapter(ProfileActivity._context, Resource.Layout.ProfileScreenChildRow,
                projectsList);
            projectListView.Adapter = projectsAdapter;

           
            return view;
        }


    }

}
