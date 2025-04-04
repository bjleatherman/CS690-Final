using Microsoft.VisualBasic;

namespace Final;

public class DataManager
{

    private string _refuelFilePath = "refuel-data.txt";
    private string _maintenanceFilePath = "maintenance-data.txt";
    private string _reminderFilePath = "reminder-data.txt";

    private FileManager _refuelFile;
    private FileManager _maintenanceFile;
    private FileManager _reminderFile;

    public List<RefuelEvent> RefuelEvents { get; }
    public List<MaintenanceEvent> MaintenanceEvents { get; }
    public List<ReminderEvent> ReminderEvents { get; }

    public DataManager()
    {
         _refuelFile = new FileManager(_refuelFilePath);
         _maintenanceFile = new FileManager(_maintenanceFilePath);
         _reminderFile = new FileManager(_reminderFilePath);

        RefuelEvents = _refuelFile.GetData(RefuelEvent.UnpackCsvLine);
        MaintenanceEvents = _maintenanceFile.GetData(MaintenanceEvent.UnpackCsvLine);
        ReminderEvents = _reminderFile.GetData(ReminderEvent.UnpackCsvLine);
    }
}