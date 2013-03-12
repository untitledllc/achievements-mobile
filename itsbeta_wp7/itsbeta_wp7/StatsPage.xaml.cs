using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using itsbeta_wp7.ViewModel;
using Telerik.Charting;
using System.Windows.Shapes;
using System.Windows.Media;



namespace itsbeta_wp7
{
    public partial class StatsPage : PhoneApplicationPage
    {
        // Constructor
        public StatsPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void StackPanel_Loaded_1(object sender, RoutedEventArgs e)
        {
        }

        private void AchievesChartPieces_Loaded(object sender, RoutedEventArgs e)
        {            
            (sender as PieSeries).ValueBinding = new GenericDataPointBinding<ProjectItem, double>() { ValueSelector = project => project.Total_badges };

            foreach (var project in ViewModelLocator.MainStatic.Projects)
            {
                if ((project.Color != null) && (project.Color!=""))
                {
                    Style testStyle = new Style();
                    testStyle.TargetType = typeof(System.Windows.Shapes.Path);
                    Setter testSetter = new Setter();
                    testSetter.Property = System.Windows.Shapes.Path.FillProperty;                    
                    testSetter.Value = project.Color.Replace("#","#FF").ToUpper();
                    testStyle.Setters.Add(testSetter);

                    testSetter = new Setter();
                    testSetter.Property = System.Windows.Shapes.Path.StrokeProperty;
                    testSetter.Value = "Black";
                    testStyle.Setters.Add(testSetter);

                    (sender as PieSeries).SliceStyles.Add(testStyle);
                } 
                else {
                    Style testStyle = new Style();
                    testStyle.TargetType = typeof(System.Windows.Shapes.Path);
                    Setter testSetter = new Setter();
                    testSetter.Property = System.Windows.Shapes.Path.FillProperty;
                    Random randonGen = new Random();
                    byte r = (byte)randonGen.Next(0, 255), g = (byte)randonGen.Next(0, 255), b = (byte)randonGen.Next(0, 255);
                    string color = "#FF"+r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
                    testSetter.Value = color;
                    testStyle.Setters.Add(testSetter);

                    testSetter = new Setter();
                    testSetter.Property = System.Windows.Shapes.Path.StrokeProperty;
                    testSetter.Value = "Black";
                    testStyle.Setters.Add(testSetter);

                    (sender as PieSeries).SliceStyles.Add(testStyle);
                };
            };
            (sender as PieSeries).ItemsSource = ViewModelLocator.MainStatic.Projects;
        }
    }
}