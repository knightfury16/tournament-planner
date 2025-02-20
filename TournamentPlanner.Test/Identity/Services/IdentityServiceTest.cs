namespace TournamentPlanner.Test.Identity.Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Domain.Exceptions;
using TournamentPlanner.Identity;
using TournamentPlanner.Identity.Model;
using Xunit;

public class IdentityServiceTest
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
    private readonly IdentityService _identityServiceMoq;

    public IdentityServiceTest()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!
        )!;

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            _mockUserManager.Object,
            contextAccessor.Object,
            userPrincipalFactory.Object,
            null!,
            null!,
            null!,
            null!
        )!;

        var roleStore = new Mock<IRoleStore<IdentityRole>>();
        _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
            roleStore.Object,
            null!,
            null!,
            null!,
            null!
        )!;

        _identityServiceMoq = new IdentityService(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockRoleManager.Object
        );
    }

    [Fact]
    public async Task RegisterApplicationUser_WithValidData_ReturnsTrue()
    {
        // Arrange
        var userDto = new ApplicationUserDto
        {
            Email = "test@test.com",
            Password = "Password123!",
            UserName = "testuser",
            PhoneNumber = "1234567890",
            DomainUserId = 1,
        };

        var applicationUser = new ApplicationUser
        {
            Email = userDto.Email,
            PasswordHash = userDto.Password,
            PhoneNumber = userDto.PhoneNumber,
            DomainUserId = userDto.DomainUserId,
        };
        var claim = new Claim("Email", "test@gmail.com");

        _mockUserManager
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), userDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(applicationUser);

        _mockUserManager
            .Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _identityServiceMoq.RegisterApplicationUserAsync(userDto);
        _mockUserManager.Verify(
            x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
            Times.Once
        );

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task RegisterApplicationUserAndSignIn_WithValidData_ReturnsTrue()
    {
        // Arrange
        var userDto = new ApplicationUserDto
        {
            Email = "test@test.com",
            Password = "Password123!",
            UserName = "testuser",
            PhoneNumber = "1234567890",
            DomainUserId = 1,
        };

        var applicationUser = new ApplicationUser
        {
            Email = userDto.Email,
            PasswordHash = userDto.Password,
            PhoneNumber = userDto.PhoneNumber,
            DomainUserId = userDto.DomainUserId,
        };
        var claim = new Claim("Email", "test@gmail.com");

        _mockUserManager
            .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), userDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(applicationUser);

        _mockUserManager
            .Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager
            .Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockSignInManager.Setup(x =>
            x.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), It.IsAny<string>())
        );

        // Act
        var result = await _identityServiceMoq.RegisterApplicationUserAndSigninAsync(userDto);
        _mockUserManager.Verify(
            x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
            Times.Once
        );

        // Assert
        Assert.True(result);
        _mockSignInManager.Verify(
            x => x.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), It.IsAny<string>()),
            Times.Once
        );
    }

    [Fact]
    public async Task LoginApplicationUser_WithValidCredentials_ReturnsTrue()
    {
        // Arrange
        var userDto = new ApplicationUserDto { Email = "test@test.com", Password = "Password123!" };

        var appUser = new ApplicationUser { Email = userDto.Email };

        _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email)).ReturnsAsync(appUser);

        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(appUser, userDto.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await _identityServiceMoq.LoginApplicationUserAsync(userDto);

        // Assert
        Assert.True(result);
        _mockSignInManager.Verify(
            x => x.PasswordSignInAsync(appUser, userDto.Password, false, false),
            Times.Once
        );
        _mockSignInManager.Verify(x => x.SignInAsync(appUser, It.IsAny<bool>(), null), Times.Once);
    }

    [Fact]
    public async Task LoginApplicationUser_WithInvalidEmail_ThrowsNotFoundException()
    {
        // Arrange
        var userDto = new ApplicationUserDto
        {
            Email = "nonexistent@test.com",
            Password = "Password123!",
        };

        _mockUserManager
            .Setup(x => x.FindByEmailAsync(userDto.Email))
            .ReturnsAsync((ApplicationUser)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _identityServiceMoq.LoginApplicationUserAsync(userDto)
        );
    }

    [Fact]
    public async Task LoginApplicationUser_WithInvalidCredential_ThrowsUnauthorizeException()
    {
        // Arrange
        var userDto = new ApplicationUserDto
        {
            Email = "nonexistent@test.com",
            Password = "Password123!",
        };

        var applicationUser = new ApplicationUser
        {
            Email = userDto.Email,
            PasswordHash = userDto.Password,
        };

        _mockUserManager
            .Setup(x => x.FindByEmailAsync(userDto.Email))
            .ReturnsAsync(applicationUser);

        _mockSignInManager
            .Setup(x => x.PasswordSignInAsync(applicationUser, userDto.Password, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(
            () => _identityServiceMoq.LoginApplicationUserAsync(userDto)
        );
    }

    // [Fact]
    //
    // public async Task AddRoleToApplicationUser_WithValidData_Succeeds()
    // {
    //     // Arrange
    //     var email = "test@test.com";
    //     var roleName = "Admin";
    //     var user = new ApplicationUser { Email = email };
    //
    //     _mockUserManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
    //
    //     _mockRoleManager.Setup(x => x.RoleExistsAsync(roleName)).ReturnsAsync(true);
    //
    //     _mockUserManager
    //         .Setup(x => x.AddToRoleAsync(user, roleName))
    //         .ReturnsAsync(IdentityResult.Success);
    //
    //     // Act & Assert
    //     await _identityServiceMoq.AddRoleToApplicationUserAsync(email, roleName);
    //
    //     _mockUserManager.Verify(x => x.AddToRoleAsync(user, roleName), Times.Once);
    // }
    //
    // [Fact]
    // public async Task GetAllClaimsOfApplicationUser_WithValidEmail_ReturnsClaims()
    // {
    //     // Arrange
    //     var email = "test@test.com";
    //     var user = new ApplicationUser { Email = email };
    //     var claims = new List<Claim>
    //     {
    //         new Claim(ClaimTypes.Email, email),
    //         new Claim(ClaimTypes.Name, "Test User"),
    //     };
    //
    //     _mockUserManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
    //
    //     _mockUserManager.Setup(x => x.GetClaimsAsync(user)).ReturnsAsync(claims);
    //
    //     // Act
    //     var result = await _identityServiceMoq.GetAllClaimsOfApplicationUser(email);
    //
    //     // Assert
    //     Assert.Equal(claims.Count, result.Count);
    //     Assert.Equal(claims[0].Type, result[0].Type);
    //     Assert.Equal(claims[0].Value, result[0].Value);
    // }
}
