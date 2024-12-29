using System;
using System.IO;

namespace FlashcardsMVP.Services
{
    public class GetFilePath
    {
        private readonly string _resourcesPath;

        public GetFilePath()
        {
            _resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Flashcards");
        }

        public string GetDeckFilePath(string deckName)
        {
            return Path.Combine(_resourcesPath, $"{deckName}.fcs");
        }
    }
}
