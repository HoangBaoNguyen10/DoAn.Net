using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows;
using QuanLyBenhVien.Database; // Nhớ đổi đúng namespace của mày

namespace QuanLyBenhVien
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestKetNoi();
        }

        private void TestKetNoi()
        {
            // Thử lấy tên bệnh nhân đầu tiên từ dữ liệu mẫu tao đã tạo cho mày
            string query = "SELECT TOP 1 TenBN FROM BenhNhan";
            object result = DatabaseConnect.ExecuteScalar(query);

            if (result != null)
            {
                MessageBox.Show("Ngon rồi! Đã kết nối được SQL. Bệnh nhân đầu tiên: " + result.ToString());
            }
            else
            {
                MessageBox.Show("Chưa ổn, kiểm tra lại Connection String trong DatabaseConnect.cs!");
            }
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT MaBN, TenBN, SDT, MaBHYT FROM BenhNhan";
            dgBenhNhan.ItemsSource = DatabaseConnect.ExecuteTable(query).DefaultView;
        }
    }
}
