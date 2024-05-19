namespace AndGPT.Core.Helpers;

public static class ImageFormatDetector
{
    public static string DetectImageFormat(byte[] imageData)
    {
        ArgumentNullException.ThrowIfNull(imageData);

        if (imageData.Length < 4)
        {
            return "Unknown";
        }

        // Check for PNG
        if (imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47)
        {
            return "PNG";
        }

        // Check for JPEG
        if (imageData[0] == 0xFF && imageData[1] == 0xD8 && imageData[2] == 0xFF)
        {
            return "JPEG";
        }

        // Check for BMP
        if (imageData[0] == 0x42 && imageData[1] == 0x4D)
        {
            return "BMP";
        }

        // Add more formats as needed...

        return "Unknown";
    }
}
