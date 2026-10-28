using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Zxcvbn;

namespace MyApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //"My" doesn't say what this controller handles. A clearer name is, for example, UsersController
    //(the route would then be api/Users, so the fetch calls in wwwroot would change too)
    public class MyController : ControllerBase
    {

        //The id should come from the route (GET api/My/5), so write [HttpGet("{id}")].
        //Now it is read from the query string (api/My?id=5)
        [HttpGet]
        public ActionResult<UserDetails> Get(int id)
        {
            using (StreamReader reader = System.IO.File.OpenText("file.txt"))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    UserDetails user = JsonSerializer.Deserialize<UserDetails>(currentUserInFile);
                    if (user.UserId == id)
                        return Ok(user);

                }
                //When no user has this id, the request itself is fine, so return NotFound() (or NoContent()), not BadRequest
                return BadRequest("A user hadn't found");
            }
        }
        [HttpPost("register")]
        public ActionResult<UserDetails> Post([FromBody] UserDetails userDetails)
        {
            if (!ModelState.IsValid) // Check if the validation checks passed
            {
                return BadRequest(ModelState);
            }
            var result = Core.EvaluatePassword(userDetails.Password);

            if (result.Score < 3)
                return BadRequest("Check your password strength");
            // נבצע בדיקה שהמייל אינו קיים במערכת עדיין
            using (StreamReader reader = System.IO.File.OpenText("file.txt"))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    UserDetails user = JsonSerializer.Deserialize<UserDetails>(currentUserInFile);
                    if (user.Email == userDetails.Email)
                        return BadRequest("The email adress you entered is inside the system. You registered already or have a typing error");

                }
            }
            int numberOfUsers = System.IO.File.ReadLines("file.txt").Count();
            userDetails.UserId = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(userDetails);
            System.IO.File.AppendAllText("file.txt", userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Get), new { id = userDetails.UserId }, userDetails);




        }
        [HttpPost("passwordStrength")]
        public IActionResult Post([FromBody] Password password)
        {

            var result = Core.EvaluatePassword(password.Password_);

            int score = result.Score;                  
            var warning = result.Feedback.Warning;
            var suggestions = result.Feedback.Suggestions; 
            if (result.Score < 3)
            {
                return BadRequest(new
                {
                    message = " Your password is too weak",
                    score = result.Score,
                    warning = result.Feedback.Warning,
                    suggestions = result.Feedback.Suggestions
                });
            }
            return Ok(new
            {
                message = " Good password ",
                score = result.Score,
                
            });
        }

        [HttpPost("login")]
        public ActionResult<UserDetails> Post([FromBody] LoginDetails loginDetails)
        {
            if (!ModelState.IsValid) //  Check if the validation checks passed
            {
                return BadRequest(ModelState);
            }
            using (StreamReader reader = System.IO.File.OpenText("file.txt"))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    UserDetails user = JsonSerializer.Deserialize<UserDetails>(currentUserInFile);
                    if (user.Email == loginDetails.Email && user.Password == loginDetails.Password)
                        return Ok(user);

                }
            }
            return NotFound("User Details don't exist");




        }

        [HttpPut("{id}")]
        public ActionResult<UserDetails> Put(int id,[FromBody] UserDetails userToUpdate)
        {

            if (!ModelState.IsValid) // Check if the validation checks passed
            {
                return BadRequest(ModelState);
            }
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText("file.txt"))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    UserDetails user = JsonSerializer.Deserialize<UserDetails>(currentUserInFile);
                    if (user.UserId == id)
                    {
                        textToReplace = currentUserInFile;
                        //fix: Found- break
                        break;
                    }
                }
            }

            if (textToReplace != string.Empty)
            {
                //fix: Keep the id, otherwise the updated user is saved without it
                userToUpdate.UserId = id;
                string text = System.IO.File.ReadAllText("file.txt");
                text = text.Replace(textToReplace, JsonSerializer.Serialize(userToUpdate));
                System.IO.File.WriteAllText("file.txt", text);
            }
            return Ok(userToUpdate);

        }
    }
}
