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
    public partial class ManageBillDetail : Form
    {
        // Kết nối SQL Server
        private string connectionString = @"Data Source=Strix-G17;Initial Catalog=Mana_Computer;Integrated Security=True";

        public ManageBillDetail()
        {
            InitializeComponent();
        }

        private void ManageBillDetail_Load(object sender, EventArgs e)
        {
            // Cấu hình ListView
            lvBillDetail.View = View.Details;
            lvBillDetail.FullRowSelect = true; // Hiển thị toàn bộ hàng khi chọn
            lvBillDetail.GridLines = true;     // Hiển thị đường lưới
            lvBillDetail.Columns.Clear();      // Xóa cấu hình cột cũ (nếu có)

            // Thêm cột vào ListView với độ rộng cụ thể
            lvBillDetail.Columns.Add("BillDetailID", 100);
            lvBillDetail.Columns.Add("BillID", 100);
            lvBillDetail.Columns.Add("ProductID", 100);
            lvBillDetail.Columns.Add("Quantity", 100);
            lvBillDetail.Columns.Add("Price", 100);
            lvBillDetail.Columns.Add("TotalAmount", 120);

            LoadBillDetails();
        }

        // Tải dữ liệu từ SQL lên ListView
        private void LoadBillDetails()
        {
            lvBillDetail.Items.Clear(); // Xóa dữ liệu cũ trong ListView
            string query = "SELECT * FROM BillDetail";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Kiểm tra NULL trước khi thêm vào ListView
                        string billDetailID = reader["BillDetailID"]?.ToString() ?? "N/A";
                        string billID = reader["BillID"]?.ToString() ?? "N/A";
                        string productID = reader["ProductID"]?.ToString() ?? "N/A";
                        string quantity = reader["Quantity"]?.ToString() ?? "0";
                        string price = reader["Price"]?.ToString() ?? "0";
                        string totalAmount = reader["TotalAmount"]?.ToString() ?? "0";

                        ListViewItem item = new ListViewItem(billDetailID);
                        item.SubItems.Add(billID);
                        item.SubItems.Add(productID);
                        item.SubItems.Add(quantity);
                        item.SubItems.Add(price);
                        item.SubItems.Add(totalAmount);

                        lvBillDetail.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
            }
        }

        // Chức năng Thêm mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO BillDetail (BillID, ProductID, Quantity, Price, TotalAmount) VALUES (@BillID, @ProductID, @Quantity, @Price, @TotalAmount)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BillID", txtBillID.Text);
                cmd.Parameters.AddWithValue("@ProductID", txtProductID.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtPrice.Text));

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Thêm thành công!");
            LoadBillDetails();
        }

        // Chức năng Sửa
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "UPDATE BillDetail SET BillID = @BillID, ProductID = @ProductID, Quantity = @Quantity, Price = @Price, TotalAmount = @TotalAmount WHERE BillDetailID = @BillDetailID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BillDetailID", txtBillDetailID.Text);
                cmd.Parameters.AddWithValue("@BillID", txtBillID.Text);
                cmd.Parameters.AddWithValue("@ProductID", txtProductID.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtPrice.Text));

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Cập nhật thành công!");
            LoadBillDetails();
        }

        // Chức năng Xóa
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM BillDetail WHERE BillDetailID = @BillDetailID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BillDetailID", txtBillDetailID.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Xóa thành công!");
            LoadBillDetails();
        }

        // Đổ dữ liệu từ ListView vào TextBox khi chọn hàng
        private void lvBillDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvBillDetail.SelectedItems.Count > 0)
            {
                var selectedItem = lvBillDetail.SelectedItems[0];
                txtBillDetailID.Text = selectedItem.SubItems[0].Text;
                txtBillID.Text = selectedItem.SubItems[1].Text;
                txtProductID.Text = selectedItem.SubItems[2].Text;
                txtQuantity.Text = selectedItem.SubItems[3].Text;
                txtPrice.Text = selectedItem.SubItems[4].Text;
                txtTotalAmount.Text = selectedItem.SubItems[5].Text;
            }
        }
    }
}
