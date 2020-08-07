using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Siterm.WPF.Controls
{
    /// <summary>
    ///     Interaktionslogik für AutoCompleteTextBox.xaml
    /// </summary>
    public partial class AutoCompleteTextBox : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TextPropertyChanged)
        );

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            "Placeholder",
            typeof(string),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(null, null)
        );

        public static readonly DependencyProperty SuggestionListProperty = DependencyProperty.Register(
            "SuggestionList",
            typeof(IEnumerable<string>),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(null, null)
        );

        public AutoCompleteTextBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public IEnumerable<string> SuggestionList
        {
            get => (IEnumerable<string>)GetValue(SuggestionListProperty);
            set => SetValue(SuggestionListProperty, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public event TextChangedEventHandler TextChanged;

        private void AutoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AutoTextBox is null || AutoListPopup is null || AutoList is null) return;
            // Verification.  
            if (AutoList.SelectedIndex <= -1)
            {
                // Disable.  
                CloseAutoSuggestionBox();

                // Info.  
                return;
            }

            // Disable.  
            CloseAutoSuggestionBox();

            // Settings.  
            AutoTextBox.Text = AutoList.SelectedItem.ToString() ?? string.Empty;
            AutoList.SelectedIndex = -1;
        }


        private void AutoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AutoTextBox is null || AutoListPopup is null || AutoList is null) return;

            Text = AutoTextBox.Text;

            // Verification.  
            if (string.IsNullOrEmpty(AutoTextBox.Text))
            {
                // Disable.  
                CloseAutoSuggestionBox();

                // Info.  
                return;
            }

            // Enable.  
            OpenAutoSuggestionBox();

            // Settings.  
            AutoList.ItemsSource = SuggestionList?.Where(p =>
                    p.ToLower(CultureInfo.CurrentCulture)
                        .Contains(AutoTextBox.Text.ToLower(CultureInfo.CurrentCulture)))
                .ToList();

            TextChanged?.Invoke(this, e);
        }

        private void CloseAutoSuggestionBox()
        {
            AutoListPopup.Visibility = Visibility.Collapsed;
            AutoListPopup.IsOpen = false;
            AutoList.Visibility = Visibility.Collapsed;
        }

        private void OpenAutoSuggestionBox()
        {
            AutoListPopup.Visibility = Visibility.Visible;
            AutoListPopup.IsOpen = true;
            AutoList.Visibility = Visibility.Visible;
        }

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoCompleteTextBox) d).NotifyPropertyChanged("Text");
        }
    }
}