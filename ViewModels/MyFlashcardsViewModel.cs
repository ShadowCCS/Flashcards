using System.Windows.Input;
using FlashcardsMVP.Services;
using FlashcardsMVP.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Diagnostics;
using FlashcardsMVP.Logs;
using FlashcardsMVP.Views;
using System.Windows.Controls;

namespace FlashcardsMVP.ViewModels
{
    public class MyFlashcardsViewModel : INotifyPropertyChanged
    {
        private Deck _selectedDeck;
        private bool _noDecksFound;
        private ObservableCollection<Deck> _decks;
        private string flashcardDirectory = "Flashcards"; // Flashcard directory
        private FileSystemWatcher _fileSystemWatcher;
        private UserControl _currentView;

        // CurrentView to bind to the ContentControl
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        // Property to hold all decks
        public ObservableCollection<Deck> Decks
        {
            get => _decks;
            set
            {
                _decks = value;
                OnPropertyChanged(nameof(Decks));
            }
        }

        // The selected deck
        public Deck SelectedDeck
        {
            get => _selectedDeck;
            set
            {
                if (_selectedDeck != value)
                {
                    _selectedDeck = value;
                    OnPropertyChanged(nameof(SelectedDeck));
                    OnPropertyChanged(nameof(IsDeckSelected));

                    Log.Write($"SelectedDeck changed: {_selectedDeck?.Name}");

                    // Set the UserControl (CurrentView) and bind the ViewModel (CurrentViewModel)
                    if (_selectedDeck != null)
                    {
                        CurrentView = new DeckInformationView { DataContext = new DeckInformationViewModel(_selectedDeck) };
                    }
                    else
                    {
                        CurrentView = null;  // Or reset the view if no deck is selected
                    }
                }
            }
        }

        // Check if a deck is selected
        public bool IsDeckSelected => SelectedDeck != null;

        // Property for "No Decks Found" message
        public bool NoDecksFound
        {
            get => _noDecksFound;
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

        // Constructor for MyFlashcardsViewModel
        public MyFlashcardsViewModel()
        {
            Decks = new ObservableCollection<Deck>();
            LoadDecksAsync();
            SetupFileSystemWatcher();

            // Commands
            LearnDeckCommand = new RelayCommand(LearnDeck, () => IsDeckSelected);
            EditDeckCommand = new RelayCommand(EditDeck, () => IsDeckSelected);
            ExportDeckCommand = new RelayCommand(ExportDeck, () => IsDeckSelected);
            DeleteDeckCommand = new RelayCommand(DeleteDeck, () => IsDeckSelected);
        }

        // File system watcher setup to detect file changes
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

                _fileSystemWatcher.Created += OnFileChanged;
                _fileSystemWatcher.Deleted += OnFileChanged;
                _fileSystemWatcher.Changed += OnFileChanged;
                _fileSystemWatcher.Renamed += OnFileChanged;

                _fileSystemWatcher.EnableRaisingEvents = true;
            }
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            LoadDecksAsync(); // Reload decks when a file is created, deleted, changed, or renamed
        }

        // Update the visibility of the decks
        private void UpdateDecksVisibility()
        {
            DecksVisibility = Decks != null && Decks.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            NoDecksFound = Decks == null || Decks.Count == 0;
        }

        // Command actions for Learn, Edit, Export, Delete
        public ICommand LearnDeckCommand { get; }
        public ICommand EditDeckCommand { get; }
        public ICommand ExportDeckCommand { get; }
        public ICommand DeleteDeckCommand { get; }

        // Handle Learn Deck Command
        private void LearnDeck()
        {
            // Implement logic for learning a deck
        }

        // Handle Edit Deck Command
        private void EditDeck()
        {
            // Implement logic for editing a deck
        }

        // Handle Export Deck Command
        private void ExportDeck()
        {
            // Implement logic for exporting a deck
        }

        // Handle Delete Deck Command
        private void DeleteDeck()
        {
            // Implement logic for deleting a deck
        }

        // Async method to load decks from the directory
        private async Task LoadDecksAsync()
        {
            var newDecks = new List<Deck>();
            NoDecksFound = false;

            await Task.Run(() =>
            {
                string resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), flashcardDirectory);

                if (Directory.Exists(resourcesPath))
                {
                    var files = Directory.GetFiles(resourcesPath, "*.fcs");
                    foreach (var file in files)
                    {
                        try
                        {
                            var deck = DeckParser.ParseDeck(file); // Load the deck file
                            newDecks.Add(deck);
                        }
                        catch (Exception ex)
                        {
                            // Log error if deck cannot be loaded
                        }
                    }
                }
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                Decks.Clear();
                foreach (var deck in newDecks)
                {
                    Decks.Add(deck);
                }
                NoDecksFound = Decks.Count == 0;
            });
        }

        // INotifyPropertyChanged implementation for property change notifications
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Stop the file system watcher when no longer needed
        public void StopWatching()
        {
            _fileSystemWatcher?.Dispose();
        }

        // Property to manage Decks visibility
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
    }
}
