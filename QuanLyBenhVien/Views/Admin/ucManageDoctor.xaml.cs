using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using QuanLyBenhVien.Database;

namespace QuanLyBenhVien.Views.Admin
{
    public partial class ucManageDoctor : UserControl
    {
        public ucManageDoctor()
        {
            InitializeComponent();
            LoadDoctorData();
        }

        private void LoadDoctorData()
        {
            try
            {
                // Câu query chuẩn: Join để lấy TenCV thay vì ChucVu
                // Lọc VaiTro = 0 và TenCV có chữ 'Bác sĩ' để ra đúng danh sách bác sĩ
                string query = @"SELECT nv.MaNV, nv.HoTen, cv.TenCV, k.TenKhoa, nv.SDT 
                        FROM NhanVien nv 
                        INNER JOIN TaiKhoan tk ON nv.MaNV = tk.MaNV 
                        INNER JOIN ChucVu cv ON nv.MaCV = cv.MaCV 
                        LEFT JOIN Khoa k ON nv.MaKhoa = k.MaKhoa 
                        WHERE cv.TenCV LIKE N'%Bác sĩ%'";

                DataTable dt = DatabaseConnect.ExecuteTable(query);
                if (dt != null)
                {
                    dgDoctor.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách bác sĩ: " + ex.Message);
            }
        }
    }
}