namespace Final.Tests;

using Final.Domain;

public class DataManagerTest
{

    
    private static readonly string _testDirectory = "test-data";
    private readonly string _refuelFilePath = Path.Combine(_testDirectory, "refuel-file.txt");
    private readonly string _maintenanceFilePath = Path.Combine(_testDirectory, "maintenance-file.txt");
    private readonly string _reminderFilePath = Path.Combine(_testDirectory, "reminder-file.txt");
    private readonly DataManager _dm;

    public DataManagerTest()
    {
        _dm = new DataManager(_refuelFilePath,_maintenanceFilePath, _reminderFilePath);
    }

    // Refuel
    [Fact]
    public void Test_AddRefuelEvent()
    {
        var initialCount = _dm.GetRefuelEvents().Count();
        var refuelEvent = new RefuelEvent(10.5, 15000.0, 45.50);

        _dm.AddRefuelEvent(refuelEvent);
        var events = _dm.GetRefuelEvents();

        Assert.Equal(initialCount + 1, events.Count());
        Assert.Contains(events, e => e.Id == refuelEvent.Id);
    }

    [Fact]
    public void Test_DeleteRefuelEvent()
    {
        var refuelEvent = new RefuelEvent(10.5, 15000.0, 45.50);
        _dm.AddRefuelEvent(refuelEvent);
        var initialCount = _dm.GetRefuelEvents().Count();

        _dm.DeleteRefuelEvent(refuelEvent.Id);
        var events = _dm.GetRefuelEvents();

        Assert.Equal(initialCount - 1, events.Count());
        Assert.DoesNotContain(events, e => e.Id == refuelEvent.Id);
    }

    [Fact]
    public void Test_GetRefuelEvents()
    {
        var event1 = new RefuelEvent(DateTime.Now.AddDays(-2), 10, 1000, 50, Guid.NewGuid());
        var event2 = new RefuelEvent(DateTime.Now.AddDays(-1), 12, 1100, 60, Guid.NewGuid());
        var event3 = new RefuelEvent(DateTime.Now, 11, 1200, 55, Guid.NewGuid());
            _dm.AddRefuelEvent(event1); // Add out of order
            _dm.AddRefuelEvent(event3);
            _dm.AddRefuelEvent(event2);


        var events = _dm.GetRefuelEvents().ToList();

        Assert.Equal(3, events.Count);
        Assert.Equal(event3.Id, events[0].Id); // Should be most recent first
        Assert.Equal(event2.Id, events[1].Id);
        Assert.Equal(event1.Id, events[2].Id);
    }

    [Fact]
    public void Test_EditRefuelEvent()
    {
        
        var originalEvent = new RefuelEvent(10.0, 10000.0, 50.0);
        _dm.AddRefuelEvent(originalEvent);
        var newFuelAmount = 12.5;
        var newOdometer = 10150.0;

        _dm.EditRefuelEvent(originalEvent.Id, fuelAdded: newFuelAmount, odometer: newOdometer);
        var editedEvent = _dm.GetRefuelEventById(originalEvent.Id);

        Assert.Equal(newFuelAmount, editedEvent.FuelAdded);
        Assert.Equal(newOdometer, editedEvent.Odometer);
        Assert.Equal(originalEvent.Cost, editedEvent.Cost); // Cost should remain unchanged
        Assert.Equal(originalEvent.EventDateTime, editedEvent.EventDateTime); // Date should remain unchanged
    }

    [Fact]
    public void Test_GetRefuelEventById()
    {
        var refuelEvent = new RefuelEvent(10.5, 15000.0, 45.50);
        _dm.AddRefuelEvent(refuelEvent);

        var foundEvent = _dm.GetRefuelEventById(refuelEvent.Id);

        Assert.NotNull(foundEvent);
        Assert.Equal(refuelEvent.Id, foundEvent.Id);
        Assert.Equal(refuelEvent.FuelAdded, foundEvent.FuelAdded);
    }

    [Fact]
    public void Test_GetRefuelEventById_NonExistingId()
    {
        var nonExistentId = Guid.NewGuid();

        Assert.Throws<Exception>(() => _dm.GetRefuelEventById(nonExistentId));
    }
}