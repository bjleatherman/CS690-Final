namespace Final;

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
        Console.WriteLine($"Fuel Added: {FuelAdded}");
        Console.WriteLine($"Odometer: {Odometer}");
        Console.WriteLine($"Cost: {Cost}");

        return string.Join(',', 
        Convert.ToString(EventDateTime), 
        Convert.ToString(FuelAdded), 
        Convert.ToString(Odometer), 
        Convert.ToString(Cost), 
        Convert.ToString(Id));
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