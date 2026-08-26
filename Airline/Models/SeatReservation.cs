public class SeatReservation
{
    public int Id { get; set; }
    public int SeatId { get; set; }
    public Seat Seat { get; set; }
    public int FlightId { get; set; }
    public Flight Flight { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public float Price { get; set; }

}