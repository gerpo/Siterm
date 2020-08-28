using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Siterm.Instructions.Models;
using Siterm.Support.Misc;

namespace Siterm.WPF.Controls
{
    public partial class UserInputBox
    {
        public static readonly DependencyProperty UserDraftProperty = DependencyProperty.Register(
            nameof(UserDraft),
            typeof(UserDraft),
            typeof(UserInputBox)
        );

        public static readonly DependencyProperty UserEmailListProperty = DependencyProperty.Register(
            nameof(UserEmailList),
            typeof(IEnumerable<string>),
            typeof(UserInputBox)
        );

        public UserInputBox()
        {
            InitializeComponent();
            InputTextBox.TextChanged += OnTextChanged;
        }

        public UserDraft UserDraft
        {
            get => (UserDraft) GetValue(UserDraftProperty);
            set => SetValue(UserDraftProperty, value);
        }

        public IEnumerable<string> UserEmailList
        {
            get => (IEnumerable<string>) GetValue(UserEmailListProperty);
            set => SetValue(UserEmailListProperty, value);
        }

        private void ActivateUserInput()
        {
            UserFirstName.IsEnabled = true;
            UserLastName.IsEnabled = true;
        }

        private void ClearUser()
        {
            UserFirstName.Text = string.Empty;
            UserLastName.Text = string.Empty;
        }

        private void DeactivateUserInput()
        {
            UserFirstName.IsEnabled = false;
            UserLastName.IsEnabled = false;
        }

        private void FillUserForHome(string inputText)
        {
            var cleanText = inputText.Trim().Split(new[] {"@", "@"}, StringSplitOptions.RemoveEmptyEntries)[0];
            var nameSplit = cleanText.Split('.');

            UserDraft.Email = $"{cleanText}@{UiStrings.HomeEmailEnding}";

            if (nameSplit.Length < 2) return;
            UserDraft.FirstName = nameSplit[0];
            UserDraft.LastName = nameSplit[1];
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is AutoCompleteTextBox inputTextBox)) return;

            var inputText = inputTextBox.Text.Trim();

            DeactivateUserInput();
            ClearUser();

            switch (true)
            {
                case var _ when inputText.EndsWith("@@", StringComparison.CurrentCulture) &&
                                inputText.Split('.').Length == 2:
                    FillUserForHome(inputText);
                    break;
                case var _ when Helper.IsValidEmail(inputText):
                    TryFillUser(inputText);
                    break;
                default:
                    ClearUser();
                    break;
            }
        }

        private void TryFillUser(string inputText)
        {
            ActivateUserInput();

            var nameSplit = inputText.Split('@')[0].Split('.');

            switch (nameSplit.Length)
            {
                case 1:
                    UserLastName.Text = nameSplit[0];
                    break;
                case var _ when nameSplit.Length >= 2:
                    UserFirstName.Text = nameSplit[0];
                    UserLastName.Text = nameSplit[1];
                    break;
            }
        }
    }
}