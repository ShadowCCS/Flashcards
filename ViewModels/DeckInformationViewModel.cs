using System;
using System.Collections.ObjectModel;
using FlashcardsMVP.ViewModels; // Assuming Deck model exists

namespace FlashcardsMVP.ViewModels
{
    public class DeckInformationViewModel : BaseViewModel
    {
        private Deck _deck;

        public DeckInformationViewModel(Deck deck)
        {
            _deck = deck;
            DeckName = _deck.Name;
            NumberOfCards = _deck.Cards.Count;
            Cards = new ObservableCollection<Flashcard>(_deck.Cards);
        }

        public string DeckName { get; }
        public int NumberOfCards { get; }
        public ObservableCollection<Flashcard> Cards { get; }
    }


}
