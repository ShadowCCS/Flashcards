using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlashcardsMVP.Services;
using FlashcardsMVP.Views;
using FlashcardsMVP.Logs;

namespace FlashcardsMVP.ViewModels
{
    public class DeckManagerViewModel : BaseViewModel
    {
        private readonly MyFlashcardsViewModel _parentViewModel;
        private string _deckName;
        private ObservableCollection<CardViewModel> _cards;
        private bool _isEditMode;

        public ICommand SaveCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand AddCardCommand { get; }
        public ICommand RemoveCardCommand { get; }

        public ICommand DeleteDeckCommand { get; }

        public DeckManagerViewModel(Deck deck, MyFlashcardsViewModel parentViewModel, bool isEditMode = false)
        {
            _parentViewModel = parentViewModel;
            _isEditMode = isEditMode;

            if (_isEditMode && deck != null)
            {
                // Editing existing deck
                DeckName = deck.Name;
                Cards = new ObservableCollection<CardViewModel>(deck.Cards.Select(c => new CardViewModel(c)));
            }
            else
            {
                // Creating new deck
                DeckName = string.Empty;  // Empty name for new deck
                Cards = new ObservableCollection<CardViewModel>();
                AddCard();
            }

            SaveCommand = new RelayCommand(Save);
            GoBackCommand = new RelayCommand(GoBack);
            AddCardCommand = new RelayCommand(AddCard);
            RemoveCardCommand = new RelayCommand(RemoveCard);
            DeleteDeckCommand = new RelayCommand(DeleteDeck);
        }

        public string DeckName
        {
            get => _deckName;
            set
            {
                _deckName = value;
                OnPropertyChanged(nameof(DeckName));
            }
        }

        public ObservableCollection<CardViewModel> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropertyChanged(nameof(Cards));
            }
        }

        // Dynamic page title and button text based on edit/create mode
        public string PageTitle => _isEditMode ? "Edit Deck" : "Create a New Deck";
        public string SaveButtonText => _isEditMode ? "Save" : "Create";

        private Deck selectedDeck;
        private void Save()
        {
            // Ensure the deck name is provided
            if (string.IsNullOrEmpty(DeckName))
            {
                MessageBox.Show("Deck name cannot be empty.");
                return;
            }
            var filePathService = new GetFilePath();
            // Create or overwrite the .fcs file based on whether we are editing or creating
            string deckFilePath = filePathService.GetDeckFilePath(DeckName);
            if (_isEditMode && File.Exists(deckFilePath))
            {
                // Overwrite the existing deck file if in edit mode
                File.Delete(deckFilePath);
            }

            CreateDeckFile(deckFilePath);
            _parentViewModel.LoadDecksAsync();  // Refresh deck list

            selectedDeck = DeckParser.ParseDeck(deckFilePath);
            GoBack();
        }

        private void GoBack()
        {
            try
            {
                _parentViewModel.CurrentView = new DeckInformationView
                {
                    DataContext = new DeckInformationViewModel(selectedDeck, _parentViewModel)
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return;
            }
            // Go back to the previous view, either the deck information or deck list view
        }

        private void AddCard()
        {
            Cards.Add(new CardViewModel(new Flashcard { Front = "Front", Back = "Back" }));
        }

        private void RemoveCard(object card)
        {
            if (card is CardViewModel cardToRemove)
            {
                Cards.Remove(cardToRemove);
            }
        }

        public class CardViewModel : BaseViewModel
        {
            private string _front;
            private string _back;

            public CardViewModel(Flashcard card)
            {
                Front = card.Front;
                Back = card.Back;
            }

            public string Front
            {
                get => _front;
                set
                {
                    _front = value;
                    OnPropertyChanged(nameof(Front));
                }
            }

            public string Back
            {
                get => _back;
                set
                {
                    _back = value;
                    OnPropertyChanged(nameof(Back));
                }
            }
        }

        private void DeleteDeck()
        {
            var deckManager = new DeckManager();
            deckManager.DeleteDeck(DeckName);
        }

        private void CreateDeckFile(string deckFilePath)
        {
            // Create or overwrite the deck file
            using (var writer = new StreamWriter(deckFilePath))
            {
                writer.WriteLine("[FCS1.0]");
                writer.WriteLine("Header:");
                writer.WriteLine("    FileType: Flashcard");
                writer.WriteLine("    Version: 1.0");
                writer.WriteLine("    Checksum: CRC32");
                writer.WriteLine("Index:");
                writer.WriteLine("    DeckNameOffset: 128");
                writer.WriteLine("    CardsOffset: 256");
                writer.WriteLine("Content:");
                writer.WriteLine($"    DeckName: \"{DeckName}\"");
                writer.WriteLine("    Icon: <Base64>");
                writer.WriteLine("    Cards:");
                foreach (var card in Cards)
                {
                    writer.WriteLine($"        [\"{card.Front}\",\"{card.Back}\"],");
                }
            }
        }
    }
}
