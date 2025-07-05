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
            GetConfig();
            if (string.IsNullOrEmpty(configFilePath))
            {
                configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.ini");
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
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker");

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker"));
            }

            try
            {
                File.WriteAllText(configFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.ini"), GetConfigText);
            }
            catch (Exception) { }
        }

        public static void GetConfig()
        {
            try
            {
                if (File.Exists(configFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.ini")))
                {
                    var configLines = File.ReadAllLines(configFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.ini"));
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
            configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PalettePicker", "config.ini");
            isFirstRun = true;
            currentLanguage = 0;
            isDarkMode = false;
            username = "none";
            password = "none";
            SetConfig();
        }
    }
}
