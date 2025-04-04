using Microsoft.VisualBasic;

namespace Final;

public class DataManager
{

    private string _refuelFilePath = "refuel-data.txt";
    private string _maintenanceFilePath = "maintenance-data.txt";
    private string _reminderFilePath = "reminder-data.txt";

    public List<RefuelEvent> RefuelEvents { get; }
    public List<MaintenanceEvent> MaintenanceEvents { get; }
    public List<ReminderEvent> ReminderEvents { get; }

    public DataManager()
    {
        var refuelFile = new FileManager(_refuelFilePath);
        var maintenanceFile = new FileManager(_maintenanceFilePath);
        var reminderFilePath = new FileManager(_reminderFilePath);

        throw new NotImplementedException();
    }

    private List<VariantType> UnpackData()
    {
        throw new NotImplementedException();
    }
}