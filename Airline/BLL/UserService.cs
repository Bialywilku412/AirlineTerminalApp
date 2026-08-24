public class UserService
{
    private readonly UserRepository _repository;

    public UserService(UserRepository repository)
    {
        _repository = repository;
    }

    public Result<bool> RegisterUser(User user)
    {
        List<User> users = _repository.GetAllUsers();
        foreach(User u in users)
        {
            if(user.Login == u.Login)
            {
                return new Result<bool>(false, "This username already exists. Try again.");
            }
        }

        if (user.Password.Length < 7)
            return new Result<bool>(false, "Password is to short. Try again.");

        _repository.RegisterUser(user);
        return new Result<bool>(true, "Account createad successfully");
    }

    public Result<List<User>> GetAllUsers()
    {
        List<User> users = _repository.GetAllUsers();
        if (users.Count == 0)
            return new Result<List<User>>(false, "No users");

        return new Result<List<User>>(true,"succes", users);
    }

    public Result<User> GetUserByLogin(string login)
    {
        List<User> users = _repository.GetAllUsers();
        User user = users.FirstOrDefault(x => x.Login == login);
        if (user == null)
            return new Result<User>(false, "User does not exist.");
        else
            return new Result<User>(true, "succes", user);
    }

    public Result<bool> LogingIn(string login, string password)
    {
        Result<User> foundUser = GetUserByLogin(login);
        if (foundUser.Data == null)
            return new Result<bool>(false, "User with this username does not exist. Try again.");
        else if (foundUser.Data.Password != password)
            return new Result<bool>(false, "Incorrect password");
        else
            return new Result<bool>(true, "succes");
    }
}