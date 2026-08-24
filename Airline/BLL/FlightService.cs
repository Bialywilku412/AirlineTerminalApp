public class FlightService
{
    private readonly FlightRepository _repository;

    public FlightService(FlightRepository repository)
    {
        _repository = repository; 
    }

    public Result<bool> AddFlight(Flight flight)
    {
        if (flight.Origin == flight.Destination)
            return new Result<bool>(false, "Adding failed, from airport cannot be same as to airport");

        _repository.AddFlight(flight);
        return new Result<bool>(true, "Flight added successfully");
    }

    public Result<List<Flight>> ShowAllFlights()
    {
        var flights = _repository.ShowAllFlights();
        if (flights.Count == 0)
            return new Result<List<Flight>>(false, "No flights");

        return new Result<List<Flight>>(true, "succes", flights);
    }
}