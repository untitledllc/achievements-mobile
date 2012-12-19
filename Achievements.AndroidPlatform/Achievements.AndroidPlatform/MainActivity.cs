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
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MainActivityLayout);

            List<AchievementsListData> achievementsList = new List<AchievementsListData>();

            for (int i = 0; i < 10; i++)
			{
                achievementsList.Add(new AchievementsListData(){AchieveNameText = String.Format("Achieve [{0}]",i)});
			}

            ListView achievementsListView = FindViewById<ListView>(Resource.Id.AchivementsListView);

            var adapter = new AchievementsListItemAdapter(this, Resource.Layout.MainLayoutListRow, achievementsList);
            achievementsListView.Adapter = adapter;



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
    
}