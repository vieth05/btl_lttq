using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom_04_QLKS
{
    public partial class login : Form
    {
        DBConnect db = new DBConnect();

        public login()
        {
            InitializeComponent();

            // Cấu hình form
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng Nhập - Hệ Thống QLKS";

            // Enter key để đăng nhập
            txt_TK.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) btnDangNhap.PerformClick(); };
            txt_MK.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) btnDangNhap.PerformClick(); };
        }

        public class GetDataUser
        {
            public static string tennhanvien;
            public static string phanquyen;
        }

        private void chboHienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (chboHienMK.Checked)
            {
                txt_MK.PasswordChar = '\0';
            }
            else
            {
                txt_MK.PasswordChar = '●';
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenTK = txt_TK.Text.Trim();
            string matKhau = txt_MK.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(tenTK) || string.IsNullOrEmpty(matKhau))
            {
                CustomMessageBox.ShowWarning(
                    "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!",
                    "Thiếu thông tin"
                );
                return;
            }

            try
            {
                // Kiểm tra đăng nhập (nên dùng Parameterized Query để tránh SQL Injection)
                string sql = $"SELECT COUNT(*) FROM TAIKHOAN WHERE TENTK = '{tenTK}' AND MATKHAU = '{matKhau}'";
                int count = Convert.ToInt32(db.getScalar(sql));

                if (count > 0)
                {
                    // Lấy phân quyền
                    string sql1 = $"SELECT PHANQUYEN FROM TAIKHOAN WHERE TENTK = '{tenTK}' AND MATKHAU = '{matKhau}'";
                    object kq = db.getScalar(sql1);
                    if (kq != null)
                    {
                        GetDataUser.phanquyen = kq.ToString();
                    }

                    // Lấy tên nhân viên
                    string sql2 = $"SELECT TENNV FROM NHANVIEN NV JOIN TAIKHOAN TK ON NV.TENTK = TK.TENTK WHERE NV.TENTK = '{tenTK}' AND TK.MATKHAU = '{matKhau}'";
                    object kq1 = db.getScalar(sql2);
                    if (kq1 != null)
                    {
                        GetDataUser.tennhanvien = kq1.ToString();
                    }

                    // Hiển thị thông báo thành công
                    CustomMessageBox.ShowSuccess(
                        $"Chào mừng {GetDataUser.tennhanvien}!\nBạn đã đăng nhập thành công.",
                        "Đăng nhập thành công"
                    );

                    // Mở form menu
                    menu menuForm = new menu();
                    menuForm.Show();

                    // Ẩn form đăng nhập
                    this.Hide();

                    // ✅ QUAN TRỌNG: Khi đóng form menu thì thoát toàn bộ app
                    menuForm.FormClosed += (s, args) =>
                    {
                        db.Dispose(); // Đóng connection
                        this.Close(); // Đóng form login
                        Application.Exit();
                        Environment.Exit(0); // Chắc chắn thoát hẳn
                    };
                }
                else
                {
                    CustomMessageBox.ShowError(
                        "Tên đăng nhập hoặc mật khẩu không chính xác!\nVui lòng kiểm tra lại.",
                        "Đăng nhập thất bại"
                    );

                    // Clear password
                    txt_MK.Clear();
                    txt_MK.Focus();
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowError(
                    "Đã xảy ra lỗi trong quá trình đăng nhập:\n\n" + ex.Message,
                    "Lỗi hệ thống"
                );
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Có thể thêm hiệu ứng click vào logo nếu muốn
        }

        private void txt_TK_TextChanged(object sender, EventArgs e)
        {
            // Có thể thêm validation real-time nếu cần
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Có thể thêm custom paint nếu cần
        }

        // Override FormClosing để đảm bảo thoát hẳn
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Đóng connection
            db.Dispose();

            // Thoát toàn bộ app
            Application.Exit();
            Environment.Exit(0);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}