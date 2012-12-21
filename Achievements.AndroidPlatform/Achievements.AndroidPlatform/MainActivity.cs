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
        Display _display;
        bool _isBarCategoriesListOpen = false;
        ListView _categoriesListView;
        public static List<bool> _selectedCategoriesList = new List<bool>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _display = WindowManager.DefaultDisplay;

            SetContentView(Resource.Layout.MainActivityLayout);

            ImageButton badgesBarBackgroundImageButton = FindViewById<ImageButton>(Resource.Id.BadgesBarBackgroundImageButton);
            //---------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------
            
            #region AchievementsList Local
            List<AchievementsListData> achievementsList = new List<AchievementsListData>();

            for (int i = 0; i < 10; i++)
			{
                achievementsList.Add(new AchievementsListData(){AchieveNameText = String.Format("Achieve [{0}]",i)});
			}

            ListView achievementsListView = FindViewById<ListView>(Resource.Id.AchivementsListView);
            
            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.MainLayoutListRow, achievementsList);
            achievementsListView.Adapter = adapter;
            #endregion
            //---------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------
            
            #region CategoriesList Local
            List<CategoriesListData> categoriesList = new List<CategoriesListData>();

            for (int i = 0; i < 3; i++)
            {
                categoriesList.Add(new CategoriesListData() { CategoryNameText = String.Format("Category [{0}]", i), IsCategoryActive = true });
                _selectedCategoriesList.Add(categoriesList[i].IsCategoryActive);
            }

            _categoriesListView = new ListView(this);

            var categoriesAdapter = new CategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow, categoriesList);
            
            _categoriesListView.Adapter = categoriesAdapter;
            

            LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);

            PopupWindow categoriesPopupWindow = new PopupWindow(_categoriesListView, 
                LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            FindViewById<ImageButton>(Resource.Id.BadgesBarBackgroundImageButton).Click += delegate
            {
                if (!_isBarCategoriesListOpen)
                {
                    categoriesPopupWindow.ShowAsDropDown(FindViewById<ImageButton>(Resource.Id.BadgesBarBackgroundImageButton), 0, 0);
                    _isBarCategoriesListOpen = true;
                    return;
                }
                if (_isBarCategoriesListOpen)
                {
                    categoriesPopupWindow.Dismiss();
                    _isBarCategoriesListOpen = false;
                }
            };
            #endregion

            //---------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------
            
            #region SubCategories Local
            

            Button[] categoryButtons;
            LinearLayout categoriesLinearLayout = FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout);
            
            FindViewById<ImageButton>(Resource.Id.BadgesBarBackgroundImageButton).Click += delegate
            {

                int checkedCategoriesCount = _selectedCategoriesList.Count(e => e == true);
                categoryButtons = new Button[checkedCategoriesCount];
                //исправить нумурацию на ключи
                for (int i = 0; i < checkedCategoriesCount; i++)
                {
                    categoryButtons[i] = new Button(this);
                    categoryButtons[i].Text = "aa" + i.ToString();
                    categoriesLinearLayout.AddView(categoryButtons[i]);
                    categoryButtons[i].SetBackgroundColor(global::Android.Graphics.Color.DarkGray);
                    categoryButtons[i].SetTextColor(global::Android.Graphics.Color.Gray);
                    categoryButtons[i].Gravity = GravityFlags.Left;
                    categoryButtons[i].LayoutParameters.Width = _display.Width / checkedCategoriesCount;
                }
            };
            #endregion
            //---------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------
            
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
            
            CheckBox categoriesCheckBox = (CheckBox)view.FindViewById(Resource.Id.checkBox1);
            categoriesCheckBox.Checked = item.IsCategoryActive;

            return view;
        }
    }
    
}