﻿using System.ComponentModel.DataAnnotations;

namespace MockSchoolManagement.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="邮箱地址")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="确认密码")]
        [Compare("Password",ErrorMessage ="密码与确认密码不一致，请重新输入。")]
        public string ConfirmPassword { get; set; }
    }
}
