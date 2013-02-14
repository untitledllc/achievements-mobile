using Microsoft.Phone.Controls;
using Telerik.Windows.Data;
using itsbeta_wp7.ViewModel;
using System;

namespace itsbeta_wp7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.AllBadges.GroupDescriptors.Add(GroupedBadgesList);
        }

        public GenericGroupDescriptor<BadgeItem, string> GroupedBadgesList = new GenericGroupDescriptor<BadgeItem, string>(item => item.Project_api_name);

        private void AllBadges_GroupHeaderItemTap(object sender, Telerik.Windows.Controls.GroupHeaderItemTapEventArgs e)
        {

        }

        private void AllBadges_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentBadge = (BadgeItem)this.AllBadges.SelectedItem;
                NavigationService.Navigate(new Uri("/BadgePage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void QRButton_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/QRRead.xaml", UriKind.Relative));
            }
            catch { };
        }
    }
}
