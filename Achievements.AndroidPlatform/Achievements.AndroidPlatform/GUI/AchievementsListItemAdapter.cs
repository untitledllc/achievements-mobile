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

            ImageView achivePicture = (ImageView)view.FindViewById(Resource.Id.AchiveImageView);
            //achivePicture.SetImageURI(null);
            achivePicture.DrawingCacheEnabled = true;
            achivePicture.SetImageBitmap(GetImageBitmap(item.AchievePicUrl));

            //achivePicture.SetImageDrawable(Android.Graphics.Drawables.Drawable.CreateFromPath("http://cs419128.userapi.com/v419128252/2c14/p1yGlr_wlpM.jpg"));
                //SetImageURI(Android.Net.Uri.Parse(item.AchievePicUrl));

            Animation listviewAnimation = new ScaleAnimation((float)1.0, (float)1.0, (float)0, (float)1.0);//new TranslateAnimation(0, 0, MainActivity._display.Height, 0);
            Animation animation = new TranslateAnimation(MainActivity._display.Width / 2, 0, 0, 0);
            //Animation tranimation = new AlphaAnimation(0f, 0.9f);

            listviewAnimation.Duration = 750;
            animation.Duration = 750;
            view.StartAnimation(listviewAnimation);
            view.StartAnimation(animation);
            //view.StartAnimation(tranimation);

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


            return view;
        }

        private Bitmap GetImageBitmap(String url)
        {
            Bitmap bm = null;

            Java.Net.URL aURL = new Java.Net.URL(url);
            Java.Net.URLConnection conn = aURL.OpenConnection();
            conn.Connect();

            Stream stream = conn.InputStream;
            BufferedStream bsteam = new BufferedStream(stream);

            bm = BitmapFactory.DecodeStream(bsteam);
            bsteam.Close();
            stream.Close();

            return bm;
        } 
    }

}
