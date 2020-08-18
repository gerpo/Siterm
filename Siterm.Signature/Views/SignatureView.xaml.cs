using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using Siterm.Signature.Models;
using Siterm.Signature.ViewModels;
using Siterm.Support.Misc;

namespace Siterm.Signature.Views
{
    public partial class SignatureView : MetroWindow, IActivable
    {
        public SignatureView(SignatureViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public Task ActivateAsync(object parameter)
        {
            if (!(parameter is SignatureBundle signatureBundle)) return Task.CompletedTask;

            ((SignatureViewModel) DataContext).UserDraft = signatureBundle.UserDraft;
            ((SignatureViewModel) DataContext).SignatureCause = signatureBundle.Cause;

            return Task.CompletedTask;
        }

        private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ConfirmSignature_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}