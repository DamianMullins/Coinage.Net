using System.ComponentModel;

namespace Coinage.Web.Models.Customer
{
    public class RegisterModel
    {
        public int Id { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }
    }
}