using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace itsbeta_wp7.ViewModel
{

    public class AchievesItem: BadgeAllItem {
        private DateTime _create_time = DateTime.Now;

        /// <summary>
        /// Дата и время создание
        /// </summary>
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

        /// <summary>
        /// Дата создания в котротком формате
        /// </summary>
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

        private string _badge_name = "";
        /// <summary>
        /// Имя бейджа
        /// </summary>
        public string Badge_name
        {
            get
            {
                return _badge_name;
            }
            set
            {
                _badge_name = value;
                RaisePropertyChanged("Badge_name");
            }
        } 

        
    }

    /// <summary>
    /// Бонусы для достижений
    /// </summary>
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

        /// <summary>
        /// Тип бонусв
        /// Один из вариантов - ("discount" | "bonus" | "present")
        /// </summary>
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
                RaisePropertyChanged("Bonus_type");
            }
        }

        /// <summary>
        /// Цвет бонуса
        /// </summary>
        public SolidColorBrush Bonus_color {
            get
            {
                switch (Bonus_type.ToString())
                {
                    /*
                     * скидка: c8e5f0
                        [02.03.2013 18:04:06] Antonina(art): акция: bde0a9
                        [02.03.2013 18:04:22] Antonina(art): подарок: cbafdc
                     * */
                    case "bonus":
                        return new SolidColorBrush(new Color() { A = 255, R = 189, G = 224, B = 169 });
                    case "present":
                        return new SolidColorBrush(new Color() { A = 255, R = 203, G = 175, B = 220 });
                    case "discount":
                        return new SolidColorBrush(new Color() { A = 255, R = 200, G = 229, B = 240 });
                    default:
                        return new SolidColorBrush(new Color() { A = 255, R = 189, G = 224, B = 169 });
                };
            }
            private set {
            }
        }

        /// <summary>
        /// Название бонуса (от типа бонуса)
        /// </summary>
        public string Bonus_name
        {
            get
            {
                string out_name = "";
                switch (Bonus_type)
                {
                    case "discount": out_name = AppResources.discount_title;  break;
                    case "bonus": out_name = AppResources.bonus_title; break;
                    case "present": out_name = AppResources.present_title; break;
                }
                return out_name;
            }
            private set
            {
            }
        }

        private string _bonus_desc = "";
        /// <summary>
        /// Описание бонуса
        /// </summary>
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
        /// <summary>
        /// Api_name бейджа
        /// </summary>
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
        /// <summary>
        /// Список бонусов
        /// </summary>
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
        /// <summary>
        /// Api_name категории которой принадлежит достижение
        /// </summary>
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
        /// <summary>
        /// Display_name категории, которой принадлежит достижение
        /// </summary>
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
        /// <summary>
        /// Отображаемоеназвание достижения
        /// </summary>
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
        /// <summary>
        /// Описание достижения - краткое
        /// </summary>
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

        private string _details = "";
        /// <summary>
        /// Полное описание достижения
        /// </summary>
        public string Details
        {
            get
            {
                return _details;
            }
            set
            {
                _details = value;
                RaisePropertyChanged("Details");
            }
        }
        
        private string _adv = "";
        /// <summary>
        /// Реклама в достижении данном
        /// </summary>
        public string Adv
        {
            get
            {
                return _adv;
            }
            set
            {
                _adv = value;
                RaisePropertyChanged("Adv");
            }
        }

        private string _pic = "";
        /// <summary>
        /// Изображение достижения (ссылка)
        /// </summary>
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
        /// <summary>
        /// не используется более
        /// </summary>
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
        /// <summary>
        /// Api_name проекта,которому принадлежит достижение
        /// </summary>
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
        /// <summary>
        /// Display_name проекта, которому принадлежит достижение
        /// </summary>
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
        /// <summary>
        /// Идентификатор опубликованного на facebook достижения
        /// </summary>
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