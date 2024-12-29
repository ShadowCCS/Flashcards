using FlashcardsMVP.Views.Extras;
using FlashcardsMVP.ViewModels.Extras;
using System;

namespace FlashcardsMVP.Services
{
    public class DialogService
    {
        public bool ShowConfirmationDialog(string title, string message, string confirmButtonText, string cancelButtonText, string confirmButtonColor = null)
        {
            // Create the ViewModel
            var confirmationViewModel = new ConfirmationBoxViewModel();

            // Initialize the ViewModel with the parameters
            confirmationViewModel.Initialize(title, message, confirmButtonText, cancelButtonText, confirmButtonColor);

            // Create the view
            var confirmationView = new ConfirmationBoxView
            {
                DataContext = confirmationViewModel
            };

            // Set the CloseWindow action to close only this confirmation window
            confirmationViewModel.CloseWindow = () => confirmationView.Close();

            // Open the dialog (ShowDialog is a modal window method)
            confirmationView.ShowDialog();

            // Return the result of the confirmation
            return confirmationViewModel.Result;
        }
    }
}
