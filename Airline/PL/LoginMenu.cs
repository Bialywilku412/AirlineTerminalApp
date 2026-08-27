using Spectre.Console;

public static class LoginMenu
{
    public static void ShowLoginMenu
    (
        PlaneService planeService,
        UserService userService,
        FlightService flightService,
        SeatService seatService,
        SeatReservationService seatReservationService
    )
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
                Register(userService);
            else if (choice == "Login")
                Login(planeService, userService, flightService, seatService, seatReservationService);
            else
                isRunning = false;
        }
    }

    private static void Login
    (
        PlaneService planeService,
        UserService userService,
        FlightService flightService,
        SeatService seatService,
        SeatReservationService seatReservationService
    )
    {
        Console.Clear();
        var login = AnsiConsole.Ask<string>("Username: ");
        var password = AnsiConsole.Ask<string>("Password: ");
        var logged = userService.LogingIn(login, password);
        var user = userService.GetUserByLogin(login).Data;

        if (logged.Success)
        {
            Console.Clear();
            MainMenu.Menu(user, planeService, userService, flightService, seatService, seatReservationService);
        }
        else
        {
            Console.WriteLine(logged.Message);
        }
    }

    private static void Register(UserService userService)
    {
        var login = AnsiConsole.Ask<string>("Username: ");
        var password = AnsiConsole.Ask<string>("Password: ");

        User newUser = new User
        {
            Login = login,
            Password = password
        };

        var result = userService.RegisterUser(newUser);
        Console.WriteLine(result.Message);
    }
}