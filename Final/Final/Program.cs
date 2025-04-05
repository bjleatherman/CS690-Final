namespace Final;

using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string baseDir = AppContext.BaseDirectory;
        string dataFolder = "data";
        string dataFolderPath = Path.Combine(baseDir, dataFolder);
        
        string refuelFileName = "refuel-file.txt";
        string maintenanceFileName = "maintenance-file.txt";
        string reminderFileName = "reminder-file.txt";

        if (!Directory.Exists(dataFolderPath)) { Directory.CreateDirectory(dataFolderPath); }
        
        string refuelFilePath = Path.Combine(dataFolderPath, refuelFileName);
        string maintenanceFilePath = Path.Combine(dataFolderPath, maintenanceFileName);
        string reminderFilePath = Path.Combine(dataFolderPath, reminderFileName);


        var dataManager = new DataManager(refuelFilePath, maintenanceFilePath, reminderFilePath);
        var menu = new MenuSystem(dataManager);
        var consoleUi = new ConsoleUi(menu, dataManager);
        consoleUi.Show();
    }
}
