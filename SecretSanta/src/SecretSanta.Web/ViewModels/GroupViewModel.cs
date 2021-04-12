using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Web.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; internal set; }

        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; } = "";
    }
}
