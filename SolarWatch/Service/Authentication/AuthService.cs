using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Service.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
    {
        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            // If the specified role does not exist, you can handle this error as needed.
            // You might want to log an error or return an error message.
            // For now, I'll just return an error message.
            Console.WriteLine("Role Doesn't exist");
            return new AuthResult(false, email, username, "Role does not exist", "");
        }

        var user = new IdentityUser { UserName = username, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, username, role);
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(user, role);

        if (!addToRoleResult.Succeeded)
        {
            // If adding the user to the role fails, return an error
            //return new AuthResult(false, email, username, "Failed to add user to role", "");
            Console.WriteLine($"AddtoResult failed {role}");
            foreach (var error in addToRoleResult.Errors)
            {
                Console.WriteLine(error.Description);
            }

            return InvalidRole(role);
        }

        return new AuthResult(true, email, username, "", role);
    }

    private static AuthResult FailedRegistration(IdentityResult result, string email, string username, string role)
    {
        var authResult = new AuthResult(false, email, username, "", role);

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }

    private static AuthResult InvalidRole(string role)
    {
        var result = new AuthResult(false, "", "", "", role);
        result.ErrorMessages.Add("Bad credentials", "Invalid Role");
        return result;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);
        if (managedUser == null)
        {
            return InvalidEmail(email);
        }
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }
        
        var userRole = await _userManager.GetRolesAsync(managedUser);
        var accessToken = _tokenService.CreateToken(managedUser, userRole.Count>0? userRole[0] : "User");
        return new AuthResult(true, managedUser.Email, managedUser.UserName, accessToken, userRole.Count>0? userRole[0] : "User");
    }
    
    private static AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult(false, email, "", "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }

    private static AuthResult InvalidPassword(string email, string userName)
    {
        var result = new AuthResult(false, email, userName, "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }

}