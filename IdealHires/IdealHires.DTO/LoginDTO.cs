using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdealHires.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "User Name is required.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string UserType { get; set; }
    }
}