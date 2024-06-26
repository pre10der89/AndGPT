﻿using System.Text;
using AndGPT.Core.Contracts.Services;
using Newtonsoft.Json;

#pragma warning disable CS8603 // Possible null reference return.

namespace AndGPT.Core.Services;

public class FileService : IFileService
{
    public string ReadAllText(string path)
    {
        return File.Exists(path) ? File.ReadAllText(path) : default;
    }

    public string ReadAllText(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        
        return ReadAllText(path);
    }

    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }
}
