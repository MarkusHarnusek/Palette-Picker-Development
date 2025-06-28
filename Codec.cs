namespace PalettePicker
{
    internal class Codec
    {
        // <summary>
        // All encodable and decodable stringd
        // </summary>
        private static List<string> codecValues = [
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
            "collections"
            ];

        // <summary>
        // The charset for encoding and decoding
        // </summary>
        private static string charSet = "abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // <summary>
        // The method to encode strings
        // </summary>
        // <param name="input">
        // The string to be encoded
        // </param>
        // <returns>
        // The encoded string
        // </returns>
        public static string Encode(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || !codecValues.Contains(input))
            {
                return input;
            }

            return charSet[codecValues.IndexOf(input)].ToString();
        }

        // <summary>
        // The method to decode strings
        // </summary>
        // <param name="input">
        // The string to be decoded
        // </param>
        // <returns>
        // The decoded string
        // </returns>
        public static string Decode(string input)
        {
            if (string.IsNullOrEmpty(input) || !codecValues.Contains(input))
            {
                return input; 
            }

            return codecValues[charSet.IndexOf(input)];
        }
    }
}
