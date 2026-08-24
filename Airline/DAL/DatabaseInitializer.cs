using System;
using Microsoft.Data.Sqlite;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string dbPath = "database.db")
    {
        _connectionString = $"Data Source={dbPath}";
    }

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var flightsCommand = connection.CreateCommand();
        flightsCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Flights (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Origin TEXT NOT NULL,
                Destination TEXT NOT NULL
            );";
        flightsCommand.ExecuteNonQuery();

        using var usersCommand = connection.CreateCommand();
        usersCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Login TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                Rank TEXT DEFAULT 'User'
            );";
        usersCommand.ExecuteNonQuery();

        using var planeCommand = connection.CreateCommand();
        planeCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Planes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Model TEXT NOT NULL,
                Capacity INTEGER
            );";
        planeCommand.ExecuteNonQuery();

        using var seatCommand = connection.CreateCommand();
        seatCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Seats (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PlaneId INTEGER NOT NULL,
                Row TEXT NOT NULL,
                Column INTEGER NOT NULL,
                Class TEXT NOT NULL,
                FOREIGN KEY (PlaneId) REFERENCES Planes(ID),
                UNIQUE (PlaneId, Row, Column)
            );";
        seatCommand.ExecuteNonQuery();

    }
    
    public void AddPlane(string model, int capacity)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Planes (Model, Capacity)
            VALUES ($model, $capacity);";

        command.Parameters.AddWithValue("$model", model);
        command.Parameters.AddWithValue("$capacity", capacity);

        command.ExecuteNonQuery();
    }

    public void SeedSeats(int planeId)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string[] rows = { "A", "B", "C", "D", "E", "F" };

        foreach (string row in rows)
        {
            for (int column = 1; column <= 33; column++)
            {
                string seatClass =
                    (column == 16 || column == 17)
                    ? "Preferred"
                    : "Standard";

                using var command = connection.CreateCommand();

                command.CommandText = @"
                    INSERT OR IGNORE INTO Seats
                        (PlaneId, Row, Column, Class)
                    VALUES
                        ($planeId, $row, $column, $class);";

                command.Parameters.AddWithValue("$planeId", planeId);
                command.Parameters.AddWithValue("$row", row);
                command.Parameters.AddWithValue("$column", column);
                command.Parameters.AddWithValue("$class", seatClass);

                command.ExecuteNonQuery();
            }
        }
    }
}