using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Android.Support.V4.Widget;
using Android.Graphics;
using Android.Content.PM;

namespace Achievements.Android
{
    [Activity(Label = "Achievements.Android", MainLauncher = true, Icon = "@drawable/icon", 
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen"/*,
                ScreenOrientation = ScreenOrientation.Landscape*/)]
    public class MainCategoriesActivity : Activity
    {
         protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var display = WindowManager.DefaultDisplay;
            var horiPager = new HorizontalPager(this.ApplicationContext, display);

            var backgroundColors = new Color[] { Color.Red, Color.Blue, Color.Cyan, Color.Green, Color.Yellow };

            ImageView tittleImage = new ImageView(this.ApplicationContext);
            tittleImage.SetImageBitmap(BitmapFactory.DecodeStream(Assets.Open("category_medicine.png")));

            ImageView tittleImage2 = new ImageView(this.ApplicationContext);
            tittleImage2.SetImageBitmap(BitmapFactory.DecodeStream(Assets.Open("category_medicine.png")));


            AddCategory(horiPager, tittleImage);
            AddCategory(horiPager, tittleImage2);

            tittleImage.Click += delegate { StartActivity(typeof(UnderCategoryActivity)); };


            SetContentView(horiPager);
        }

         private void AddCategory(HorizontalPager h_pager, ImageView image)
         {
             h_pager.AddView(image);
         }

    }
}

