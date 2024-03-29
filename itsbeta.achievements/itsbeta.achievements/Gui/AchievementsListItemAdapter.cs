﻿using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Graphics;
using System.IO;
using Android.Graphics.Drawables;

namespace itsbeta.achievements.gui
{
    public class AchievementsListItemAdapter : ArrayAdapter<AchievementsListData>
    {
        private IList<AchievementsListData> Items;
        Context _context;
        Typeface _font;

        public AchievementsListItemAdapter(Context context, int textViewResourceId, IList<AchievementsListData> items)
            : base(context, textViewResourceId, items)
        {
            Items = items;
            _context = context;
            _font = Typeface.CreateFromAsset(context.Assets, "Roboto-Light.ttf");  
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                view = inflater.Inflate(Resource.Layout.secondscreenlistrow, null);
            }

            AchievementsListData item = Items[position];

            TextView achiveNameTextView = (TextView)view.FindViewById(Resource.Id.AchiveNameTextView);
            achiveNameTextView.Text = item.AchieveNameText;
            achiveNameTextView.SetTypeface(_font, TypefaceStyle.Normal);
            try
            {
                achiveNameTextView.SetTextColor(Color.ParseColor(item.HexColor));
            }
            catch 
            {
            }

            TextView achiveDescriptionTextView = (TextView)view.FindViewById(Resource.Id.AchiveDescriptionTextView);
            achiveDescriptionTextView.Text = item.AchieveDescriptionText;
            achiveDescriptionTextView.SetTypeface(_font, TypefaceStyle.Normal);

            TextView achiveReceivedDate = (TextView)view.FindViewById(Resource.Id.AchiveReceiveDateTextView);
            achiveReceivedDate.Text = LocalDateTime(item.AchieveReceivedTime).Date.ToString().Remove(10);
            achiveReceivedDate.SetTypeface(_font, TypefaceStyle.Normal);

            ImageView achivePicture = (ImageView)view.FindViewById(Resource.Id.AchiveImageView);


            LinearLayout bonusesColumnLL = (LinearLayout)view.FindViewById(Resource.Id.bonusesColumn_linearLayout);
            bonusesColumnLL.RemoveAllViews();

            foreach (var bonus in item.Bonuses)
            {
                LayoutInflater layoutInflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                View bonusView = layoutInflater.Inflate(Resource.Layout.secondscreenlistrowll, null);

                ImageView bonusPictureleft = (ImageView)bonusView.FindViewById(Resource.Id.BonusImageView_left);
                ImageView bonusPictureright = (ImageView)bonusView.FindViewById(Resource.Id.BonusImageView_right);
                ImageView bonusPicturecenter = (ImageView)bonusView.FindViewById(Resource.Id.BonusCenterImageView);

                ImageView discountPictureleft = (ImageView)bonusView.FindViewById(Resource.Id.Bonus_DiscountImageView_left);
                ImageView discountPictureright = (ImageView)bonusView.FindViewById(Resource.Id.Bonus_DiscountImageView_right);
                ImageView discountPicturecenter = (ImageView)bonusView.FindViewById(Resource.Id.Bonus_DiscountCenterImageView);

                ImageView giftPictureleft = (ImageView)bonusView.FindViewById(Resource.Id.Bonus_GiftImageView_left);
                ImageView giftPictureRight = (ImageView)bonusView.FindViewById(Resource.Id.Bonus_GiftImageView_right);
                ImageView giftPicturecenter = (ImageView)bonusView.FindViewById(Resource.Id.Bonus_GiftCenterImageView);

                bonusPictureleft.Visibility = ViewStates.Invisible;
                bonusPictureright.Visibility = ViewStates.Invisible;
                bonusPicturecenter.Visibility = ViewStates.Invisible;

                discountPictureleft.Visibility = ViewStates.Invisible;
                discountPicturecenter.Visibility = ViewStates.Invisible;
                discountPictureright.Visibility = ViewStates.Invisible;

                giftPictureleft.Visibility = ViewStates.Invisible;
                giftPictureRight.Visibility = ViewStates.Invisible;
                giftPicturecenter.Visibility = ViewStates.Invisible;

                if (bonus.Type == "discount")
                {
                    bonusPictureleft.Visibility = ViewStates.Invisible;
                    bonusPictureright.Visibility = ViewStates.Invisible;
                    bonusPicturecenter.Visibility = ViewStates.Invisible;

                    discountPictureleft.Visibility = ViewStates.Visible;
                    discountPicturecenter.Visibility = ViewStates.Visible;
                    discountPictureright.Visibility = ViewStates.Visible;

                    giftPictureleft.Visibility = ViewStates.Invisible;
                    giftPictureRight.Visibility = ViewStates.Invisible;
                    giftPicturecenter.Visibility = ViewStates.Invisible;

                    bonusesColumnLL.AddView(bonusView);
                }
                if (bonus.Type == "bonus")
                {
                    bonusPictureleft.Visibility = ViewStates.Visible;
                    bonusPictureright.Visibility = ViewStates.Visible;
                    bonusPicturecenter.Visibility = ViewStates.Visible;

                    discountPictureleft.Visibility = ViewStates.Invisible;
                    discountPicturecenter.Visibility = ViewStates.Invisible;
                    discountPictureright.Visibility = ViewStates.Invisible;

                    giftPictureleft.Visibility = ViewStates.Invisible;
                    giftPictureRight.Visibility = ViewStates.Invisible;
                    giftPicturecenter.Visibility = ViewStates.Invisible;

                    bonusesColumnLL.AddView(bonusView);
                }
                if (bonus.Type == "present")
                {
                    bonusPictureleft.Visibility = ViewStates.Invisible;
                    bonusPictureright.Visibility = ViewStates.Invisible;
                    bonusPicturecenter.Visibility = ViewStates.Invisible;

                    discountPictureleft.Visibility = ViewStates.Invisible;
                    discountPicturecenter.Visibility = ViewStates.Invisible;
                    discountPictureright.Visibility = ViewStates.Invisible;

                    giftPictureleft.Visibility = ViewStates.Visible;
                    giftPictureRight.Visibility = ViewStates.Visible;
                    giftPicturecenter.Visibility = ViewStates.Visible;

                    bonusesColumnLL.AddView(bonusView);
                }

                bonusPictureleft.Dispose();
                bonusPictureright.Dispose();
                bonusPicturecenter.Dispose();

                discountPictureleft.Dispose();
                discountPictureright.Dispose();
                discountPicturecenter.Dispose();

                giftPictureleft.Dispose();
                giftPictureRight.Dispose();
                giftPicturecenter.Dispose();
            }


            view.DrawingCacheEnabled = true;

            using (var d = Drawable.CreateFromPath(@"/data/data/ru.hintsolutions.itsbeta/cache/pictures/" + "achive" +
                item.AchieveApiName +
                ".PNG"))
            {
                achivePicture.SetImageDrawable(d);
            }

            return view;
        }
        

        DateTime LocalDateTime(string strDateTime)
        {
            DateTime univDateTime;
            DateTime localDateTime;

            univDateTime = DateTime.Parse(strDateTime);

            localDateTime = univDateTime.ToLocalTime();

            return localDateTime;
        }
    }
}
