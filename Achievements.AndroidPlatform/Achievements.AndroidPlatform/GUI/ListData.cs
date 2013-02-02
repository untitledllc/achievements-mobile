using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;

namespace Achievements.AndroidPlatform.GUI
{

    public class AchievementsListData
    {
        public string AchieveApiName;
        public string AchieveNameText;
        public string AchieveDescriptionText;
        public string AchievePicUrl;
        public string AchieveReceivedTime;

        public Button PostFacebookButton;
        public Button PostTwitterButton;
        public Button PostVKButton;
    }

    public class CategoriesListData
    {
        public string CategoryNameText;
        public bool IsCategoryActive;
    }

    public class SubCategoriesListData
    {
        public string SubCategoryNameText;
        public bool IsSubCategoryActive;
    }

}
