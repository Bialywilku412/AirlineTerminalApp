public class PlaneService
{
    private readonly PlaneRepository _repository;

    public PlaneService(PlaneRepository repository)
    {
        _repository = repository;
    }

    public Result<List<Plane>> ShowAllPlanes()
    {
        var planes = _repository.ShowAllPlanes();
        if (planes.Count == 0)
            return new Result<List<Plane>>(false, "No flights");

        return new Result<List<Plane>>(true, "succes", planes);
    }
}