using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6), DataType(DataType.Password)]
        public string UserPassword { get; set; } = string.Empty;

        [Compare(nameof(UserPassword), ErrorMessage = "Password is not matched!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
