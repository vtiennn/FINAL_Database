using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FINAL_Database
{
    public partial class ManageComputer : Form
    {
        public ManageComputer()
        {
            InitializeComponent();
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            Bill billForm = new Bill();
            billForm.Show();

            this.Hide();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            ManageStaff manageStaffForm = new ManageStaff();
            manageStaffForm.Show();

            this.Hide();
        }
        private void btnOpenStaffForm_Click(object sender, EventArgs e)
        {
            ManageStaff manageStaffForm = new ManageStaff();
            this.Hide();  // Ẩn form hiện tại
            manageStaffForm.ShowDialog();  // Hiển thị form ManageStaff dưới dạng modal
            this.Show();  // Hiển thị lại form chính sau khi đóng form ManageStaff
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            ManageCustomer manageCustomerForm = new ManageCustomer();
            manageCustomerForm.Show();

            this.Hide();
        }

        private void btnBillDetail_Click(object sender, EventArgs e)
        {
            ManageBillDetail manageBillDetailForm = new ManageBillDetail();
            manageBillDetailForm.Show();

            this.Hide();
        }
    }
}
