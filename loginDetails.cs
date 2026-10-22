using System.ComponentModel.DataAnnotations;

namespace MyApiProject.Controllers
{
    public class LoginDetails
    {
        [EmailAddress]

        public required string Email { get; set; }

        [StringLength(20, MinimumLength = 4, ErrorMessage = "הסיסמא חייבת להיות לפחות 4 תווים.")]
        public required string  Password { get; set; }
        

       
        public string Title { get; set; }
        public  string Body { get; set; }
    }
}
