﻿using System.ComponentModel;

namespace Coinage.Web.Models.Customers
{
    public class RegisterModel
    {
        public int Id { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string Phone { get; set; }
    }
}