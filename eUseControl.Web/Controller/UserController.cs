using Microsoft.AspNetCore.Mvc;
using eUseControl.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eUseControl.Web.Controller
{
    [Route(template: "api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // In-memory storage for users (for demonstration purposes)
        private static List<User> _users = new();
        private static int _nextId = 1;

        [HttpGet(template: "all")]
        public IActionResult GetAllUsers()
        {
            return Ok(_users);
        }

        [HttpGet(template: "{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound(new { Message = $"User with ID {id} not found" });

            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            // Exercise 3: Validation — 400 if Username is empty
            if (string.IsNullOrWhiteSpace(user.Username))
                return BadRequest(new { Message = "Username cannot be empty" });

            user.Id = _nextId++;
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            _users.Add(user);

            return Created($"/api/user/{user.Id}", user);
        }

        [HttpPut(template: "{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);

            if (existingUser == null)
                return NotFound(new { Message = $"User with ID {id} not found" });

            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;
            existingUser.IsActive = updatedUser.IsActive;  // Exercise 1

            return Ok(existingUser);
        }

        [HttpDelete(template: "{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound(new { Message = $"User with ID {id} not found" });

            _users.Remove(user);

            return NoContent();
        }
    }
}
