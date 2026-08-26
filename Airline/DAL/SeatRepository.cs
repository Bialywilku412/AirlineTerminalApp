using Microsoft.Data.Sqlite;

public class SeatRepository
{
    private readonly string _connectionString = "Data Source=database.db";

    public List<Seat> GetSeatsByPlaneId(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Seats
            WHERE id = PlaneId
        ";

        List<Seat> seats = new();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Seat seat = new Seat
            {
                Id = reader.GetInt32(0),
                PlaneId = reader.GetInt32(1),
                Row = reader.GetInt32(2),
                Column = reader.GetChar(3),
                Class = Enum.Parse<SeatClass>(reader.GetString(4))
            };
            seats.Add(seat);
        }
        return seats;
    }
}
