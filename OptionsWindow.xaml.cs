using System.Windows;

namespace PalettePicker
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        int language = 0;

        public OptionsWindow()
        {
            InitializeComponent();
        }

        private void SetLanguage(int languageID)
        {
            switch (languageID)
            {
                case 0:
                    Txt_WindowTitle.Text = "Options";
                    Btn_Preferences.Content = "Preferences";
                    Txt_Preferences_Title.Text = "Preferences";
                    Txt_Preferences_Language_Title.Text = "Language";
                    Btn_Apply.Content = "Apply";

                    break;

                case 1:
                    Txt_WindowTitle.Text = "Einstellungen";
                    Btn_Preferences.Content = "Präferenzen";
                    Txt_Preferences_Title.Text = "Präferenzen";
                    Txt_Preferences_Language_Title.Text = "Sprache";
                    Btn_Apply.Content = "Anwenden";

                    break;

                case 2:
                    Txt_WindowTitle.Text = "Ajustes";
                    Btn_Preferences.Content = "Ajustes";
                    Txt_Preferences_Title.Text = "Ajustes";
                    Txt_Preferences_Language_Title.Text = "Idioma";
                    Btn_Apply.Content = "Aplicar";

                    break;

                case 3:
                    Txt_WindowTitle.Text = "Paramètres";
                    Btn_Preferences.Content = "Preferencias";
                    Txt_Preferences_Title.Text = "Préférences";
                    Txt_Preferences_Language_Title.Text = "Langue";
                    Btn_Apply.Content = "Appliquer";

                    break;

                case 4:
                    Txt_WindowTitle.Text = "设置";
                    Btn_Preferences.Content = "首选项";
                    Txt_Preferences_Title.Text = "首选项";
                    Txt_Preferences_Language_Title.Text = "语言";
                    Btn_Apply.Content = "应用";

                    break;

                case 5:
                    Txt_WindowTitle.Text = "Configurações";
                    Btn_Preferences.Content = "Preferências";
                    Txt_Preferences_Title.Text = "Preferências";
                    Txt_Preferences_Language_Title.Text = "Idioma";
                    Btn_Apply.Content = "Aplicar";

                    break;

                case 6:
                    Txt_WindowTitle.Text = "Настройки";
                    Btn_Preferences.Content = "Предпочтения";
                    Txt_Preferences_Title.Text = "Предпочтения";
                    Txt_Preferences_Language_Title.Text = "Язык";
                    Btn_Apply.Content = "Применить";

                    break;
            }
        }

        private (string msg, string title) GetTranslatedEmptyLanguageSelectionErrorMessage(int languageId)
        {
            if (Lst_Language.SelectedItem == null)
            {
                string msg = string.Empty;
                string title = string.Empty;

                switch (languageId)
                {
                    case 0:
                        msg = "Please select a language.";
                        title = "Error";
                        break;

                    case 1:
                        msg = "Bitte wählen Sie eine Sprache aus.";
                        title = "Fehler";
                        break;

                    case 2:
                        msg = "Por favor, seleccione un idioma.";
                        title = "Error";
                        break;

                    case 3:
                        msg = "Veuillez sélectionner une langue.";
                        title = "Erreur";
                        break;

                    case 4:
                        msg = "请选择一种语言。";
                        title = "错误";
                        break;

                    case 5:
                        msg = "Por favor, selecione um idioma.";
                        title = "Erro";
                        break;

                    case 6:
                        msg = "Пожалуйста, выберите язык.";
                        title = "Ошибка";
                        break;
                }

                return (msg, title);
            }

            return (string.Empty, string.Empty);
        }

        private void Btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (Lst_Language.SelectedItem == null)
            {
                MessageBox.Show(GetTranslatedEmptyLanguageSelectionErrorMessage(MainWindow.currentLanguage).msg, GetTranslatedEmptyLanguageSelectionErrorMessage(MainWindow.currentLanguage).title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MainWindow.currentLanguage = Lst_Language.SelectedIndex;
                SetLanguage(MainWindow.currentLanguage);
            }
        }
    }
}
