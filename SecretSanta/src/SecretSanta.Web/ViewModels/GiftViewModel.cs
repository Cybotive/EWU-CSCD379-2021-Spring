using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Web.ViewModels
{
    public class GiftViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; } = "";
        [Required]
        [Display(Name = "Description")]
        public string Desc { get; set; } = "";
        [Required]
        [Display(Name = "Gift URL")]
        public Uri? Url { get; set; }
        [Display(Name = "Gift Priority")]
        public int Priority { get; set; } = -1;
        [Display(Name = "Gift Recipient")]
        public UserViewModel? Recipient { get; set; }
    }
}
