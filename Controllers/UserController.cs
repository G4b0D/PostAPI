using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserService _userService;
    public UserController(IConfiguration configuration)
    {
        _userService = new UserService(configuration);
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        IEnumerable<User> users = await _userService.GetUsers();
        if(!users.Any())
        {
            NotFound();
        }
        return Ok(users);
    }

    [HttpGet("/{userId}")]
    public async Task<IActionResult> GetSingleUser(int userId)
    {
        User user = await _userService.GetUserWithId(userId);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> EditUser(UserToEdit user)
    {
        bool result = await _userService.UpdateUser(user);
        if(result)
        {
            return Ok(user);
        }
        return BadRequest();
    }

   
   

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser(UserDto user)
    {
        bool result = await _userService.CreateUser(user);
        if(!result) { return BadRequest(); }
        return Ok();
    }

    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        bool result = await _userService.DeleteUser(userId);
        if(!result) { return BadRequest(); }
        return Ok();
    }
}

