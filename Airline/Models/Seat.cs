public class Seat
{
    public int Id { get; set; }
    public int PlaneId { get; set; }
    public Plane Plane { get; set; }

    public int Row { get; set; }
    public char Column { get; set; }
    public SeatClass Class { get; set; }

}