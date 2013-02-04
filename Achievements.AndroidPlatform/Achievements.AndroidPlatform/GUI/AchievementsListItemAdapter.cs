using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Graphics;
using System.IO;

namespace Achievements.AndroidPlatform.GUI
{
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
            achiveDescriptionTextView.Text = item.AchieveDescriptionText;


            TextView achiveReceivedDate = (TextView)view.FindViewById(Resource.Id.AchiveReceiveDateTextView);
            achiveReceivedDate.Text = item.AchieveReceivedTime;

            ImageView achivePicture = (ImageView)view.FindViewById(Resource.Id.AchiveImageView);

            achivePicture.DrawingCacheEnabled = true;

            achivePicture.SetImageBitmap(BitmapFactory.DecodeFile(@"/data/data/Achievements.AndroidPlatform/cache/achPics/" + "achive" +
                item.AchieveApiName +
                ".PNG"
                ));


            Animation listviewAnimation = new ScaleAnimation((float)1.0, (float)1.0, (float)0, (float)1.0);//new TranslateAnimation(0, 0, MainActivity._display.Height, 0);
            Animation animation = new TranslateAnimation(MainActivity._display.Width, 0, 200, 0);
            Animation animrotate = new RotateAnimation(45f, 0f);


            listviewAnimation.Duration = 750;
            animation.Duration = 750;
            animrotate.Duration = 750;

            AnimationSet asa = new AnimationSet(true);
            asa.AddAnimation(listviewAnimation);
            asa.AddAnimation(animrotate);
            asa.AddAnimation(animation);

            view.StartAnimation(asa);
            //view.StartAnimation(animation);
            //view.StartAnimation(animrotate);

            Button twitterButton = (Button)view.FindViewById(Resource.Id.twitter_button);
            Button vkButton = (Button)view.FindViewById(Resource.Id.vk_button);
            Button facebookButton = (Button)view.FindViewById(Resource.Id.facebook_button);
            twitterButton.SetHighlightColor(Android.Graphics.Color.DarkGray);

            twitterButton.Click += delegate
            {
                //twitterButton.SetHighlightColor(Android.Graphics.Color.DarkGray);
            };

            vkButton.Click += delegate
            {
                //_buttonClickAnimation.Start();
            };

            facebookButton.Click += delegate
            {
                //_buttonClickAnimation.Start();
            };

            view.Click += delegate { MainActivity.AchieveListSelectedEventTextView.Text = item.AchieveApiName; };

            return view;
        }

    }

}
