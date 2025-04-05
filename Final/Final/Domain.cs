using System.Data.Common;

namespace Final;

public class Event : ICsvSerializable
{
    public DateTime EventDateTime;
    public Guid Id;

    // Default ctor uses current datetime
    public Event()
    {
        EventDateTime = DateTime.Now;
        Id = new Guid();
    }

    // Overload for events added from files
    public Event(DateTime eventDateTime, Guid id)
    {
        EventDateTime = eventDateTime;
        Id = id;
    }

    public string PackToCsv()
    {
        return string.Join(',', Convert.ToString(EventDateTime), Convert.ToString(Id));
    }
}

public class MaintenanceEvent : Event
{
    public string MaintenanceType { get; set; }
    public double Odometer { get; set; }
    public double Cost { get; set; }

    // Ctor, event time = current time
    public MaintenanceEvent(string maintenanceType, double odometer, double cost)
    {
        MaintenanceType = maintenanceType;
        Odometer = odometer;
        Cost = cost;
    }

    // Overload for event from file
    public MaintenanceEvent(DateTime eventDateTime, string maintenanceType, double odometer, double cost, Guid id)
        : base(eventDateTime, id)
    {
        MaintenanceType = maintenanceType;
        Odometer = odometer;
        Cost = cost;
    }

    public new string PackToCsv()
    {
        return string.Join(',', EventDateTime, MaintenanceType, Odometer, Cost, Id);
    }

    public static MaintenanceEvent UnpackCsvLine (string csvLine)
    {
        string[] data = csvLine.Split(',');

        var eventDateTime = DateTime.Parse(data[0]);
        string maintenanceType  = data[1];
        double odometer = double.Parse(data[2]);
        double cost = double.Parse(data[3]);
        var id = Guid.Parse(data[4]);

        return new MaintenanceEvent(eventDateTime, maintenanceType, odometer, cost, id);
    }
}

public class RefuelEvent : MaintenanceEvent
{
    public double FuelAdded { get; set; }

    // Ctor, event time = current time
    public RefuelEvent(double fuelAdded, double odometer, double cost)
        : base("Refuel", odometer, cost)
    {
        this.FuelAdded = fuelAdded;

    }

    // Overload for event from file
    public RefuelEvent(DateTime eventDateTime, double fuelAdded, double odometer, double cost, Guid id)
        : base(eventDateTime, "Refuel", odometer, cost, id)
    {
        FuelAdded = fuelAdded;
    }
    
    public new string PackToCsv()
    {
        return string.Join(',', EventDateTime, FuelAdded, Odometer, Cost, Id);
    }

    public new static RefuelEvent UnpackCsvLine(string csvLine)
    {
        string[] data = csvLine.Split(',');

        var eventDateTime = DateTime.Parse(data[0]);
        double fuelAdded = double.Parse(data[1]);
        double odometer = double.Parse(data[2]);
        double cost = double.Parse(data[3]);
        var id = Guid.Parse(data[4]);

        return new RefuelEvent(eventDateTime, fuelAdded, odometer, cost, id);
    }
}

public class ReminderEvent : Event
{
    public string ReminderText { get; set; }
    public bool IsSilenced { get; set; }
    public DateTime ReminderTime { get; set; }

    // Ctor, event time = current time
    public ReminderEvent(string reminderText, DateTime reminderTime)
    {
        ReminderText = reminderText;
        ReminderTime = reminderTime;

        // Only set the alarm if the reminder time has not happened yet
        IsSilenced = DateTime.Now < reminderTime;
    }
    
    // Overload for event from file
    public ReminderEvent(DateTime eventDateTime, string reminderText, DateTime reminderTime, bool isSilenced, Guid id)
        : base(eventDateTime, id)
    {
        ReminderText = reminderText;
        ReminderTime = reminderTime;
        IsSilenced = isSilenced;
    }

    public new string PackToCsv()
    {
        return string.Join(',', EventDateTime, ReminderText, ReminderTime, IsSilenced, Id);
    }

    public static ReminderEvent UnpackCsvLine(string csvLine)
    {
        string[] data = csvLine.Split();

        var eventDateTime = DateTime.Parse(data[0]);
        string reminderText = data[1];
        var reminderTime = DateTime.Parse(data[2]);
        bool isSilenced = bool.Parse(data[3]);
        var id = Guid.Parse(data[4]);

        return new ReminderEvent(eventDateTime, reminderText, reminderTime, isSilenced, id);
    }

    public void TurnOffReminder()
    {
        IsSilenced = true;
    }
}