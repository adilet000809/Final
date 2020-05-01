using System;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; } 
    }
}