using FlashcardsMVP.Services;
using FlashcardsMVP.ViewModels;
using System;
using System.Windows.Input;

namespace FlashcardsMVP.ViewModels.Extras
{
    public class ConfirmationBoxViewModel : BaseViewModel
    {
        private string _title;
        private string _message;
        private string _confirmButtonText;
        private string _cancelButtonText;
        private ICommand _confirmCommand;
        private ICommand _cancelCommand;
        private bool _result;
        private string _confirmButtonColor;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string ConfirmButtonText
        {
            get => _confirmButtonText;
            set => SetProperty(ref _confirmButtonText, value);
        }

        public string CancelButtonText
        {
            get => _cancelButtonText;
            set => SetProperty(ref _cancelButtonText, value);
        }

        // Button color property
        public string ConfirmButtonColor
        {
            get => _confirmButtonColor ?? "#448E64"; // Default color (Green)
            set => SetProperty(ref _confirmButtonColor, value);
        }

        public ICommand ConfirmCommand => _confirmCommand ??= new RelayCommand(OnConfirm);
        public ICommand CancelCommand => _cancelCommand ??= new RelayCommand(OnCancel);

        // Method to handle confirm action
        private void OnConfirm()
        {
            _result = true;
            CloseWindow();
        }

        // Method to handle cancel action
        private void OnCancel()
        {
            _result = false;
            CloseWindow();
        }

        // Property to access the result of the dialog
        public bool Result => _result;

        // Method to close the window (this can be called when the user clicks on a button)
        public Action CloseWindow { get; set; }

        // Method to initialize the dialog with parameters
        public void Initialize(string title, string message, string confirmButtonText, string cancelButtonText, string confirmButtonColor = null)
        {
            Title = title;
            Message = message;
            ConfirmButtonText = confirmButtonText;
            CancelButtonText = cancelButtonText;

            // Check if a color argument is provided, and set the color accordingly
            if (string.Equals(confirmButtonColor, "Red", StringComparison.OrdinalIgnoreCase))
            {
                ConfirmButtonColor = "#C74A4C"; // Red color
            }
            else
            {
                ConfirmButtonColor = "#448E64"; // Default Green color
            }
        }
    }
}
