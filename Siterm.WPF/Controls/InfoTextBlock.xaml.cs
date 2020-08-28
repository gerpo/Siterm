using System.Windows;

namespace Siterm.WPF.Controls
{
    public partial class InfoTextBlock
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(InfoTextBlock), new PropertyMetadata(default(string)));

        public InfoTextBlock()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}