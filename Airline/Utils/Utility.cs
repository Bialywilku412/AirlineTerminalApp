using Spectre.Console;

public static class Utility
{
    public static void Return()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Return"));

        if (choice != null)
            Console.Clear();
            return;
    }
}