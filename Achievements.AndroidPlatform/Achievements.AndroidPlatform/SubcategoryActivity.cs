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
	[Activity(Label = "UnderCategoryActivity",
	          Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class SubcategoryActivity : Activity
    {
        int _achieveNumber = 10;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SubcategoryLayout);
            
			//Cheating(not forever):
            HorizontalScrollView firstLine = FindViewById<HorizontalScrollView>(Resource.Id.horizontalScrollView1);
			LinearLayout linearLayout = new LinearLayout(this);
			linearLayout.HorizontalScrollBarEnabled = true;

            //PopupWindow popUpWindow = new PopupWindow(this);
            //RelativeLayout popUpLayout = new RelativeLayout(this);//FindViewById<RelativeLayout>(Resource.Layout.AchieveLayout);

            Button testButton = new Button(this);
            testButton.Text = "Test";


            //popUpWindow.ContentView = popUpLayout;

            testButton.Click += delegate
            {
                StartActivity(typeof(AchieveActivity));
                //popUpWindow.ShowAtLocation(popUpLayout, GravityFlags.Center, 10, 10);
                //popUpWindow.Update(50, 50, 300, 80);
            };


            for (int achieve = 0; achieve < _achieveNumber; achieve++) 
			{
                if (achieve == 3)
                {
                    linearLayout.AddView(testButton);
                }
                else 
                linearLayout.AddView(new Button(this) { Text = "Achieve " + achieve });
			}
			firstLine.AddView(linearLayout);


			HorizontalScrollView secondLine = FindViewById<HorizontalScrollView>(Resource.Id.horizontalScrollView2);
			LinearLayout linearLayout2 = new LinearLayout(this);
			linearLayout2.HorizontalScrollBarEnabled = true;

            for (int achieve = 0; achieve < _achieveNumber; achieve++) 
			{
                linearLayout2.AddView(new Button(this) { Text = "Achieve " + achieve });
			}
			secondLine.AddView(linearLayout2);
        }
    }
}