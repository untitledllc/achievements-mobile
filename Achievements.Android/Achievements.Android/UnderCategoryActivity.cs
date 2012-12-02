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

namespace Achievements.Android
{
    [Activity(Label = "My Activity")]
    public class UnderCategoryActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.underCategory);


            HorizontalScrollView firstLine = FindViewById<HorizontalScrollView>(Resource.Id.horizontalScrollView1);
            firstLine.AddView(new Button(this) { Text = "1" });
            //firstLine.AddView(new Button(this) { Text = "2" });
            //firstLine.AddView(new Button(this) { Text = "3" });
            //firstLine.AddView(new Button(this) { Text = "4" });
            //firstLine.AddView(new Button(this) { Text = "5" });
            //firstLine.AddView(new Button(this) { Text = "6" });
            //firstLine.AddView(new Button(this) { Text = "7" });


        }
    }
}