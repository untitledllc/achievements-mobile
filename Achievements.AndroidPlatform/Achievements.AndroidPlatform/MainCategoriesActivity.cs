using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android.Support.V4.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Achievements.AndroidPlatform.WebControls;

using Achievements.AndroidPlatform.JsonProcessor;

namespace Achievements.AndroidPlatform
{
    [Activity(Label = "Achievements.Android", MainLauncher = true, Icon = "@drawable/icon", 
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainCategoriesActivity : FragmentActivity
    {
        static int NUM_ITEMS;
        MyAdapter adapter;
        ViewPager pager;

        static Categories _categories;

        static Intent[] _intentArray;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.MainLayout);

            string access_token = "059db4f010c5f40bf4a73a28222dd3e3";

            _categories = new Categories(access_token);

            var count = _categories.Count;
            NUM_ITEMS = count;


            adapter = new MyAdapter(SupportFragmentManager);

            pager = FindViewById<ViewPager>(Resource.Id.pager);
            pager.Adapter = adapter;

            _intentArray = new Intent[NUM_ITEMS];
            for (int i = 0; i < NUM_ITEMS; i++)
            {
                _intentArray[i] = new Intent(this, typeof(ProjectsActivity));
            }
        }

        protected class ArrayListFragment : Fragment
        {
            int num;

            public ArrayListFragment()
            {

            }

            public ArrayListFragment(int num)
            {
                var args = new Bundle();
                args.PutInt("num", num);
                Arguments = args;
            }

            public override void OnCreate(Bundle p0)
            {
                base.OnCreate(p0);

                num = Arguments != null ? Arguments.GetInt("num") : 1;
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                var v = inflater.Inflate(Resource.Layout.CategoryLayout, container, false);

                TextView categoryName = v.FindViewById<TextView>(Resource.Id.CategoryName);
                ImageView miniCategoryImage = v.FindViewById<ImageView>(Resource.Id.miniCategoryImage);
                ImageView mainCategoryImage = v.FindViewById<ImageView>(Resource.Id.mainCategoryImage);

                for (int i = 0; i < NUM_ITEMS; i++)
                {
                    if (num == i)
                    {
                        categoryName.Text = _categories.CategoriesArray()[i].Title;
                        //mainCategoryImage.SetImageResource(Resource.Drawable.zdravoohranenie);// SetImageBitmap(BitmapFactory.DecodeStream(Assets.Open("category_medicine.png")));
                        mainCategoryImage.Click += delegate
                        {
                            Console.WriteLine("Click! " + num);
                            _intentArray[i].PutExtra("category_name", categoryName.Text);
                            _intentArray[i].PutExtra("category_id", _categories.CategoriesArray()[i].Id);
                            StartActivity(_intentArray[i]);
                        };
                        return v;
                    }
                }
                
                return v;
            }

            public override void OnActivityCreated(Bundle p0)
            {
                base.OnActivityCreated(p0);
            }
        }

        protected class MyAdapter : FragmentPagerAdapter
        {
            public MyAdapter(FragmentManager fm)
                : base(fm)
            {
            }

            public override int Count
            {
                get
                {
                    return NUM_ITEMS;
                }
            }

            public override Fragment GetItem(int position)
            {
                return new ArrayListFragment(position);
            }

        }
    }
}

