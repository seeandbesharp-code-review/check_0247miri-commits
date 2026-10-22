using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace MyApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {

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

        [HttpPut]
        public ActionResult<UserDetails> Put([FromBody] UserDetails userToUpdate)
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
                    if (user.UserId == userToUpdate.UserId)
                        textToReplace = currentUserInFile;
                }
            }

            if (textToReplace != string.Empty)
            {
                
                string text = System.IO.File.ReadAllText("file.txt");
                text = text.Replace(textToReplace, JsonSerializer.Serialize(userToUpdate));
                System.IO.File.WriteAllText("file.txt", text);
            }
            return Ok(userToUpdate);

        }
    }
}
