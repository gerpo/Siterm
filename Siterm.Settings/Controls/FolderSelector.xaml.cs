using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace Siterm.Settings.Controls
{
    public partial class FolderSelector : INotifyPropertyChanged
    {
        public static DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(FolderSelector),
                new PropertyMetadata("Folder Selector"));

        public static DependencyProperty PathProperty =
            DependencyProperty.Register(nameof(Path), typeof(string), typeof(FolderSelector),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PathPropertyChanged));

        public static DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(FolderSelector),
                new PropertyMetadata("..."));

        public FolderSelector()
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

        private void SelectFolderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                Path = folderBrowserDialog.SelectedPath;
        }
    }
}