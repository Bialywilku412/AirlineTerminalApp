using Spectre.Console;

public static class LoginMenu
{
    private static readonly UserRepository _repository = new UserRepository();
    private static readonly UserService _service = new UserService(_repository);
    public static void ShowLoginMenu()
    {
        bool isRunning = true;
        AnsiConsole.MarkupLine("[green]Welcome at Airport Rotterdam[/].");
        while (isRunning)
        {

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Login or register to enter.")
                    .AddChoices("Login", "Register", "Exit"));

            if (choice == "Register")
                Register();
            else if (choice == "Login")
                Login();
            else
                isRunning = false;
        }
    }

    private static void Login()
    {
        Console.Clear();
        var login = AnsiConsole.Ask<string>("Username: ");
        var password = AnsiConsole.Ask<string>("Password: ");
        var logged = _service.LogingIn(login, password);
        var user = _service.GetUserByLogin(login).Data;

        if (logged.Success)
        {
            Console.Clear();
            MainMenu.Menu(user);
        }
        else
        {
            Console.WriteLine(logged.Message);
        }
    }

    private static void Register()
    {
        var login = AnsiConsole.Ask<string>("Username: ");
        var password = AnsiConsole.Ask<string>("Password: ");

        User newUser = new User
        {
            Login = login,
            Password = password
        };

        var result = _service.RegisterUser(newUser);
        Console.WriteLine(result.Message);
    }
}