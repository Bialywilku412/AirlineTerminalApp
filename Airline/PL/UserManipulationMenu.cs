using Spectre.Console;

public static class UserManipulationMenu
{
    private static readonly PlaneRepository planeRepository = new PlaneRepository();
    private static readonly PlaneService _planeService = new PlaneService(planeRepository);
    private static readonly FlightRepository _repository = new FlightRepository();
    private static readonly FlightService _service = new FlightService(_repository, _planeService);
    private static readonly UserRepository userRepository = new UserRepository();
    private static readonly UserService _userService = new UserService(userRepository);

    public static void Menu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an [green]environment[/]:")
                .AddChoices("Users Overview", "Return"));

        switch (choice)
        {
            case "Users Overview":
                UsersOverview();
                break;
            case "Return":
                return;
        }

    }

    private static void UsersOverview()
    {
        List<User> users = _userService.GetAllUsers().Data;

        var table = new Table();

        table.AddColumn("User");
        table.AddColumn("Login");
        table.AddColumn("Rank");

        foreach (var user in users)
        {
            table.AddRow(user.Id.ToString(), user.Login, user.Rank.ToString());
        }

        AnsiConsole.Write(table);
    }
}