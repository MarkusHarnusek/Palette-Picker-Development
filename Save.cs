using PalettePicker.Resources.OptionsWindowResources;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace PalettePicker
{
    internal class Save
    {
        private static string filePath = string.Empty;

        private static void SetLanguage(int languageID)
        {
            string[] cultures = { "en-UK", "de-DE", "es-ES", "fr-FR", "zh-CN", "pt-PT", "ru-RU" };

            if (languageID < 0 || languageID >= cultures.Length)
            {
                languageID = 0;
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultures[languageID]);
        }

        public Save()
        {
            SetLanguage(MainWindow.currentLanguage);
        }

        
        private static string GetShortTranslatedErrMsg(int errorId, int languageId)
        {
            switch (errorId)
            {
                case 0:
                    switch (languageId)
                    {
                        case 0: return "File does not exist.";
                        case 1: return "Datei existiert nicht.";
                        case 2: return "El archivo no existe.";
                        case 3: return "Le fichier n'existe pas.";
                        case 4: return "文件不存在。";
                        case 5: return "O arquivo não existe.";
                        case 6: return "Файл не существует.";
                    }

                    break;

                case 1:
                    switch (languageId)
                    {
                        case 0: return "File is not a JSON file.";
                        case 1: return "Datei ist keine JSON-Datei.";
                        case 2: return "El archivo no es un archivo JSON.";
                        case 3: return "Le fichier n'est pas un fichier JSON.";
                        case 4: return "文件不是 JSON 文件。";
                        case 5: return "O arquivo não é um arquivo JSON.";
                        case 6: return "Файл не является файлом JSON.";
                    }

                    break;

                case 2:
                    switch (languageId)
                    {
                        case 0: return "File is empty.";
                        case 1: return "Datei ist leer.";
                        case 2: return "El archivo está vacío.";
                        case 3: return "Le fichier est vide.";
                        case 4: return "文件是空的。";
                        case 5: return "O arquivo está vazio.";
                        case 6: return "Файл пуст.";
                    }

                    break;

                case 3:
                    switch (languageId)
                    {
                        case 0: return "JSON data was null.";
                        case 1: return "JSON-Daten waren null.";
                        case 2: return "Los datos JSON eran nulos.";
                        case 3: return "Les données JSON étaient nulles.";
                        case 4: return "JSON 数据为 null。";
                        case 5: return "Os dados JSON eram nulos.";
                        case 6: return "Данные JSON были нулевыми.";
                    }

                    break;

                case 4:
                    switch (languageId)
                    {
                        case 0: return "File does not contain the correct number of value pairs.";
                        case 1: return "Datei enthält nicht die richtige Anzahl von Wertpaaren.";
                        case 2: return "El archivo no contiene el número correcto de pares de valores.";
                        case 3: return "Le fichier ne contient pas le bon nombre de paires de valeurs.";
                        case 4: return "文件不包含正确数量的值对。";
                        case 5: return "O arquivo não contém o número correto de pares de valores.";
                        case 6: return "Файл не содержит правильного количества пар значений.";
                    }

                    break;

                case 5:
                    switch (languageId)
                    {
                        case 0: return "Missing required key";
                        case 1: return "Fehlender erforderlicher Schlüssel";
                        case 2: return "Falta la clave requerida";
                        case 3: return "Clé requise manquante";
                        case 4: return "缺少必需的密钥";
                        case 5: return "Falta a chave obrigatória";
                        case 6: return "Отсутствует обязательный ключ";
                    }

                    break;

                case 6:
                    switch (languageId)
                    {
                        case 0: return "Error reading file";
                        case 1: return "Fehler beim Lesen der Datei";
                        case 2: return "Error al leer el archivo";
                        case 3: return "Erreur de lecture du fichier";
                        case 4: return "读取文件时出错";
                        case 5: return "Erro ao ler o arquivo";
                        case 6: return "Ошибка чтения файла";
                    }

                    break;
            }

            return string.Empty;
        }

        public static void SelectFile()
        {
            string title = Resources.SaveClassResources.SaveClass.FileErrMsg;

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = title,
                CheckFileExists = true,
                Multiselect = false,
                CheckPathExists = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }

            if (string.IsNullOrEmpty(filePath))
            {
                System.Windows.MessageBox.Show(Resources.SaveClassResources.SaveClass.SelectErrMsg,
                    Resources.SaveClassResources.SaveClass.SelectErrTitle,
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (!ValidatePallleteJsonFile(filePath, out string errMsg))
            {
                System.Windows.MessageBox.Show(Resources.SaveClassResources.SaveClass.FileErrMsg,
                    Resources.SaveClassResources.SaveClass.FileErrTitle,
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return;
            }

            MainWindow.editingFilePath = filePath;
        }

        public static string GetSavePath()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = Resources.SaveClassResources.SaveClass.SaveDialog,
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DefaultExt = ".json",
                AddExtension = true,
                OverwritePrompt = true
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                return saveFileDialog.FileName;
            }
            else
            {
                System.Windows.MessageBox.Show(
                    Resources.SaveClassResources.SaveClass.FolderSelectErrMsg,
                    Resources.SaveClassResources.SaveClass.FolderSelectErrTitle,
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return string.Empty;
            }
        }

        public static string SaveFile(string primary1, string primary2, string seconadary1, string secondary2, string text, bool readOnly, bool homeVisible, bool pinned)
        {
            string path = GetSavePath();

            if (!string.IsNullOrEmpty(path) && Directory.Exists(Path.GetDirectoryName(path)))
            {
                readOnly = false;
                homeVisible = true;
                pinned = true;

                Pallete palette = new Pallete
                {
                    valid = true,
                    filePath = Path.GetDirectoryName(path),
                    paletteName = Path.GetFileNameWithoutExtension(path),
                    primary1 = primary1,
                    primary2 = primary2,
                    secondary1 = seconadary1,
                    secondary2 = secondary2,
                    text = text,
                    readOnly = readOnly,
                    homeVisible = homeVisible,
                    pinned = pinned
                };

                string jsonString = JsonSerializer.Serialize(palette);
                File.WriteAllText(path, jsonString);

                return Path.GetFileNameWithoutExtension(path);
            }
            else
            {
                System.Windows.MessageBox.Show(Resources.SaveClassResources.SaveClass.SaveErrMsg,
                    Resources.SaveClassResources.SaveClass.SaveErrTitle,
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return string.Empty;
            }
        }

        public static (string name, string primary1, string primary2, string secondary1, string secondary2, string text, bool readOnly, bool homeVisible, bool pinned) GetSaveInfo(string path)
        {
            if (!ValidatePallleteJsonFile(path, out string errMsg))
            {
                System.Windows.MessageBox.Show(Resources.SaveClassResources.SaveClass.FileReadErrMsg + errMsg,
                    Resources.SaveClassResources.SaveClass.FileReadErrTitle,
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, false, false);
            }

            Dictionary<string, string>? jsonData = GetJsonValues(path);
            if (jsonData == null)
            {
                System.Windows.MessageBox.Show(Resources.SaveClassResources.SaveClass.FileReadErrMsg + errMsg, 
                    Resources.SaveClassResources.SaveClass.FileReadErrTitle,
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, false, false);
            }

            string name = jsonData["paletteName"];
            string primary1 = jsonData["primary1"];
            string primary2 = jsonData["primary2"];
            string secondary1 = jsonData["secondary1"];
            string secondary2 = jsonData["secondary2"];
            string text = jsonData["text"];
            bool readOnly = jsonData["readOnly"] == "true";
            bool homeVisible = jsonData["homeVisible"] == "true";
            bool pinned = jsonData["pinned"] == "true";

            return (name, primary1, primary2, secondary1, secondary2, text, readOnly, homeVisible, pinned);
        }

        private static Dictionary<string, string>? GetJsonValues(string path)
        {
            if (!ValidatePallleteJsonFile(path, out string errMsg))
            {
                System.Windows.MessageBox.Show(Resources.SaveClassResources.SaveClass.FileReadErrMsg + errMsg,
                    Resources.SaveClassResources.SaveClass.FileReadErrTitle,
                    System.Windows.MessageBoxButton.OK, 
                    System.Windows.MessageBoxImage.Warning);
                return null;
            }

            string jsonContent = File.ReadAllText(path);
            Dictionary<string, string> jsonData = new Dictionary<string, string>();

            string[] lines = jsonContent.Split(new[] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] keyValue = line.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim().Trim('"');
                    string value = keyValue[1].Trim().Trim('"');
                    jsonData.Add(key, value);
                }
            }

            return jsonData;
        }

        private static bool ValidatePallleteJsonFile(string jsonFile, out string errorMessage)
        {
            if (!File.Exists(jsonFile))
            {
                errorMessage = Resources.SaveClassResources.SaveClass.FileNotExistErr;
                return false;
            }

            string fileExtension = Path.GetExtension(jsonFile);
            if (fileExtension != ".json")
            {
                errorMessage = Resources.SaveClassResources.SaveClass.FileNotJSONErr;
                return false;
            }

            try
            {
                string? content = File.ReadAllText(jsonFile);

                if (string.IsNullOrWhiteSpace(content))
                {
                    errorMessage = Resources.SaveClassResources.SaveClass.FileEmptyErr;
                    return false;
                }

                var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

                if (jsonData == null)
                {
                    errorMessage = Resources.SaveClassResources.SaveClass.JSONNullErr;
                    return false;
                }

                if (jsonData.Count != 11)
                {
                    errorMessage = Resources.SaveClassResources.SaveClass.WrongPairCountErr;
                    return false;
                }

                string[] requiredKeys = { "valid", "filePath", "paletteName", "primary1", "primary2", "secondary1", "secondary2", "text", "readOnly", "homeVisible", "pinned" };
                foreach (string key in requiredKeys)
                {
                    if (!jsonData.ContainsKey(key))
                    {
                        errorMessage = Resources.SaveClassResources.SaveClass.MissingKeyErr + key;
                        return false;
                    }
                }

                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = Resources.SaveClassResources.SaveClass.ReadingErr + ex.Message;
                return false;
            }
        }
    }
}
