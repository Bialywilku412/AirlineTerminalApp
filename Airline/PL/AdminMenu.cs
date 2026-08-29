using Spectre.Console;

public static class AdminMenu
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
                .AddChoices("Flights", "Add Flight", "Cancel Flight", "Users Menu", "Return"));

        switch(choice)
        {
            case "Flights":
                MainMenu.FlightsOverview();
                break;
            case "Add Flight":
                AddFlight();
                break;
            case "Cancel Flight":
                CancelFlight();
                break;
            case "Users Menu":
                UserManipulationMenu.Menu();
                break;
            case "Return":
                return;
        }

    }

    private static void AddFlight()
    {
        List<Plane> Planes = _planeService.ShowAllPlanes().Data;

        var table = new Table();

        table.AddColumn("PlaneId");
        table.AddColumn("Model");
        table.AddColumn("Capacity");

        foreach (Plane plane in Planes)
        {
            table.AddRow(plane.ID.ToString(), plane.Model, plane.Capacity.ToString());
        }

        AnsiConsole.Write(table);
        var planeInput = AnsiConsole.Ask<int>("Plane ID: ");

        // From Airport
        Console.WriteLine("From what airport");
        foreach(Airports airport in Enum.GetValues(typeof(Airports)))
        {
            Console.WriteLine(airport);
        }

        string originInput = Console.ReadLine();
        if(!Enum.TryParse<Airports>(originInput, true, out var origin))
        {
            Console.WriteLine("Airport does not exist");
            return;
        }

        //To Airport
        Console.WriteLine("To what airport");
        foreach (Airports airport in Enum.GetValues(typeof(Airports)))
        {
            Console.WriteLine(airport);
        }

        string destinationInput = Console.ReadLine();
        if(!Enum.TryParse<Airports>(destinationInput, true, out var destination))
        {
            Console.WriteLine("Airport does not exist");
            return;
        }

        //Adding flight
        Flight flight = new Flight
        {
            Origin = origin,
            Destination = destination,
            AssignedPlaneId = planeInput
        };

        var result = _service.AddFlight(flight);
        Console.WriteLine(result.Message);
    }

    private static void CancelFlight()
    {
        MainMenu.FlightsOverview();

        var flightId = AnsiConsole.Ask<int>("Enter flight id that you want to cancel: ");

        Result<Flight> result = _service.GetFlightById(flightId);
        if (!result.Success)
        {
            Console.WriteLine(result.Message);
            return;
        }

        Flight flight = result.Data;

        var table = new Table();

        table.AddColumn("Flight ID");
        table.AddColumn("Plane");
        table.AddColumn("Origin");
        table.AddColumn("Destination");

        table.AddRow(flight.ID.ToString(), flight.AssignedPlane.Model, flight.Origin.ToString(), flight.Destination.ToString());

        AnsiConsole.Write(table);

        if(AnsiConsole.Confirm("Are you sure you want to cancel this flight?"))
        {
            _service.DeleteFlightById(flightId);
        }
    }
}