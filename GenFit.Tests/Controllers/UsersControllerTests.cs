using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using GenFit.API.Controllers.V1;
using GenFit.Application.Services;
using GenFit.Application.Common;
using GenFit.Application.DTOs;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GenFit.Tests.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<UsersController>> _mockLogger;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<UsersController>>();
        _controller = new UsersController(_mockUserService.Object, _mockLogger.Object);
        
        // Configurar o contexto HTTP para o controller
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "http";
        httpContext.Request.Host = new HostString("localhost", 5000);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetUsers_ReturnsOk_WithPagedResult()
    {
        // Arrange
        var parameters = new PaginationParameters { PageNumber = 1, PageSize = 10 };
        var expectedResult = new PagedResult<UserDto>
        {
            Items = new List<UserDto>(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _mockUserService.Setup(s => s.GetUsersAsync(It.IsAny<PaginationParameters>(), It.IsAny<string>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetUsers(parameters);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<PagedResult<UserDto>>(okResult.Value);
        Assert.Equal(1, returnValue.PageNumber);
        Assert.Equal(10, returnValue.PageSize);
    }

    [Fact]
    public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 999;
        _mockUserService.Setup(s => s.GetUserByIdAsync(userId))
            .ReturnsAsync((UserDto?)null);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetUser_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new UserDto
        {
            Id = userId,
            Nome = "Test User",
            Email = "test@example.com",
            Role = "candidate"
        };

        _mockUserService.Setup(s => s.GetUserByIdAsync(userId))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userId, returnValue.Id);
        Assert.Equal("Test User", returnValue.Nome);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        _mockUserService.Setup(s => s.DeleteUserAsync(userId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 999;
        _mockUserService.Setup(s => s.DeleteUserAsync(userId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
