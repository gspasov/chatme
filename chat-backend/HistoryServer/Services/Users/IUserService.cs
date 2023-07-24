using HistoryServer.Models;
using ErrorOr;

namespace HistoryServer.Services.Users;
public interface IUserService
{
    Task<ErrorOr<Created>> CreateUser(User user);
    Task<ErrorOr<User>> GetUser(string id);
    Task<ErrorOr<List<User>>> GetUsers();
    Task<ErrorOr<User>> DisconnectUser(string userId);
    Task<ErrorOr<Deleted>> DeleteUser(string id);
}
