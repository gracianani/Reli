using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ReliMobileAdmin.Models
{
    public class CreateMessageViewModel
    {
        public IEnumerable<User> Users { get; set; }
        [Required(ErrorMessage = "请选择联系人")]
        public int SelectedUserId { get; set; }
        [Required(ErrorMessage="发送消息不能为空")]
        public string Message { get; set; }
        public bool? IsCreated { get; set; }
    }
}