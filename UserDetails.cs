using System;
using System.ComponentModel.DataAnnotations;
public class UserDetails
{
    [EmailAddress]
    public required string Email { get; set; }
   

    //[StringLength(20, MinimumLength = 4, ErrorMessage = "הסיסמא חייבת להיות לפחות 4 תווים.")]
    public required string Password { get; set; }
   

    [StringLength(20, MinimumLength = 2, ErrorMessage = "שם פרטי חייב להיות לפחות 2 תווים.")]
    public required string FirstName { get; set; }
   

    [StringLength(20, MinimumLength = 2, ErrorMessage = "שם משפחה חייב להיות לפחות 2 תווים.")]
    public required string LastName { get; set; }

    public  int UserId { get; set; }

    //Are Title and Body needed? They look like they came from a fetch example and are not user details,
    //but now they are saved in file.txt for every user
    public  string Title { get; set; }
    public  string Body { get; set; }
}

