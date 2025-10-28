using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom_04_QLKS
{
    public partial class HOADON1 : Form
    {
        DBConnect db = new DBConnect();
        public HOADON1()
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.FormClosing += HOADON1_FormClosing;
        }

        public void load_cbbMAPHONG()
        {
            string query = "SELECT * FROM PHONG";
            DataTable ds = db.getTable(query);
            cbbMaPhong.DataSource = ds;
            cbbMaPhong.DisplayMember = "MAPHONG";
            cbbMaPhong.ValueMember = "MAPHONG";
        }

        public void load_cbbMANV()
        {

            string query = "SELECT * FROM NHANVIEN";
            DataTable ds = db.getTable(query);
            cbbMANV.DataSource = ds;
            cbbMANV.DisplayMember = "MANV";
            cbbMANV.ValueMember = "MANV";
        }
        public void load_cbbMADV()
        {

            string query = "SELECT * FROM DICHVU";
            DataTable ds = db.getTable(query);
            cbbMaDV.DataSource = ds;
            cbbMaDV.DisplayMember = "MADV";
            cbbMaDV.ValueMember = "MADV";

        }

        public void load_cbbMAKH()
        {
            string query = "SELECT * FROM KHACHHANG";
            DataTable ds = db.getTable(query);
            cbbMAKH.DataSource = ds;
            cbbMAKH.DisplayMember = "MAKH";
            cbbMAKH.ValueMember = "MAKH";

        }
        public void load_cbbMaHD_CTHD()
        {
            string query = "SELECT * FROM HOADON";
            DataTable ds = db.getTable(query);
            cbbMaHD.DataSource = ds;
            cbbMaHD.DisplayMember = "MAHD";
            cbbMaHD.ValueMember = "MAHD";
        }

        public void load_DTGV()
        {
            try
            {
                string query = "SELECT * FROM HOADON";
                DataTable dataTable = db.getTable(query);
                dtgvHoadon.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu Hóa đơn: " + ex.Message);
            }
        }


        public void load_CTHD()
        {
            try
            {
                string query = "SELECT * FROM CTHOADON";
                DataTable dataTable = db.getTable(query);
                dtgvCTHD.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu Chi tiết hóa đơn: " + ex.Message);
            }
        }


        private void HOADON1_Load(object sender, EventArgs e)
        {
            load_DTGV();
            load_cbbMAKH();
            load_cbbMANV();
            load_CTHD();
            load_cbbMaHD_CTHD();
            load_cbbMAPHONG();
            load_cbbMADV();

        }

        float TINHTHANTIEN()
        {
            float total = 0;
            float tongTien = (int.Parse(txtSoLuong.Text) * float.Parse(txtGiaDV.Text));
            total += tongTien;
            return total;
        }
        private void dtgvHoadon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dtgvHoadon.CurrentRow.Index;
            txtMaHD.Text = dtgvHoadon.Rows[index].Cells[0].Value.ToString();
            cbbMANV.Text = dtgvHoadon.Rows[index].Cells[1].Value.ToString();
            cbbMAKH.Text = dtgvHoadon.Rows[index].Cells[2].Value.ToString();
            txtNgay.Text = dtgvHoadon.Rows[index].Cells[3].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string ma = txtMaHD.Text.Trim();
                string makh = cbbMAKH.SelectedValue?.ToString();
                string manv = cbbMANV.SelectedValue?.ToString();
                string ngay = txtNgay.Text.Trim();

                // validate dữ liệu
                if (string.IsNullOrEmpty(ma) || string.IsNullOrEmpty(makh) || string.IsNullOrEmpty(manv) || string.IsNullOrEmpty(ngay))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // kiểm tra định dạng ngày
                DateTime ngayHD;
                bool hopLe = DateTime.TryParseExact(
                    ngay,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out ngayHD
                );

                if (!hopLe)
                {
                    MessageBox.Show("Ngày không hợp lệ! Hãy nhập theo định dạng dd/MM/yyyy (ví dụ: 25/10/2025).",
                        "Lỗi định dạng ngày",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // thêm dữ liệu vào datagridview
                DataTable dt = (DataTable)dtgvHoadon.DataSource;
                DataRow row = dt.NewRow();
                row["MAHD"] = ma;
                row["MANV"] = manv;
                row["MAKH"] = makh;
                row["NGAY"] = ngayHD.ToString("yyyy-MM-dd"); // định dạng ngày chuẩn sql 
                dt.Rows.Add(row);

                // khai báo chuỗi truy vấn
                string chuoitv = "INSERT INTO HOADON (MAHD, MANV, MAKH, NGAY) VALUES (" +
                                 $"'{ma}', '{manv}', '{makh}', '{ngayHD:yyyy-MM-dd}')";

                int kq = db.getNonQuery(chuoitv);

                if (kq == 1)
                {
                    MessageBox.Show("Thêm hóa đơn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể thêm hóa đơn!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnThem1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (cbbMaHD.SelectedValue == null || cbbMaDV.SelectedValue == null ||
                    cbbMaPhong.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ thông tin!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSoLuong.Text) ||
                    string.IsNullOrWhiteSpace(txtGiaDV.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ số lượng và giá dịch vụ!",
                                  "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra số lượng và giá hợp lệ
                if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải là số nguyên dương!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtGiaDV.Text, out decimal giaDV) || giaDV <= 0)
                {
                    MessageBox.Show("Giá dịch vụ phải là số dương!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tính thành tiền
                float thanhTien = TINHTHANTIEN();
                txtThanhTien.Text = thanhTien.ToString("N0"); // Format số

                // Lấy giá trị
                string maHD = cbbMaHD.SelectedValue.ToString();
                string maDV = cbbMaDV.SelectedValue.ToString();
                string maPhong = cbbMaPhong.SelectedValue.ToString();

                string sqlQuery = string.Format(
                    "INSERT INTO CTHOADON (MAHD, MADV, MAPHONG, SOLUONG, GIADV, THANHTIEN) " +
                    "VALUES ('{0}', '{1}', '{2}', {3}, {4}, {5})",
                    maHD.Replace("'", "''"),
                    maDV.Replace("'", "''"),
                    maPhong.Replace("'", "''"),
                    soLuong,
                    giaDV,
                    thanhTien
                );

                int kq = db.getNonQuery(sqlQuery);
                if (kq == 1)
                {
                    MessageBox.Show("Thêm chi tiết hóa đơn thành công!", "Thành công",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh DataGridView
                    LoadChiTietHoaDon(); // Gọi hàm load lại dữ liệu

                    // Clear input
                    ClearInputCTHD();
                }
                else
                {
                    MessageBox.Show("Thêm chi tiết hóa đơn thất bại!", "Lỗi",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputCTHD()
        {
            cbbMaHD.SelectedIndex = -1;
            cbbMaDV.SelectedIndex = -1;
            cbbMaPhong.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtGiaDV.Clear();
            txtThanhTien.Clear();
        }

        // Hàm load lại chi tiết hóa đơn
        private void LoadChiTietHoaDon()
        {
            string query = "SELECT * FROM CTHOADON";
            dtgvCTHD.DataSource = db.getTable(query);
        }

        private void dtgvCTHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dtgvCTHD.CurrentRow.Index;
            cbbMaHD.Text = dtgvCTHD.Rows[index].Cells[0].Value.ToString();
            cbbMaDV.Text = dtgvCTHD.Rows[index].Cells[1].Value.ToString();
            cbbMaPhong.Text = dtgvCTHD.Rows[index].Cells[2].Value.ToString();
            txtSoLuong.Text = dtgvCTHD.Rows[index].Cells[3].Value.ToString();
            txtGiaDV.Text = dtgvCTHD.Rows[index].Cells[4].Value.ToString();
            txtThanhTien.Text = dtgvCTHD.Rows[index].Cells[5].Value.ToString();
        }

        private void XOA_CT_Click(object sender, EventArgs e)
        {
            if (dtgvCTHD.SelectedRows.Count > 0)
            {
                // Lấy index của dòng được chọn
                int rowIndex = dtgvCTHD.SelectedRows[0].Index;

                // Xóa dòng tương ứng trong DataTable
                ((DataTable)dtgvCTHD.DataSource).Rows.RemoveAt(rowIndex);
            }
            string chuoitv = "DELETE FROM CTHOADON WHERE MAHD = '" + cbbMaHD.SelectedValue + "' AND MADV = '" + cbbMaDV.SelectedValue + "' AND MAPHONG = '" + cbbMaPhong.SelectedValue + "'";
            int kq = db.getNonQuery(chuoitv);
            if (kq < 0)
                MessageBox.Show("Không thể xóa dòng dữ liệu này");
            else
                MessageBox.Show("Bạn đã xóa thành công");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dtgvCTHD.SelectedRows.Count > 0)
            {
                int rowIndex = dtgvHoadon.SelectedRows[0].Index;
                ((DataTable)dtgvHoadon.DataSource).Rows.RemoveAt(rowIndex);
            }
            string chuoitv = "DELETE FROM HOADON WHERE MAHD = '" + txtMaHD.Text + "' AND MANV = '" + cbbMANV.SelectedValue + "' AND MAKH = '" + cbbMAKH.SelectedValue + "'";
            int kq = db.getNonQuery(chuoitv);
            if (kq < 0)
                MessageBox.Show("Không thể xóa dòng dữ liệu này");
            else
                MessageBox.Show("Bạn đã xóa thành công");
        }


        private void SUA_CT_Click(object sender, EventArgs e)
        {
            if (dtgvCTHD.SelectedRows.Count > 0)
            {
                int rowIndex = dtgvCTHD.SelectedRows[0].Index;
                dtgvCTHD.Rows[rowIndex].Cells["SoLuong"].Value = txtSoLuong.Text;
                dtgvCTHD.Rows[rowIndex].Cells["giadv"].Value = txtGiaDV.Text;
            }
            txtThanhTien.Text = TINHTHANTIEN().ToString();
            string chuoitv = "Update CTHOADON set soluong = '" + txtSoLuong.Text + "',  thanhtien = '" + txtThanhTien.Text + "' where mahd ='" + cbbMaHD.SelectedValue + "'";
            int kq = db.getNonQuery(chuoitv);
            if (kq < 0)
                MessageBox.Show("Sửa thất bại");
            else
                MessageBox.Show("Bạn đã sửa thành công");
        }

        private void btnTrangMenu_Click(object sender, EventArgs e)
        {
            if (HasUnsavedChanges())
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có thay đổi chưa lưu. Bạn có muốn lưu trước khi thoát?",
                    "Cảnh báo",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        SaveData();
                        MessageBox.Show("Đã lưu dữ liệu thành công!", "Thông báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool HasUnsavedChanges()
        {
            bool hoaDonHasData = !string.IsNullOrWhiteSpace(txtMaHD.Text) ||
                                 cbbMAKH.SelectedIndex > -1 ||
                                 cbbMANV.SelectedIndex > -1 ||
                                 !string.IsNullOrWhiteSpace(txtNgay.Text);

            bool chiTietHasData = cbbMaHD.SelectedIndex > -1 ||
                                  cbbMaDV.SelectedIndex > -1 ||
                                  cbbMaPhong.SelectedIndex > -1 ||
                                  !string.IsNullOrWhiteSpace(txtSoLuong.Text) ||
                                  !string.IsNullOrWhiteSpace(txtGiaDV.Text);

            return hoaDonHasData || chiTietHasData;
        }

        private void SaveData()
        {
            bool saved = false;

            if (!string.IsNullOrWhiteSpace(txtMaHD.Text) &&
                cbbMAKH.SelectedIndex > -1 &&
                cbbMANV.SelectedIndex > -1)
            {
                btnThem_Click(null, null);
                saved = true;
            }

            if (cbbMaHD.SelectedIndex > -1 &&
                cbbMaDV.SelectedIndex > -1 &&
                !string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                btnThem1_Click(null, null);
                saved = true;
            }

            if (!saved)
            {
                throw new Exception("Không có dữ liệu hợp lệ để lưu");
            }
        }

        private void HOADON1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (HasUnsavedChanges())
                {
                    DialogResult result = MessageBox.Show(
                        "Bạn có thay đổi chưa lưu. Bạn có muốn lưu trước khi thoát?",
                        "Cảnh báo",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            SaveData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                            e.Cancel = true;
                            return;
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private void txtMaHD_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNgay_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
