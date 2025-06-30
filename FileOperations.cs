using System.IO;
using System.Security.Policy;
using System.Windows.Input;
using System.Windows.Markup;

namespace PalettePicker
{
    /// <summary>
    /// Provides static methods for file operations related to PublicProfile objects, including reading, writing, and parsing profile data.
    /// Handles logging of errors and exceptions encountered during file operations.
    /// </summary>
    internal class FileOperations
    {
        /// <summary>
        /// Stores the latest log message, including errors and exceptions encountered during file operations.
        /// </summary>
        public static string log = string.Empty;

        #region Public Profile

        /// <summary>
        /// Reads a .palette file from the specified path and parses it into a PublicProfile object.
        /// Logs errors if the file does not exist, is not a .palette file, or if an exception occurs during reading.
        /// </summary>
        /// <param name="path">The file path to the .palette file.</param>
        /// <returns>A PublicProfile object if successful; otherwise, null.</returns>
        public static PublicProfile? GetPublicProfile(string path)
        {
            if (!File.Exists(path))
            {
                log = $"--- ERROR --- \n{DateTime.Now:HH} File does not exist";
                return null;
            }

            string content = string.Empty;

            try
            {
                if (Path.GetExtension(path) != ".palette")
                {
                    log = $"--- ERROR --- \n{DateTime.Now:HH} File is not a \".palette\" file \n---";
                    return null;
                }

                content = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                log = $"--- EXCEPTION --- \n{DateTime.Now:HH} {ex.Message}\n---";
                return null;
            }

            return SavePublicProfileFromText(content) ?? null;
        }

        /// <summary>
        /// Parses the provided text into a PublicProfile object after decoding and splitting the content.
        /// Logs errors if the content does not have the correct format or if an exception occurs during parsing.
        /// </summary>
        /// <param name="input">The encoded profile text to parse.</param>
        /// <returns>A PublicProfile object if parsing is successful; otherwise, null.</returns>
        public static PublicProfile? SavePublicProfileFromText(string input)
        {
            // Decode the input string and remove leading/trailing whitespace
            input = Codec.DecodeFile(input).Trim();
            // Split the decoded string into lines using ';' as the separator
            // Collections (like colors and tags) are enclosed in brackets and may contain semicolons
            List<string> lines = new List<string>();
            int i = 0;
            while (i < input.Length)
            {
                int sepIndex = input.IndexOf(';', i);
                if (sepIndex == -1) sepIndex = input.Length;
                string part = input.Substring(i, sepIndex - i);
                // If this part contains an opening bracket, it is a collection
                if (part.Contains("["))
                {
                    int openBracket = input.IndexOf('[', i);
                    int closeBracket = input.IndexOf(']', openBracket);
                    // Include everything up to the closing bracket
                    part = input.Substring(i, closeBracket - i + 1);
                    lines.Add(part);
                    // Move i to after the closing bracket and next semicolon
                    i = closeBracket + 2; // skip ']' and ';'
                }
                else
                {
                    lines.Add(part);
                    i = sepIndex + 1;
                }
            }

            // Check if the number of value pairs is correct (should be 14)
            if (lines.Count != 14)
            {
                log = $"--- ERROR --- \n{DateTime.Now:HH} File does not contain correct amount (14) value pairs \n---";
            }

            // Dictionary to store key-value pairs from the file
            Dictionary<string, string> values = new Dictionary<string, string>();

            try
            {
                // Parse each line into key-value pairs and add to the dictionary
                foreach (string line in lines)
                {
                    int idx = line.IndexOf(':');
                    values.Add(line.Substring(0, idx).Trim(), line.Substring(idx + 1).Trim());
                }

                // Dictionary to store color name-value pairs
                Dictionary<string, string> colors = new Dictionary<string, string>();

                // Parse the 'colors' field, remove brackets, and split by ';'
                foreach (string value in values["colors"].Trim('[', ']').Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        int idx = value.IndexOf(':');
                        colors.Add(value.Substring(0, idx), value.Substring(idx + 1));
                    }
                }

                // Parse the 'tags' field, remove brackets, and split by ';'
                List<string> tags = values["tags"].Trim('[', ']').Split(';').Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

                // Create and return a new PublicProfile object with parsed values
                return new PublicProfile
                {
                    Id = values["id"],
                    Name = values["name"],
                    Description = values["description"],
                    Author = values["author"],
                    IsPublic = bool.Parse(values["public"]),
                    Created = DateTime.Parse(values["created"]),
                    Modified = DateTime.Parse(values["modified"]),
                    IsDark = bool.Parse(values["isdark"]),
                    BaseColor = values["basecolor"],
                    Views = int.Parse(values["views"]),
                    Favorites = int.Parse(values["favorites"]),
                    Downloads = int.Parse(values["downloads"]),
                    Colors = colors,
                    Tags = tags
                };
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during parsing
                log = $"--- EXCEPTION --- \n{DateTime.Now:HH} {ex.Message}\n---";
                return null;
            }
        }

        /// <summary>
        /// Saves the provided PublicProfile object to the specified path as an encoded .palette file.
        /// Logs exceptions if writing fails.
        /// </summary>
        /// <param name="path">The file path to save the profile.</param>
        /// <param name="input">The PublicProfile object to save.</param>
        public static void SavePublicProfile(string path, PublicProfile input)
        {
            try
            {
                File.WriteAllText(path, Codec.EncodeFile(GetSavePublicProfileText(input).Trim()));
            }
            catch (Exception ex)
            {
                log = $"--- EXCEPTION --- \n{DateTime.Now:HH} {ex.Message}\n---";
                return;
            }
        }

