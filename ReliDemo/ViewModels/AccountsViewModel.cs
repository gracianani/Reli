using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ReliDemo.Models;
using ReliDemo.Infrastructure.Services;
using ReliDemo.Infrastructure.Repositories;

namespace ReliDemo.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "姓名")]
        public string 姓名 { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "当前密码")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }

    public class LogInViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }

    public class ProfileViewModel
    {
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "姓名")]
        public string 姓名 { get; set; }

        [Display(Name = "重置密码")]
        public string 密码 { get; set; }

        public int UserId { get; set; }

        public User TheUser
        {
            get
            {
                var repo = new UserRepository();
                return (User)repo.Find(UserId);
            }
        }

        public List<IdAndNameAndChecked> Authorizes
        {
            get
            {
                var results = new List<IdAndNameAndChecked>();
                foreach (var role in Enum.GetValues(typeof(Role)))
                {
                    results.Add(
                        new IdAndNameAndChecked() { 
                            Id = (int)role, 
                            Name = role.ToString(), 
                            Checked = TheUser.权限们.IndexOf(role.ToString()) >=0 });
                }
                return results;
            }
        }
    }
}