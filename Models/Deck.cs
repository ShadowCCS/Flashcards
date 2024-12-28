using System.Collections.Generic;

public class Deck
{
    public string Name { get; set; }

    public Guid Id { get; set; }
    public string IconBase64 { get; set; }
    public List<Flashcard> Cards { get; set; } = new List<Flashcard>();

    public int NumberOfCards => Cards?.Count ?? 0;

    public Deck()
    {
        Id = Guid.NewGuid();
    }
}
