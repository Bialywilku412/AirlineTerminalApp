public class SeatService
{
    private readonly SeatRepository _repository;

    public SeatService(SeatRepository repository)
    {
        _repository = repository;
    }

    public Result<List<Seat>> GetSeatsByPlaneId(int id)
    {
        List<Seat> seats = _repository.GetSeatsByPlaneId(id);
        if (seats.Count == 0)
            return new Result<List<Seat>>(false, "No seats found");

        return new Result<List<Seat>>(true, "succes", seats);
    }

}