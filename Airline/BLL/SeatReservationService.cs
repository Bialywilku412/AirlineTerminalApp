public class SeatReservationService
{
    private readonly SeatReservationRepository _repository;
    private readonly FlightService _flightService;
    private readonly SeatService _seatService;

    public SeatReservationService(SeatReservationRepository repository, FlightService flightService, SeatService seatService)
    {
        _repository = repository;
        _flightService = flightService;
        _seatService = seatService;
    }

    public Result<SeatReservation> AddReservation(SeatReservation seatReservation)
    {
        _repository.AddReservation(seatReservation);
        return new Result<SeatReservation>(true, "succes", seatReservation);
    }

    public Result<List<SeatReservation>> GetSeatReservationsByFlightId(int id)
    {
        List<SeatReservation> seatReservations = _repository.GetSeatReservationsByFlightId(id);
        if (seatReservations.Count == 0)
            return new Result<List<SeatReservation>>(false, "No reservations found");

        return new Result<List<SeatReservation>>(true, "succes", seatReservations);
    }

    public Result<List<SeatReservation>> GetSeatReservationsByUserId(int id)
    {
        List<SeatReservation> seatReservations = _repository.GetSeatReservationsByUserId(id);
        if (seatReservations.Count == 0)
            return new Result<List<SeatReservation>>(false, "No reservations found");

        foreach(var reservation in seatReservations)
        {
            reservation.Seat = _seatService.Ge
        }

        return new Result<List<SeatReservation>>(true, "succes", seatReservations);
    }
}