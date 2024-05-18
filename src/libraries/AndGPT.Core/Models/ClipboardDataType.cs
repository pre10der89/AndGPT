namespace AndGPT.Core.Models;

public enum ClipboardDataType
{
    None,       // No data available
    Text,       // Textual data
    Image,      // Image data
    Uri,        // URI data
    Binary,     // Binary data (files, etc.)
    Custom      // Custom data types not covered by the above categories
}
