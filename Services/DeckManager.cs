using FlashcardsMVP.Logs;
using System;
using System.IO;
using Microsoft.Win32;
using System.Windows;

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

        private void GetFolderDialog(string deckName)
        {
            // Open folder dialog for the user to select the destination folder
            var folderDialog = new OpenFolderDialog
            {
                Title = "Select Destination Folder"
            };

            if (folderDialog.ShowDialog() == true)
            {
                var folderName = folderDialog.FolderName;

                // Validate folder name before proceeding
                if (string.IsNullOrWhiteSpace(folderName))
                {
                    Log.Write("Invalid folder path selected.");
                    return;
                }

                CopyFileToDestination(folderName, deckName);
            }
            else
            {
                Log.Write("Folder selection was canceled.");
            }
        }


        private void CopyFileToDestination(string destinationPath, string deckName)
        {
            try
            {
                // Get the deck file path using the file path service
                string deckFilePath = _filePathService.GetDeckFilePath(deckName);
                if (string.IsNullOrWhiteSpace(deckFilePath) || !File.Exists(deckFilePath))
                {
                    Log.Error("The deck file does not exist or the path is invalid.");
                    return;
                }

                // Combine destination path and deck name to create the target file path
                string destinationFilePath = Path.Combine(destinationPath, deckName+".fcs");

                // Copy the file to the destination
                File.Copy(deckFilePath, destinationFilePath, overwrite: true);
                Log.Write($"Deck file successfully exported to: {destinationFilePath}");
            }
            catch (IOException ioEx)
            {
                Log.Error($"An I/O error occurred: {ioEx.Message}");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Log.Error($"Access denied: {uaEx.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void ExportDeck(string deckName)
        {
            // Validate deck name before proceeding
            if (string.IsNullOrWhiteSpace(deckName))
            {
                Log.Error("Invalid deck name provided.");
                return;
            }

            GetFolderDialog(deckName);
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
