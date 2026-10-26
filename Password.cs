using System.ComponentModel.DataAnnotations;

namespace MyApiProject
{
    public class Password
    {
        
        public required string Password_ { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
    }
}
