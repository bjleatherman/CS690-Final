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