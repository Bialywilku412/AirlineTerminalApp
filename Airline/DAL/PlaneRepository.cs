using Microsoft.Data.Sqlite;

public class PlaneRepository
{
    private readonly string _connectionString = "Data Source=database.db";

    public List<Plane> ShowAllPlanes()
    {
        var planes = new List<Plane>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Model, Capacity FROM Planes;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var plane = new Plane
            {
                ID = reader.GetInt32(0),
                Model = reader.GetString(1),
                Capacity = reader.GetInt32(2)
            };
            planes.Add(plane);
        }

        return planes;
    }
}
