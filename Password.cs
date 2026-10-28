using System.ComponentModel.DataAnnotations;

namespace MyApiProject
{
    public class Password
    {
        
        //In C#, property names don't end with "_". A clearer name is, for example, Value
        //(a property can't have the same name as its class, so Password isn't possible here)
        public required string Password_ { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
    }
}
