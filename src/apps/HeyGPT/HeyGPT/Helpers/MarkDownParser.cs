using System.Text.RegularExpressions;

namespace HeyGPT.App.Helpers;
public class MarkdownParser
{
    public static List<string> ParseMarkdown(string input)
    {
        var parts = new List<string>();
        var pattern = @"(\*\*[^*]+\*\*|__[^_]+__|\*[^*]+\*|_[^_]+_|\[.+\]\(.+\))";

        // Using Regex to split the input while keeping the delimiters (Markdown syntax)
        string[] splitInput = Regex.Split(input, pattern);

        foreach (var part in splitInput)
        {
            if (!string.IsNullOrEmpty(part))
            {
                parts.Add(part);
            }
        }

        return parts;
    }
}
