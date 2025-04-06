namespace Final.Domain;

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

    public override string PackToCsv()
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