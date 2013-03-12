using GalaSoft.MvvmLight;
using System;

namespace itsbeta_wp7.ViewModel
{
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
    public class ProjectItem : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ProjectItem class.
        /// </summary>
        public ProjectItem()
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

        private string _color = "";
        public string Color
        {
            get
            {
                if (_color == "")
                {
                    Random randonGen = new Random();
                    byte r = (byte)randonGen.Next(0, 255), g = (byte)randonGen.Next(0, 255), b = (byte)randonGen.Next(0, 255);
                    _color = "#FF" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
                };
                return _color;
            }
            set
            {
                _color = value;
                RaisePropertyChanged("Color");
            }
        }

        public string ChartLabel
        {
            private set
            {
            }
            get
            {
                return Display_name + "(" + Activated_badges_count + ")";
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

        private int _total_badges = 0;
        public int Total_badges
        {
            get
            {
                return _total_badges;
            }
            set
            {
                _total_badges = value;
                RaisePropertyChanged("Total_badges");
            }
        }

        private int _bonuses_count = 0;
        public int Bonuses_count
        {
            get
            {
                return _bonuses_count;
            }
            set
            {
                _bonuses_count = value;
                RaisePropertyChanged("Bonuses_count");
            }
        }

        private int _activated_badges_count = 0;
        public int Activated_badges_count
        {
            get
            {
                return _activated_badges_count;
            }
            set
            {
                _activated_badges_count = value;
                RaisePropertyChanged("Activated_badges_count");
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

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}