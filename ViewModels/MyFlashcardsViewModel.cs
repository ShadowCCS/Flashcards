using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using FlashcardsMVP;
using FlashcardsMVP.Logs;

namespace FlashcardsMVP.ViewModels
{
    public class MyFlashcardsViewModel : INotifyPropertyChanged
    {

        private Deck _selectedDeck;
        private bool _noDecksFound;

        private string flashcardDirectory = "Flashcards";
        // List of all the decks
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

        // The deck that is selected by the user
        public Deck SelectedDeck
        {
            get => _selectedDeck;
            set
            {
                if (_selectedDeck != value)
                {
                    _selectedDeck = value;
                    OnPropertyChanged(nameof(SelectedDeck));
                    OnPropertyChanged(nameof(IsDeckSelected)); // Update the visibility when the deck is selected
                }
            }
        }



        private Visibility _decksVisibility;
        public Visibility DecksVisibility
        {
            get { return _decksVisibility; }
            set
            {
                _decksVisibility = value;
                OnPropertyChanged(nameof(DecksVisibility));
            }
        }

        // Boolean property to track whether decks are found
        public bool IsDecksFound => Decks != null && Decks.Count > 0;



        // Property to handle visibility of "No Decks Found" message
        public bool NoDecksFound
        {
            get { return _noDecksFound; }
            set
            {
                if (_noDecksFound != value)
                {
                    _noDecksFound = value;
                    OnPropertyChanged(nameof(NoDecksFound));
                    OnPropertyChanged(nameof(IsDeckSelected));
                    UpdateDecksVisibility();
                }
            }
        }
        public bool IsDeckSelected => SelectedDeck != null;


        private void UpdateDecksVisibility()
        {
            // If there are no decks or if NoDecksFound is true, hide the ListBox and show the "No Decks Found" message.
            DecksVisibility = (Decks != null && Decks.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private FileSystemWatcher _fileSystemWatcher;

        // Constructor to load the decks
        public MyFlashcardsViewModel()
        {
            Log.Clean();
            Log.Write("Log Cleaned");
            Decks = new ObservableCollection<Deck>(); // Initialize Decks collection
            LoadDecksAsync();
            SetupFileSystemWatcher();
        }

        private void SetupFileSystemWatcher()
        {
            string resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), flashcardDirectory);

            if (Directory.Exists(resourcesPath))
            {
                _fileSystemWatcher = new FileSystemWatcher(resourcesPath)
                {
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                    Filter = "*.fcs"
                };

                _fileSystemWatcher.Created += (sender, e) =>
                {
                    Log.Write($"File created: {e.FullPath}");  // Log when a new file is created
                    LoadDecksAsync();
                };
                _fileSystemWatcher.Deleted += (sender, e) =>
                {
                    Log.Write($"File deleted: {e.FullPath}");  // Log when a file is deleted
                    LoadDecksAsync();
                };
                _fileSystemWatcher.Changed += (sender, e) =>
                {
                    Log.Write($"File changed: {e.FullPath}");  // Log when a file is modified
                    LoadDecksAsync();
                };
                _fileSystemWatcher.Renamed += (sender, e) =>
                {
                    Log.Write($"File renamed: {e.OldFullPath} to {e.FullPath}");  // Log when a file is renamed
                    LoadDecksAsync();
                };

                _fileSystemWatcher.EnableRaisingEvents = true;
            }
        }



        private async Task LoadDecksAsync()
        {
            Log.Write("LoadDecksAsync started");

            var newDecks = new List<Deck>();
            NoDecksFound = false;

            await Task.Run(() =>
            {
                Log.Write("Task.Run started");

                string resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), flashcardDirectory);

                if (Directory.Exists(resourcesPath))
                {
                    Log.Write($"Directory exists: {resourcesPath}");
                    var files = Directory.GetFiles(resourcesPath, "*.fcs");
                    foreach (var file in files)
                    {
                        try
                        {
                            var deck = DeckParser.ParseDeck(file); // Simulate time-consuming operation
                            newDecks.Add(deck);
                            Log.Write($"Deck loaded: {deck.Name}");
                        }
                        catch (Exception ex)
                        {
                            Log.Write($"Error loading deck from {file}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Log.Write("Resources directory not found");
                }
                Log.Write("Task.Run completed");
            });

            Log.Write("Before Dispatcher.Invoke");
            Application.Current.Dispatcher.Invoke(() =>
            {
                Decks.Clear(); // Clear existing items
                foreach (var deck in newDecks)
                {
                    Decks.Add(deck); // Add new items
                    Log.Write($"Added deck to Decks: {deck.Name}");
                }
                NoDecksFound = Decks.Count == 0; // Update "NoDecksFound" visibility
                Log.Write($"NoDecksFound updated to: {NoDecksFound}");
            });

            Log.Write("LoadDecksAsync completed");
        }





        // INotifyPropertyChanged implementation to notify the view about property changes
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StopWatching()
        {
            _fileSystemWatcher?.Dispose();
        }



    }
}