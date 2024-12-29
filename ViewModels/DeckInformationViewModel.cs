using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FlashcardsMVP.Services;
using FlashcardsMVP.ViewModels;
using FlashcardsMVP.Views;
using FlashcardsMVP.Logs;

namespace FlashcardsMVP.ViewModels
{
    public class DeckInformationViewModel : BaseViewModel
    {
        private readonly Deck _deck;
        private readonly MyFlashcardsViewModel _parentViewModel;
        private readonly DeckManager _deckManager;

        // Properties
        public string DeckName { get; }
        public int NumberOfCards { get; }
        public ObservableCollection<Flashcard> Cards { get; }

        // Commands
        public ICommand EditDeckCommand { get; }
        public ICommand DeleteDeckCommand { get; }
        public ICommand ExportDeckCommand { get; }

        // Constructor
        public DeckInformationViewModel(Deck deck, MyFlashcardsViewModel parentViewModel, DeckManager deckManager = null)
        {
            _deck = deck ?? throw new ArgumentNullException(nameof(deck));
            _parentViewModel = parentViewModel ?? throw new ArgumentNullException(nameof(parentViewModel));
            _deckManager = deckManager ?? new DeckManager();

            DeckName = _deck.Name;
            NumberOfCards = _deck.Cards.Count;
            Cards = new ObservableCollection<Flashcard>(_deck.Cards);

            EditDeckCommand = new RelayCommand(EditDeck);
            DeleteDeckCommand = new RelayCommand(DeleteDeck);
            ExportDeckCommand = new RelayCommand(ExportDeck);
        }

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
        private void ExportDeck()
        {
            try
            {
                _deckManager.ExportDeck(_deck.Name);
                Log.Write($"Deck '{_deck.Name}' was successfully exported.");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to export deck '{_deck.Name}': {ex.Message}");
            }
        }

        private void DeleteDeck()
        {
            try
            {
                _deckManager.DeleteDeck(_deck.Name);
                Log.Write($"Deck '{_deck.Name}' was successfully deleted.");
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to delete deck '{_deck.Name}': {ex.Message}");
            }
        }
    }
}
