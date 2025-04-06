namespace Final;

using Spectre.Console;
using Spectre.Console.Cli;
using Final.Domain;
using System.Collections.Immutable;
using System.ComponentModel;

public class MenuSystem
{

    private readonly DataManager _dataManager;

    // Root Menu and Options
    public readonly MenuItem RootMenu= new("Car History");
    private readonly MenuItem _refuelMenu = new("Refuel Options");
    private readonly MenuItem _maintenanceMenu = new("Maintenance Options");
    private readonly MenuItem _reminderMenu = new("Reminder Options");

    // Refuel
    private readonly MenuItem _createRefuelMenu;
    private readonly MenuItem _readEditRefuelMenu;

    // Maintenance
    private readonly MenuItem _createMaintenanceMenu;
    private readonly MenuItem _readEditMaintenanceMenu;

    // Reminders
    private readonly MenuItem _createReminderMenu;
    private readonly MenuItem _readEditReminderMenu;

    public MenuSystem(DataManager dataManager)
    {
        _dataManager = dataManager;

        // Create the individual menu pieces
        _createRefuelMenu = BuildCreateRefuelMenu();
        _readEditRefuelMenu = BuildReadEditRefuelMenu();
        _createMaintenanceMenu = BuildCreateMaintenanceMenu();
        _readEditMaintenanceMenu = BuildReadEditMaintenanceMenu();
        _createReminderMenu = BuildCreateReminderMenu();
        _readEditReminderMenu = BuildReadEditReminderMenu();

        // Build the menu from the pieces
        BuildMenu();
    }
    private void BuildMenu()
    {
        // Root Menu
        RootMenu.Children.Add(_refuelMenu);
        RootMenu.Children.Add(_maintenanceMenu);
        RootMenu.Children.Add(_reminderMenu);

        // Refuel Menu
        _refuelMenu.Children.Add(_createRefuelMenu);
        _refuelMenu.Children.Add(_readEditRefuelMenu);

        // Maintenance Menu
        _maintenanceMenu.Children.Add(_createMaintenanceMenu);
        _maintenanceMenu.Children.Add(_readEditMaintenanceMenu);

        // Reminder Menu
        _reminderMenu.Children.Add(_createReminderMenu);
        _reminderMenu.Children.Add(_readEditReminderMenu);
    }

    /*******************
        Refuel Section
    *******************/

    private MenuItem BuildCreateRefuelMenu()
    {
        return new MenuItem("Add Refuel Event", () => {
            double fuelAdded = AnsiConsole.Prompt(new TextPrompt<double>("Amount of Fuel Added: "));
            double odometer = AnsiConsole.Prompt(new TextPrompt<double>("Amount Current Odometer: "));
            double cost = AnsiConsole.Prompt(new TextPrompt<double>("Amount Spent: "));

            var refuelEvent = new RefuelEvent(fuelAdded, odometer, cost);
            _dataManager.AddRefuelEvent(refuelEvent);
        });
    }

    private MenuItem BuildReadEditRefuelMenu()
    {
        return new MenuItem("All Refuel Events", () =>{
            GetRefuelMenuItems();
        });
    }

    private void GetRefuelMenuItems()
    {

        _readEditRefuelMenu.Children.Clear();

        MenuItem menuItem;
        string title;

        var refuelEvents = _dataManager.GetRefuelEvents();

        foreach (var e in refuelEvents)
        {
            title = $"{e.MaintenanceType}: Gallons: {e.FuelAdded}, Cost: {e.Cost}, Odometer: {e.Odometer} | {e.EventDateTime.ToString("h:mm M/d/yy")}";

            menuItem = new MenuItem(title);

            GenerateRefuelEditSubMenus(menuItem, e.Id);

            _readEditRefuelMenu.Children.Add(menuItem);
        }
    }

    private void GenerateRefuelEditSubMenus(MenuItem menuItem, Guid id)
    {
        menuItem.Children.Clear();

        var e = _dataManager.GetRefuelEventById(id);

        menuItem.Children.Add(CreateRefuelEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Date",
            currentValue:e.EventDateTime,
            promptText:"Enter New Date Time: ",
            editAction: (newValue) => _dataManager.EditRefuelEvent(id, eventDateTime:newValue)
        ));
        
