using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using PickDuel.Api.Controllers;
using PickDuel.Application.DTOs.Users;
using PickDuel.Application.Mappers;
using PickDuel.Application.Users;
using PickDuel.Domain.Entities;
using PickDuel.Tests.Common;
using PickDuel.Application.Mappers.Interfaces;

namespace PickDuel.Tests.Api.Controllers;

public class UsersControllerTests
{
    private IUserService _userService;

    private IUserMapper _userMapper;

    private UsersController _controller;


    [SetUp]
    public void Setup()
    {
        _userService = Substitute.For<IUserService>();

        _userMapper = Substitute.For<IUserMapper>();

        _controller = new UsersController(
            _userService,
            _userMapper
        );
    }


    [Test]
    public async Task GetUser_ShouldReturnOk_WhenUserExists()
    {
        var user = TestDataFactory.CreateUser();

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            CreatedAt = user.CreatedAt
        };


        _userService
            .GetUserAsync(
                user.Id,
                Arg.Any<CancellationToken>()
            )
            .Returns(user);


        _userMapper
            .ToDto(user)
            .Returns(userDto);


        var response = await _controller.GetUser(
            user.Id,
            CancellationToken.None
        );


        var result = response.Result as OkObjectResult;


        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Value, Is.EqualTo(userDto));
        });
    }


    [Test]
    public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();


        _userService
            .GetUserAsync(
                userId,
                Arg.Any<CancellationToken>()
            )
            .Returns((User?)null);


        var response = await _controller.GetUser(
            userId,
            CancellationToken.None
        );


        Assert.That(
            response.Result,
            Is.TypeOf<NotFoundResult>()
        );
    }


    [Test]
    public async Task CreateUser_ShouldReturnCreated_WhenUserIsValid()
    {
        var request = new CreateUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Username = "johndoe"
        };

        var user = TestDataFactory.CreateUser();

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            CreatedAt = user.CreatedAt
        };


        _userMapper
            .ToEntity(request)
            .Returns(user);


        _userService
            .CreateUserAsync(
                user,
                Arg.Any<CancellationToken>()
            )
            .Returns(user);


        _userMapper
            .ToDto(user)
            .Returns(userDto);


        var response = await _controller.CreateUser(
            request,
            CancellationToken.None
        );


        var result = response.Result as CreatedAtActionResult;


        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ActionName, Is.EqualTo(nameof(UsersController.GetUser)));
            Assert.That(result.Value, Is.EqualTo(userDto));
        });
    }


    [Test]
    public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();

        var request = new UpdateUserRequest
        {
            FirstName = "Updated",
            LastName = "User",
            Email = "updated@test.com",
            Username = "updateduser"
        };


        _userService
            .GetUserAsync(
                userId,
                Arg.Any<CancellationToken>()
            )
            .Returns((User?)null);


        var response = await _controller.UpdateUser(
            userId,
            request,
            CancellationToken.None
        );


        Assert.That(
            response.Result,
            Is.TypeOf<NotFoundResult>()
        );
    }


    [Test]
    public async Task UpdateUser_ShouldReturnOk_WhenUserExists()
    {
        var user = TestDataFactory.CreateUser();

        var request = new UpdateUserRequest
        {
            FirstName = "Updated",
            LastName = "User",
            Email = "updated@test.com",
            Username = "updateduser"
        };

        var dto = new UserDto
        {
            Id = user.Id,
            FirstName = "Updated",
            LastName = "User",
            Email = "updated@test.com",
            Username = "updateduser",
            CreatedAt = user.CreatedAt
        };


        _userService
            .GetUserAsync(
                user.Id,
                Arg.Any<CancellationToken>()
            )
            .Returns(user);


        _userService
            .UpdateUserAsync(
                user,
                Arg.Any<CancellationToken>()
            )
            .Returns(user);


        _userMapper
            .ToDto(user)
            .Returns(dto);


        var response = await _controller.UpdateUser(
            user.Id,
            request,
            CancellationToken.None
        );


        var result = response.Result as OkObjectResult;


        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Value, Is.EqualTo(dto));
        });


        _userMapper
            .Received(1)
            .UpdateEntity(
                user,
                request
            );
    }


    [Test]
    public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();


        _userService
            .GetUserAsync(
                userId,
                Arg.Any<CancellationToken>()
            )
            .Returns((User?)null);


        var response = await _controller.DeleteUser(
            userId,
            CancellationToken.None
        );


        Assert.That(
            response,
            Is.TypeOf<NotFoundResult>()
        );
    }


    [Test]
    public async Task DeleteUser_ShouldReturnNoContent_WhenUserExists()
    {
        var user = TestDataFactory.CreateUser();


        _userService
            .GetUserAsync(
                user.Id,
                Arg.Any<CancellationToken>()
            )
            .Returns(user);


        var response = await _controller.DeleteUser(
            user.Id,
            CancellationToken.None
        );


        Assert.That(
            response,
            Is.TypeOf<NoContentResult>()
        );


        await _userService
            .Received(1)
            .DeleteUserAsync(
                user.Id,
                Arg.Any<CancellationToken>()
            );
    }
}