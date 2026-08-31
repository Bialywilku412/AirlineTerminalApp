using Spectre.Console;

public static class MainMenu
{
    private static PlaneService _planeService;
    private static UserService _userService;
    private static FlightService _flightService;
    private static SeatService _seatService;
    private static SeatReservationService _seatReservationService;

    public static void Menu
    (
        User user,
        PlaneService planeService,
        UserService userService,
        FlightService flightService,
        SeatService seatService,
        SeatReservationService seatReservationService
    )
    {
        _planeService = planeService;
        _userService = userService;
        _flightService = flightService;
        _seatService = seatService;
        _seatReservationService = seatReservationService;

        bool isRunning = true;
        User loggedInUser = user;

        AnsiConsole.MarkupLine($"[green]Welcome {user.Login}[/]");
        while (isRunning)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices("Flights", "Book Flight", "Reservations", "Cancel Reservation", "Admin Menu"));

            switch (choice)
            {
                case "Flights":
                    FlightsOverview();
                    break;
                case "Book Flight":
                    BookFlight(user);
                    break;
                case "Reservations":
                    ReservationsOverview(user);
                    break;
                case "Cancel Reservation":
                    CancelReservation(user);
                    break;
                case "Admin Menu":
                    AdminMenu.Menu();
                    break;
            }
        }
    }

    public static void FlightsTable()
    {
        List<Flight> flights = _flightService.ShowAllFlights().Data;

        var table = new Table();

        table.AddColumn("Flight ID");
        table.AddColumn("Plane");
        table.AddColumn("Origin");
        table.AddColumn("Destination");

        foreach (Flight flight in flights)
        {
            table.AddRow(flight.ID.ToString(), flight.AssignedPlane.Model, flight.Origin.ToString(), flight.Destination.ToString());
        }

        AnsiConsole.Write(table);
    }
    public static void FlightsOverview()
    {
        Console.Clear();
        FlightsTable();
        Utility.Return();
    }

    public static void BookFlight(User user)
    {
        FlightsTable();
        var flightId = AnsiConsole.Ask<int>("For what flight you want to book a ticket?");
        Flight flight = _flightService.GetFlightById(flightId).Data;

        List<Seat> allSeats = _seatService.GetSeatsByPlaneId(flight.AssignedPlaneId).Data;
        List<SeatReservation> allSeatReservations = _seatReservationService.GetSeatReservationsByFlightId(flightId).Data;

        List<int> seatIds = new();
        foreach(SeatReservation res in allSeatReservations)
        {
            seatIds.Add(res.SeatId);
        }

        List<Seat> availibleSeats = allSeats
             .Where(seat => !seatIds.Contains(seat.Id))
             .ToList();

        foreach (Seat seat in availibleSeats)
        {
            Console.WriteLine($"{seat.Id} ,{seat.Row}, {seat.Column}");
        }

        var seatId = AnsiConsole.Ask<int>("Enter seat id that you want to book?");

        SeatReservation seatReservation = new SeatReservation
        {
            UserId = user.Id,
            FlightId = flightId,
            SeatId = seatId,
            Price = 15
        };

        SeatReservation reserved = _seatReservationService.AddReservation(seatReservation).Data;
        Console.Clear();
        ReservationsOverview(user);
    }

    public static void CancelReservation(User user)
    {
        ReservationsTable(user);

        var reservationId = AnsiConsole.Ask<int>("What reservation do you want to cancel?");
        SeatReservation reservation = _seatReservationService.GetSeatReservationById(reservationId).Data;

        if (AnsiConsole.Confirm("Are you sure you want to cancel this flight?"))
        {
            _seatReservationService.DeleteReservationByUser(reservation);
        }

        Console.Clear();
        ReservationsOverview(user);
    }

    private static void ReservationsTable(User user)
    {
        List<SeatReservation> seatReservations = _seatReservationService.GetSeatReservationsByUserId(user.Id).Data;

        var table = new Table();

        table.AddColumn("Reservation ID");
        table.AddColumn("Plane");
        table.AddColumn("Origin");
        table.AddColumn("Destination");
        table.AddColumn("Seat row");
        table.AddColumn("Seat column");
        table.AddColumn("Seat class");

        foreach (SeatReservation reservation in seatReservations)
        {

            table.AddRow
            (
                reservation.Id.ToString(),
                reservation.Flight.AssignedPlane.Model,
                reservation.Flight.Origin.ToString(),
                reservation.Flight.Destination.ToString(),
                reservation.Seat.Row.ToString(),
                reservation.Seat.Column.ToString(),
                reservation.Seat.Class.ToString()
            );
        }

        AnsiConsole.Write(table);
    }
    public static void ReservationsOverview(User user)
    {
        ReservationsTable(user);
        Utility.Return();
    }
}