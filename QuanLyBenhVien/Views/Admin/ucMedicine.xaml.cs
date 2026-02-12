using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using QuanLyBenhVien.Database;

namespace QuanLyBenhVien.Views.Admin
{
    public partial class ucMedicine : UserControl
    {
        public ucMedicine()
        {
            InitializeComponent();
            LoadMedicine(); // Chạy phát là đổ dữ liệu luôn
        }

        // Hàm lấy danh sách thuốc từ SQL
        private void LoadMedicine()
        {
            try
            {
                // SQL của mày là 'Gia', không phải 'DonGia'
                string query = "SELECT MaThuoc, TenThuoc, DonVi, SoLuongTon, Gia FROM Thuoc";
                DataTable dt = DatabaseConnect.ExecuteTable(query);
                if (dt != null)
                {
                    dgMedicine.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kho thuốc: " + ex.Message);
            }
        }

        // Hàm xử lý khi nhấn nút Cập Nhật Tồn Kho
        private void btnUpdateStock_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra xem đã chọn dòng nào trong bảng chưa
            if (dgMedicine.SelectedItem == null)
            {
                MessageBox.Show("Mày phải chọn một loại thuốc trong bảng trước!");
                return;
            }

            // 2. Kiểm tra xem đã nhập số lượng chưa
            if (string.IsNullOrEmpty(txtImportQty.Text))
            {
                MessageBox.Show("Nhập số lượng cần thêm vào đã chứ!");
                return;
            }

            // Lấy dòng đang chọn
            DataRowView row = (DataRowView)dgMedicine.SelectedItem;
            string maThuoc = row["MaThuoc"].ToString();
            int soLuongThem = int.Parse(txtImportQty.Text);

            // Tạm thời thông báo, tí nữa tao sẽ chỉ mày viết SQL UPDATE
            MessageBox.Show($"Đã sẵn sàng cập nhật thuốc {maThuoc} thêm {soLuongThem} đơn vị!");

            // Sau khi update xong thì load lại bảng cho nó mới
            // LoadMedicine();
            txtImportQty.Clear();
        }




    }
}