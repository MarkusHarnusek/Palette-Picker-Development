using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalettePicker
{
    // <summary>
    // The server profile of a palette, storing metadata about the palette.
    // </summary>

    internal class PaletteProfile
    {
        // <summary>
        // The unique identifier for the palette
        // </summary>
        public string? id { get; set; }

        // <summary>
        // The name of the palette.
        // </summary>
        public string? name { get; set; }

        // <summary>
        // The desciription of the palette
        // </summary>
        public string? description { get; set; }

        // <summary>
        // The author of the palette.
        // </summary>
        public string? author { get; set; }

        // <summary>
        // Shows if the palette is public
        // </summary>
        public bool isPublic { get; set; }

        // <summary>
        // The creation date of the palette
        // </summary>
        public DateTime? createdAt { get; set; }

        // <summary>
        // The last modification date of the palette
        // </summary>
        public DateTime? modifiedAt { get; set; }

        // <summary>
        // Indicates if the palette is dark
        // </summary>
        public bool isDark { get; set; }

        // <summary>
        // The base color of the palette
        // </summary>
        public string? baseColor { get; set; }

        // <summary>
        // The number of times the palette has been viewed
        // </summary>

        public int views { get; set; }
        // <summary>
        // How many have favorited the palette
        // </summary>
        public int favorites { get; set; }

        // <summary>
        // The number of times the palette has been downloaded
        // </summary>
        public int downloads { get; set; }

        // <summary
        // All the colors in the palette
        // </summary>
        public Dictionary<string, string>? colors { get; set; }

        // <summary>
        // The tags associated with the palette 
        // </summary>
        public List<string>? tags { get; set; }
    }
}
