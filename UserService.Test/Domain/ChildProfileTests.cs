// UserService.Test/Domain/ChildProfileTests.cs
using FluentAssertions;
using UserService.Domain.Exceptions;
using UserService.Domain.Models;
using Xunit;

namespace UserService.Test.Domain;

public class ChildProfileTests
{
    [Fact]
    public void Create_WithValidAge_CreatesProfile()
    {
        // Act
        var profile = ChildProfile.Create(Guid.NewGuid(), "Albi", 8, "avatar-1");

        // Assert
        profile.Should().NotBeNull();
        profile.DisplayName.Should().Be("Albi");
        profile.Age.Should().Be(8);
        profile.CurrentLevel.Should().Be(1);
    }

    [Fact]
    public void Create_WithAgeTooYoung_ThrowsBusinessException()
    {
        // Act
        var act = () => ChildProfile.Create(Guid.NewGuid(), "Albi", 3, "avatar-1");

        // Assert
        act.Should().Throw<BusinessException>()
            .WithMessage("*4*");
    }

    [Fact]
    public void Create_WithAgeTooOld_ThrowsBusinessException()
    {
        // Act
        var act = () => ChildProfile.Create(Guid.NewGuid(), "Albi", 13, "avatar-1");

        // Assert
        act.Should().Throw<BusinessException>()
            .WithMessage("*12*");
    }

    [Fact]
    public void UpdateLevel_WithHigherLevel_UpdatesLevel()
    {
        // Arrange
        var profile = ChildProfile.Create(Guid.NewGuid(), "Albi", 8, "avatar-1");

        // Act
        profile.UpdateLevel(2);

        // Assert
        profile.CurrentLevel.Should().Be(2);
    }

    [Fact]
    public void UpdateLevel_WithLowerLevel_ThrowsBusinessException()
    {
        // Arrange
        var profile = ChildProfile.Create(Guid.NewGuid(), "Albi", 8, "avatar-1");

        // Act
        var act = () => profile.UpdateLevel(0);

        // Assert
        act.Should().Throw<BusinessException>();
    }
}