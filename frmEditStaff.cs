using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace CarRent
{
    public partial class frmEditStaff : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private string userCode;
        private string currentImagePath;

        public frmEditStaff(string userCode)
        {
            InitializeComponent();
            this.userCode = userCode;
            LoadStaffData();
        }

        private void LoadStaffData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM tblUsers WHERE UserCode = @UserCode";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserCode", userCode);
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFullName.Text = reader["Fullname"].ToString();
                                txtUsername.Text = reader["Username"].ToString();
                                txtPassword.Text = reader["Password"].ToString();
                                cmbRole.Text = reader["Role"].ToString();
                                cmbStatus.Text = reader["Status"].ToString();
                                currentImagePath = reader["UserImage"]?.ToString();

                                // Load image if exists
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
                MessageBox.Show("Error loading staff data: " + ex.Message);
            }
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentImagePath = openFileDialog.FileName;
                    pbxProfile.Image = Image.FromFile(currentImagePath);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all required fields!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE tblUsers 
                                   SET Fullname = @Fullname,
                                       Username = @Username,
                                       [Password] = @Password,
                                       Role = @Role,
                                       [Status] = @Status,
                                       UserImage = @UserImage
                                   WHERE UserCode = @UserCode";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Fullname", txtFullName.Text);
                        command.Parameters.AddWithValue("@Username", txtUsername.Text);
                        command.Parameters.AddWithValue("@Password", txtPassword.Text);
                        command.Parameters.AddWithValue("@Role", cmbRole.Text);
                        command.Parameters.AddWithValue("@Status", cmbStatus.Text);
                        command.Parameters.AddWithValue("@UserImage", currentImagePath);
                        command.Parameters.AddWithValue("@UserCode", userCode);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Staff details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No changes were made.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating staff: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 