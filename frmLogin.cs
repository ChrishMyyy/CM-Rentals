using System.Data.OleDb;
namespace CarRent
{
    public partial class frmLogin : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private bool isPasswordVisible = false;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxLoginUsername.Text) || string.IsNullOrEmpty(tbxLoginPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Create SQL query to check credentials and that user is active
                    string query = "SELECT UserCode, Fullname, Role, UserImage FROM tblUsers WHERE Username = @Username AND Password = @Password AND Status = 'Active'";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Username", tbxLoginUsername.Text);
                        command.Parameters.AddWithValue("@Password", tbxLoginPassword.Text);

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Login successful
                                string? userCode = reader["UserCode"]?.ToString();
                                string? fullName = reader["Fullname"]?.ToString();
                                string? role = reader["Role"]?.ToString();
                                string? userImage = reader["UserImage"]?.ToString();

                                // Store user information in Session
                                Session.CurrentUserCode = userCode;
                                Session.CurrentUserFullName = fullName;
                                Session.CurrentUserImagePath = userImage ?? ""; // Use the image path from database or empty string if null
                                Session.CurrentUserRole = role ?? ""; // Store the user's role

                                MessageBox.Show($"Welcome, {fullName}!", "Login Successful",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Open appropriate dashboard based on role
                                if (role == "Admin")
                                {
                                    frmAdminDashboard adminDashboard = new frmAdminDashboard();
                                    adminDashboard.Show();
                                }
                                else
                                {
                                frmDashboard dashboard = new frmDashboard();
                                dashboard.Show();
                                }
                                this.Hide(); // Hide login form
                            }
                            else
                            {
                                // Login failed
                                MessageBox.Show("Invalid username or password, or account is inactive.",
                                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            frmRegistration registrationForm = new frmRegistration();
            registrationForm.Show();
        }

        private void tbxLoginUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxLoginPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPasswordVisibility_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            tbxLoginPassword.PasswordChar = isPasswordVisible ? '\0' : '●';
            btnPasswordVisibility.Image = isPasswordVisible ? Properties.Resources.eye_open : Properties.Resources.eye_close;
        }
    }
}
