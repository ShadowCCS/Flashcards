using System.IO;

public static class DeckParser
{
    public static Deck ParseDeck(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Deck file not found.");

        var lines = File.ReadAllLines(filePath);

        if (lines[0].Trim() != "[FCS1.0]")
            throw new InvalidDataException("Invalid file format. Expected [FCS1.0].");

        var header = new Dictionary<string, string>();
        var index = new Dictionary<string, int>();
        var content = new List<string>();
        int i = 1;

        while (i < lines.Length)
        {
            string line = lines[i].Trim();

            if (line.StartsWith("Header:"))
            {
                i++;
                while (i < lines.Length && !lines[i].Trim().StartsWith("Index:"))
                {
                    ParseKeyValue(lines[i++], header);
                }
            }
            else if (line.StartsWith("Index:"))
            {
                i++;
                while (i < lines.Length && !lines[i].Trim().StartsWith("Content:"))
                {
                    ParseKeyValue(lines[i++], index); // This works for numeric index values
                }
            }
            else if (line.StartsWith("Content:"))
            {
                i++;
                while (i < lines.Length)
                {
                    content.Add(lines[i++].Trim());
                }
            }
            else
            {
                i++;
            }
        }

        var deck = new Deck
        {
            Name = GetContentValue(content, "DeckName"),
            //IconBase64 = GetContentValue(content, "Icon"), Missing Implementation
            Cards = ParseCards(content)
        };

        return deck;
    }

    private static void ParseKeyValue(string line, IDictionary<string, string> dict)
    {
        if (string.IsNullOrWhiteSpace(line)) return;

        var parts = line.Split(':', 2);
        if (parts.Length == 2)
        {
            dict[parts[0].Trim()] = parts[1].Trim();
        }
    }

    private static void ParseKeyValue(string line, IDictionary<string, int> dict)
    {
        if (string.IsNullOrWhiteSpace(line)) return;

        var parts = line.Split(':', 2);
        if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int result))
        {
            dict[parts[0].Trim()] = result; // Handle the numeric index values
        }
    }

    private static string GetContentValue(List<string> content, string key)
    {
        foreach (var line in content)
        {
            if (line.StartsWith($"{key}:"))
            {
                return line.Substring($"{key}:".Length).Trim().Trim('"');
            }
        }
        return null;
    }

    private static List<Flashcard> ParseCards(List<string> content)
    {
        var cards = new List<Flashcard>();
        bool cardsSection = false;

        foreach (var line in content)
        {
            if (line.StartsWith("Cards:"))
            {
                cardsSection = true;
                continue;  // Skip the line starting with "Cards:"
            }

            if (cardsSection)
            {
                // Check if line starts with "[" and ends with "]"
                if (line.StartsWith("[") && line.EndsWith("],"))
                {
                    // Remove the leading "[" and trailing "]" and remove commas
                    var cleanedLine = line.Trim('[', ']', ',').Trim();

                    // Split the line into front and back
                    var cardData = cleanedLine.Split(',');

                    if (cardData.Length == 2)
                    {
                        // Remove extra quotes if they exist around the front/back data
                        var front = cardData[0].Trim('"');
                        var back = cardData[1].Trim('"');

                        // Add the card to the list
                        cards.Add(new Flashcard
                        {
                            Front = front,
                            Back = back
                        });
                    }
                }
            }
        }

        return cards;
    }

}
