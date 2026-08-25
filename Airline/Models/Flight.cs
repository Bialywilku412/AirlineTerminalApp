public class Flight
{
    public int ID { get; set; }
    //public string ?FlightNumber { get; set; }

    public Airports Origin { get; set; }

    public Airports Destination { get; set; }
    public int AssignedPlaneId { get; set; }
    public Plane AssignedPlane { get; set; }


}