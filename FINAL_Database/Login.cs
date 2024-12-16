using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FINAL_Database
{
    public partial class frmLogin : Form
    {

        public frmLogin()
        {

            InitializeComponent();
            // Gắn sự kiện KeyDown cho các TextBox
            txtUserName.KeyDown += TextBox_KeyDown;
            txtPass.KeyDown += TextBox_KeyDown;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Mã khởi tạo khác (nếu có)
        }

        // Hàm kết nối cơ sở dữ liệu
        private SqlConnection ConnectToDatabase()
        {
            string connectionString = "Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";
            return new SqlConnection(connectionString);
        }

        // Hàm kiểm tra đăng nhập
        private bool CheckLogin(string username, string password)
        {
            using (SqlConnection conn = ConnectToDatabase())
            {
                conn.Open();
                string query = "SELECT * FROM Staff WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                return reader.HasRows; // Kiểm tra nếu có kết quả trả về
            }
        }

        // Sự kiện xử lý khi nhấn Enter trên bàn phím
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Kiểm tra phím Enter
            {
                PerformLogin(); // Thực hiện đăng nhập
                e.Handled = true; // Ngăn chặn xử lý mặc định
                e.SuppressKeyPress = true; // Ẩn âm thanh beep
            }
        }

        // Sự kiện xử lý khi nhấn nút Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            PerformLogin(); // Thực hiện đăng nhập
        }

        // Phương thức thực hiện đăng nhập
        private void PerformLogin()
        {
            string username = txtUserName.Text.Trim();
            string password = txtPass.Text.Trim();

            if (CheckLogin(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở form quản lý sản phẩm sau khi đăng nhập thành công
                ManageComputer mainForm = new ManageComputer();
                mainForm.Show();

                // Ẩn form đăng nhập
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Clear(); // Xóa mật khẩu để người dùng nhập lại
                txtPass.Focus(); // Đưa con trỏ vào ô mật khẩu
            }
        }
    }
}
