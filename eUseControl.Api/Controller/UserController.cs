using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Api.Domain;

namespace eUseControl.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private static List<User> _users = new();
    private static int _nextId = 1;

    [HttpGet("all")]
    public IActionResult GetAllUsers()
    {
        return Ok(_users);
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new { Message = "User with ID " + id + " not found" });
        }
        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        // Exercise 3: Validation
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            return BadRequest(new { Message = "Username cannot be empty" });
        }

        user.Id = _nextId++;
        user.CreatedAt = DateTime.UtcNow;
        _users.Add(user);
        return Created("", user);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        user.Username = updatedUser.Username;
        user.Email = updatedUser.Email;
        user.IsActive = updatedUser.IsActive; // Exercise 1

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        _users.Remove(user);
        return NoContent();
    }
}