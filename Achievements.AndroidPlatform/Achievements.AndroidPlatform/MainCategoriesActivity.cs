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

namespace Achievements.AndroidPlatform
{
    [Activity(Label = "Achievements.Android", MainLauncher = true, Icon = "@drawable/icon", 
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainCategoriesActivity : FragmentActivity
    {
        const int NUM_ITEMS = 10;
        MyAdapter adapter;
        ViewPager pager;

        static Intent _startActivityIntent;
        static Intent _startActivityIntent2;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.MainLayout);

            adapter = new MyAdapter(SupportFragmentManager);

            pager = FindViewById<ViewPager>(Resource.Id.pager);
            pager.Adapter = adapter;
            
            _startActivityIntent = new Intent(this, typeof(SubcategoryActivity));
            _startActivityIntent2 = new Intent(this, typeof(LoginScreenActivity));
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

                if (num == 0)
                {
                    categoryName.Text = "Facebook!";
                    mainCategoryImage.SetImageResource(Resource.Drawable.zdravoohranenie);// SetImageBitmap(BitmapFactory.DecodeStream(Assets.Open("category_medicine.png")));
                    mainCategoryImage.Click += delegate
                    {
                        Console.WriteLine("Click! " + num);
                        StartActivity(_startActivityIntent2);
                    };
                    return v;
                }

                if (num == 1)
                {
                    categoryName.Text = "Здравоохранение";
                    mainCategoryImage.SetImageResource(Resource.Drawable.zdravoohranenie);// SetImageBitmap(BitmapFactory.DecodeStream(Assets.Open("category_medicine.png")));
                    mainCategoryImage.Click += delegate 
                    {
                        Console.WriteLine("Click! " + num);
                        StartActivity(_startActivityIntent);
                    };
                    return v;
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

