using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBenhVien.Models
{
    public class NhanVien
    {
        public int MaNV { get; set; } // SQL dùng kiểu INT
        public string HoTen { get; set; }
        public string SDT { get; set; }
        public string TenCV { get; set; } // Để chứa tên chức vụ từ bảng ChucVu
        public string TenKhoa { get; set; } // Để chứa tên khoa từ bảng Khoa
        public string TrangThai { get; set; }
    }
}
