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
            Application.Current.Resources["P1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#070d0d"));
            Application.Current.Resources["P2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
            Application.Current.Resources["S1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0055bf"));
            Application.Current.Resources["S2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87cefa"));
            Application.Current.Resources["T"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eefafb"));
        }

        /// <summary>
        /// Applies the ligth theme to the application.
        /// </summary>
        public void SetLightTheme()
        {
            Application.Current.Resources["P1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eefafb"));
            Application.Current.Resources["P2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87cefa"));
            Application.Current.Resources["S1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0055bf"));
            Application.Current.Resources["S2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
            Application.Current.Resources["T"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#070d0d"));
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
