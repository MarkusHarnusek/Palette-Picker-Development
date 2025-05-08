using System.IO;
using System.Text.Json;

namespace PalletePicker
{
    internal class Save
    {
        private static string filePath = string.Empty;

        public static void SelectFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Select the color pallete",
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
                System.Windows.MessageBox.Show("No file selected.", "Select Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (!ValidatePallleteJsonFile(filePath, out string errMsg))
            {
                System.Windows.MessageBox.Show($"The selected file is not a valid pallete JSON file. {errMsg}", "Select Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            MainWindow.editingFilePath = filePath;
        }

        public static string GetSavePath()
        {
            Microsoft.Win32.OpenFolderDialog dialog = new();
            dialog.Title = "Select the folder to save the pallete";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Multiselect = false;

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FolderName;
            }
            else
            {
                System.Windows.MessageBox.Show("No folder selected.", "Select Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return string.Empty;
            }
        }

        public static void SaveFile(string path, string palleteName, string primary1, string primary2, string seconadary1, string secondary2, string text, bool readOnly, bool homeVisible, bool pinned)
        {
            // Fixed values > Implemt logic with later update

            readOnly = false;
            homeVisible = true;
            pinned = true;

            Pallete pallete = new Pallete
            {
                valid = true,
                filePath = path,
                palleteName = palleteName,
                primary1 = primary1,
                primary2 = primary2,
                secondary1 = seconadary1,
                secondary2 = secondary2,
                text = text,
                readOnly = readOnly,
                homeVisible = homeVisible,
                pinned = pinned
            };

            string jsonString = JsonSerializer.Serialize(pallete);
            File.WriteAllText($"{pallete.palleteName}.json", pallete.filePath);

        }

        public static (string name, string primary1, string primary2, string secondary1, string secondary2, string text, bool readOnly, bool homeVisible, bool pinned) GetSaveInfo(string path)
        {
            if (!ValidatePallleteJsonFile(path, out string errMsg))
            {
                System.Windows.MessageBox.Show($"Error occured while extracting json save file with following error message: {errMsg}", "File Read Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, false, false);
            }

            Dictionary<string, string>? jsonData = GetJsonValues(path);
            if (jsonData == null)
            {
                System.Windows.MessageBox.Show("Error occured while extracting json save file.", "File Read Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, false, false);
            }

            string name = jsonData["name"];
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
                System.Windows.MessageBox.Show($"Error occured while extracting json save file with following error message: {errMsg}", "File Read Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
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
                errorMessage = "File does not exist.";
                return false;
            }

            string fileExtension = Path.GetExtension(jsonFile);
            if (fileExtension != ".json")
            {
                errorMessage = "File is not a JSON file.";
                return false;
            }

            try
            {
                string content = File.ReadAllText(jsonFile);

                if (string.IsNullOrWhiteSpace(content))
                {
                    errorMessage = "File is empty.";
                    return false;
                }

                content.Replace("{", string.Empty);
                content.Replace("}", string.Empty);
                string[] lines = content.Split("\n");

                if (lines.Length != 11)
                {
                    errorMessage = "File does not contain the correct number of value pairs.";
                    return false;
                }
                else
                {
                    Dictionary<string, string> jsonData = new Dictionary<string, string>();
                    foreach (string line in lines)
                    {
                        string[] keyValue = line.Split(":");
                        if (keyValue.Length != 2)
                        {
                            errorMessage = "File does not contain valid key-value pairs.";
                            return false;
                        }
                        string key = keyValue[0].Trim().Trim('"');
                        string value = keyValue[1].Trim().Trim('"');
                        if (jsonData.ContainsKey(key))
                        {
                            errorMessage = $"Duplicate key found: {key}";
                            return false;
                        }
                        jsonData[key] = value;
                    }

                    for (int i = 0; i < jsonData.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (jsonData.Keys.ToArray()[i] != "name")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 1:
                                if (jsonData.Keys.ToArray()[i] != "filePath")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 2:
                                if (jsonData.Keys.ToArray()[i] != "palleteName")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 3:
                                if (jsonData.Keys.ToArray()[i] != "primary1")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 4:
                                if (jsonData.Keys.ToArray()[i] != "primary2")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 5:
                                if (jsonData.Keys.ToArray()[i] != "secondary1")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 6:
                                if (jsonData.Keys.ToArray()[i] != "secondary2")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 7:
                                if (jsonData.Keys.ToArray()[i] != "text")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 8:

                                if (jsonData.Keys.ToArray()[i] != "readOnly")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 9:

                                if (jsonData.Keys.ToArray()[i] != "homeVisible")
                                {
                                    errorMessage = $"Wrong element at index {i}."; return false;
                                }
                                break;

                            case 10:

                                if (jsonData.Keys.ToArray()[i] != "pinned")
                                {
                                    errorMessage = $"Wrong element at index {i}"; return false;
                                }
                                break;
                        }
                    }

                    errorMessage = string.Empty;
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error reading file: {ex.Message}";
                return false;
            }
        }
    }
}
