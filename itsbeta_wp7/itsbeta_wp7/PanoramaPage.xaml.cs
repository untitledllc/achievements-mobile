using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using itsbeta_wp7.ViewModel;
using Telerik.Windows.Data;
using Coding4Fun.Toolkit.Controls;

namespace itsbeta_wp7
{
    public partial class PanoramaPage : PhoneApplicationPage
    {
        public PanoramaPage()
        {
            InitializeComponent();
            //this.AllCategories.GroupDescriptors.Add(GroupedProjectsList);
        }

        public GenericGroupDescriptor<ProjectItem, string> GroupedProjectsList = new GenericGroupDescriptor<ProjectItem, string>(item => item.Category_api_name);

        private void AllCategories_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentCategory = (CategoryItem)this.AllCategories.SelectedItem;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void AllCategories_GroupHeaderItemTap(object sender, Telerik.Windows.Controls.GroupHeaderItemTapEventArgs e)
        {

        }

        private void LastAchieves_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentAchieve = (AchievesItem)this.LastAchieves.SelectedItem;
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

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ViewModelLocator.MainStatic.LoadAchievements();
        }

        private void QrReaderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/QRRead.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void ActivateCOdeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/ActivatePage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void RadImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messagePrompt = new MessagePrompt
                {
                    Title = "Выход",
                    Message = "Вы хотите выйти?",
                    IsCancelVisible = true
                };
                messagePrompt.ActionPopUpButtons.FirstOrDefault().Click += new RoutedEventHandler(customButton_Click);
                messagePrompt.Show();                
            }
            catch { };
        }

        void customButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModelLocator.UserStatic.LogOut = false;
                NavigationService.Navigate(new Uri("/FacebookPages/FacebookLoginPage.xaml", UriKind.Relative));
                //NavigationService.GoBack();
            }
            catch { };
        }

    }
}