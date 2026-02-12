using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input; // Cần cái này để dùng MouseButtonEventArgs
using QuanLyBenhVien.Database;

namespace QuanLyBenhVien.Views // 1. PHẢI CÓ CHỮ .Views Ở ĐÂY
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // 2. Hàm để nắm giữ và kéo cửa sổ đi (Khớp với MouseLeftButtonDown bên XAML)
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // 3. Hàm để đóng app khi nhấn nút X
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // 4. Hàm xử lý đăng nhập chính
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem tên txtUsername và txtPassword bên XAML đã đặt đúng chưa
            string user = txtUsername.Text;
            string pass = txtPassword.Password;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            // Truy vấn lấy vai trò từ Database mẫu của mày
            string query = "SELECT VaiTro FROM TaiKhoan WHERE Username = @user AND Password = @pass";
            SqlParameter[] p = {
                new SqlParameter("@user", user),
                new SqlParameter("@pass", pass)
            };

            object result = DatabaseConnect.ExecuteScalar(query, p);

            if (result != null)
            {
                int vaiTro = Convert.ToInt32(result);

                if (vaiTro == 1) // Admin
                {
                    MessageBox.Show("Chào sếp Admin!");

                    // Đừng dùng MainWindow cũ nữa!
                    // Mở cái MainWindow_Admin mày mới tạo trong folder Admin ấy
                    QuanLyBenhVien.Views.Admin.MainWindow_Admin adminWin = new QuanLyBenhVien.Views.Admin.MainWindow_Admin();
                    adminWin.Show();

                    this.Close(); // Đóng cái màn hình đăng nhập lại
                }
                else // Nhân viên / Bác sĩ
                {
                    MessageBox.Show("Chào Bác sĩ/Nhân viên!");
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu rồi mày ơi!");
            }
        }
    }
}