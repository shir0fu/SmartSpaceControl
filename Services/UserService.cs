using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartSpaceControl.Models.Models;
using SmartSpaceControl.Models.Dto;

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
        var existingUser = await _userManager.FindByEmailAsync(userRegisterModel.Email);
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

        result = await _userManager.AddPasswordAsync(newUser, userRegisterModel.Password);
    }
}
