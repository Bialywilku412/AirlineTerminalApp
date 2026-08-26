using Spectre.Console;

public static class MainMenu
{
    private static readonly PlaneRepository planeRepository = new PlaneRepository();
    private static readonly PlaneService planeService = new PlaneService(planeRepository);
    private static readonly FlightRepository _repository = new FlightRepository();
    private static readonly FlightService _service = new FlightService(_repository, planeService);
    public static void Menu(User user)
    {
        bool isRunning = true;
        User loggedInUser = user;

        AnsiConsole.MarkupLine($"[green]Welcome {user.Login}[/]");
        while (isRunning)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select")
                    .AddChoices("Flights", "Book Flight", "Admin Menu"));

            switch (choice)
            {
                case "Flights":
                    FlightsOverview();
                    break;
                case "Book Flight":
                    break;
                case "Admin Menu":
                    AdminMenu.Menu();
                    break;
            }
        }
    }

    public static void FlightsOverview()
    {
        List<Flight> flights = _service.ShowAllFlights().Data;

        var table = new Table();

        table.AddColumn("Flight ID");
        table.AddColumn("Plane");
        table.AddColumn("Origin");
        table.AddColumn("Destination");

        foreach (Flight flight in flights)
        {
            table.AddRow(flight.ID.ToString(),flight.AssignedPlane.Model ,flight.Origin.ToString(), flight.Destination.ToString());
        }

        AnsiConsole.Write(table);
    }

    public static void BookFlight()
    {
        FlightsOverview();
        var flightId = AnsiConsole.Ask<int>("For what flight you want to book a ticket?");


    }
}