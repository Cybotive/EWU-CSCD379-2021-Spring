using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Web.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";
    }
}
