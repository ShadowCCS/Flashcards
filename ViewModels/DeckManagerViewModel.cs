using FlashcardsMVP.Services;
using FlashcardsMVP.Views;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FlashcardsMVP.ViewModels
{
    public class DeckManagerViewModel : BaseViewModel
    {
        private readonly MyFlashcardsViewModel _parentViewModel;
        private string _deckName;
        private ObservableCollection<CardViewModel> _cards;

        public ICommand SaveCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand AddCardCommand { get; }
        public ICommand RemoveCardCommand { get; }

        public DeckManagerViewModel(Deck deck, MyFlashcardsViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;

            // Initialize deck properties
            DeckName = deck.Name;
            Cards = new ObservableCollection<CardViewModel>(deck.Cards.Select(c => new CardViewModel(c)));

            // Initialize commands
            SaveCommand = new RelayCommand(Save);
            GoBackCommand = new RelayCommand(GoBack);
            AddCardCommand = new RelayCommand(AddCard);
            RemoveCardCommand = new RelayCommand(RemoveCard);
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

        private void Save()
        {
            // Serialize changes back to the .fcs file
            string filePath = Path.Combine("Flashcards", $"{DeckName}.fcs");

            if (string.IsNullOrEmpty(DeckName))
            {
                MessageBox.Show("Deck name cannot be empty.");
                return;
            }

            using (var writer = new StreamWriter(filePath))
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

            // Notify parent view model to update the deck
            _parentViewModel.LoadDecksAsync();
            GoBack();
        }

        private void GoBack()
        {
            _parentViewModel.CurrentView = new DeckInformationView
            {
                DataContext = new DeckInformationViewModel(_parentViewModel.SelectedDeck, _parentViewModel)
            };
        }

        private void AddCard()
        {
            Cards.Add(new CardViewModel(new Flashcard { Front = "New Front", Back = "New Back" }));
        }

        private void RemoveCard(object card)
        {
            if (card is CardViewModel cardToRemove)
            {
                Cards.Remove(cardToRemove);
            }
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
}
