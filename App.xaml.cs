using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace PalettePicker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region ThemeSetMethods

        /// <summary>
        /// Applies the dark theme to the application.
        /// </summary>
        public void SetDarkTheme()
        {
            Current.Resources["P1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#070d0d"));
            Current.Resources["P2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
            Current.Resources["S1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0055bf"));
            Current.Resources["S2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87cefa"));
            Current.Resources["T"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eefafb"));
        }

        /// <summary>
        /// Applies the ligth theme to the application.
        /// </summary>
        public void SetLightTheme()
        {
            Current.Resources["P1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eefafb"));
            Current.Resources["P2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87cefa"));
            Current.Resources["S1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0055bf"));
            Current.Resources["S2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
            Current.Resources["T"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#070d0d"));
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
