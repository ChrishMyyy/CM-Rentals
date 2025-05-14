using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace CarRent
{
    public partial class frmEditCustomers : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private string customerCode;
        private string currentImagePath;

        public frmEditCustomers(string customerCode)
        {
            InitializeComponent();
            this.customerCode = customerCode;
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM tblCustomers WHERE CustomerCode = @CustomerCode";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerCode", customerCode);
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFullName.Text = reader["FullName"].ToString();
                                txtContactNumber.Text = reader["ContactNumber"].ToString();
                                txtAddress.Text = reader["Address"].ToString();
                                cmbStatus.Text = reader["Status"].ToString();
                                currentImagePath = reader["LicenseImage"]?.ToString();

                                if (!string.IsNullOrEmpty(currentImagePath) && System.IO.File.Exists(currentImagePath))
                                {
                                    pbxProfile.Image = Image.FromFile(currentImagePath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer data: " + ex.Message);
            }
        }

      

        // Add btnUploadImage_Click, btnCancel_Click similar to frmEditStaff
    }
}
