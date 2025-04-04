namespace Final;

using System.IO;
using System.Collections.Generic;

public class FileManager
{
    string FileName;

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

    public void AppendLine(string line)
    {
        File.AppendAllText(FileName, line + Environment.NewLine);
    }

}