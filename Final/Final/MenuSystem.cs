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
    public readonly MenuItem RootMenu = new("Root Menu");
    private readonly MenuItem _refuelMenu = new("Refuel Options");
    private readonly MenuItem _maintenanceMenu = new("Maintenance Options");
    private readonly MenuItem _reminderMenu = new("Reminder Options");


    //**Begin CRUD**//
    // Refuel
    private readonly MenuItem _createRefuelMenu;
    private readonly MenuItem _readEditRefuelMenu;

    public MenuSystem(DataManager dataManager)
    {
        _dataManager = dataManager;

        // Create the individual menu pieces
        _createRefuelMenu = BuildCreateRefuelMenu();
        _readEditRefuelMenu = BuildReadEditRefuelMenu();

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
    }

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
            title = $"{e.MaintenanceType}: Gallons: {e.FuelAdded}, Cost: {e.Cost}, Odometer: {e.Odometer} | {e.EventDateTime.ToString("h:m M/d/yy")}";

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