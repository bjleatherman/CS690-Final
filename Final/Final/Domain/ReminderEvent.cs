namespace Final;

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