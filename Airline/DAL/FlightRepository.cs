using Microsoft.Data.Sqlite;

public class FlightRepository
{
    private readonly string _connectionString = "Data Source=database.db";

    public void AddFlight(Flight flight)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Flights (Origin, Destination, PlaneId)
            VALUES ($origin, $destination, $planeId);";
        command.Parameters.AddWithValue("$origin", flight.Origin.ToString());
        command.Parameters.AddWithValue("$destination", flight.Destination.ToString());
        command.Parameters.AddWithValue("$planeId", flight.AssignedPlaneId);
        command.ExecuteNonQuery();
    }

    public List<Flight> ShowAllFlights()
    {
        var flights = new List<Flight>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Origin, Destination, PlaneId FROM Flights;";

        using var reader = command.ExecuteReader();
        while(reader.Read())
        {
            var flight = new Flight
            {
                ID = reader.GetInt32(0),
                Origin = Enum.Parse<Airports>(reader.GetString(1)),
                Destination = Enum.Parse<Airports>(reader.GetString(2)),
                AssignedPlaneId = reader.GetInt32(3)
            };
            flights.Add(flight);
        }

        return flights;
    }
}