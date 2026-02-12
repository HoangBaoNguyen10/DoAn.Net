using System.Windows;
// Mày có thể xóa dòng using Admin bị tối đi nếu nó cùng namespace

namespace QuanLyBenhVien.Views.Admin
{
    public partial class MainWindow_Admin : Window
    {
        public MainWindow_Admin()
        {
            InitializeComponent();
        }

        // Hàm hỗ trợ để chuyển đổi nội dung hiển thị
        private void _switchPage(object page)
        {
            // MainContentPresenter là tên x:Name của ContentControl bên file XAML
            MainContentPresenter.Content = page;
        }

        private void btnNhanVien_Click(object sender, RoutedEventArgs e)
        {
            // Gọi UserControl quản lý nhân viên
            _switchPage(new ucManageStaff());
        }

        private void btnBacSi_Click(object sender, RoutedEventArgs e)
        {
            // Gọi UserControl quản lý bác sĩ
            _switchPage(new ucManageDoctor());
        }

        private void btnThuoc_Click(object sender, RoutedEventArgs e)
        {
            // Gọi UserControl quản lý kho thuốc
            _switchPage(new ucMedicine());
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Quay lại màn hình đăng nhập
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}