        /// <summary>
        /// Generates the text representation of a PublicProfile object for saving to a file.
        /// </summary>
        /// <param name="input">The PublicProfile object to convert.</param>
        /// <returns>A string containing the profile data in the correct format.</returns>
        public static string GetSavePublicProfileText(PublicProfile input)
        {
            string content = string.Empty;
            content += $"id:{input.Id};";
            content += $"name:{input.Name};";
            content += $"description:{input.Description};";
            content += $"author:{input.Author};";
            content += $"public:{input.IsPublic};";
            content += $"created:{input.Created:yyyy-MM-dd};";
            content += $"modified:{input.Modified:yyyy-MM-dd};";
            content += $"isdark:{input.IsDark};";
            content += $"basecolor:{input.BaseColor};";
            content += $"views:{input.Views};";
            content += $"favorites:{input.Favorites};";
            content += $"downloads:{input.Downloads};";
            content += $"colors:[{string.Join(";", (input.Colors ?? Enumerable.Empty<KeyValuePair<string, string>>()).Select(c => $"{c.Key}:{c.Value}"))}];";
            content += $"tags:[{string.Join(";", input.Tags ?? Enumerable.Empty<string>())}]";
            return content.Trim();
        }

        #endregion

        #region Local Profile

        /// <summary>
        /// Reads a .local file from the specified path and parses it into a PrivateProfile object.
        /// Logs errors if the file does not exist, is not a .local file, or if an exception occurs during reading.
        /// </summary>
        /// <param name="path">The file path to the .local file.</param>
        /// <returns>A PrivateProfile object if successful; otherwise, null.</returns>
        public static PrivateProfile? GetPrivateProfile(string path)
        {
            if (!File.Exists(path))
            {
                log = $"--- ERROR --- \n{DateTime.Now:HH} File does not exist";
                return null;
            }

            string content = string.Empty;

            try
            {
                if (Path.GetExtension(path) != ".local")
                {
                    log = $"--- ERROR --- \n{DateTime.Now:HH} File is not a \".local\" file \n---";
                    return null;
                }

                content = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                log = $"--- EXCEPTION --- \n{DateTime.Now:HH} {ex.Message}\n---";
                return null;
            }

            return SavePrivateProfileFromText(content) ?? null;
        }

        /// <summary>
        /// Parses the provided text into a PrivateProfile object after decoding and splitting the content.
        /// Logs errors if the content does not have the correct format or if an exception occurs during parsing.
        /// </summary>
        /// <param name="input">The encoded profile text to parse.</param>
        /// <returns>A PrivateProfile object if parsing is successful; otherwise, null.</returns>
        public static PrivateProfile? SavePrivateProfileFromText(string input)
        {
            // Decode the input string and remove leading/trailing whitespace
            input = Codec.Decode(input).Trim();
            // Split the decoded string into lines using ';' as the separator
            string[] lines = input.Split(';');

            // Check if the number of value pairs is correct (should be 4)
            if (lines.Length != 4)
            {
                log = $"--- ERROR --- \n{DateTime.Now:HH} File does not contain correct amount (4) value pairs \n---";
            }

            // Dictionary to store key-value pairs from the file
            Dictionary<string, string> values = new Dictionary<string, string>();

            try
            {
                // Parse each line into key-value pairs and add to the dictionary
                foreach (string line in lines)
                {
                    values.Add(line.Split(':')[0].Trim(), line.Split(':')[1].Trim());
                }

                // List to store collections, parsed from the 'collections' field
                List<string> collection = values["collections"].Remove('[', ']').Split(';').ToList();

                // Create and return a new PrivateProfile object with parsed values
                return new PrivateProfile
                {
                    Id = values["id"],
                    Path = values["path"],
                    Pinned = bool.Parse(values["pinned"]),
                    Collections = collection
                };
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during parsing
                log = $"--- EXCEPTION --- \n{DateTime.Now:HH} {ex.Message}\n---";
                return null;
            }
        }

        /// <summary>
        /// Saves the provided PrivateProfile object to the specified path as an encoded .local file.
        /// Logs exceptions if writing fails.
        /// </summary>
        /// <param name="path">The file path to save the profile.</param>
        /// <param name="input">The PrivateProfile object to save.</param>
        public static void SavePrivateProfile(string path, PrivateProfile input)
        {
            try
            {
                // Write the encoded profile text to the file
                File.WriteAllText(path, Codec.Encode(GetSavePrivateProfileText(input)));
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during file writing
                log = $"--- EXCEPTION --- \n{DateTime.Now:HH} {ex.Message}\n---";
                return;
            }
        }

        /// <summary>
        /// Generates the text representation of a PrivateProfile object for saving to a file.
        /// </summary>
        /// <param name="input">The PrivateProfile object to convert.</param>
        /// <returns>A string containing the profile data in the correct format.</returns>
        public static string GetSavePrivateProfileText(PrivateProfile input)
        {
            // Build the content string by concatenating all profile fields in the required format
            string content = string.Empty;
            content += $"id:{input.Id};";
            content += $"path:{input.Path};";
            content += $"pinned:{input.Pinned};";
            content += $"collections:[{string.Join(";", input.Collections ?? Enumerable.Empty<string>())}]";
            // Return the formatted content string
            return content.Trim();
        }

        #endregion
    }
}
