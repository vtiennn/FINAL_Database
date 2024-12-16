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
    public partial class ManageCustomer : Form
    {

        private string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";

        public ManageCustomer()
        {
            InitializeComponent();
            LoadCustomerData();
        }
        private void SetupListView()
        {
            // Xóa tất cả các cột cũ nếu có
            lvCustomer.Clear();

            // Thiết lập chế độ hiển thị là Details
            lvCustomer.View = View.Details;

            // Thêm các cột vào ListView
            lvCustomer.Columns.Add("CustomerID", 100, HorizontalAlignment.Left);
            lvCustomer.Columns.Add("Name", 150, HorizontalAlignment.Left);
            lvCustomer.Columns.Add("Email", 150, HorizontalAlignment.Left);
            lvCustomer.Columns.Add("Phone", 100, HorizontalAlignment.Left);
            lvCustomer.Columns.Add("Address", 200, HorizontalAlignment.Left);
            lvCustomer.Columns.Add("DateOfBirth", 120, HorizontalAlignment.Left);
            lvCustomer.Columns.Add("LoyaltyPoints", 100, HorizontalAlignment.Right);
            lvCustomer.Columns.Add("NameBill", 150, HorizontalAlignment.Left);

            // Thiết lập chế độ FullRowSelect để chọn nguyên dòng
            lvCustomer.FullRowSelect = true;

            // Gọi hàm LoadCustomerData để tải dữ liệu vào ListView
            LoadCustomerData();
        }
        private void ManageCustomer_Load(object sender, EventArgs e)
        {
            // Gọi hàm tải dữ liệu lên ListView
            LoadCustomerData();
        }
        private void lvCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
            if (lvCustomer.SelectedItems.Count > 0)
            {
                // Lấy hàng được chọn
                ListViewItem selectedItem = lvCustomer.SelectedItems[0];

                // Gán giá trị từ cột tương ứng vào các TextBox
                txtCustomerID.Text = selectedItem.SubItems[0].Text;  // CustomerID
                txtCustomerName.Text = selectedItem.SubItems[1].Text;       // Name
                txtEmail.Text = selectedItem.SubItems[2].Text;      // Email
                txtPhone.Text = selectedItem.SubItems[3].Text;      // Phone
                txtAddress.Text = selectedItem.SubItems[4].Text;    // Address
                dtpDateOfBirth.Text = selectedItem.SubItems[5].Text; // DateOfBirth
                txtLoyaltyPoints.Text = selectedItem.SubItems[6].Text; // LoyaltyPoints
                txtNameBill.Text = selectedItem.SubItems[7].Text;   // NameBill
            }
        }

        private void LoadCustomerData()
        {
            // Chuỗi kết nối đến database
            string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";
            string query = "SELECT CustomerID, Name, Email, Phone, Address, DateOfBirth, LoyaltyPoints, NameBill FROM Customer";

            // Xóa dữ liệu cũ trong ListView
            lvCustomer.Items.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["CustomerID"].ToString());
                        item.SubItems.Add(reader["Name"].ToString());
                        item.SubItems.Add(reader["Email"].ToString());
                        item.SubItems.Add(reader["Phone"].ToString());
                        item.SubItems.Add(reader["Address"].ToString());
                        item.SubItems.Add(Convert.ToDateTime(reader["DateOfBirth"]).ToShortDateString());
                        item.SubItems.Add(reader["LoyaltyPoints"].ToString());
                        item.SubItems.Add(reader["NameBill"].ToString());

                        lvCustomer.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Customer (Name, Email, Phone, Address, DateOfBirth, LoyaltyPoints, NameBill) VALUES (@Name, @Email, @Phone, @Address, @DateOfBirth, @LoyaltyPoints, @NameBill)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", txtCustomerName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value);
                cmd.Parameters.AddWithValue("@LoyaltyPoints", txtLoyaltyPoints.Text);
                cmd.Parameters.AddWithValue("@NameBill", txtNameBill.Text);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm khách hàng thành công!");
                    LoadCustomerData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvCustomer.SelectedItems.Count > 0)
            {
                string query = "UPDATE Customer SET Name = @Name, Email = @Email, Phone = @Phone, Address = @Address, DateOfBirth = @DateOfBirth, LoyaltyPoints = @LoyaltyPoints, NameBill = @NameBill WHERE CustomerID = @CustomerID";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CustomerID", lvCustomer.SelectedItems[0].SubItems[0].Text);
                    cmd.Parameters.AddWithValue("@Name", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value);
                    cmd.Parameters.AddWithValue("@LoyaltyPoints", txtLoyaltyPoints.Text);
                    cmd.Parameters.AddWithValue("@NameBill", txtNameBill.Text);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật khách hàng thành công!");
                        LoadCustomerData();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật khách hàng: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để cập nhật.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvCustomer.SelectedItems.Count > 0)
            {
                string query = "DELETE FROM Customer WHERE CustomerID = @CustomerID";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CustomerID", lvCustomer.SelectedItems[0].SubItems[0].Text);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa khách hàng thành công!");
                        LoadCustomerData();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lvCustomer.Items.Clear();
            string query = "SELECT * FROM Customer WHERE Name LIKE @SearchText OR Phone LIKE @SearchText";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SearchText", "%" + txtSearchCustomer.Text + "%");

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["CustomerID"].ToString());
                        item.SubItems.Add(reader["Name"].ToString());
                        item.SubItems.Add(reader["Email"].ToString());
                        item.SubItems.Add(reader["Phone"].ToString());
                        item.SubItems.Add(reader["Address"].ToString());
                        item.SubItems.Add(Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd"));
                        item.SubItems.Add(reader["LoyaltyPoints"].ToString());
                        item.SubItems.Add(reader["NameBill"].ToString());
                        lvCustomer.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
                }
            }
        }

        private void ClearForm()
        {
            txtCustomerName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtLoyaltyPoints.Clear();
            txtNameBill.Clear();
        }
    }
}
