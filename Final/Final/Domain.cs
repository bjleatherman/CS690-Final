namespace Final;

public class Event
{
    public DateTime EventDateTime;

    // Default ctor uses current datetime
    public Event() : this(DateTime.Now) { }

    // Overload for events added from files
    public Event(DateTime eventDateTime)
    {
        EventDateTime = eventDateTime;
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
        this.MaintenanceType = maintenanceType;
        this.Odometer = odometer;
        this.Cost = cost;
    }

    // Overload for event from file
    public MaintenanceEvent(DateTime eventDateTime, string maintenanceType, double odometer, double cost)
        : base(eventDateTime)
    {
        this.MaintenanceType = maintenanceType;
        this.Odometer = odometer;
        this.Cost = cost;
    }

    public static MaintenanceEvent UnpackCsvLine (string csvLine)
    {
        string[] data = csvLine.Split(',');

        var eventDateTime = DateTime.Parse(data[0]);
        string maintenanceType  = data[1];
        double odometer = double.Parse(data[2]);
        double cost = double.Parse(data[3]);

        return new MaintenanceEvent(eventDateTime, maintenanceType, odometer, cost);
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
    public RefuelEvent(DateTime eventDateTime, double fuelAdded, double odometer, double cost)
        : base(eventDateTime, "Refuel", odometer, cost)
    {
        this.FuelAdded = fuelAdded;

    }

    public new static RefuelEvent UnpackCsvLine(string csvLine)
    {
        string[] data = csvLine.Split(',');

        var eventDateTime = DateTime.Parse(data[0]);
        double fuelAdded = double.Parse(data[1]);
        double odometer = double.Parse(data[2]);
        double cost = double.Parse(data[3]);

        return new RefuelEvent(eventDateTime, fuelAdded, odometer, cost);
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
    public ReminderEvent(DateTime eventDateTime, string reminderText, DateTime reminderTime, bool isSilenced)
        : base(eventDateTime)
    {
        ReminderText = reminderText;
        ReminderTime = reminderTime;
        IsSilenced = isSilenced;
    }

    public static ReminderEvent UnpackCsvLine(string csvLine)
    {
        string[] data = csvLine.Split();

        var eventDateTime = DateTime.Parse(data[0]);
        string reminderText = data[1];
        var reminderTime = DateTime.Parse(data[2]);
        bool isSilenced = bool.Parse(data[3]);

        return new ReminderEvent(eventDateTime, reminderText, reminderTime, isSilenced);
    }
}