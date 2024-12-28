using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using FlashcardsMVP.Services;
using FlashcardsMVP.ViewModels;
using FlashcardsMVP.Views;

namespace FlashcardsMVP.ViewModels
{
    public class DeckInformationViewModel : BaseViewModel
    {
        private Deck _deck;
        private readonly MyFlashcardsViewModel _parentViewModel;

        // Command for editing the deck
        public ICommand EditDeckCommand { get; }

        // Constructor
        public DeckInformationViewModel(Deck deck, MyFlashcardsViewModel parentViewModel)
        {
            _deck = deck;
            DeckName = _deck.Name;
            NumberOfCards = _deck.Cards.Count;
            Cards = new ObservableCollection<Flashcard>(_deck.Cards);

            _parentViewModel = parentViewModel;

            // Initialize the EditDeckCommand and bind it to the EditDeck method
            EditDeckCommand = new RelayCommand(EditDeck);
        }

        // Properties
        public string DeckName { get; }
        public int NumberOfCards { get; }
        public ObservableCollection<Flashcard> Cards { get; }

        // Method to handle EditDeckCommand
        private void EditDeck()
        {
            // Create the DeckManagerView and bind the corresponding ViewModel
            var deckManagerView = new DeckManagerView
            {
                DataContext = new DeckManagerViewModel(_deck, _parentViewModel, isEditMode: true) // Pass isEditMode as true for editing
            };

            // Switch to DeckManagerView
            _parentViewModel.CurrentView = deckManagerView;
        }
    }
}
