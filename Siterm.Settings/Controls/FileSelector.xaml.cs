using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace Siterm.Settings.Controls
{
    public partial class FileSelector : UserControl, INotifyPropertyChanged
    {
        public static DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(FileSelector),
                new PropertyMetadata("Folder Selector"));

        public static DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(FileSelector),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PathPropertyChanged));

        public static DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(FileSelector),
                new PropertyMetadata("..."));

        public FileSelector()
        {
            InitializeComponent();
        }

        public string Placeholder
        {
            get => (string) GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public string Path
        {
            get => (string) GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public string ButtonText
        {
            get => (string) GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private static void PathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FolderSelector) d).NotifyPropertyChanged("Path");
        }

        private void SelectFileBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new OpenFileDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                Path = folderBrowserDialog.FileName;
        }
    }
}