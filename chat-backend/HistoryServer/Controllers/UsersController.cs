using HistoryServer.ApiContracts.User;
using HistoryServer.Models;
using HistoryServer.Services.Users;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HistoryServer.Controllers;

public class UsersController : ApiController
{
    private readonly IUserService _userService;


    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var user = new User(
            id: request.Id,
            username: request.Username,
            isOnline: request.IsOnline
        );
        ErrorOr<Created> createUserResult = await _userService.CreateUser(user);

        return createUserResult.Match(
            created => CreatedAtGetUser(user),
            errors => Problem(errors)
        );
    }

    [HttpGet("{userId}/disconnect")]
    public async Task<IActionResult> DisconnectUser(string userId)
    {
        ErrorOr<User> updateUserResult = await _userService.DisconnectUser(userId);

        return updateUserResult.Match(
            result => CreatedAtGetUser(result),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers() 
    {
        ErrorOr<List<User>> getUsersResult = await _userService.GetUsers();

        return getUsersResult.Match(
            users => Ok(users.Select(user => MapUserResponse(user))),
            errors => Problem()
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        ErrorOr<User> getUserResult = await _userService.GetUser(id);

        return getUserResult.Match(
            user => Ok(MapUserResponse(user)),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        ErrorOr<Deleted> deleteUserResponse = await _userService.DeleteUser(id);

        return deleteUserResponse.Match<IActionResult>(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }

    private static UserResponse MapUserResponse(User user)
    {
        return new UserResponse(
            Id: user.Id,
            Username: user.Username,
            IsOnline: user.IsOnline,
            CreatedAt: user.CreatedAt,
            UpdatedAt: user.UpdatedAt
        );
    }

    private CreatedAtActionResult CreatedAtGetUser(User user)
    {
        return CreatedAtAction(
            actionName: nameof(GetUser),
            routeValues: new { id = user.Id },
            value: MapUserResponse(user)
        );
    }
}

