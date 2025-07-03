using System.IO;

namespace PalettePicker
{
    internal class Config
    {
        public static string? configFilePath { get; set;}
        public static bool isFirstRun { get; set; }
        public static int currentLanguage { get; set; }
        public static bool isDarkMode { get; set; }
        public static string? username { get; set; }
        public static string? password { get; set; }

        public static void AppInit()
        {
            if (string.IsNullOrEmpty(configFilePath))
            {
                configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.json");
                SetDefaultConfig();
            }
            else
            {
                GetConfig();
            }
        }

        public static string GetConfigText =>
            $"PATH={configFilePath ?? string.Empty}\n" +
            $"FIRST_RUN={isFirstRun}\n" +
            $"LANGUAGE={currentLanguage}\n" +
            $"DARK_MODE={isDarkMode}\n" +
            $"USERNAME={username ?? "none"}\n" +
            $"PASSWORD={password ?? "none"}";

        public static void SetConfig()
        {
            // Implement logic to create the directory if it doesn't exist      

            try
            {
                File.WriteAllText(configFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.json"), GetConfigText);
            }
            catch (Exception) { }
        }

        public static void GetConfig()
        {
            try
            {
                if (File.Exists(configFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.json")))
                {
                    var configLines = File.ReadAllLines(configFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.json"));
                    foreach (var line in configLines)
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            switch (parts[0])
                            {
                                case "PATH":
                                    configFilePath = parts[1];
                                    break;
                                case "FIRST_RUN":
                                    isFirstRun = bool.Parse(parts[1]);
                                    break;
                                case "LANGUAGE":
                                    currentLanguage = int.TryParse(parts[1], out int val) ? val : 0;
                                    break;
                                case "DARK_MODE":
                                    isDarkMode = bool.Parse(parts[1]);
                                    break;
                                case "USERNAME":
                                    username = parts[1];
                                    break;
                                case "PASSWORD":
                                    password = parts[1];
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        public static void SetDefaultConfig()
        {
            configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.json");
            isFirstRun = true;
            currentLanguage = 0;
            isDarkMode = false;
            username = "none";
            password = "none";
            SetConfig();
        }
    }
}
