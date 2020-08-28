using System.ComponentModel;
using System.Windows;
using Siterm.Substance.Models;

namespace Siterm.WPF.Controls
{
    public partial class SubstanceInfoPanel : INotifyPropertyChanged
    {
        public static readonly DependencyProperty SubstanceProperty = DependencyProperty.Register(
            nameof(Substance),
            typeof(Domain.Models.Substance),
            typeof(SubstanceInfoPanel),
            new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SubstancePropertyChanged)
        );

        public static readonly DependencyProperty SubstanceInfoProperty = DependencyProperty.Register(
            nameof(SubstanceInfo), 
            typeof(SubstanceInformation), 
            typeof(SubstanceInfoPanel), 
            new PropertyMetadata(default(SubstanceInformation)));

        public SubstanceInfoPanel()
        {
            InitializeComponent();
        }

        public Domain.Models.Substance Substance
        {
            get => (Domain.Models.Substance) GetValue(SubstanceProperty);
            set => SetValue(SubstanceProperty, value);
        }

        public bool HasSubstance => !(Substance is null);

        public SubstanceInformation SubstanceInfo
        {
            get => (SubstanceInformation) GetValue(SubstanceInfoProperty);
            set => SetValue(SubstanceInfoProperty, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void SubstancePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SubstanceInfoPanel) d).NotifyPropertyChanged(nameof(Substance));
            ((SubstanceInfoPanel) d).NotifyPropertyChanged(nameof(HasSubstance));
        }

        public void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}