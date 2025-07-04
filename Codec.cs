namespace PalettePicker
{
    /// <summary>
    /// Provides encoding and decoding functionality for specific profile string keys and file content.
    /// </summary>
    internal class Codec
    {
        // List of all encodable and decodable string keys
        private static List<string> codecValues = new List<string> {
            "id",
            "name",
            "description",
            "author",
            "public",
            "created",
            "modified",
            "isdark",
            "basecolor",
            "views",
            "favorites",
            "downloads",
            "colors",
            "tags",
            "path",
            "pinned",
            "collections",
            "email",
            "passwordHash",
            "palettes",
            "followers",
            "following"
        };

        /// <summary>
        /// The charset for encoding and decoding
        /// </summary>
        private static string charSet = "abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// All characters that will be identified as separators between words
        /// </summary>
        private static char[] separators = [ ':', ';', '[', ']' ];

        /// <summary>
        /// Encodes a single key string to its corresponding character in the charset.
        /// </summary>
        /// <param name="input">The string to be encoded</param>
        /// <returns>The encoded string</returns>
        public static string Encode(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || !codecValues.Contains(input))
            {
                return input;
            }
            return charSet[codecValues.IndexOf(input)].ToString();
        }

        /// <summary>
        /// Encodes a file content string by encoding each key and preserving separators.
        /// </summary>
        /// <param name="input">The string to be encoded</param>
        /// <returns>The encoded string</returns>
        public static string EncodeFile(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            string result = string.Empty;
            string newWord = string.Empty;
            List<string> words = new List<string>();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (separators.Contains(c))
                {
                    if (!string.IsNullOrEmpty(newWord))
                        words.Add(newWord);
                    words.Add(c.ToString());
                    newWord = string.Empty;
                }
                else
                {
                    newWord += c.ToString();
                }
            }
            if (!string.IsNullOrEmpty(newWord))
                words.Add(newWord);

            foreach (string word in words)
            {
                if (separators.Contains(word[0]))
                {
                    result += word;
                }
                else
                {
                    result += Encode(word);
                }
            }
            return result;
        }

        /// <summary>
        /// Decodes a single character or string to its corresponding key string.
        /// </summary>
        /// <param name="input">The string to be decoded</param>
        /// <returns>The decoded string</returns>
        public static string Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            if (input.Length == 1 && charSet.Contains(input[0]))
            {
                int idx = charSet.IndexOf(input[0]);
                if (idx >= 0 && idx < codecValues.Count)
                    return codecValues[idx];
            }
            return input;
        }

        /// <summary>
        /// Decodes an encoded file content string back to its original form.
        /// </summary>
        /// <param name="input">The encoded string</param>
        /// <returns>The decoded string</returns>
        public static string DecodeFile(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            string result = string.Empty;
            string word = string.Empty;
            foreach (char c in input)
            {
                if (separators.Contains(c))
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        result += Decode(word);
                        word = string.Empty;
                    }
                    result += c;
                }
                else
                {
                    word += c;
                }
            }
            if (!string.IsNullOrEmpty(word))
                result += Decode(word);
            return result;
        }
    }
}
