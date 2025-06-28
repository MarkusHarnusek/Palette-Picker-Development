using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalettePicker.Palette
{
    // <summary>
    // Holds all local data for a palette
    // </summary>

    internal class PaletteLocal
    {
        // <summary>
        // The save path of the palette
        // </summary>
        public string? savePath { get; set; }

        // <summary>
        // Shows if the palette is pinned on the home screen
        // </summary>
        public bool isPinned { get; set; }

        // <summary>
        // List of all collections the palette is in
        // </summary>
        public List<string> collections { get; set; } = new List<string>();
    }
}
