using Microsoft.Data.Sqlite;

public class UserRepository
{
    private readonly string _connectionString = "Data Source=database.db";

    public void RegisterUser(User user)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users (Login, PasswordHash)
            VALUES ($login, $passwordHash);";
        command.Parameters.AddWithValue("$login", user.Login);
        command.Parameters.AddWithValue("$passwordHash", user.Password);
        command.ExecuteNonQuery();
    }

    public List<User> GetAllUsers()
    {   
        var users = new List<User>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Users;";

        using var reader = command.ExecuteReader();
        while(reader.Read())
        {
            var user = new User
            {
                Id = reader.GetInt32(0),
                Login = reader.GetString(1),
                Password = reader.GetString(2),
                Rank = reader.GetString(3)
            };
            users.Add(user);
        }

        return users;
    }

   
}