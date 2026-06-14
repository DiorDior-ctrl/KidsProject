using FluentAssertions;
using UserService.Domain.Exceptions;
using UserService.Domain.Models;
using Xunit;

namespace UserService.Test.Domain;

public class UserTests
{
    [Fact]
    public void CreateParent_WithValidData_CreatesUser()
    {
        // Act
        var user = User.CreateParent("keycloak-id", "parent@test.com");

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be("parent@test.com");
        user.Role.Should().Be(UserService.Domain.Enums.UserRole.Parent);
        user.IsActive.Should().BeTrue();
        user.DeletedAt.Should().BeNull();
    }

    [Fact]
    public void CreateChild_WithValidData_CreatesUser()
    {
        // Act
        var user = User.CreateChild("keycloak-id", "child@test.com");

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be("child@test.com");
        user.Role.Should().Be(UserService.Domain.Enums.UserRole.Child);
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void SoftDelete_WhenCalled_SetsDeletedAtAndIsActiveFalse()
    {
        // Arrange
        var user = User.CreateParent("keycloak-id", "parent@test.com");

        // Act
        user.SoftDelete();

        // Assert
        user.DeletedAt.Should().NotBeNull();
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_AfterSoftDelete_RestoresUser()
    {
        // Arrange
        var user = User.CreateParent("keycloak-id", "parent@test.com");
        user.SoftDelete();

        // Act
        user.Activate();

        // Assert
        user.DeletedAt.Should().BeNull();
        user.IsActive.Should().BeTrue();
    }
}