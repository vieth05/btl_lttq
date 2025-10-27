using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Nhom_04_QLKS
{
    internal class DBConnect
    {
        private SqlConnection conn;

        // Constructor: CHỈ khởi tạo connection, KHÔNG test
        public DBConnect()
        {
            conn = new SqlConnection("Data Source=DESKTOP-L4JFPFV;Initial Catalog=QLKHACHSAN2;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;");
        }

        // Method riêng để test connection (gọi khi cần)
        public bool TestConnection()
        {
            try
            {
                conn.Open();
                conn.Close();
                CustomMessageBox.ShowSuccess("Kết nối cơ sở dữ liệu thành công!", "Kết nối");
                return true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError(
                    "Không thể kết nối đến cơ sở dữ liệu:\n\n" + ex.Message,
                    "Lỗi kết nối"
                );
                return false;
            }
        }

        public void Open()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        public void Close()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        // Thêm Dispose để đóng connection khi không dùng
        public void Dispose()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
            }
        }

        // Thực hiện câu lệnh không trả dữ liệu (INSERT, UPDATE, DELETE)
        public int getNonQuery(string sql)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    Open();
                    int result = cmd.ExecuteNonQuery();
                    Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError(
                    "Lỗi thực thi câu lệnh:\n\n" + ex.Message,
                    "Lỗi Database"
                );
                return -1;
            }
        }

        // Trả về 1 giá trị duy nhất (ví dụ SELECT COUNT(*))
        public object getScalar(string sql)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    Open();
                    object result = cmd.ExecuteScalar();
                    Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError(
                    "Lỗi truy vấn dữ liệu:\n\n" + ex.Message,
                    "Lỗi Database"
                );
                return null;
            }
        }

        // Trả về bảng dữ liệu (SELECT *)
        public DataTable getTable(string sql)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    da.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError(
                    "Lỗi tải dữ liệu:\n\n" + ex.Message,
                    "Lỗi Database"
                );
                return null;
            }
        }

        // Cập nhật DataTable lên SQL Server
        public int updateTable(DataTable dtnew, string query)
        {
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    SqlCommandBuilder cb = new SqlCommandBuilder(da);
                    int result = da.Update(dtnew);
                    return result;
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError(
                    "Lỗi cập nhật dữ liệu:\n\n" + ex.Message,
                    "Lỗi Database"
                );
                return -1;
            }
        }
    }
}