namespace Final.Tests;

using System.IO;
using Final.Domain;

public class FileManagerTest
{
    private readonly FileManager _mf;
    private static readonly string _testDirectory = "test-data";
    private readonly string _maintenanceFilePath = Path.Combine(_testDirectory, "maintenance-file.txt");

    private readonly List<MaintenanceEvent> _mEvents = [];

    public FileManagerTest()
    {
        
        if (!Directory.Exists(_testDirectory)) { Directory.CreateDirectory(_testDirectory); }  

        _mf = new FileManager(_maintenanceFilePath);

        _mEvents.Add(new MaintenanceEvent("testing", 100, 100));
        _mEvents.Add(new MaintenanceEvent("code", 100, 100));
        _mEvents.Add(new MaintenanceEvent("is", 100, 100));
        _mEvents.Add(new MaintenanceEvent("fun", 100, 100));

        _mf.ReplaceFileData(_mEvents);

    }

    [Fact]
    public void Test_CreateDataDirectory()
    {
        FileManager.CreateDataDirectory();
        Assert.True(Directory.Exists(FileManager.GetDataDirectoryPath()));
    }

    [Fact]
    public void Test_GetData()
    {
        var testData = _mf.GetData(MaintenanceEvent.UnpackCsvLine);

        Assert.True(testData.Count == _mEvents.Count);

        foreach (var item in testData)
        {

            var targetItem = _mEvents.FirstOrDefault(i => i.Id == item.Id);
            Assert.NotNull(targetItem);

            Assert.Equal(targetItem.MaintenanceType, item.MaintenanceType);
            Assert.Equal(targetItem.Cost, item.Cost);
            Assert.Equal(targetItem.Odometer, item.Odometer);
            Assert.Equal(targetItem.EventDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), item.EventDateTime.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }

    [Fact]
    public void Test_ReplaceFileData()
    {
        _mEvents.Add(new MaintenanceEvent("Testing Againsting", 200, 200));

        _mf.ReplaceFileData(_mEvents);

        var testData = _mf.GetData(MaintenanceEvent.UnpackCsvLine);

        Assert.True(testData.Count == _mEvents.Count);

        foreach (var item in testData)
        {
            var targetItem = _mEvents.FirstOrDefault(i => i.Id == item.Id);
            Assert.NotNull(targetItem);

            Assert.Equal(targetItem.MaintenanceType, item.MaintenanceType);
            Assert.Equal(targetItem.Cost, item.Cost);
            Assert.Equal(targetItem.Odometer, item.Odometer);
            Assert.Equal(targetItem.EventDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), item.EventDateTime.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }
}
