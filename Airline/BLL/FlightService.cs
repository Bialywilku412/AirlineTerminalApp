public class FlightService
{
    private readonly FlightRepository _repository;
    private readonly PlaneService _planeService;

    public FlightService(FlightRepository repository, PlaneService planeService)
    {
        _repository = repository;
        _planeService = planeService;
    }

    public Result<bool> AddFlight(Flight flight)
    {
        if (flight.Origin == flight.Destination)
            return new Result<bool>(false, "Adding failed, from airport cannot be same as to airport");

        var planeResult = _planeService.GetPlaneById(flight.AssignedPlaneId);
        if(!planeResult.Success || planeResult.Data == null)
            return new Result<bool>(false, "Adding failed, assigned plane does not exist.");

        flight.AssignedPlane = planeResult.Data;

        _repository.AddFlight(flight);
        return new Result<bool>(true, "Flight added successfully");
    }

    public Result<List<Flight>> ShowAllFlights()
    {
        var flights = _repository.ShowAllFlights();
        if (flights.Count == 0)
            return new Result<List<Flight>>(false, "No flights");

        foreach (var flight in flights)
        {
            flight.AssignedPlane = _planeService.GetPlaneById(flight.AssignedPlaneId).Data;
        }

        return new Result<List<Flight>>(true, "succes", flights);
    }

    public Result<Flight> GetFlightById(int id)
    {
        Flight flight = _repository.GetFlightById(id);
        if (flight == null)
            return new Result<Flight>(false, "No flight found");

        return new Result<Flight>(true, "succes", flight);
    }
}