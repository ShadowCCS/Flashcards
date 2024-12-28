using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FlashcardsMVP.Logs;
using FlashcardsMVP.Services;

namespace FlashcardsMVP.ViewModels
{
    public class DeckListViewModel : BaseViewModel
    {
        private ObservableCollection<Deck> _decks;
        public ObservableCollection<Deck> Decks
        {
            get => _decks;
            set
            {
                _decks = value;
                OnPropertyChanged(nameof(Decks));
            }
        }

        private bool _noDecksFound;
        public bool NoDecksFound
        {
            get => _noDecksFound;
            set
            {
                _noDecksFound = value;
                OnPropertyChanged(nameof(NoDecksFound));
            }
        }

        private Visibility _decksVisibility;
        public Visibility DecksVisibility
        {
            get => _decksVisibility;
            set
            {
                _decksVisibility = value;
                OnPropertyChanged(nameof(DecksVisibility));
            }
        }

        public DeckListViewModel()
        {
            // Initialize the collection of decks
            Decks = new ObservableCollection<Deck>();
            LoadDecksAsync();
        }

        private async Task LoadDecksAsync()
        {
            Log.Write("LoadDecksAsync started");

            var newDecks = new List<Deck>();
            NoDecksFound = false;

            await Task.Run(() =>
            {
                string resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Flashcards");

                if (Directory.Exists(resourcesPath))
                {
                    var files = Directory.GetFiles(resourcesPath, "*.fcs");
                    foreach (var file in files)
                    {
                        try
                        {
                            var deck = DeckParser.ParseDeck(file);  // Assuming DeckParser is a utility that reads and creates Deck objects.
                            newDecks.Add(deck);
                            Log.Write($"Deck loaded: {deck.Name}");
                        }
                        catch (Exception ex)
                        {
                            Log.Write($"Error loading deck from {file}: {ex.Message}");
                        }
                    }
                }
            });

            // Update the UI on the main thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                Decks.Clear();
                foreach (var deck in newDecks)
                {
                    Decks.Add(deck);
                }

                NoDecksFound = Decks.Count == 0; // Update the "No Decks Found" message visibility
            });
        }
    }
}
