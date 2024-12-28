using System.Windows.Input;
using FlashcardsMVP.Services;
using FlashcardsMVP.Views;

namespace FlashcardsMVP.ViewModels
{
    public class DeckCreationMethodViewModel : BaseViewModel
    {
        private readonly MyFlashcardsViewModel _parentViewModel;

        // Command for creating a new deck
        public ICommand CreateOwnCommand { get; }

        public DeckCreationMethodViewModel(MyFlashcardsViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            CreateOwnCommand = new RelayCommand(CreateOwn);
        }

        // Logic for creating the own deck
        private void CreateOwn()
        {
            // Ensure that DeckManagerView is initialized correctly with proper ViewModel
            var deckManagerViewModel = new DeckManagerViewModel(null, _parentViewModel, isEditMode: false);
            var deckManagerView = new DeckManagerView
            {
                DataContext = deckManagerViewModel
            };

            // Update the parent ViewModel's CurrentView to show the DeckManagerView
            _parentViewModel.CurrentView = deckManagerView;
        }

    }
}
