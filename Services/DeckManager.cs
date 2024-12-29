using FlashcardsMVP.Logs;
using System;
using System.IO;

namespace FlashcardsMVP.Services
{
    public class DeckManager
    {
        private readonly DialogService _dialogService;
        private readonly GetFilePath _filePathService;

        public DeckManager()
        {
            _dialogService = new DialogService();
            _filePathService = new GetFilePath();
        }

        public void DeleteDeck(string deckName)
        {
            bool confirmDelete = _dialogService.ShowConfirmationDialog("Delete", "Are you sure you want to delete this item?", "Delete", "Cancel", "Red");

            if (confirmDelete)
            {
                try
                {
                    string deckFilePath = _filePathService.GetDeckFilePath(deckName);
                    if (File.Exists(deckFilePath))
                    {
                        File.Delete(deckFilePath);
                    }

                    Log.Write("Deck deleted successfully.");
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during deletion
                    Log.Error($"Error deleting deck: {ex.Message}");
                }
            }
        }
    }
}
