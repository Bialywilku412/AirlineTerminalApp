public class SeatReservationService
{
    private readonly SeatReservationRepository _repository;
    private readonly FlightService _flightService;
    private readonly SeatService _seatService;
    private readonly PlaneService _planeService;

    public SeatReservationService(SeatReservationRepository repository, FlightService flightService, SeatService seatService, PlaneService planeService)
    {
        _repository = repository;
        _flightService = flightService;
        _seatService = seatService;
        _planeService = planeService;
    }

    public Result<SeatReservation> AddReservation(SeatReservation seatReservation)
    {
        _repository.AddReservation(seatReservation);
        return new Result<SeatReservation>(true, "succes", seatReservation);
    }

    public Result<List<SeatReservation>> GetSeatReservationsByFlightId(int id)
    {
        List<SeatReservation> seatReservations = _repository.GetSeatReservationsByFlightId(id);
        return new Result<List<SeatReservation>>(true, "succes", seatReservations);
    }

    public Result<List<SeatReservation>> GetSeatReservationsByUserId(int id)
    {
        List<SeatReservation> seatReservations = _repository.GetSeatReservationsByUserId(id);

        foreach(var reservation in seatReservations)
        {
            reservation.Seat = _seatService.GetSeatById(reservation.SeatId).Data;
            reservation.Flight = _flightService.GetFlightById(reservation.FlightId).Data;
            reservation.Flight.AssignedPlane = _planeService.GetPlaneById(reservation.FlightId).Data;
        }

        return new Result<List<SeatReservation>>(true, "succes", seatReservations);
    }

    public Result<SeatReservation> GetSeatReservationById(int id)
    {
        SeatReservation reservation = _repository.GetReservationById(id);

        reservation.Seat = _seatService.GetSeatById(reservation.SeatId).Data;
        reservation.Flight = _flightService.GetFlightById(reservation.FlightId).Data;
        reservation.Flight.AssignedPlane = _planeService.GetPlaneById(reservation.FlightId).Data;

        return new Result<SeatReservation>(true, "succes", reservation);
    }

    public Result<bool> DeleteReservationByUser(SeatReservation seatReservation)
    {
        if(seatReservation == null)
            return new Result<bool>(false, "Reservation was not found");

        _repository.DeleteReservationByUser(seatReservation);
        return new Result<bool>(true, "success");
    }
}