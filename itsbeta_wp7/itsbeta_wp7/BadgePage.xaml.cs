using Microsoft.Phone.Controls;
using System.Windows.Data;
using System;
using System.Windows.Media;

namespace itsbeta_wp7
{
    public class BonusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //discount" | "bonus" | "present
            switch (value.ToString())
            {
                case "bonus":
                    return new SolidColorBrush(new Color() { A = 255, R = 122, G = 190, B = 91 });
                case "present":
                    return new SolidColorBrush(new Color() { A = 255, R = 160, G = 103, B = 176 });
                case "discount":
                    return new SolidColorBrush(new Color() { A = 255, R = 147, G = 213, B = 239 });
                default:
                    return new SolidColorBrush(new Color() { A = 255, R = 122, G = 190, B = 91 });
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Description for BadgePage.
    /// </summary>
    public partial class BadgePage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the BadgePage class.
        /// </summary>
        public BadgePage()
        {
            InitializeComponent();
        }
    }
}