namespace Final;

using Spectre.Console;
using Spectre.Console.Cli;

public class ConsoleUi
{
    private readonly MenuSystem _menu;
    private readonly DataManager _dataManager;
    private readonly MenuItem _rootMenu;
    private readonly MenuItem _backSelect = new MenuItem("Go Back");
    private readonly MenuItem _exitSelect = new MenuItem("Exit");

    public ConsoleUi(MenuSystem menu, DataManager dataManager)
    {
        _menu = menu;
        _dataManager = dataManager;
        _rootMenu = _menu.RootMenu;
    }

    public void Show()
    {
        var menuStack = new Stack<MenuItem>();
        MenuItem currentMenu = _rootMenu;

        while(true)
        {
            // Process current menu
            MenuItem selectedItem = ShowMenuAndGetSelection(currentMenu, menuStack.Count != 0);

            // Process user selection
            if (selectedItem == _backSelect)
            {
                if (menuStack.Count!=0) { currentMenu = menuStack.Pop(); }
            }
            else if (selectedItem == _exitSelect)
            {
                break; //Exit Program
            }
            else if (selectedItem.Command != null)
            {
                ExecuteCommand(selectedItem);
            }
            
            
            if (selectedItem.Children != null && selectedItem.Children.Count != 0)
            {
                menuStack.Push(currentMenu);
                currentMenu = selectedItem;
            }
            else
            {
                // AnsiConsole.Markup($"[grey]No Action defined for '{selectedItem._title}.'[/]");
                // AnsiConsole.Confirm("Press Enter To Confirm");
            }
        }
    }

    private MenuItem ShowMenuAndGetSelection(MenuItem menuItem, bool useBack)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[green]{menuItem._title}[/]").LeftJustified());

        var choices = new List<MenuItem>();
        if (menuItem.Children != null) { choices.AddRange(menuItem.Children); }
        MenuItem backType  = useBack ? _backSelect: _exitSelect;

        if (choices.Count != 0) 
        {
            choices.Add(backType);
        }
        else
        {
            return backType;
        }

        var prompt = new SelectionPrompt<MenuItem>()
            .AddChoices(choices)
            .UseConverter(i => i._title);

        return AnsiConsole.Prompt(prompt);
     }

     private void ExecuteCommand(MenuItem item)
    {
        item.Command?.Invoke();
    }
}