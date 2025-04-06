namespace Final;

using System.Net.Http.Headers;
using Final.Domain;

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
        return RefuelEvents.OrderByDescending(e => e.EventDateTime);
    }

    public void EditRefuelEvent(Guid id, DateTime? eventDateTime=null, double? fuelAdded=null, double? cost=null, double? odometer=null)
    {
        var fuelEvent = RefuelEvents.FirstOrDefault(i => i.Id == id) ?? null;

        if (fuelEvent != null){
            var newRefuelEvent = new RefuelEvent(
                eventDateTime??fuelEvent.EventDateTime,
                fuelAdded??fuelEvent.FuelAdded,
                odometer??fuelEvent.Odometer,
                cost??fuelEvent.Cost,
                id
            );
            DeleteRefuelEvent(id);
            AddRefuelEvent(newRefuelEvent);
        }
    }

    public RefuelEvent GetRefuelEventById(Guid id)
    {
        var refuelEvent = (RefuelEvents.FirstOrDefault(i => i.Id == id) ?? null) 
            ?? throw new Exception($"Could not find refuel event with Guid: {id}");
        return refuelEvent;
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

    public IEnumerable<MaintenanceEvent> GetMaintenanceEvents()
    {
        return MaintenanceEvents.OrderByDescending(e => e.EventDateTime);
    }

    public void EditMaintenanceEvent(Guid id, DateTime? eventDateTime=null, string? maintenanceType=null, double? cost=null, double? odometer=null)
    {
        var maintenanceEvent = MaintenanceEvents.FirstOrDefault(i => i.Id == id) ?? null;

        if (maintenanceEvent != null){
            var newMaintenanceEvent = new MaintenanceEvent(
                eventDateTime: eventDateTime??maintenanceEvent.EventDateTime,
                maintenanceType: maintenanceType??maintenanceEvent.MaintenanceType,
                odometer: odometer??maintenanceEvent.Odometer,
                cost: cost??maintenanceEvent.Cost,
                id: id
            );
            DeleteMaintenanceEvent(id);
            AddMaintenanceEvent(newMaintenanceEvent);
        }
    }

    public MaintenanceEvent GetMaintenanceEventById(Guid id)
    {
        var maintenanceEvent = (MaintenanceEvents.FirstOrDefault(i => i.Id == id) ?? null) 
            ?? throw new Exception($"Could not find maintenance event with Guid: {id}");
        return maintenanceEvent;
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

    

    public IEnumerable<ReminderEvent> GetReminderEvents()
    {
        return ReminderEvents.OrderByDescending(e => e.ReminderTime);
    }

    public void EditReminderEvent(Guid id, DateTime? eventDateTime=null, string? reminderText=null, DateTime? reminderTime=null, bool? isSilenced=null)
    {
        var reminderEvent = ReminderEvents.FirstOrDefault(i => i.Id == id) ?? null;

        if (reminderEvent != null){
            var newReminderEvent = new ReminderEvent(
                eventDateTime: eventDateTime??reminderEvent.EventDateTime,
                reminderText: reminderText??reminderEvent.ReminderText,
                reminderTime: reminderTime??reminderEvent.ReminderTime,
                isSilenced: isSilenced??reminderEvent.IsSilenced,
                id: id
            );
            DeleteReminderEvent(id);
            AddReminderEvent(newReminderEvent);
        }
    }

    public ReminderEvent GetReminderEventById(Guid id)
    {
        var reminderEvent = (ReminderEvents.FirstOrDefault(i => i.Id == id) ?? null) 
            ?? throw new Exception($"Could not find reminder event with Guid: {id}");
        return reminderEvent;
    }

    public void TurnOffReminderAlarm(Guid id)
    {
        var targetEvent = (ReminderEvents.FirstOrDefault(e => e.Id == id) ?? null)
            ?? throw new Exception($"Could not find reminder event with Guid: {id}");
        
        targetEvent.TurnOffReminder();
        SynchronizeData();
    }

    public void ToggleReminderAlarm(Guid id)
    {
        var targetEvent = (ReminderEvents.FirstOrDefault(e => e.Id == id) ?? null)
            ?? throw new Exception($"Could not find reminder event with Guid: {id}");
        
        targetEvent.ToggleReminder();
        SynchronizeData();
    }
}