using System.ComponentModel.DataAnnotations;

namespace FootballScoresDbApi.Models.DTOs
{
    public class LoginUserDTO
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Your password should have at least {2} and max {1} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
