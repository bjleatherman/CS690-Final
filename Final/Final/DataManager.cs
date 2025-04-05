using System.Diagnostics.Tracing;

namespace Final;

public class DataManager
{

    private readonly string _refuelFilePath = "refuel-data.txt";
    private readonly string _maintenanceFilePath = "maintenance-data.txt";
    private readonly string _reminderFilePath = "reminder-data.txt";

    private readonly FileManager _refuelFile;
    private readonly FileManager _maintenanceFile;
    private readonly FileManager _reminderFile;

    private List<RefuelEvent> RefuelEvents { get; }
    private List<MaintenanceEvent> MaintenanceEvents { get; }
    private List<ReminderEvent> ReminderEvents { get; }

    public DataManager()
    {
         _refuelFile = new FileManager(_refuelFilePath);
         _maintenanceFile = new FileManager(_maintenanceFilePath);
         _reminderFile = new FileManager(_reminderFilePath);

        RefuelEvents = _refuelFile.GetData(RefuelEvent.UnpackCsvLine);
        MaintenanceEvents = _maintenanceFile.GetData(MaintenanceEvent.UnpackCsvLine);
        ReminderEvents = _reminderFile.GetData(ReminderEvent.UnpackCsvLine);
    }

    public void SynchronizeData() 
    {
        _refuelFile.ReplaceFileData(RefuelEvents);
        _maintenanceFile.ReplaceFileData(MaintenanceEvents);
        _reminderFile.ReplaceFileData(ReminderEvents);
    }

    // Add Events
    public void AddEvent<T>(List<T> events, T newEvent) where T : Event
    {
        events.Add(newEvent);
        SynchronizeData();
    }

    // Delete Events
    public void DeleteEvent<T>(List<T> events, Guid id) where T : Event
    {
        var eventToDelete = events.FirstOrDefault(e => e.Id == id);

        if (eventToDelete != null)
        {
            events.Remove(eventToDelete);
            SynchronizeData();
        }
    }

    // Refuel Events
    public void AddRefuelEvent(RefuelEvent refuelEvent)
    {
        AddEvent(RefuelEvents, refuelEvent);
    }

    public void DeleteRefuelEvent(Guid id) 
    {
        DeleteEvent(RefuelEvents, id);
    }

    // Maintenance Events
    public void AddMaintenanceEvent(MaintenanceEvent maintenanceEvent)
    {
        AddEvent(MaintenanceEvents, maintenanceEvent);
    }
    
    public void DeleteMaintenanceEvent(Guid id)
    {
        DeleteEvent(MaintenanceEvents, id);
    }

    // Reminder Events
    public void AddReminderEvent(ReminderEvent reminderEvent)
    {
        AddEvent(ReminderEvents, reminderEvent);
    }

    public void DeleteReminderEvent(Guid id)
    {
        DeleteEvent(ReminderEvents, id);
    }

    public void TurnOffReminderAlarm(Guid id)
    {
        var targetEvent = ReminderEvents.FirstOrDefault(e => e.Id == id) ?? null;
        if(targetEvent != null)
        {
            targetEvent.TurnOffReminder();
            SynchronizeData();
        } 
        else
        {
            throw new Exception($"Could not find Guid: {Convert.ToString(id)} in Reminder Events.");
        }
    }
}