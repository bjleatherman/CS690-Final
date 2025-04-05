namespace Final;

using Spectre.Console;
using Spectre.Console.Cli;

public class ConsoleUi
{
    private readonly MenuSystem _menu;
    private readonly DataManager _dataManager;

    public ConsoleUi(MenuSystem menu, DataManager dataManager)
    {
        _menu = menu;
        _dataManager = dataManager;
    }

    public void Show()
    {
        _menu.RunMenu();
    }
}