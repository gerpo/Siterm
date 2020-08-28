using System.Windows;

namespace Siterm.WPF.Controls
{
    public partial class InfoTextPanel
    {
        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register(nameof(MainText), typeof(string), typeof(InfoTextPanel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty SubTextProperty =
            DependencyProperty.Register(nameof(SubText), typeof(string), typeof(InfoTextPanel),
                new PropertyMetadata(""));

        public InfoTextPanel()
        {
            InitializeComponent();
        }

        public string MainText
        {
            get => (string) GetValue(MainTextProperty);
            set => SetValue(MainTextProperty, value);
        }


        public string SubText
        {
            get => (string) GetValue(SubTextProperty);
            set => SetValue(SubTextProperty, value);
        }
    }
}