using GalaSoft.MvvmLight;

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
    public class CategoryItem : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the CategoryItem class.
        /// </summary>
        public CategoryItem()
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