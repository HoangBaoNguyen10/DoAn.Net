using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using QuanLyBenhVien.Database;

namespace QuanLyBenhVien.Views.Admin
{
    public partial class ucManageStaff : UserControl
    {
        public ucManageStaff()
        {
            InitializeComponent();
            LoadData(); // Gọi hàm đổ dữ liệu ngay khi hiện lên
        }

        private void LoadData()
        {
            try
            {
                // Câu query chuẩn dựa trên file SQL mày gửi
                string query = @"SELECT nv.MaNV, nv.HoTen, cv.TenCV, k.TenKhoa 
                        FROM NhanVien nv 
                        LEFT JOIN ChucVu cv ON nv.MaCV = cv.MaCV 
                        LEFT JOIN Khoa k ON nv.MaKhoa = k.MaKhoa";

                DataTable dt = DatabaseConnect.ExecuteTable(query);
                if (dt != null)
                {
                    dgStaff.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu nhân sự: " + ex.Message);
            }
        }
    }
}