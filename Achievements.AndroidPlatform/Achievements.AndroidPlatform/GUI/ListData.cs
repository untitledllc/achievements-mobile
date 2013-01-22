using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;

namespace Achievements.AndroidPlatform.GUI
{

    public class AchievementsListData
    {
        public string AchieveNameText;
        public string AchieveDescriptionText;
        public string AchieveReceiveDateText;
        public string AchievePicUrl;

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
