using Microsoft.Data.Sqlite;

public class SeatReservationRepository
{
    private readonly string _connectionString = "Data Source=database.db";

    public void AddReservation(SeatReservation seatReservation)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO SeatReservations (SeatId, FlightId, UserId, Price)
            VALUES ($seatId, $flightId, $userId, $price)
        ";

        command.Parameters.AddWithValue("$seatId", seatReservation.SeatId);
        command.Parameters.AddWithValue("$flightId", seatReservation.FlightId);
        command.Parameters.AddWithValue("$userId", seatReservation.UserId);
        command.Parameters.AddWithValue("$price", seatReservation.Price);
        command.ExecuteNonQuery();
    }

    public List<SeatReservation> GetSeatReservationsByFlightId(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM SeatReservations
            WHERE id = FlightId
        ";

        List<SeatReservation> reservations = new();
        using var reader = command.ExecuteReader();
        while(reader.Read())
        {
            SeatReservation reservation = new SeatReservation
            {
                Id = reader.GetInt32(0),
                SeatId = reader.GetInt32(1),
                FlightId = reader.GetInt32(2),
                Price = reader.GetFloat(3)
            };
            reservations.Add(reservation);
        }
        return reservations;
    }
}
