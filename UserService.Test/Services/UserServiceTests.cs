using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.Application.DTOs.Requests;
using UserService.Application.Repositories.Interfaces;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Exceptions;
using UserService.Domain.Models;
using Xunit;

namespace UserService.Test.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IChildProfileRepository> _childProfileRepositoryMock;
    private readonly Mock<IParentChildRelationRepository> _relationRepositoryMock;
    private readonly Mock<IKeyCloakService> _keycloakServiceMock;
    private readonly Mock<ILogger<Application.Services.UserService>> _loggerMock;
    private readonly Application.Services.UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _childProfileRepositoryMock = new Mock<IChildProfileRepository>();
        _relationRepositoryMock = new Mock<IParentChildRelationRepository>();
        _keycloakServiceMock = new Mock<IKeyCloakService>();
        _loggerMock = new Mock<ILogger<Application.Services.UserService>>();

        _userService = new Application.Services.UserService(
            _userRepositoryMock.Object,
            _childProfileRepositoryMock.Object,
            _relationRepositoryMock.Object,
            _keycloakServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterParentAsync_WhenEmailExists_ThrowsBusinessException()
    {
        // Arrange
        var request = new RegisterParentRequest("test@test.com", "Password1!", "Password1!");

        _userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(request.Email, default))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _userService.RegisterParentAsync(request);

        // Assert
        await act.Should().ThrowAsync<BusinessException>()
            .WithMessage("*email*");
    }

    [Fact]
    public async Task RegisterParentAsync_WhenEmailIsNew_ReturnsAuthResponse()
    {
        // Arrange
        var request = new RegisterParentRequest("new@test.com", "Password1!", "Password1!");

        _userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(request.Email, default))
            .ReturnsAsync(false);

        _keycloakServiceMock
            .Setup(x => x.RegisterUserAsync(request.Email, request.Password, "Parent", default))
            .ReturnsAsync("keycloak-id-123");

        _keycloakServiceMock
            .Setup(x => x.GetTokenAsync(request.Email, request.Password, default))
            .ReturnsAsync("jwt-token-123");

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<User>(), default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.RegisterParentAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.AccesToken.Should().Be("jwt-token-123");
        result.TokenType.Should().Be("Bearer");
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, default))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _userService.GetByIdAsync(userId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ReturnsUserResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.CreateParent("keycloak-id", "parent@test.com");

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, default))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("parent@test.com");
        result.Role.Should().Be("Parent");
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new LoginRequest("notfound@test.com", "Password1!");

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, default))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _userService.LoginAsync(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    // Fix testi i dytë — ndrysho mesazhin
    [Fact]
    public async Task LoginAsync_WhenUserIsInactive_ThrowsBusinessException()
    {
        var request = new LoginRequest("inactive@test.com", "Password1!");
        var user = User.CreateParent("keycloak-id", "inactive@test.com");
        user.SoftDelete();

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email, default))
            .ReturnsAsync(user);

        var act = async () => await _userService.LoginAsync(request);

        await act.Should().ThrowAsync<BusinessException>()
            .WithMessage("*caktivizua*"); // ← ndrysho
    }

    [Fact]
    public async Task DeactivateUserAsync_WhenUserExists_SoftDeletesUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.CreateParent("keycloak-id", "parent@test.com");

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, default))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>(), default))
            .Returns(Task.CompletedTask);

        _keycloakServiceMock
            .Setup(x => x.DeleteUserAsync(It.IsAny<string>(), default))
            .Returns(Task.CompletedTask);

        // Act
        await _userService.DeactivateUserAsync(userId);

        // Assert
        user.DeletedAt.Should().NotBeNull();
        user.IsActive.Should().BeFalse();
    }
}