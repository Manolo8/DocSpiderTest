using System.ComponentModel.DataAnnotations;

namespace Core.Application.Users.Dtos.Plain {
    public class UserCredentialsDto {
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(48, MinimumLength = 4)]
        public string Password { get; set; }
    }
}