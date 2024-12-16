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
    public partial class Bill : Form
    {
        private string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";

        public Bill()
        {
            InitializeComponent();
            txtSearch.TextChanged += txtSearch_TextChanged;
            lvViewBill.SelectedIndexChanged += lvViewBill_SelectedIndexChanged;
        }

        private void frmBill_Load(object sender, EventArgs e)
        {
            LoadAllBills();
        }

        // Tải tất cả hóa đơn từ database
        private void LoadAllBills()
        {
            string query = "SELECT BillID, CustomerID, StaffID, Date, Status, NameBill, TotalAmount FROM Bill";
            lvViewBill.Items.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, con);
                try
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["BillID"].ToString());
                        item.SubItems.Add(reader["CustomerID"].ToString());
                        item.SubItems.Add(reader["StaffID"].ToString());
                        item.SubItems.Add(Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"));
                        item.SubItems.Add(reader["Status"].ToString());
                        item.SubItems.Add(reader["NameBill"].ToString());
                        item.SubItems.Add(reader["TotalAmount"].ToString()); // Hiển thị TotalAmount
                        lvViewBill.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Sự kiện chọn mục trong ListView tự động điền vào TextBox
        private void lvViewBill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvViewBill.SelectedItems.Count > 0)
            {
                var selectedItem = lvViewBill.SelectedItems[0];

                txtBillID.Text = selectedItem.SubItems[0].Text;
                txtCustomerID.Text = selectedItem.SubItems[1].Text;
            }
        }

        // Thêm dữ liệu mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Bill (BillID, CustomerID, StaffID, Date, Status, NameBill, TotalAmount) VALUES (@BillID, @CustomerID, @StaffID, @Date, @Status, @NameBill, @TotalAmount)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@BillID", txtBillID.Text);
                command.Parameters.AddWithValue("@CustomerID", txtCustomerID.Text);
                command.Parameters.AddWithValue("@StaffID", txtStaffID.Text);
                command.Parameters.AddWithValue("@Date", dtpDate.Value);
                command.Parameters.AddWithValue("@Status", txtStatus.Text);
                command.Parameters.AddWithValue("@NameBill", txtNameBill.Text);
                command.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text); // Thêm TotalAmount

                try
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAllBills(); // Refresh danh sách hóa đơn
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Cập nhật dữ liệu đã chọn
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvViewBill.SelectedItems.Count > 0)
            {
                string query = "UPDATE Bill SET CustomerID = @CustomerID, StaffID = @StaffID, Date = @Date, Status = @Status, NameBill = @NameBill, TotalAmount = @TotalAmount WHERE BillID = @BillID";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@BillID", txtBillID.Text);
                    command.Parameters.AddWithValue("@CustomerID", txtCustomerID.Text);
                    command.Parameters.AddWithValue("@StaffID", txtStaffID.Text);
                    command.Parameters.AddWithValue("@Date", dtpDate.Value);
                    command.Parameters.AddWithValue("@Status", txtStatus.Text);
                    command.Parameters.AddWithValue("@NameBill", txtNameBill.Text);
                    command.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text); // Cập nhật TotalAmount

                    try
                    {
                        con.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllBills(); // Refresh danh sách hóa đơn
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Xóa dữ liệu đã chọn
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvViewBill.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = lvViewBill.SelectedItems[0];
            string query = "DELETE FROM Bill WHERE BillID = @BillID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@BillID", selectedItem.SubItems[0].Text);

                try
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    LoadAllBills(); // Cập nhật ListView sau khi xóa

                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Làm trống các TextBox mới
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtBillID.Clear();
            txtCustomerID.Clear();
            txtStaffID.Clear();
            txtNameBill.Clear();
            txtTotalAmount.Clear();
            txtStatus.Clear();
            txtBillID.Focus();
        }

        // Tìm kiếm hóa đơn theo CustomerID
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            lvViewBill.Items.Clear();
            string query = "SELECT BillID, CustomerID FROM Bill WHERE LOWER(CustomerID) LIKE @SearchText";

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
                        ListViewItem item = new ListViewItem(reader["BillID"].ToString());
                        item.SubItems.Add(reader["CustomerID"].ToString());
                        lvViewBill.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Bill_Load(object sender, EventArgs e)
        {
            lvViewBill.Columns.Add("Mã hóa đơn", 100);
            lvViewBill.Columns.Add("Mã khách hàng", 150);
            lvViewBill.Columns.Add("Mã nhân viên", 100);
            lvViewBill.Columns.Add("Ngày", 120);
            lvViewBill.Columns.Add("Trạng thái", 120);
            lvViewBill.Columns.Add("Tên hóa đơn", 100);
            lvViewBill.Columns.Add("Tổng tiền", 120);

            lvViewBill.View = View.Details; // Hiển thị theo dạng chi tiết
            lvViewBill.FullRowSelect = true; // Chọn cả hàng
            lvViewBill.GridLines = true; // Hiển thị đường viền
            LoadAllBills();
        }
    }
}