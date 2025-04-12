namespace Final;

using System;
using System.IO;
using System.Collections.Generic;
using Final.Domain;
using System.IO.Enumeration;

public class FileManager
{
    string FileName;
    static readonly string _dataFolderPath = Path.Combine(AppContext.BaseDirectory, "data");

    public FileManager(string fileName)
    {
        FileName = fileName;
        if(!File.Exists(FileName))
        {
            File.Create(FileName).Close();
        }
    }

    public List<T> GetData<T>(Func<string, T> unpackCsv)
    {
        var list = new List<T>();

        foreach (string line in File.ReadLines(FileName))
        {
            list.Add(unpackCsv(line));
        }

        return list;
    }

    public void ReplaceFileData<T>(List<T> data) where T : ICsvSerializable
    {

        File.Delete(FileName);
        foreach (T item in data)
        {
            AppendLine(item.PackToCsv());
        }
    }

    private void AppendLine(string line)
    {
        File.AppendAllText(FileName, line + Environment.NewLine);
    }
    
    public static void CreateDataDirectory()
    {
        if (!Directory.Exists(_dataFolderPath)) { Directory.CreateDirectory(_dataFolderPath); }
    }

    public static string GetDataDirectoryPath()
    {
        return _dataFolderPath;
    }

    public static string GetFullFileName(string fileName)
    {
        return Path.Combine(_dataFolderPath, fileName);
    }
}