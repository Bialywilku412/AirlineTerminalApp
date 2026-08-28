using Microsoft.Data.Sqlite;

public class SeatRepository
{
    private readonly string _connectionString = "Data Source=database.db";

    public Seat GetSeatById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Seats WHERE Id = @Id;";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        Seat seat = null;
        while (reader.Read())
        {
            seat = new Seat
            {
                Id = reader.GetInt32(0),
                PlaneId = reader.GetInt32(1),
                Row = reader.GetInt32(2),
                Column = reader.GetChar(3),
                Class = Enum.Parse<SeatClass>(reader.GetString(4))
            };
        }
        return seat;

    }

    public List<Seat> GetSeatsByPlaneId(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Seats
            WHERE PlaneId = @PlaneId
        ";
        command.Parameters.AddWithValue("@PlaneId", id);

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
