using Microsoft.AspNetCore.Identity;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Services.Helpers;

namespace SmartSpaceControl.Services;

public interface IUserService
{
    public Task RegisterUser(UserRegisterModel userRegisterModel);
}

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task RegisterUser(UserRegisterModel userRegisterModel)
    {
        User? existingUser = await _userManager.FindByEmailAsync(userRegisterModel.Email);
        if (existingUser != null)
        {
            throw new Exception("User with " + userRegisterModel.Email + " email already registered!");
        }
        User newUser = new User()
        {
            Email = userRegisterModel.Email,
            UserName = string.IsNullOrEmpty(userRegisterModel.UserName) ? userRegisterModel.Email : userRegisterModel.UserName
        };
        var result = await _userManager.CreateAsync(newUser);
        if (result.Succeeded)
        {
            result = await _userManager.AddPasswordAsync(newUser, userRegisterModel.Password);
            if (!result.Succeeded) 
            {
                CheckHelper.ThrowResultExeptions(result);
            }
        }
        else
        {
            CheckHelper.ThrowResultExeptions(result);
        }
        
    }
}
