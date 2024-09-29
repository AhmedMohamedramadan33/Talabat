using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.Dtos.IdentityDto
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }


    }
}
