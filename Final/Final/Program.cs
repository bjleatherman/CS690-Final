namespace Final;

using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        FileManager.CreateDataDirectory();   
        string refuelFilePath = FileManager.GetFullFileName("refuel-file.txt");
        string maintenanceFilePath = FileManager.GetFullFileName("maintenance-file.txt");
        string reminderFilePath = FileManager.GetFullFileName("reminder-file.txt");

        var dataManager = new DataManager(refuelFilePath, maintenanceFilePath, reminderFilePath);
        var menu = new MenuSystem(dataManager);
        var consoleUi = new ConsoleUi(menu, dataManager);
        consoleUi.Show();
    }
}
