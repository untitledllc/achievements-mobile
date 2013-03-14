using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Graphics;

namespace itsbeta.achievements.gui
{

    public class AchievementsListData
    {
        public string AchieveApiName;
        public string AchieveNameText;
        public string AchieveDescriptionText;
        public string AchievePicUrl;
        public string AchieveReceivedTime;
        public DateTime AchieveReceivedDateTime
        {
            get
            {
                return LocalDateTime(AchieveReceivedTime);
            }
            private set { }
        }
        public string HexColor;
        public Color AchieveProjectColor
        {
            get
            {
                return Color.ParseColor(HexColor);
            }
            private set { }
        }


        private DateTime LocalDateTime(string strDateTime)
        {
            DateTime univDateTime;
            DateTime localDateTime;

            univDateTime = DateTime.Parse(strDateTime);

            localDateTime = univDateTime.ToLocalTime();

            return localDateTime;
        }
        public string BonusStatus;
        public ItsBeta.Core.Achieves.ParentCategory.ParentProject.Achieve.Bonus[] Bonuses { get; set; } 

        public Button PostFacebookButton;
        public Button PostTwitterButton;
        public Button PostVKButton;
    }

    public class CategoriesListData
    {
        public string CategoryNameText;
    }

    public class SubCategoriesListData
    {
        public string SubCategoryNameText;
    }

}
