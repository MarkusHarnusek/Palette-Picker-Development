using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace PalettePicker
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        /// <summary>
        /// The current save path for user preferences.
        /// </summary>
        private string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "PalettePicker");

        /// <summary>
        /// Gets the collection of predefined language options.
        /// </summary>
        public ObservableCollection<string> MyItems { get; } = new ObservableCollection<string> {
            "Select a language...",
            "English",
            "Deutsch",
            "Español",
            "Français",
            "中国人",
            "Português",
            "Русский"
        };

        public OptionsWindow()
        {
            InitializeComponent();
            DataContext = this;
            SetLanguage(MainWindow.currentLanguage);
            Txb_Preferences_SavePath.Text = savePath;
        }

        /// <summary>
        /// Resets the visibility of all grid elements to a collapsed state.
        /// </summary>
        /// <remarks>This method sets the visibility of the grid elements <see cref="Grd_Appearance"/>, 
        /// <see cref="Grd_Language"/>, <see cref="Grd_None"/>, and <see cref="Grd_Preferences"/>  to <see
        /// cref="Visibility.Collapsed"/>. Use this method to ensure that all grids are hidden.</remarks>

        private void ResetGrdVisibility()
        {
            Grd_Appearance.Visibility = Visibility.Collapsed;
            Grd_Language.Visibility = Visibility.Collapsed;
            Grd_None.Visibility = Visibility.Collapsed;
            Grd_Preferences.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Sets the current UI culture for the application based on the specified language ID.
        /// </summary>
        /// <remarks>This method updates the application's UI culture to match the specified language ID.
        /// The supported languages are: English (UK), German (DE), Spanish (ES), French (FR), Chinese (ZH), Portuguese
        /// (PT), and Russian (RU). After setting the culture, the UI is refreshed to reflect the change.</remarks>
        /// <param name="languageID">The ID of the language to set. Valid values range from 0 to the number of supported languages minus one. If
        /// the value is out of range, the default language (English) is used.</param>

        private void SetLanguage(int languageID)
        {
            string[] cultures = { "en-UK", "de-DE", "es-ES", "fr-FR", "zh-CN", "pt-PT", "ru-RU" };

            if (languageID < 0 || languageID >= cultures.Length)
            {
                languageID = 0;
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultures[languageID]);
            UpdateUI();
        }

        /// <summary>
        /// Updates the user interface elements with localized resource values.
        /// </summary>
        /// <remarks>This method sets the text and content of various UI components based on the current
        /// localized resources. It ensures that the UI reflects the appropriate language and settings.</remarks>

        private void UpdateUI()
        {
            Title = PalettePicker.Resources.OptionsWindowResources.OptionsWindow.WindowTitle;
            Txt_WindowTitle.Text = PalettePicker.Resources.OptionsWindowResources.OptionsWindow.WindowTitle;
            Btn_Language.Content = PalettePicker.Resources.OptionsWindowResources.OptionsWindow.LanguageGridButton;
            Txt_Language_Title.Text = PalettePicker.Resources.OptionsWindowResources.OptionsWindow.LanguageSideButton;
        }

        #region MenuSelectionControls

        /// <summary>
        /// Handles the click event for the language selection button.
        /// </summary>
        /// <remarks>This method resets the visibility of other UI elements and makes the language
        /// selection grid visible.</remarks>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the click action.</param>
        private void Btn_Language_Click(object sender, RoutedEventArgs e)
        {
            ResetGrdVisibility();
            Grd_Language.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the click event for the Appearance button.
        /// </summary>
        /// <remarks>This method resets the visibility of other UI elements and makes the Appearance grid
        /// visible.</remarks>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the click action.</param>
        private void Btn_Appearance_Click(object sender, RoutedEventArgs e)
        {
            ResetGrdVisibility();
            Grd_Appearance.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the click event for the Preferences button.
        /// </summary>
        /// <remarks>This method resets the visibility of other UI elements and makes the Preferences grid
        /// visible.</remarks>
        /// <param name="sender">The source of the event, typically the button that was clicked.</param>
        /// <param name="e">The event data associated with the click action.</param>
        private void Btn_Preferences_Click(object sender, RoutedEventArgs e)
        {
            ResetGrdVisibility();
            Grd_Preferences.Visibility = Visibility.Visible;
        }

        #endregion

        #region LanguageMenu

        /// <summary>
        /// Handles the selection change event for the language combo box. Updates the application's current language
        /// and applies the selected language to all open windows.
        /// </summary>
        /// <remarks>This method updates the application's language based on the selected item in the
        /// combo box. It ensures that all open windows reflect the newly selected language. The first item in the combo
        /// box is treated as a placeholder and does not trigger a language change.</remarks>
        /// <param name="sender">The source of the event, typically the language combo box.</param>
        /// <param name="e">The event data containing information about the selection change.</param>
        private void Cmb_Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cmb_Language.SelectedItem != null && Cmb_Language.SelectedIndex != 0)
            {
                MainWindow.currentLanguage = Cmb_Language.SelectedIndex - 1;
                SetLanguage(MainWindow.currentLanguage);

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is ColorPickerWindow colorPickerWindow)
                    {
                        ColorPickerWindow.SetLanguage(MainWindow.currentLanguage, colorPickerWindow);
                    }
                    else if (window is MainWindow mainWindow)
                    {
                        MainWindow.SetLanguage(MainWindow.currentLanguage, mainWindow);
                    }
                }
            }
        }

        #endregion

        #region PreferencesMenu

        /// <summary>
        /// Handles the <see cref="System.Windows.Input.KeyDown"/> event for the save path text box. Updates the save
        /// path when the Enter key is pressed, validating the input and displaying a warning if the path is invalid.
        /// </summary>
        /// <remarks>If the Enter key is pressed and the entered path does not exist, a warning message is
        /// displayed, and the text box value is reset to the previous valid path. If the path is valid, it updates the
        /// save path and shifts focus to the preferences button.</remarks>
        /// <param name="sender">The source of the event, typically the save path text box.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing event data, including the pressed
        /// key.</param>
        private void Txb_Preferences_SavePath_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (!Path.Exists(Txb_Preferences_SavePath.Text)) // Check if the path exists
                {
                    MessageBox.Show(PalettePicker.Resources.OptionsWindowResources.OptionsWindow.PreferencesFolderSelectErrMsg,
                        PalettePicker.Resources.OptionsWindowResources.OptionsWindow.PreferencesFolderSelectErrTitle,
                        MessageBoxButton.OK, MessageBoxImage.Warning);

                    Txb_Preferences_SavePath.Text = savePath;
                }
                else
                {
                    savePath = Txb_Preferences_SavePath.Text;
                    Btn_Preferences.Focus();
                }
            }
        }

        #endregion
    }
}
