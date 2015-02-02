using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReliMobileAdmin.Models
{
    public class CreateWarningViewModel
    {
        [Required(ErrorMessage = "请填写预警标题")]
        public string WarningTitle { get; set; }

        [Required(ErrorMessage = "发送消息不能为空")]
        public string WarningContent { get; set; }
        public bool? IsCreated { get; set; }
    }
}