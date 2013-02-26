﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace itsbeta_wp7.ViewModel
{

    public class AchievesItem: BadgeAllItem {
        private DateTime _create_time = DateTime.Now;
        public DateTime Create_time
        {
            get
            {
                return _create_time;
            }
            set
            {
                _create_time = value;
                RaisePropertyChanged("Create_time");
                RaisePropertyChanged("Short_Create_time");
            }
        }

        public string Short_Create_time
        {
            get
            {
                return _create_time.ToShortDateString();
            }
            private set
            {
            }
        }  
    }

    public class BonusItem : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the BadgeItem class.
        /// </summary>
        public BonusItem()
        {
        }

        /// <summary>
        /// ("discount" | "bonus" | "present")
        /// </summary>
        private string _bonus_type = "";
        public string Bonus_type
        {
            get
            {
                return _bonus_type;
            }
            set
            {
                _bonus_type = value;
                RaisePropertyChanged("Bonus_type");
                RaisePropertyChanged("Bonus_name");
            }
        }

        public string Bonus_name
        {
            get
            {
                string out_name = "";
                switch (Bonus_type)
                {
                    case "discount": out_name = "Скидка";  break;
                    case "bonus": out_name = "Бонус"; break;
                    case "present": out_name = "Подарок"; break;
                }
                return out_name;
            }
            private set
            {
            }
        }

        private string _bonus_desc = "";
        public string Bonus_desc
        {
            get
            {
                return _bonus_desc;
            }
            set
            {
                _bonus_desc = value;
                RaisePropertyChanged("Bonus_desc");
            }
        }

    }

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class BadgeAllItem : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the BadgeItem class.
        /// </summary>
        public BadgeAllItem()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real": Connect to service, etc...
            ////}
        }

        private string _api_name = "";
        public string Api_name
        {
            get
            {
                return _api_name;
            }
            set
            {
                _api_name = value;
                RaisePropertyChanged("Api_name");
            }
        }

        private ObservableCollection<BonusItem> _bonuses = new ObservableCollection<BonusItem>();
        public ObservableCollection<BonusItem> Bonuses
        {
            get
            {
                return _bonuses;
            }
            set
            {
                _bonuses = value;
                RaisePropertyChanged("Bonuses");
            }
        }

        private string _category_api_name = "";
        public string Category_api_name
        {
            get
            {
                return _category_api_name;
            }
            set
            {
                _category_api_name = value;
                RaisePropertyChanged("Category_api_name");
            }
        }

        private string _category_name = "";
        public string Category_name
        {
            get
            {
                return _category_name;
            }
            set
            {
                _category_name = value;
                RaisePropertyChanged("Category_name");
            }
        }

        private string _display_name = "";
        public string Display_name
        {
            get
            {
                return _display_name;
            }
            set
            {
                _display_name = value;
                RaisePropertyChanged("Display_name");
            }
        }

        private string _desc = "";
        public string Desc
        {
            get
            {
                return _desc;
            }
            set
            {
                _desc = value;
                RaisePropertyChanged("Desc");
            }
        }

        private string _pic = "";
        public string Pic
        {
            get
            {
                return _pic;
            }
            set
            {
                _pic = value;
                RaisePropertyChanged("Pic");
            }
        }

        private string _bonus = "";
        public string Bonus
        {
            get
            {
                return _bonus;
            }
            set
            {
                _bonus = value;
                RaisePropertyChanged("Desc");
            }
        }

        private string _project_api_name = "";
        public string Project_api_name
        {
            get
            {
                return _project_api_name;
            }
            set
            {
                _project_api_name = value;
                RaisePropertyChanged("Project_api_name");
            }
        }

        private string _project_name = "";
        public string Project_name
        {
            get
            {
                return _project_name;
            }
            set
            {
                _project_name = value;
                RaisePropertyChanged("Project_name");
            }
        }

        private string _fb_id = "";
        public string Fb_id
        {
            get
            {
                return _fb_id;
            }
            set
            {
                _fb_id = value;
                RaisePropertyChanged("Fb_id");
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}