        menuItem.Children.Add(CreateRefuelEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Fuel Added",
            currentValue:e.FuelAdded,
            promptText:"Enter New Fuel Amount: ",
            editAction: (newValue) => _dataManager.EditRefuelEvent(id, fuelAdded:newValue)
        ));
              
        menuItem.Children.Add(CreateRefuelEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Cost",
            currentValue:e.Cost,
            promptText:"Enter New Fuel Cost: ",
            editAction: (newValue) => _dataManager.EditRefuelEvent(id, cost:newValue)
        ));
   
        menuItem.Children.Add(CreateRefuelEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Odometer",
            currentValue:e.Odometer,
            promptText:"Enter New Odometer: ",
            editAction: (newValue) => _dataManager.EditRefuelEvent(id, odometer:newValue)
        ));        
        
        menuItem.Children.Add(
            new MenuItem($"DELETE", () =>{
                if(AnsiConsole.Confirm("Delete Event? "))
                {
                    _dataManager.DeleteRefuelEvent(e.Id);
                    menuItem.Children.Clear();
                    GetRefuelMenuItems();
                }
            }
        ));
    }

    private MenuItem CreateRefuelEditSubMenuItem<T>(
        MenuItem menuItem, 
        Guid id, 
        string titlePrefix, 
        T currentValue,
        string promptText,
        Action<T> editAction
    )
    {
        return CreateEditSubMenuItem(
            menuItem:menuItem, 
            id:id, 
            titlePrefix:titlePrefix, 
            currentValue:currentValue,
            promptText:promptText,
            editAction:editAction, 
            RegenerateParentMenus: () => GetRefuelMenuItems(),
            RegenerateSubMenus: GenerateRefuelEditSubMenus
        );
    }

    
    /**************************
        Maintenance Section
    **************************/


    private MenuItem BuildCreateMaintenanceMenu()
    {
        return new MenuItem("Add Maintenance Event", () => {
            string maintenanceType = AnsiConsole.Prompt(new TextPrompt<string>("Maintenance Type: "));
            double odometer = AnsiConsole.Prompt(new TextPrompt<double>("Amount Current Odometer: "));
            double cost = AnsiConsole.Prompt(new TextPrompt<double>("Amount Spent: "));

            var maintenanceEvent = new MaintenanceEvent(maintenanceType, odometer, cost);
            _dataManager.AddMaintenanceEvent(maintenanceEvent);
        });
    }

    private MenuItem BuildReadEditMaintenanceMenu()
    {
        return new MenuItem("All Maintenance Events", () =>{
            GetMaintenanceMenuItems();
        });
    }

    private void GetMaintenanceMenuItems()
    {

        _readEditMaintenanceMenu.Children.Clear();

        MenuItem menuItem;
        string title;

        var maintenanceEvents = _dataManager.GetMaintenanceEvents();

        foreach (var e in maintenanceEvents)
        {
            title = $"{e.MaintenanceType} | Cost: {e.Cost}, Odometer: {e.Odometer} | {e.EventDateTime.ToString("h:mm M/d/yy")}";

            menuItem = new MenuItem(title);

            GenerateMaintenanceEditSubMenus(menuItem, e.Id);

            _readEditMaintenanceMenu.Children.Add(menuItem);
        }
    }

    private void GenerateMaintenanceEditSubMenus(MenuItem menuItem, Guid id)
    {
        menuItem.Children.Clear();

        var e = _dataManager.GetMaintenanceEventById(id);

        menuItem.Children.Add(CreateMaintenanceEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Date",
            currentValue:e.EventDateTime,
            promptText:"Enter New Date Time: ",
            editAction: (newValue) => _dataManager.EditMaintenanceEvent(id, eventDateTime:newValue)
        ));
        
        menuItem.Children.Add(CreateMaintenanceEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Maintenance Type",
            currentValue:e.MaintenanceType,
            promptText:"Enter New Maintenance Type: ",
            editAction: (newValue) => _dataManager.EditMaintenanceEvent(id, maintenanceType:newValue)
        ));
              
        menuItem.Children.Add(CreateMaintenanceEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Cost",
            currentValue:e.Cost,
            promptText:"Enter Maintenance Cost: ",
            editAction: (newValue) => _dataManager.EditMaintenanceEvent(id, cost:newValue)
        ));
   
        menuItem.Children.Add(CreateMaintenanceEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Odometer",
            currentValue:e.Odometer,
            promptText:"Enter New Odometer: ",
            editAction: (newValue) => _dataManager.EditMaintenanceEvent(id, odometer:newValue)
        ));        
        
        menuItem.Children.Add(
            new MenuItem($"DELETE", () =>{
                if(AnsiConsole.Confirm("Delete Event? "))
                {
                    _dataManager.DeleteMaintenanceEvent(e.Id);
                    menuItem.Children.Clear();
                    GetMaintenanceMenuItems();
                }
            }
        ));
    }

    private MenuItem CreateMaintenanceEditSubMenuItem<T>(
        MenuItem menuItem, 
        Guid id, 
        string titlePrefix, 
        T currentValue,
        string promptText,
        Action<T> editAction
    )
    {
        return CreateEditSubMenuItem(
            menuItem:menuItem, 
            id:id, 
            titlePrefix:titlePrefix, 
            currentValue:currentValue,
            promptText:promptText,
            editAction:editAction, 
            RegenerateParentMenus: () => GetMaintenanceMenuItems(),
            RegenerateSubMenus: GenerateMaintenanceEditSubMenus
        );
    }
  
    
    /**********************
        Reminder Section
    **********************/


    private MenuItem BuildCreateReminderMenu()
    {
        return new MenuItem("Add Reminder", () => {
            string reminderText = AnsiConsole.Prompt(new TextPrompt<string>("Reminder for: "));
            var reminderTime = AnsiConsole.Prompt(new TextPrompt<DateTime>("Reminder Date Time: "));

            var reminderEvent = new ReminderEvent(reminderText, reminderTime);
            _dataManager.AddReminderEvent(reminderEvent);
        });
    }

    private MenuItem BuildReadEditReminderMenu()
    {
        return new MenuItem("All Reminder Events", () =>{
            GetReminderMenuItems();
        });
    }

    private void GetReminderMenuItems()
    {

        _readEditReminderMenu.Children.Clear();

        MenuItem menuItem;
        string title;

        var reminderEvents = _dataManager.GetReminderEvents();

        foreach (var e in reminderEvents)
        {
            title = $"{e.ReminderText} | Alarm Time: {e.ReminderTime}, Will Ring: {!e.IsSilenced} | Time Alarm was Set: {e.EventDateTime.ToString("h:mm M/d/yy")}";

            menuItem = new MenuItem(title);

            GenerateReminderEditSubMenus(menuItem, e.Id);

            _readEditReminderMenu.Children.Add(menuItem);
        }
    }

    private void GenerateReminderEditSubMenus(MenuItem menuItem, Guid id)
    {
        menuItem.Children.Clear();
        
        string alarmToggleTo;

        var e = _dataManager.GetReminderEventById(id);

        alarmToggleTo = e.IsSilenced ? "ON" : "OFF";

        menuItem.Children.Add(CreateReminderEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Reminder Text",
            currentValue:e.ReminderText,
            promptText:"Enter New Reminder Text: ",
            editAction: (newValue) => _dataManager.EditReminderEvent(id, reminderText:newValue)
        ));
        
        menuItem.Children.Add(CreateReminderEditSubMenuItem(
            menuItem: menuItem,
            id:id,
            titlePrefix:"Edit Reminder Time",
            currentValue:e.ReminderTime,
            promptText:"Enter New Reminder Time: ",
            editAction: (newValue) => _dataManager.EditReminderEvent(id, reminderTime:newValue)
        ));
              
        menuItem.Children.Add(
            new MenuItem($"Turn Alarm {alarmToggleTo}", ()=>{
                _dataManager.ToggleReminderAlarm(e.Id);
                menuItem.Children.Clear();
                GetReminderMenuItems();
            })
        );        
        
        menuItem.Children.Add(
            new MenuItem("DELETE", () =>{
                if(AnsiConsole.Confirm("Delete Event? "))
                {
                    _dataManager.DeleteReminderEvent(e.Id);
                    menuItem.Children.Clear();
                    GetReminderMenuItems();
                }
            }
        ));
    }

    private MenuItem CreateReminderEditSubMenuItem<T>(
        MenuItem menuItem, 
        Guid id, 
        string titlePrefix, 
        T currentValue,
        string promptText,
        Action<T> editAction
    )
    {
        return CreateEditSubMenuItem(
            menuItem:menuItem, 
            id:id, 
            titlePrefix:titlePrefix, 
            currentValue:currentValue,
            promptText:promptText,
            editAction:editAction, 
            RegenerateParentMenus: () => GetReminderMenuItems(),
            RegenerateSubMenus: GenerateReminderEditSubMenus
        );
    }
 
    /**************************
        Helper Section
    **************************/

    private static MenuItem CreateEditSubMenuItem<T>(
        MenuItem menuItem, 
        Guid id, 
        string titlePrefix, 
        T currentValue,
        string promptText,
        Action<T> editAction,
        Action RegenerateParentMenus,
        Action<MenuItem, Guid> RegenerateSubMenus
    )
    {
        string title = string.Join(": ", titlePrefix, currentValue);
        return new MenuItem(title, ()=>{
            T newValue = AnsiConsole.Prompt(new TextPrompt<T>(promptText));
            editAction(newValue);
            RegenerateParentMenus();
            RegenerateSubMenus(menuItem, id);
        });
    }
}

public class MenuItem
{
    public readonly string _title;
    public Action? Command { get; set; }
    public List<MenuItem> Children { get; } = new List<MenuItem>();

    public MenuItem(string title, Action? command = null)
    {
        _title = title;
        Command = command;
    }
}