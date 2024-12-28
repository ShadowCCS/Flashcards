using FlashcardsMVP.Services;
using System.Windows.Input;
using FlashcardsMVP.ViewModels;

public class EditDeckViewModel : BaseViewModel
{
    private Deck _selectedDeck;
    public Deck SelectedDeck
    {
        get => _selectedDeck;
        set
        {
            _selectedDeck = value;
            OnPropertyChanged(nameof(SelectedDeck));
        }
    }

    public ICommand SaveCommand { get; }

    public EditDeckViewModel(Deck selectedDeck)
    {
        SelectedDeck = selectedDeck;
        SaveCommand = new RelayCommand(SaveDeck);
    }

    private void SaveDeck()
    {
        // Logic to save the deck
    }
}
