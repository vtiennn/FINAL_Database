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
using System.Xml.Linq;

namespace FINAL_Database
{
    public partial class ManageStaff : Form
    {
        public ManageStaff()
        {
            InitializeComponent();
            lvStaff.SelectedIndexChanged += lvStaff_SelectedIndexChanged;
            txtSearchStaff.TextChanged += txtSearchStaff_TextChanged; // Gắn sự kiện tìm kiếm
            btnAdd.Click += btnAdd_Click;
            btnNew.Click += btnNew_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
           
        }

        private void frmManageStaff_Load(object sender, EventArgs e)
        {
            LoadStaffData(); // Tải danh sách nhân viên
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True"))
                {
                    con.Open();
                    MessageBox.Show("Kết nối database thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database: " + ex.Message);
            }
        }

        // Phương thức tải toàn bộ danh sách nhân viên
        private void LoadStaffData()
        {
            string query = "SELECT StaffID, Name, Email, Phone, Username FROM Staff";
            lvStaff.Items.Clear();

            string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string staffId = reader["StaffID"].ToString();
                        string name = reader["Name"].ToString();
                        string email = reader["Email"].ToString();
                        string phone = reader["Phone"].ToString();
                        string username = reader["Username"].ToString();

                        MessageBox.Show($"StaffID: {staffId}, Name: {name}, Email: {email}");

                        ListViewItem item = new ListViewItem(staffId);
                        item.SubItems.Add(name);
                        item.SubItems.Add(email);
                        item.SubItems.Add(phone);
                        item.SubItems.Add(username);

                        lvStaff.Items.Add(item);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            txtStaffID.Clear();
            txtStaffName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtStaffID.Focus();
        }

        private void lvStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvStaff.SelectedItems.Count > 0)
            {
                var selectedItem = lvStaff.SelectedItems[0];

                txtStaffID.Text = selectedItem.SubItems[0].Text;
                txtStaffName.Text = selectedItem.SubItems[1].Text;
                txtEmail.Text = selectedItem.SubItems[2].Text;
                txtPhone.Text = selectedItem.SubItems[3].Text;
            }
            // Kiểm tra xem có mục nào được chọn trong ListView không
            if (lvStaff.SelectedItems.Count > 0)
            {
                var selectedItem = lvStaff.SelectedItems[0];

                // Điền dữ liệu vào các TextBox tương ứng từ các SubItems trong ListView
                txtStaffID.Text = selectedItem.SubItems[0].Text;     // StaffID
                txtStaffName.Text = selectedItem.SubItems[1].Text;   // Name
                txtEmail.Text = selectedItem.SubItems[2].Text;        // Email
                txtPhone.Text = selectedItem.SubItems[3].Text;        // Phone
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";
            string query = "INSERT INTO Staff (StaffID, Name, Email, Phone) " +
                             "VALUES (@StaffID, @Name, @Email, @Phone)";

            // Lấy dữ liệu từ các TextBox
            string staffID = txtStaffID.Text;
            string name = txtStaffName.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, con);

                // Thêm tham số vào câu lệnh SQL
                command.Parameters.AddWithValue("@StaffID", staffID);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);

                try
                {
                    con.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Thêm mới nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear các TextBox sau khi thêm
                    txtStaffID.Clear();
                    txtStaffName.Clear();
                    txtEmail.Clear();
                    txtPhone.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvStaff.SelectedItems.Count > 0)
            {
                var selectedItem = lvStaff.SelectedItems[0];

                string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";
                string query = "UPDATE Staff SET Name = @Name, Email = @Email, Phone = @Phone WHERE StaffID = @StaffID";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, con);

                    // Gán giá trị từ TextBox và ListView
                    command.Parameters.AddWithValue("@StaffID", selectedItem.SubItems[0].Text);
                    command.Parameters.AddWithValue("@Name", txtStaffName.Text.Trim());
                    command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    command.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());

                    try
                    {
                        con.Open();
                        int rowsAffected = command.ExecuteNonQuery(); // Thực hiện lệnh SQL

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Cập nhật trực tiếp ListView
                            selectedItem.SubItems[1].Text = txtStaffName.Text.Trim();
                            selectedItem.SubItems[2].Text = txtEmail.Text.Trim();
                            selectedItem.SubItems[3].Text = txtPhone.Text.Trim();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvStaff.SelectedItems.Count > 0)
            {
                var selectedItem = lvStaff.SelectedItems[0];

                string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";
                string query = "DELETE FROM Staff WHERE StaffID = @StaffID";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@StaffID", txtStaffID.Text);

                    try
                    {
                        con.Open();
                        command.ExecuteNonQuery();
                        lvStaff.Items.Remove(selectedItem); // Xóa khỏi ListView
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        btnNew_Click(sender, e); // Xóa các TextBox sau khi xóa
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSearchStaff_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchStaff.Text.Trim().ToLower();
            lvStaff.Items.Clear();

            string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";
            string query = "SELECT StaffID, Name, Email, Phone FROM Staff WHERE LOWER(Name) LIKE @SearchText";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                try
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["StaffID"].ToString());
                        item.SubItems.Add(reader["Name"].ToString());
                        item.SubItems.Add(reader["Email"].ToString());
                        item.SubItems.Add(reader["Phone"].ToString());
                        lvStaff.Items.Add(item);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

