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

        using var flightsCommand = connection.CreateCommand();
        flightsCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Flights (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Origin TEXT NOT NULL,
                Destination TEXT NOT NULL,
                PlaneId INTEGER NOT NULL,
                FOREIGN KEY (PlaneId) REFERENCES Planes(Id)
            );";
        flightsCommand.ExecuteNonQuery();

        using var seatCommand = connection.CreateCommand();
        seatCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Seats (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PlaneId INTEGER NOT NULL,
                Row INTEGER NOT NULL,
                Column TEXT NOT NULL,
                Class TEXT NOT NULL,
                FOREIGN KEY (PlaneId) REFERENCES Planes(Id),
                UNIQUE (PlaneId, Row, Column)
            );";
        seatCommand.ExecuteNonQuery();

        using var seatReservationCommand = connection.CreateCommand();
        seatReservationCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS SeatReservations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SeatId INTEGER NOT NULL,
                FlightId INTEGER NOT NULL,
                UserId INTEGER NOT NULL,
                Price FLOAT NOT NULL,
                FOREIGN KEY (SeatId) REFERENCES Seats(Id),
                FOREIGN KEY (FlightId) REFERENCES Flights(Id),
                FOREIGN KEY (UserId) REFERENCES Users(Id)
            );";
        seatReservationCommand.ExecuteNonQuery();

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

        string[] columns = { "A", "B", "C", "D", "E", "F" };

        foreach (string column in columns)
        {
            for (int row = 1; row <= 33; row++)
            {
                string seatClass =
                    (row == 16 || row == 17)
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