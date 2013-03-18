using Microsoft.Phone.Controls;
using Telerik.Windows.Data;
using itsbeta_wp7.ViewModel;
using System;
using Microsoft.Phone.Net.NetworkInformation;
using System.Windows;

namespace itsbeta_wp7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.AllBadges.GroupDescriptors.Add(GroupedBadgesList);
            //Sort.SortMode 
            this.AllBadges.SortDescriptors.Add(Sort);
        }

        public GenericGroupDescriptor<AchievesItem, string> GroupedBadgesList = new GenericGroupDescriptor<AchievesItem, string>(item => item.Project_name);
        public GenericSortDescriptor<AchievesItem, TimeSpan> Sort = new GenericSortDescriptor<AchievesItem, TimeSpan>(item => DateTime.Now - item.Create_time);

        private void AllBadges_GroupHeaderItemTap(object sender, Telerik.Windows.Controls.GroupHeaderItemTapEventArgs e)
        {

        }

        private void AllBadges_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentAchieve = (AchievesItem)this.AllBadges.SelectedItem;
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

        private void ActivateBadge_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/ActivatePage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void AllBadges_RefreshRequested(object sender, EventArgs e)
        {
            ViewModelLocator.MainStatic.LoadAchievements();
            AllBadges.StopPullToRefreshLoading(true);
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
