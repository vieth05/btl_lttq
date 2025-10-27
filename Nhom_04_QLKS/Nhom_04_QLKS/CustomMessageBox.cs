using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nhom_04_QLKS
{
    public class CustomMessageBox : Form
    {
        private Label lblMessage;
        private Label lblIcon;
        private Button btnOK;
        private Panel panelTop;

        public enum MessageType
        {
            Success,
            Error,
            Warning,
            Info
        }

        private CustomMessageBox(string message, string title, MessageType type)
        {
            InitializeComponents(message, title, type);
        }

        private void InitializeComponents(string message, string title, MessageType type)
        {
            // Form settings
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(450, 250);
            this.BackColor = Color.White;

            // Panel top với màu theo loại message
            panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = GetColorByType(type)
            };

            // Icon
            lblIcon = new Label
            {
                Text = GetIconByType(type),
                Font = new Font("Segoe UI", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15)
            };

            // Title
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(90, 25)
            };

            // Message
            lblMessage = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = false,
                Size = new Size(400, 80),
                Location = new Point(25, 100),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Button OK
            btnOK = new Button
            {
                Text = "OK",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = GetColorByType(type),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 45),
                Location = new Point(165, 190),
                Cursor = Cursors.Hand
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += (s, e) => this.Close();

            // Hover effect
            btnOK.MouseEnter += (s, e) =>
            {
                btnOK.BackColor = GetDarkerColor(GetColorByType(type));
            };
            btnOK.MouseLeave += (s, e) =>
            {
                btnOK.BackColor = GetColorByType(type);
            };

            // Add controls
            panelTop.Controls.Add(lblIcon);
            panelTop.Controls.Add(lblTitle);
            this.Controls.Add(panelTop);
            this.Controls.Add(lblMessage);
            this.Controls.Add(btnOK);

            // Shadow effect
            this.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                    Color.FromArgb(200, 200, 200), 2, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 2, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 2, ButtonBorderStyle.Solid,
                    Color.FromArgb(200, 200, 200), 2, ButtonBorderStyle.Solid);
            };
        }

        private Color GetColorByType(MessageType type)
        {
            switch (type)
            {
                case MessageType.Success:
                    return Color.FromArgb(114, 151, 98); // #729762
                case MessageType.Error:
                    return Color.FromArgb(231, 76, 60); // Đỏ
                case MessageType.Warning:
                    return Color.FromArgb(241, 196, 15); // Vàng
                case MessageType.Info:
                    return Color.FromArgb(52, 152, 219); // Xanh dương
                default:
                    return Color.FromArgb(52, 73, 94);
            }
        }

        private string GetIconByType(MessageType type)
        {
            switch (type)
            {
                case MessageType.Success:
                    return "✓";
                case MessageType.Error:
                    return "✕";
                case MessageType.Warning:
                    return "⚠";
                case MessageType.Info:
                    return "ℹ";
                default:
                    return "•";
            }
        }

        private Color GetDarkerColor(Color color)
        {
            return Color.FromArgb(
                Math.Max(0, color.R - 30),
                Math.Max(0, color.G - 30),
                Math.Max(0, color.B - 30)
            );
        }

        // Static methods để gọi dễ dàng
        public static void Show(string message, string title = "Thông báo", MessageType type = MessageType.Info)
        {
            using (CustomMessageBox msgBox = new CustomMessageBox(message, title, type))
            {
                msgBox.ShowDialog();
            }
        }

        public static void ShowSuccess(string message, string title = "Thành công")
        {
            Show(message, title, MessageType.Success);
        }

        public static void ShowError(string message, string title = "Lỗi")
        {
            Show(message, title, MessageType.Error);
        }

        public static void ShowWarning(string message, string title = "Cảnh báo")
        {
            Show(message, title, MessageType.Warning);
        }

        public static void ShowInfo(string message, string title = "Thông tin")
        {
            Show(message, title, MessageType.Info);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomMessageBox
            // 
            this.ClientSize = new System.Drawing.Size(274, 229);
            this.Name = "CustomMessageBox";
            this.Load += new System.EventHandler(this.CustomMessageBox_Load);
            this.ResumeLayout(false);

        }

        private void CustomMessageBox_Load(object sender, EventArgs e)
        {

        }
    }
}