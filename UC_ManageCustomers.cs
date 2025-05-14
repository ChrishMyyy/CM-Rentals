using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRent
{
    public partial class UC_ManageCustomers : UserControl
    {
        private readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private bool isEditing = false;
        private string currentCustomerCode = string.Empty;
        private string licensePath = string.Empty;
        private string userCode;

        public UC_ManageCustomers()
        {
            InitializeComponent();
            LoadCustomers();
            SetupFilterSearch();
            SetupDataGridViewSelection();
        }

        private void LoadCustomers()
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    string query = @"SELECT 
                                    CustomerCode,
                                    FullName,
                                    ContactNumber,
                                    Address,
                                    Gender,
                                    LoyaltyPoints,
                                    CustomerStatus
                                FROM tblCustomers
                                WHERE Status = 'Active'
                                ORDER BY CustomerCode";

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvCustomerTable.DataSource = dt;

                    // Configure the DataGridView columns after data is loaded
                    dgvCustomerTable.Columns["CustomerCode"].HeaderText = "Customer";
                    dgvCustomerTable.Columns["FullName"].HeaderText = "Full Name";
                    dgvCustomerTable.Columns["ContactNumber"].HeaderText = " Contact ";
                    dgvCustomerTable.Columns["Address"].HeaderText = "Address";
                    dgvCustomerTable.Columns["Gender"].HeaderText = "Gender";
                    dgvCustomerTable.Columns["LoyaltyPoints"].HeaderText = "Points";
                    dgvCustomerTable.Columns["CustomerStatus"].HeaderText = "Status";

                    // Auto-size columns to fit content
                    dgvCustomerTable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCustomerRentHistory_Click(object sender, EventArgs e)
        {
            if (dgvCustomerTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvCustomerTable.SelectedRows[0];
            string customerCode = row.Cells["CustomerCode"].Value?.ToString() ?? "";
            string fullName = row.Cells["FullName"].Value?.ToString() ?? "";
            string licenseImage = "";

            // Fetch license image path from DB
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT LicenseImage FROM tblCustomers WHERE CustomerCode = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", customerCode);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        licenseImage = result.ToString();
                }
            }

            var historyForm = new frmCustomerRentalHistory(customerCode, fullName, licenseImage);
            historyForm.ShowDialog();
        }

        private void cbxCustomerFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxCustomerSearch.Text))
            {
                FilterCustomerData(sender, e);
            }
        }

        private void SetupFilterSearch()
        {
            // Clear existing items and add filter options based on DataGridView columns
            cbxCustomerFilter.Items.Clear();
            foreach (DataGridViewColumn column in dgvCustomerTable.Columns)
            {
                cbxCustomerFilter.Items.Add(column.HeaderText);
            }

            // Set default selection
            if (cbxCustomerFilter.Items.Count > 0)
            {
                cbxCustomerFilter.SelectedIndex = 0;
            }

            // Wire up event handlers
            tbxCustomerSearch.TextChanged += FilterCustomerData;
            cbxCustomerFilter.SelectedIndexChanged += FilterCustomerData;
        }

        private void FilterCustomerData(object sender, EventArgs e)
        {
            string searchKeyword = tbxCustomerSearch.Text.Trim().ToLower();
            string? selectedFilter = cbxCustomerFilter.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(searchKeyword) || string.IsNullOrEmpty(selectedFilter))
            {
                LoadCustomers();
                return;
            }

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string columnName = MapFilterToColumnName(selectedFilter);
                    string query = $"SELECT CustomerCode, FullName, ContactNumber, Address, Gender, LoyaltyPoints, CustomerStatus " +
                                 $"FROM tblCustomers WHERE {columnName} LIKE ? AND Status = 'Active' ORDER BY CustomerCode";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", $"%{searchKeyword}%");
                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvCustomerTable.DataSource = dt;

                        // Reapply column headers
                        dgvCustomerTable.Columns["CustomerCode"].HeaderText = "Customer";
                        dgvCustomerTable.Columns["FullName"].HeaderText = "Full Name";
                        dgvCustomerTable.Columns["ContactNumber"].HeaderText = " Contact ";
                        dgvCustomerTable.Columns["Address"].HeaderText = "Address";
                        dgvCustomerTable.Columns["Gender"].HeaderText = "Gender";
                        dgvCustomerTable.Columns["LoyaltyPoints"].HeaderText = "Points";
                        dgvCustomerTable.Columns["CustomerStatus"].HeaderText = "Status";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadCustomers();
            }
        }

        private string MapFilterToColumnName(string filterName)
        {
            return filterName switch
            {
                "Customer" => "CustomerCode",
                "Full Name" => "FullName",
                " Contact " => "ContactNumber",
                "Address" => "Address",
                "Gender" => "Gender",
                "Points" => "LoyaltyPoints",
                "Status" => "CustomerStatus",
                _ => "FullName"
            };
        }

        private void btnAddLicenseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Select License Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (pbxCustomerLicense.Image != null)
                        {
                            pbxCustomerLicense.Image.Dispose();
                            pbxCustomerLicense.Image = null;
                        }

                        licensePath = openFileDialog.FileName;
                        pbxCustomerLicense.Image = Image.FromFile(licensePath);
                        SaveLicensePathToDatabase(licensePath);
                        UpdateLicenseButtonsVisibility();
                        MessageBox.Show("License image added successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message);
                    }
                }
            }
        }

        private int GetCurrentCustomerID()
        {
            if (string.IsNullOrEmpty(currentCustomerCode))
            {
                MessageBox.Show("Please select a customer first");
                return -1;
            }

            int customerID = -1;
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CustomerID FROM tblCustomers WHERE CustomerCode = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", currentCustomerCode);
                        object? result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            customerID = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Could not find CustomerID for the selected customer.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving CustomerID: " + ex.Message);
            }

            return customerID;
        }

        private void SaveLicensePathToDatabase(string imagePath)
        {
            int customerID = GetCurrentCustomerID();
            if (customerID == -1) return;

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE tblCustomers SET LicenseImage = ? WHERE CustomerID = ?";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", imagePath);
                        command.Parameters.AddWithValue("?", customerID);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Failed to update license image. Customer not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
        }

        private void pbxCustomerLicense_Click(object sender, EventArgs e)
        {
            if (pbxCustomerLicense.Image != null)
            {
                using (Form imageViewer = new Form())
                {
                    imageViewer.Text = "License Image Viewer";
                    imageViewer.Size = new Size(800, 600);

                    PictureBox pb = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = pbxCustomerLicense.Image
                    };

                    imageViewer.Controls.Add(pb);
                    imageViewer.ShowDialog();
                }
            }
            else
            {
                btnAddLicenseImage_Click(sender, e);
            }
        }

        private void btnCustomerAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateCustomerInputs())
                    return;

                if (isEditing)
                {
                    MessageBox.Show("You are currently editing a customer. Please save your changes or clear the form first.",
                                   "Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string customerCode = GenerateCustomerCode();

                // Declare variables only once
                string gender = cbxCustomerGender.SelectedItem?.ToString() ?? string.Empty;
                string status = cbxCustomerStatus.SelectedItem?.ToString() ?? string.Empty;
                string license = string.IsNullOrEmpty(licensePath) ? string.Empty : licensePath;

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    // Check for duplicate contact number
                    string checkQuery = "SELECT COUNT(*) FROM tblCustomers WHERE ContactNumber = ?";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("?", tbxCustomerNumber.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("A customer with this contact number already exists.",
                                           "Duplicate Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert new customer
                    string insertQuery = @"INSERT INTO tblCustomers 
                                          (CustomerCode, FullName, ContactNumber, Address, Gender, CustomerStatus, LoyaltyPoints, LicenseImage) 
                                          VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("?", customerCode);
                        insertCmd.Parameters.AddWithValue("?", tbxCustomerName.Text);
                        insertCmd.Parameters.AddWithValue("?", tbxCustomerNumber.Text);
                        insertCmd.Parameters.AddWithValue("?", tbxCustomerAddress.Text);
                        insertCmd.Parameters.AddWithValue("?", gender);
                        insertCmd.Parameters.AddWithValue("?", status);
                        insertCmd.Parameters.AddWithValue("?", 0); // Initial loyalty points
                        insertCmd.Parameters.AddWithValue("?", license);

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCustomers();
                            ClearInputFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCustomerDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isEditing || string.IsNullOrEmpty(currentCustomerCode))
                {
                    MessageBox.Show("Please select a customer to delete first.",
                                   "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show("Are you sure you want to delete this customer?",
                                                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();

                        // Check if customer has rental history
                        string checkQuery = "SELECT COUNT(*) FROM tblRentals WHERE CustomerCode = ?";
                        using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("?", currentCustomerCode);

                            try
                            {
                                int rentalCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                                if (rentalCount > 0)
                                {
                                    DialogResult dr = MessageBox.Show(
                                        "This customer has rental history. Would you like to set them to 'Returned' instead of deleting?",
                                        "Customer Has Rentals",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);

                                    if (dr == DialogResult.Yes)
                                    {
                                        string updateQuery = "UPDATE tblCustomers SET CustomerStatus = 'Returned' WHERE CustomerCode = ?";
                                        using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                                        {
                                            updateCmd.Parameters.AddWithValue("?", currentCustomerCode);
                                            updateCmd.ExecuteNonQuery();
                                        }

                                        MessageBox.Show("Customer status set to Returned.", "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LoadCustomers();
                                        ClearInputFields();
                                        return;
                                    }
                                }
                            }
                            catch
                            {
                                // If tblRentals doesn't exist, continue with delete
                            }
                        }

                        // Delete the customer
                        string deleteQuery = "DELETE FROM tblCustomers WHERE CustomerCode = ?";
                        using (OleDbCommand deleteCmd = new OleDbCommand(deleteQuery, conn))
                        {
                            deleteCmd.Parameters.AddWithValue("?", currentCustomerCode);

                            int rowsAffected = deleteCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadCustomers();
                                ClearInputFields();
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadLicenseImage(int customerID)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT LicenseImage FROM tblCustomers WHERE CustomerID = ?";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", customerID);
                        object? result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string imagePath = result.ToString() ?? string.Empty;

                            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                            {
                                if (pbxCustomerLicense.Image != null)
                                {
                                    pbxCustomerLicense.Image.Dispose();
                                }
                                pbxCustomerLicense.Image = Image.FromFile(imagePath);
                                licensePath = imagePath;
                            }
                            else
                            {
                                pbxCustomerLicense.Image = null;
                            }
                        }
                        else
                        {
                            pbxCustomerLicense.Image = null;
                        }

                        UpdateLicenseButtonsVisibility();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading license image: " + ex.Message);
                }
            }
        }

        private void btnCustomerSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateCustomerInputs())
                    return;

                if (!isEditing || string.IsNullOrEmpty(currentCustomerCode))
                {
                    MessageBox.Show("Please select a customer to update first.",
                                   "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    // Check for duplicate contact number
                    string checkQuery = "SELECT COUNT(*) FROM tblCustomers WHERE ContactNumber = ? AND CustomerCode <> ?";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("?", tbxCustomerNumber.Text);
                        checkCmd.Parameters.AddWithValue("?", currentCustomerCode);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Another customer with this contact number already exists.",
                                           "Duplicate Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Update customer information
                    string updateQuery = @"UPDATE tblCustomers 
                                          SET FullName = ?, ContactNumber = ?, Address = ?, 
                                          Gender = ?, CustomerStatus = ?, LicenseImage = ? 
                                          WHERE CustomerCode = ?";

                    using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                    {
                        string gender = cbxCustomerGender.SelectedItem?.ToString() ?? string.Empty;
                        string status = cbxCustomerStatus.SelectedItem?.ToString() ?? string.Empty;

                        updateCmd.Parameters.AddWithValue("?", tbxCustomerName.Text);
                        updateCmd.Parameters.AddWithValue("?", tbxCustomerNumber.Text);
                        updateCmd.Parameters.AddWithValue("?", tbxCustomerAddress.Text);
                        updateCmd.Parameters.AddWithValue("?", gender);
                        updateCmd.Parameters.AddWithValue("?", status);
                        updateCmd.Parameters.AddWithValue("?", licensePath);
                        updateCmd.Parameters.AddWithValue("?", currentCustomerCode);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCustomers();
                            ClearInputFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCustomerClear_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private bool ValidateCustomerInputs()
        {
            if (string.IsNullOrWhiteSpace(tbxCustomerName.Text))
            {
                MessageBox.Show("Please enter a customer name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxCustomerName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbxCustomerNumber.Text))
            {
                MessageBox.Show("Please enter a contact number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxCustomerNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbxCustomerAddress.Text))
            {
                MessageBox.Show("Please enter an address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxCustomerAddress.Focus();
                return false;
            }

            if (cbxCustomerGender.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a gender.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxCustomerGender.Focus();
                return false;
            }

            if (cbxCustomerStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxCustomerStatus.Focus();
                return false;
            }

            return true;
        }

        private void ClearInputFields()
        {
            tbxCustomerName.Text = string.Empty;
            tbxCustomerNumber.Text = string.Empty;
            tbxCustomerAddress.Text = string.Empty;

            cbxCustomerGender.SelectedIndex = -1;
            cbxCustomerStatus.SelectedIndex = -1;

            if (pbxCustomerLicense.Image != null)
            {
                pbxCustomerLicense.Image.Dispose();
                pbxCustomerLicense.Image = null;
            }

            isEditing = false;
            currentCustomerCode = string.Empty;
            licensePath = string.Empty;

            UpdateLicenseButtonsVisibility();
        }

        private void UC_ManageCustomers_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCustomers();

                cbxCustomerGender.Items.Clear();
                cbxCustomerStatus.Items.Clear();
                cbxCustomerFilter.Items.Clear();

                cbxCustomerGender.Items.AddRange(new string[] { "Male", "Female", "Non-Binary" });
                cbxCustomerStatus.Items.AddRange(new string[] { "Ongoing", "Returned" });
                cbxCustomerFilter.Items.AddRange(new string[] { "FullName", "ContactNumber", "Address", "Gender", "Status", "DateRegistered" });

                if (cbxCustomerFilter.Items.Count > 0)
                {
                    cbxCustomerFilter.SelectedIndex = 0;
                }

                SetupFilterSearch();
                UpdateLicenseButtonsVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbxCustomerSearch_TextChanged_1(object sender, EventArgs e)
        {
            FilterCustomerData(sender, e);
        }

        private string GenerateCustomerCode()
        {
            string newCode = "CUST0001";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT MAX(CustomerCode) FROM tblCustomers WHERE CustomerCode LIKE 'CUST%'";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        var result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string highestCode = result.ToString() ?? string.Empty;

                            if (highestCode.Length >= 8)
                            {
                                string numberPart = highestCode.Substring(4);
                                if (int.TryParse(numberPart, out int highestNumber))
                                {
                                    newCode = $"CUST{(highestNumber + 1):D4}";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating customer code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return newCode;
        }

        private void UpdateLicenseButtonsVisibility()
        {
            bool hasImage = (pbxCustomerLicense.Image != null);

            btnDeleteLicenseImage.Visible = hasImage;
            btnAddLicenseImage.Visible = !hasImage;
        }

        private void btnDeleteLicenseImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (pbxCustomerLicense.Image == null)
                {
                    MessageBox.Show("No license image to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show("Are you sure you want to delete this license image?",
                                                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    int customerID = GetCurrentCustomerID();
                    if (customerID == -1)
                        return;

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE tblCustomers SET LicenseImage = NULL WHERE CustomerID = ?";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("?", customerID);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                if (pbxCustomerLicense.Image != null)
                                {
                                    pbxCustomerLicense.Image.Dispose();
                                    pbxCustomerLicense.Image = null;
                                }
                                licensePath = string.Empty;

                                MessageBox.Show("License image deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                UpdateLicenseButtonsVisibility();
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete license image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting license image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tbxCustomerName_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbxCustomerNumber_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbxCustomerAddress_TextChanged(object sender, EventArgs e)
        {
        }

        private void cbxCustomerStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tbxCustomerName_Click(object sender, EventArgs e)
        {
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void SetupDataGridViewSelection()
        {
            dgvCustomerTable.SelectionChanged += DgvCustomerTable_SelectionChanged;
        }

        private void DgvCustomerTable_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomerTable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvCustomerTable.SelectedRows[0];
                DisplayCustomerDetails(selectedRow);
            }
        }

        private void DisplayCustomerDetails(DataGridViewRow row)
        {
            try
            {
                // Get the customer code from the selected row
                currentCustomerCode = row.Cells["CustomerCode"].Value?.ToString() ?? string.Empty;

                // Populate the text boxes and combo boxes
                tbxCustomerName.Text = row.Cells["FullName"].Value?.ToString() ?? string.Empty;
                tbxCustomerNumber.Text = row.Cells["ContactNumber"].Value?.ToString() ?? string.Empty;
                tbxCustomerAddress.Text = row.Cells["Address"].Value?.ToString() ?? string.Empty;

                // Set gender combo box
                string gender = row.Cells["Gender"].Value?.ToString() ?? string.Empty;
                cbxCustomerGender.SelectedItem = gender;

                // Set status combo box
                string status = row.Cells["CustomerStatus"].Value?.ToString() ?? string.Empty;
                cbxCustomerStatus.SelectedItem = status;

                // Load the license image
                LoadCustomerLicense(currentCustomerCode);

                // Set editing mode
                isEditing = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying customer details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerLicense(string customerCode)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT LicenseImage FROM tblCustomers WHERE CustomerCode = ?";
                    
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", customerCode);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string imagePath = result.ToString();
                            if (File.Exists(imagePath))
                            {
                                if (pbxCustomerLicense.Image != null)
                                {
                                    pbxCustomerLicense.Image.Dispose();
                                }
                                pbxCustomerLicense.Image = Image.FromFile(imagePath);
                                licensePath = imagePath;
                            }
                            else
                            {
                                pbxCustomerLicense.Image = null;
                            }
                        }
                        else
                        {
                            pbxCustomerLicense.Image = null;
                        }
                    }
                }
                UpdateLicenseButtonsVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading license image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbxCustomerLicense.Image = null;
                licensePath = string.Empty;
                UpdateLicenseButtonsVisibility();
            }
        }
    }
}