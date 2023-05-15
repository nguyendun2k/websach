using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLySach.Models
{
    public class Account
    {
        [Required]
        [Phone(ErrorMessage ="Vui lòng nhập đúng định dạng số điện thoại")]
        public string SoDienThoai { get; set; }
        public string MatKhau { get; set; }
    }
}