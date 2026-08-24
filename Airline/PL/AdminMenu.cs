using Spectre.Console;

public static class AdminMenu
{
    private static readonly FlightRepository _repository = new FlightRepository();
    private static readonly FlightService _service = new FlightService(_repository);

    public static void Menu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an [green]environment[/]:")
                .AddChoices("Flights", "Add Flight", "Return"));

        switch(choice)
        {
            case "Flights":
                MainMenu.FlightsOverview();
                break;
            case "Add Flight":
                AddFlight();
                break;
            case "Return":
                return;
        }

    }

    private static void AddFlight()
    {
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
            Destination = destination
        };

        var result = _service.AddFlight(flight);
        Console.WriteLine(result.Message);
    }
}