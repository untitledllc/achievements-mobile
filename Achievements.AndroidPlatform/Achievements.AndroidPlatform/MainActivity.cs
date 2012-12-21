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

namespace Achievements.AndroidPlatform
{
    [Activity(Label = "Achievements", MainLauncher = true,
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        ImageView badgesBarBackgroundImageView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MainActivityLayout);

            ImageView badgesBarBackgroundImageView = FindViewById<ImageView>(Resource.Id.BadgesBarBackgroundImageView);

            #region AchievementsList Local
            List<AchievementsListData> achievementsList = new List<AchievementsListData>();

            for (int i = 0; i < 10; i++)
			{
                achievementsList.Add(new AchievementsListData(){AchieveNameText = String.Format("Achieve [{0}]",i)});
			}

            ListView achievementsListView = FindViewById<ListView>(Resource.Id.AchivementsListView);
            achievementsListView.Visibility = ViewStates.Invisible;

            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.MainLayoutListRow, achievementsList);
            achievementsListView.Adapter = adapter;
            #endregion


            #region CategoriesList Local
            List<CategoriesListData> categoriesList = new List<CategoriesListData>();

            for (int i = 0; i < 3; i++)
            {
                categoriesList.Add(new CategoriesListData() { CategoryNameText = String.Format("Category [{0}]", i) });
            }

            ListView categoriesListView = new ListView(this);//FindViewById<ListView>(Resource.Id.AchivementsListView);

            var categoriesAdapter = new CategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow, categoriesList);
            
            categoriesListView.Adapter = categoriesAdapter;
            #endregion

            LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            LinearLayout ll = new LinearLayout(this);
            //View popupView = layoutInflater.Inflate(Resource.Layout.MainLayoutCategoryDropDownList, null);
            //PopupWindow popupWindow = new PopupWindow(
            //   popupView,
            //   LinearLayout.LayoutParams.WrapContent,
            //   LinearLayout.LayoutParams.WrapContent);

            PopupWindow popupWindow = new PopupWindow(categoriesListView, 
                LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            FindViewById<Button>(Resource.Id.button1).Click += delegate
            {
                popupWindow.ShowAsDropDown(FindViewById<ImageView>(Resource.Id.BadgesBarBackgroundImageView), 0, 0);
            };
            
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }

    public class AchievementsListData
    {
        public string AchieveNameText;
        public string AchieveDescriptionText;
        public string AchieveReceiveDateText;
        public ImageView AchieveImageView;

        public Button PostFacebookButton;
        public Button PostTwitterButton;
        public Button PostVKButton;
    }

    public class CategoriesListData
    {
        public string CategoryNameText;
        public bool IsCategoryActive;
    }

    public class AchievementsListItemAdapter : ArrayAdapter<AchievementsListData>
    {
        private IList<AchievementsListData> Items;

            public AchievementsListItemAdapter(Context context, int textViewResourceId, IList<AchievementsListData> items)
                : base(context, textViewResourceId, items)
            {
                Items = items;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {

                View view = convertView;
                if (view == null)
                {
                    LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                    //выбираем разметку, которую будем наполнять данными.
                    view = inflater.Inflate(Resource.Layout.MainLayoutListRow, null);
                }

                //получаем текущий элемент
                AchievementsListData item = Items[position];

                TextView achiveNameTextView = (TextView)view.FindViewById(Resource.Id.AchiveNameTextView);
                achiveNameTextView.Text = item.AchieveNameText;

                TextView achiveDescriptionTextView = (TextView)view.FindViewById(Resource.Id.AchiveDescriptionTextView);
                //achiveDescriptionTextView.Text = item.AchieveDescriptionText;

                return view;
            }
        }

    public class CategoriesListItemAdapter : ArrayAdapter<CategoriesListData>
    {
        private IList<CategoriesListData> Items;

        public CategoriesListItemAdapter(Context context, int textViewResourceId, IList<CategoriesListData> items)
            : base(context, textViewResourceId, items)
        {
            Items = items;
        }

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

            TextView categoryNameTextView = (TextView)view.FindViewById(Resource.Id.checkBox1);
            categoryNameTextView.Text = item.CategoryNameText;
            
            return view;
        }
    }
    
}