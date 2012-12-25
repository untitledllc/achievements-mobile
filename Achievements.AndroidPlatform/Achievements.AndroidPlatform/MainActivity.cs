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
using Android.Views.Animations;

namespace Achievements.AndroidPlatform
{
    [Activity(Label = "Achievements", MainLauncher = true,
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        public static Display _display;
        bool _isBarCategoriesListOpen = false;
        ListView _categoriesListView;
        ListView _subCategoriesListView;
        public static Dictionary<string,bool> _selectedCategoriesDictionary = new Dictionary<string,bool>();
        public static Dictionary<string, bool> _selectedSubCategoriesDictionary = new Dictionary<string, bool>();
        ImageButton badgesBarBackgroundImageButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _display = WindowManager.DefaultDisplay;

            SetContentView(Resource.Layout.MainActivityLayout);

            Animation animation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.SlideInLeft);
            Animation buttonAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            

            LinearLayout categoriesLinearLayout = FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout);

            badgesBarBackgroundImageButton = FindViewById<ImageButton>(Resource.Id.BadgesBarBackgroundImageButton);
            //---------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------
            
            #region AchievementsList Local
            List<AchievementsListData> achievementsList = new List<AchievementsListData>();

            for (int i = 0; i < 40; i++)
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
                _selectedCategoriesDictionary.Add(categoriesList[i].CategoryNameText, categoriesList[i].IsCategoryActive);
            }


            _categoriesListView = new ListView(this);

            var categoriesAdapter = new CategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow, categoriesList);
            
            _categoriesListView.Adapter = categoriesAdapter;

            LayoutInflater layoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            _categoriesListView.SetWillNotCacheDrawing(true);
            PopupWindow categoriesPopupWindow = new PopupWindow(_categoriesListView, 
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);

            badgesBarBackgroundImageButton.Click += delegate
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
                    categoriesLinearLayout.StartAnimation(animation);
                    _isBarCategoriesListOpen = false;
                }
            };

            #endregion

            //---------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------
            
            #region SubCategories Local

            int checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);

            foreach (var item in _selectedCategoriesDictionary)
            {
                if (item.Value == true)
                {
                    var categoryButton = new Button(this);
                    categoryButton.Text = item.Key;
                    categoriesLinearLayout.AddView(categoryButton);
                    categoryButton.SetBackgroundColor(global::Android.Graphics.Color.DarkGray);
                    categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
                    categoryButton.Gravity = GravityFlags.Left;
                    categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;
                }
            }

            badgesBarBackgroundImageButton.Click += delegate
            {
                badgesBarBackgroundImageButton.StartAnimation(buttonAnimation);
                categoriesLinearLayout.RemoveAllViewsInLayout();
                categoriesLinearLayout.RemoveAllViews();
                //исправить нумурацию на ключи

                checkedCategoriesCount = _selectedCategoriesDictionary.Count(e => e.Value == true);
                foreach (var item in _selectedCategoriesDictionary)
                {
                    if (item.Value == true)
                    {
                        var categoryButton = new Button(this);
                        categoryButton.Text = item.Key;
                        categoriesLinearLayout.AddView(categoryButton);
                        categoryButton.Click += delegate { categoryButton.StartAnimation(buttonAnimation); };
                        categoryButton.SetBackgroundColor(global::Android.Graphics.Color.DarkGray);
                        categoryButton.SetTextColor(global::Android.Graphics.Color.Gray);
                        categoryButton.Gravity = GravityFlags.Left;
                        categoryButton.LayoutParameters.Width = _display.Width / checkedCategoriesCount;
                    }
                }
            };


            List<SubCategoriesListData> subCategoriesList = new List<SubCategoriesListData>();

            for (int i = 0; i < 5; i++)
            {
                subCategoriesList.Add(new SubCategoriesListData() { SubCategoryNameText = String.Format("SubCategory [{0}]", i), IsSubCategoryActive = true });
                _selectedSubCategoriesDictionary.Add(subCategoriesList[i].SubCategoryNameText, subCategoriesList[i].IsSubCategoryActive);
            }


            _subCategoriesListView = new ListView(this);

            var subCategoriesAdapter = new SubCategoriesListItemAdapter(this, Resource.Layout.MainLayoutCategoryDropDownListRow, subCategoriesList);

            _categoriesListView.Adapter = categoriesAdapter;

            LayoutInflater sublayoutInflater = (LayoutInflater)BaseContext.GetSystemService(LayoutInflaterService);
            _categoriesListView.SetWillNotCacheDrawing(true);
            PopupWindow subCategoriesPopupWindow = new PopupWindow(_subCategoriesListView,
                LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.WrapContent);

            

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

    public class SubCategoriesListData
    {
        public string SubCategoryNameText;
        public bool IsSubCategoryActive;
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

                Animation listviewAnimation = new ScaleAnimation((float)1.0, (float)1.0, (float)0, (float)1.0);//new TranslateAnimation(0, 0, MainActivity._display.Height, 0);
                Animation animation = new TranslateAnimation(MainActivity._display.Width / 2, 0, 0, 0);

                listviewAnimation.Duration = 750;
                animation.Duration = 750;
                view.StartAnimation(listviewAnimation);
                view.StartAnimation(animation);

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

            CheckBox categoriesCheckBox = (CheckBox)view.FindViewById(Resource.Id.checkBox1);
            categoriesCheckBox.SetWillNotCacheDrawing(true);
            categoriesCheckBox.Text = item.CategoryNameText;
            //categoriesCheckBox.Checked = item.IsCategoryActive;
            //categoriesCheckBox.Clickable = false;
            //if (position == 0)
            //{
            //    cyclecount++;
            //    if (cyclecount == 1)
            //    {
            //       categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + 0 + "]"];
            //    }
            //    if (cyclecount == 2)
            //    {
            //        cyclecount = 0;
            //    }
            //}
            //if (position != 0)
            //{
            //    if (cyclecount == 0)
            //    {
            //       categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
            //    }
            //}
            categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
            

            categoriesCheckBox.Click += delegate
            {
                MainActivity._selectedCategoriesDictionary["Category [" + position + "]"] = !MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
                categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
                   
                //if (position == 0)
                //{
                //    cyclecount++;
                //    if(cyclecount == 1)
                //    {
                //        MainActivity._selectedCategoriesDictionary["Category [" + 0 + "]"] = !MainActivity._selectedCategoriesDictionary["Category [" + 0 + "]"];
                //        categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + 0 + "]"];
                //    }
                //    if (cyclecount == 2)
                //    {
                //        cyclecount = 0;
                //    }
                //}
                //if (position != 0)
                //{
                //    if (cyclecount == 0)
                //    {
                //        MainActivity._selectedCategoriesDictionary["Category [" + position + "]"] = !MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
                //        categoriesCheckBox.Checked = MainActivity._selectedCategoriesDictionary["Category [" + position + "]"];
                //    }
                //} 
            };

            return view;
        }

        
    }

    public class SubCategoriesListItemAdapter : ArrayAdapter<SubCategoriesListData>
    {
        private IList<SubCategoriesListData> Items;


        public SubCategoriesListItemAdapter(Context context, int textViewResourceId, IList<SubCategoriesListData> items)
            : base(context, textViewResourceId, items)
        {
            Items = items;

        }

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
            SubCategoriesListData item = Items[position];

            CheckBox subCategoriesCheckBox = (CheckBox)view.FindViewById(Resource.Id.checkBox1);
            subCategoriesCheckBox.SetWillNotCacheDrawing(true);
            subCategoriesCheckBox.Text = item.SubCategoryNameText;

            subCategoriesCheckBox.Checked = MainActivity._selectedSubCategoriesDictionary["SubCategory [" + position + "]"];


            subCategoriesCheckBox.Click += delegate
            {
                MainActivity._selectedSubCategoriesDictionary["SubCategory [" + position + "]"] = !MainActivity._selectedSubCategoriesDictionary["SubCategory [" + position + "]"];
                subCategoriesCheckBox.Checked = MainActivity._selectedSubCategoriesDictionary["SubCategory [" + position + "]"];
            };

            return view;
        }


    }
    
}