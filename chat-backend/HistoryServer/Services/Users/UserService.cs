using HistoryServer.Data;
using HistoryServer.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using HistoryServer.ServiceErrors;

namespace HistoryServer.Services.Users;
public class UserService : IUserService
{
    private readonly DataContext _dataContext;

    public UserService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<ErrorOr<Created>> CreateUser(User user)
    {
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        return Result.Created;
    }

    public async Task<ErrorOr<User>> GetUser(string id)
    {
        var user = await _dataContext.Users.FindAsync(id);
        if (user == null) return Errors.User.NotFound;

        return user;
    }

    public async Task<ErrorOr<List<User>>> GetUsers()
    {
        return await _dataContext.Users.ToListAsync();
    }

    public async Task<ErrorOr<User>> DisconnectUser(string userId)
    {
        var user = await _dataContext.Users.FindAsync(userId);

        if (user == null)
        {
            return Errors.User.NotFound;
        }

        user.IsOnline = false;
        await _dataContext.SaveChangesAsync();

        return user;
    }

    public async Task<ErrorOr<Deleted>> DeleteUser(string id)
    {
        var user = new User(id: id);
        _dataContext.Users.Attach(user);
        _dataContext.Users.Remove(user);

        await _dataContext.SaveChangesAsync();

        return Result.Deleted;
    }
}
