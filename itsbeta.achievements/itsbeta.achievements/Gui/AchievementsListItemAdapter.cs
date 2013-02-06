using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Graphics;
using System.IO;

namespace itsbeta.achievements.gui
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
                view = inflater.Inflate(Resource.Layout.SecondScreenListRow, null);
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

            ImageView bonusPicture = (ImageView)view.FindViewById(Resource.Id.BonusImageView);
            ImageView discountPicture = (ImageView)view.FindViewById(Resource.Id.Bonus_DiscountImageView);

            if (item.BonusStatus == "")
            {
                bonusPicture.Visibility = ViewStates.Invisible;
                discountPicture.Visibility = ViewStates.Invisible;
            }

            if (item.BonusStatus == "discount")
            {
                bonusPicture.Visibility = ViewStates.Invisible;
                discountPicture.Visibility = ViewStates.Visible;
            }

            if (item.BonusStatus == "bonus")
            {
                bonusPicture.Visibility = ViewStates.Visible;
                discountPicture.Visibility = ViewStates.Invisible;
            }

            achivePicture.DrawingCacheEnabled = true;

            //achivePicture.SetImageBitmap(BitmapFactory.DecodeFile(@"/data/data/Achievements.AndroidPlatform/cache/achPics/" + "achive" +
            //    item.AchieveApiName +
            //    ".PNG"
            //    ));


            Animation listviewAnimation = new ScaleAnimation((float)1.0, (float)1.0, (float)0, (float)1.0);//new TranslateAnimation(0, 0, MainActivity._display.Height, 0);
            Animation animation = new TranslateAnimation(AppInfo._display.Width, 0, 200, 0);
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
            
            //view.Click += delegate { MainActivity.AchieveListSelectedEventTextView.Text = item.AchieveApiName; };

            return view;
        }

    }

}
