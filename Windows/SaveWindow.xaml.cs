using System.Windows;

namespace PalettePicker.Windows
{
    /// <summary>
    /// Interaction logic for SaveWindow.xaml
    /// </summary>
    public partial class SaveWindow : Window
    {
        private const int defaultWidth = 300;
        private const int defaultHeight = 450;

        public SaveWindow()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Width < defaultWidth)
            {
                Width = defaultWidth;
            }

            if (Height < defaultHeight)
            {
                Height = defaultHeight;
            }
        }
    }
}
