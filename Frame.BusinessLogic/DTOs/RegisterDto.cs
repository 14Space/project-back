using System.ComponentModel.DataAnnotations;

namespace Frame.BusinessLogic.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}