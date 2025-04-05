namespace Final;

using Spectre.Console;
using Spectre.Console.Cli;
using Final.Domain;

public class MenuSystem
{

    private readonly DataManager _dataManager;

    // Root Menu and Options
    private readonly MenuItem _rootMenu = new("Root Menu");
    private readonly MenuItem _back = new("Back");
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
        _rootMenu.Children.Add(_refuelMenu);
        _rootMenu.Children.Add(_maintenanceMenu);
        _rootMenu.Children.Add(_reminderMenu);
        _rootMenu.Children.Add(_back);

        // Refuel Menu
        _refuelMenu.Children.Add(_createRefuelMenu);
        _refuelMenu.Children.Add(_readEditRefuelMenu);
        _refuelMenu.Children.Add(_back);
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
        var refuelEvents = _dataManager.GetRefuelEvents();



        return new MenuItem("All Refuel Events", () =>{

        });
    }

    public void RunMenu()
    {
        RunMenuLoop(_rootMenu);
    }

    private void RunMenuLoop(MenuItem menu)
    {
        // Show Title
        AnsiConsole.Write(new Rule($"[green]{menu._title}[/]").LeftJustified());

        // Show Options
        // Catch if there are no children
        if (menu.Children == null || !menu.Children.Any())
        {
            AnsiConsole.MarkupLine($"[red]No Options Available[/]");
            AnsiConsole.Confirm("Press Enter To Confirm");
            return;
        }

        // Show menu options
        var prompt = new SelectionPrompt<MenuItem>()
            .PageSize(10)
            .MoreChoicesText("[grey]Use Arrow Keys To See More[/]")
            .AddChoices(menu.Children)
            .UseConverter(m => m._title);
        MenuItem selectedMenu = AnsiConsole.Prompt(prompt);

        // Execute the command for selected menu item if there is one
        selectedMenu.Command?.Invoke();

        // Run the selected menu if it has children
        if(selectedMenu.Children != null) { RunMenuLoop(selectedMenu); }
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