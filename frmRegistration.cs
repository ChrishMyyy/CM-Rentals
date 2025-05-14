    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Data.OleDb;


    namespace CarRent
    {
        public partial class frmRegistration : Form
        {
            private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";

            public frmRegistration()
            {
                InitializeComponent();
            }

            private void tbxRegistrationFullName_TextChanged(object sender, EventArgs e)
            {

            }

            private void tbxRegistratonUsername_TextChanged(object sender, EventArgs e)
            {

            }

            private void tbxRegistratonPassword_TextChanged(object sender, EventArgs e)
            {

            }

            private void tbxRegistratonConPass_TextChanged(object sender, EventArgs e)
            {

            }

            private void rbAdmin_CheckedChanged(object sender, EventArgs e)
            {

            }

            private void rbStaff_CheckedChanged(object sender, EventArgs e)
            {

            }
            private void ClearFields()
            {
                tbxRegistrationFullName.Clear();
                tbxRegistratonUsername.Clear();
                tbxRegistratonPassword.Clear();
                tbxRegistratonConPass.Clear();
                rbAdmin.Checked = false;
                rbStaff.Checked = false;
            }
            private string GenerateUserCode()
            {
                int count = 0;

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM tblUsers";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            count = (int)cmd.ExecuteScalar();
                        }
                        catch
                        {
                            count = 0;
                        }
                    }
                }

                return "USER" + (count + 1).ToString("D4"); // USER0001, USER0002...
            }
        private void btnRegisterUser_Click(object sender, EventArgs e)
            {
                string fullname = tbxRegistrationFullName.Text.Trim();
                string username = tbxRegistratonUsername.Text.Trim();
                string password = tbxRegistratonPassword.Text;
                string confirmPassword = tbxRegistratonConPass.Text;
                string role = rbAdmin.Checked ? "Admin" : rbStaff.Checked ? "Staff" : "";
                string status = "Pending";
                string dateCreated = DateTime.Now.ToString("yyyy-MM-dd");

                // Validation
                if (fullname == "" || username == "" || password == "" || confirmPassword == "" || role == "")
                {
                    MessageBox.Show("Please fill in all fields and select a role.", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match.", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                string userCode = GenerateUserCode();

                // Insert into tblUsers
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "INSERT INTO tblUsers (UserCode, Fullname, Username, [Password], Role, DateCreated, Status) " +
                                   "VALUES (?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", userCode);
                        cmd.Parameters.AddWithValue("?", fullname);
                        cmd.Parameters.AddWithValue("?", username);
                        cmd.Parameters.AddWithValue("?", password);
                        cmd.Parameters.AddWithValue("?", role);
                        cmd.Parameters.AddWithValue("?", dateCreated);
                        cmd.Parameters.AddWithValue("?", status);

                    try
                        {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Registration request submitted successfully! Please wait for admin approval.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
