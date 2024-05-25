namespace AndGPT.Core.Contracts.Services;

public interface IFileService
{
    string ReadAllText(string path);

    string ReadAllText(string folderPath, string fileName);

    T Read<T>(string folderPath, string fileName);

    void Save<T>(string folderPath, string fileName, T content);

    void Delete(string folderPath, string fileName);

    public static (string directory, string fileName) GetDirectoryAndFileName(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        var directory = Path.GetDirectoryName(filePath);
        var fileName = Path.GetFileName(filePath);

        return (directory ?? string.Empty, fileName);
    }
}
