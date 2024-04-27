using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs_02.Dtos
{
	public class LoginDto
	{
		[Required]
		[EmailAddress]
        public string Email { get; set; } = null!;


		[Required]
		public string Password { get; set; } = null!;
	}
}
