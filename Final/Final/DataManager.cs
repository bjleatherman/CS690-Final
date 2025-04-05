using System.Diagnostics.Tracing;

namespace Final;

public class DataManager
{
    private readonly FileManager _refuelFile;
    private readonly FileManager _maintenanceFile;
    private readonly FileManager _reminderFile;

    private List<RefuelEvent> RefuelEvents { get; }
    private List<MaintenanceEvent> MaintenanceEvents { get; }
    private List<ReminderEvent> ReminderEvents { get; }

    public DataManager(string refuelFilePath, string maintenanceFilePath, string reminderFilePath)
    {
         _refuelFile = new FileManager(refuelFilePath);
         _maintenanceFile = new FileManager(maintenanceFilePath);
         _reminderFile = new FileManager(reminderFilePath);

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
    private void AddEvent<T>(List<T> events, T newEvent) where T : Event
    {
        events.Add(newEvent);
        SynchronizeData();
    }

    // Delete Events
    private void DeleteEvent<T>(List<T> events, Guid id) where T : Event
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

    public IEnumerable<RefuelEvent> GetRefuelEvents()
    {
        return RefuelEvents.AsReadOnly();
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