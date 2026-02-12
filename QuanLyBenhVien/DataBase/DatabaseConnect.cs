using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLyBenhVien.Database // Sửa lại đúng namespace project của mày
{
    public class DatabaseConnect
    {
        // Chuỗi kết nối tới SQL Server
        // Initial Catalog: Tên database mày đã tạo ở Bước 1
        // Data Source: Dấu chấm (.) đại diện cho Localhost (máy hiện tại)
        private static string connectionString = @"Data Source=.;Initial Catalog=QLBENHVIEN;Integrated Security=True";

        // 1. Hàm lấy dữ liệu (SELECT) - Trả về một bảng dữ liệu (DataTable)
        public static DataTable ExecuteTable(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lấy bảng dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

        // 2. Hàm thực thi (INSERT, UPDATE, DELETE) - Trả về true nếu thành công
        public static bool ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thực thi SQL: " + ex.Message, "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        // 3. Hàm lấy giá trị đơn (Ví dụ: Lấy mật khẩu, đếm số lượng...)
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lấy giá trị: " + ex.Message, "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
    }
}