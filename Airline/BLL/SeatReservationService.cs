public class SeatReservationService
{
    private readonly SeatReservationRepository _repository;

    public SeatReservationService(SeatReservationRepository repository)
    {
        _repository = repository;
    }

    public Result<List<SeatReservation>> GetSeatReservationsByFlightId(int id)
    {
        List<SeatReservation> seatReservations = _repository.GetSeatReservationsByFlightId(id);
        if (seatReservations.Count == 0)
            return new Result<List<SeatReservation>>(false, "No reservations found");

        return new Result<List<SeatReservation>>(true, "succes", seatReservations);
    }
}