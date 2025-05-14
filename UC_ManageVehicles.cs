using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CarRent
{
    public partial class UC_ManageVehicles : UserControl
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private DataTable vehicleData;
        private string selectedCarCode = "";  // To track the selected car code internally
        private string currentImagePath = "";
        private frmVehicleGallery galleryForm;

        public UC_ManageVehicles()
        {
            InitializeComponent();

            // Initialize the vehicle filter combobox
            InitializeFilterComboBox();

            // Initialize status, fuel type, transmission, and vehicle type comboboxes
            InitializeComboBoxes();

            // Load vehicle data when the control is created
            LoadVehicleData();
            pbxCarImage.Click -= pbxCarImage_Click; // Prevent duplicate subscription
            pbxCarImage.Click += pbxCarImage_Click;
        }

        private void InitializeFilterComboBox()
        {
            // Add filter options to the combobox
            cbxVehicleFilter.Items.Add("CarCode");
            cbxVehicleFilter.Items.Add("Make");
            cbxVehicleFilter.Items.Add("Model");
            cbxVehicleFilter.Items.Add("Year_");
            cbxVehicleFilter.Items.Add("PlateNumber");
            cbxVehicleFilter.Items.Add("Transmission");
            cbxVehicleFilter.Items.Add("FuelType");
            cbxVehicleFilter.Items.Add("VehicleType");
            cbxVehicleFilter.Items.Add("Availability");
            cbxVehicleFilter.Items.Add("VehicleStatus");
            cbxVehicleFilter.Items.Add("DateAdded");

            // Set default filter
            cbxVehicleFilter.SelectedIndex = 0;
        }

        private void InitializeComboBoxes()
        {
            // Status ComboBox - Availability field only has Available and Not Available
            cbxVehicleStatus.Items.Clear();
            cbxVehicleStatus.Items.Add("Available");
            cbxVehicleStatus.Items.Add("Not Available");

            // Vehicle Status ComboBox - These are the ONLY valid options for VehicleStatus
            cbxVehicleInternalStatus.Items.Clear();
            cbxVehicleInternalStatus.Items.Add("Available");
            cbxVehicleInternalStatus.Items.Add("Under Maintenance");

            // Fuel Type ComboBox
            cbxFuelType.Items.Clear();
            cbxFuelType.Items.Add("Gasoline");
            cbxFuelType.Items.Add("Diesel");
            cbxFuelType.Items.Add("Electric");

            // Transmission ComboBox
            cbxVehicleTransmission.Items.Clear();
            cbxVehicleTransmission.Items.Add("Manual");
            cbxVehicleTransmission.Items.Add("Automatic");

            // Vehicle Type ComboBox
            cbxVehicleType.Items.Clear();
            cbxVehicleType.Items.Add("SUV");
            cbxVehicleType.Items.Add("Sedan");
            cbxVehicleType.Items.Add("Pickup");
            cbxVehicleType.Items.Add("Van");
        }

        private void LoadVehicleData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Keep the original query that selects all needed columns
                    string query = @"SELECT CarCode, Make, Model, Year_, PlateNumber, 
                           Transmission, FuelType, VehicleType, Availability, 
                           VehicleStatus, DateAdded, RentalPrice FROM tblVehicles";

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    vehicleData = new DataTable();
                    adapter.Fill(vehicleData);

                    // Set the DataGridView data source
                    dgvVehicles.DataSource = vehicleData;

                    // Hide Availability but show VehicleStatus
                    dgvVehicles.Columns["Availability"].Visible = false;
                    dgvVehicles.Columns["RentalPrice"].Visible = false;

                    // Apply custom style
                    ApplyVehicleGridStyle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading vehicle data: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVehicleRentalHistory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCarCode))
            {
                MessageBox.Show("Please select a vehicle first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string model = "";
            string plateNumber = "";
            if (dgvVehicles.CurrentRow != null)
            {
                model = dgvVehicles.CurrentRow.Cells["Model"].Value?.ToString() ?? "";
                plateNumber = dgvVehicles.CurrentRow.Cells["PlateNumber"].Value?.ToString() ?? "";
            }

            frmVehicleRentalHistory historyForm = new frmVehicleRentalHistory(selectedCarCode, model, plateNumber);
            historyForm.ShowDialog();
        }

        private void cbxVehicleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When the filter category changes, re-apply the filter with the current search text
            ApplyFilter();
        }

        private void tbxVehicleSearch_TextChanged(object sender, EventArgs e)
        {
            // When the search text changes, apply the filter
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (vehicleData == null || cbxVehicleFilter.SelectedItem == null)
                return;

            string filterColumn = cbxVehicleFilter.SelectedItem.ToString();
            string searchText = tbxVehicleSearch.Text.Trim();

            // Create a filtered view of the data
            DataView dv = vehicleData.DefaultView;

            if (!string.IsNullOrEmpty(searchText))
            {
                // Apply filter based on the selected column and search text
                dv.RowFilter = $"CONVERT({filterColumn}, 'System.String') LIKE '%{searchText}%'";
            }
            else
            {
                // Clear filter if search text is empty
                dv.RowFilter = string.Empty;
            }

            // Update the DataGridView with the filtered data
            dgvVehicles.DataSource = dv.ToTable();
            ApplyVehicleGridStyle();
        }

        private void ApplyVehicleGridStyle()
        {
            if (dgvVehicles.Columns.Count == 0) return;

            dgvVehicles.Columns["CarCode"].HeaderText = "Car Code";
            dgvVehicles.Columns["Make"].HeaderText = "Make";
            dgvVehicles.Columns["Model"].HeaderText = "Model";
            dgvVehicles.Columns["Year_"].HeaderText = "Year";
            dgvVehicles.Columns["PlateNumber"].HeaderText = "Plate Number";
            dgvVehicles.Columns["Transmission"].HeaderText = "Transmission";
            dgvVehicles.Columns["FuelType"].HeaderText = "Fuel Type";
            dgvVehicles.Columns["VehicleType"].HeaderText = "Vehicle Type";
            dgvVehicles.Columns["VehicleStatus"].HeaderText = "Status";
            dgvVehicles.Columns["DateAdded"].HeaderText = "Date Added";

            dgvVehicles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvVehicles.EnableHeadersVisualStyles = false;
            dgvVehicles.ColumnHeadersDefaultCellStyle.BackColor = Color.Firebrick;
            dgvVehicles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvVehicles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvVehicles.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVehicles.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 6, 8, 6); // Add vertical and horizontal padding
            dgvVehicles.ColumnHeadersHeight = 38;

            dgvVehicles.RowsDefaultCellStyle.BackColor = Color.White;
            dgvVehicles.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
            dgvVehicles.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvVehicles.DefaultCellStyle.ForeColor = Color.Black;
            dgvVehicles.DefaultCellStyle.SelectionBackColor = Color.LightCoral;
            dgvVehicles.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvVehicles.DefaultCellStyle.Padding = new Padding(6, 4, 6, 4); // Add vertical and horizontal padding

            dgvVehicles.GridColor = Color.White; // Hide gridlines
            dgvVehicles.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvVehicles.RowTemplate.Height = 34; // Increase row height for spacing
            dgvVehicles.RowHeadersVisible = false;
        }

        private void dgvVehicles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Only proceed if a valid row is clicked (not header or empty area)
            if (e.RowIndex >= 0)
            {
                DisplaySelectedVehicleDetails(e.RowIndex);
            }
        }

        private void DisplaySelectedVehicleDetails(int rowIndex)
        {
            try
            {
                // Get the selected row
                DataGridViewRow row = dgvVehicles.Rows[rowIndex];

                // Store the CarCode internally but don't display it in the form
                selectedCarCode = row.Cells["CarCode"].Value.ToString();

                // Populate the details section with the data from the selected row
                tbxVehicleMake.Text = row.Cells["Make"].Value.ToString();
                tbxVehicleModel.Text = row.Cells["Model"].Value.ToString();
                tbxVehicleYear.Text = row.Cells["Year_"].Value.ToString();
                tbxVehicleLicensePlate.Text = row.Cells["PlateNumber"].Value.ToString();

                // Set the rental rate from the database
                if (row.Cells["RentalPrice"].Value != DBNull.Value)
                {
                    tbxVehicleRentalRate.Text = row.Cells["RentalPrice"].Value.ToString();
                }
                else
                {
                    tbxVehicleRentalRate.Text = "0";
                }

                // Set the combobox selections
                cbxVehicleTransmission.Text = row.Cells["Transmission"].Value.ToString();
                cbxFuelType.Text = row.Cells["FuelType"].Value.ToString();
                cbxVehicleType.Text = row.Cells["VehicleType"].Value.ToString();

                // Set availability status from database
                if (row.Cells["Availability"].Value != DBNull.Value)
                {
                    string availability = row.Cells["Availability"].Value.ToString();
                    cbxVehicleStatus.Text = availability;
                }

                // Set vehicle internal status from database
                if (row.Cells["VehicleStatus"].Value != DBNull.Value)
                {
                    string vehicleStatus = row.Cells["VehicleStatus"].Value.ToString();
                    cbxVehicleInternalStatus.Text = vehicleStatus;
                }

                // Load the vehicle image for the selected car
                LoadVehicleImage(selectedCarCode);

                // Reset control states - enable by default
                cbxVehicleStatus.Enabled = true;
                cbxVehicleInternalStatus.Enabled = true;

                // Check if vehicle has active rentals
                bool hasActiveRental = CheckActiveRental(selectedCarCode);

                // Only disable status changes if the vehicle is actually rented
                if (hasActiveRental)
                {
                    // Set status to Rented for clarity
                    cbxVehicleInternalStatus.Text = "Rented";
                    cbxVehicleStatus.Text = "Not Available";

                    // Disable the status fields
                    cbxVehicleStatus.Enabled = false;
                    cbxVehicleInternalStatus.Enabled = false;

                    // Show message about rental status
                    MessageBox.Show("This vehicle is currently rented and status cannot be changed.",
                                  "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying vehicle details: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to check if a vehicle has active rentals
        private bool CheckActiveRental(string carCode)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // FIX 2: Make sure we're only checking rental status and not being influenced by vehicle type
                    string rentalCheckQuery = "SELECT COUNT(*) FROM tblRentals WHERE CarCode = ? AND RentalStatus = 'Active'";

                    using (OleDbCommand rentalCmd = new OleDbCommand(rentalCheckQuery, connection))
                    {
                        // Make sure parameter is correctly added
                        rentalCmd.Parameters.AddWithValue("?", carCode);

                        int activeRentals = Convert.ToInt32(rentalCmd.ExecuteScalar());

                        // Debug line (you can remove this after confirming it works)
                        // MessageBox.Show($"Car {carCode} has {activeRentals} active rentals");

                        return (activeRentals > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                MessageBox.Show($"Error in CheckActiveRental: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // New method to synchronize vehicle statuses
        private void SynchronizeVehicleStatuses(string carCode, string vehicleStatus)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Determine the correct Availability based on VehicleStatus
                    string availability = (vehicleStatus == "Available") ? "Available" : "Not Available";

                    // Check if there's an active rental
                    bool hasActiveRental = CheckActiveRental(carCode);

                    // If there's an active rental, the vehicle must be Rented and Not Available
                    if (hasActiveRental)
                    {
                        vehicleStatus = "Rented";
                        availability = "Not Available";
                    }

                    // Update both statuses in the database
                    string updateQuery = @"UPDATE tblVehicles SET 
                                         Availability = ?,
                                         VehicleStatus = ?
                                         WHERE CarCode = ?";

                    using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("Availability", availability);
                        cmd.Parameters.AddWithValue("VehicleStatus", vehicleStatus);
                        cmd.Parameters.AddWithValue("CarCode", carCode);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error synchronizing vehicle statuses: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVehicleAdd_Click(object sender, EventArgs e)
        {
            cbxVehicleStatus.Enabled = true;
            cbxVehicleInternalStatus.Enabled = true;

            // Validate input before adding
            if (!ValidateVehicleInput())
                return;

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Generate a new unique CarCode
                    string newCarCode = GenerateNewCarCode(connection);

                    // Get the vehicle status from the UI - must be one of: Available, Rented, Under Maintenance
                    string vehicleStatus = cbxVehicleInternalStatus.Text;

                    // Determine availability based on vehicle status
                    string availability = (vehicleStatus == "Available") ? "Available" : "Not Available";

                    // Insert the new vehicle
                    string insertQuery = @"INSERT INTO tblVehicles 
                                 (CarCode, Make, Model, Year_, PlateNumber, 
                                  Transmission, FuelType, VehicleType, Availability, 
                                  VehicleStatus, DateAdded, RentalPrice)
                                 VALUES 
                                 (?, ?, ?, ?, ?,
                                  ?, ?, ?, ?, 
                                  ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(insertQuery, connection))
                    {
                        // Add parameters in the same order as they appear in the query
                        cmd.Parameters.AddWithValue("CarCode", newCarCode);
                        cmd.Parameters.AddWithValue("Make", tbxVehicleMake.Text);
                        cmd.Parameters.AddWithValue("Model", tbxVehicleModel.Text);

                        // Convert Year to integer explicitly
                        if (int.TryParse(tbxVehicleYear.Text, out int yearValue))
                        {
                            cmd.Parameters.AddWithValue("Year_", yearValue);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("Year_", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("PlateNumber", tbxVehicleLicensePlate.Text);
                        cmd.Parameters.AddWithValue("Transmission", cbxVehicleTransmission.Text);
                        cmd.Parameters.AddWithValue("FuelType", cbxFuelType.Text);
                        cmd.Parameters.AddWithValue("VehicleType", cbxVehicleType.Text);
                        cmd.Parameters.AddWithValue("Availability", availability);
                        cmd.Parameters.AddWithValue("VehicleStatus", vehicleStatus);
                        cmd.Parameters.AddWithValue("DateAdded", DateTime.Now.Date);

                        // Parse rental rate and handle potential formatting issues
                        if (decimal.TryParse(tbxVehicleRentalRate.Text, System.Globalization.NumberStyles.Any,
                                           System.Globalization.CultureInfo.InvariantCulture, out decimal rentalRate))
                        {
                            cmd.Parameters.AddWithValue("RentalPrice", rentalRate);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("RentalPrice", 0m);
                        }

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Vehicle added successfully!", "Success",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the form fields
                    ClearVehicleFields();

                    // Reload the data
                    LoadVehicleData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding vehicle: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateNewCarCode(OleDbConnection connection)
        {
            // Get the highest existing CarCode
            string query = "SELECT MAX(CarCode) FROM tblVehicles WHERE CarCode LIKE 'MV%'";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            object result = cmd.ExecuteScalar();

            int newNumber = 1;

            if (result != null && result != DBNull.Value)
            {
                string lastCode = result.ToString();
                // Extract the number part of the code and increment it
                if (lastCode.Length > 2)
                {
                    string numberPart = lastCode.Substring(2); // Assuming "MV" prefix + numbers
                    if (int.TryParse(numberPart, out int lastNumber))
                    {
                        newNumber = lastNumber + 1;
                    }
                }
            }

            // Format the new code with leading zeros (e.g., MV001, MV002, etc.)
            return "MV" + newNumber.ToString("D4");
        }

        private void btnVehicleUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCarCode))
            {
                MessageBox.Show("Please select a vehicle to update.", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validate input before updating
            if (!ValidateVehicleInput())
                return;

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Get the selected vehicle status from UI
                    string vehicleStatus = cbxVehicleInternalStatus.Text;

                    // Update the basic vehicle information
                    string updateQuery = @"UPDATE tblVehicles SET 
                                         Make = ?,
                                         Model = ?,
                                         Year_ = ?,
                                         PlateNumber = ?,
                                         Transmission = ?,
                                         FuelType = ?,
                                         VehicleType = ?,
                                         RentalPrice = ?
                                         WHERE CarCode = ?";

                    using (OleDbCommand cmd = new OleDbCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("Make", tbxVehicleMake.Text);
                        cmd.Parameters.AddWithValue("Model", tbxVehicleModel.Text);

                        // Handle Year_ as integer
                        if (int.TryParse(tbxVehicleYear.Text, out int yearValue))
                        {
                            cmd.Parameters.AddWithValue("Year_", yearValue);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("Year_", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("PlateNumber", tbxVehicleLicensePlate.Text);
                        cmd.Parameters.AddWithValue("Transmission", cbxVehicleTransmission.Text);
                        cmd.Parameters.AddWithValue("FuelType", cbxFuelType.Text);
                        cmd.Parameters.AddWithValue("VehicleType", cbxVehicleType.Text);

                        // Handle RentalPrice as decimal
                        if (decimal.TryParse(tbxVehicleRentalRate.Text, out decimal rentalRate))
                        {
                            cmd.Parameters.AddWithValue("RentalPrice", rentalRate);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("RentalPrice", 0m);
                        }

                        cmd.Parameters.AddWithValue("CarCode", selectedCarCode);

                        cmd.ExecuteNonQuery();
                    }

                    // Synchronize the statuses
                    SynchronizeVehicleStatuses(selectedCarCode, vehicleStatus);

                    MessageBox.Show("Vehicle updated successfully!", "Success",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload the data
                    LoadVehicleData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating vehicle: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVehicleDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCarCode))
            {
                MessageBox.Show("Please select a vehicle to delete.", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this vehicle?",
                                                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();

                        // Check if the vehicle is currently rented
                        bool hasActiveRental = CheckActiveRental(selectedCarCode);

                        if (hasActiveRental)
                        {
                            MessageBox.Show("Cannot delete this vehicle as it is currently rented.",
                                           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Delete the vehicle
                        string deleteQuery = "DELETE FROM tblVehicles WHERE CarCode = ?";
                        using (OleDbCommand cmd = new OleDbCommand(deleteQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("CarCode", selectedCarCode);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Vehicle deleted successfully!", "Success",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the form fields
                        ClearVehicleFields();

                        // Reload the data
                        LoadVehicleData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting vehicle: " + ex.Message, "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnVehicleClear_Click(object sender, EventArgs e)
        {
            ClearVehicleFields();
        }

        private void ClearVehicleFields()
        {
            selectedCarCode = ""; // Reset internally stored CarCode
            tbxVehicleMake.Text = "";
            tbxVehicleModel.Text = "";
            tbxVehicleYear.Text = "";
            tbxVehicleLicensePlate.Text = "";
            tbxVehicleRentalRate.Text = "";
            cbxVehicleTransmission.SelectedIndex = -1;
            cbxFuelType.SelectedIndex = -1;
            cbxVehicleType.SelectedIndex = -1;
            cbxVehicleStatus.SelectedIndex = -1;
            cbxVehicleInternalStatus.SelectedIndex = -1;

            cbxVehicleStatus.Enabled = true;
            cbxVehicleInternalStatus.Enabled = true;

            // Clear the loaded vehicle image
            if (pbxCarImage.Image != null)
            {
                pbxCarImage.Image.Dispose();
                pbxCarImage.Image = null;
            }
            // Optionally, set to a default image:
            // pbxCarImage.Image = Properties.Resources.NoImageAvailable;
        }

        private bool ValidateVehicleInput()
        {
            // Check for required fields
            if (string.IsNullOrWhiteSpace(tbxVehicleMake.Text) ||
                string.IsNullOrWhiteSpace(tbxVehicleModel.Text) ||
                string.IsNullOrWhiteSpace(tbxVehicleYear.Text) ||
                string.IsNullOrWhiteSpace(tbxVehicleLicensePlate.Text) ||
                string.IsNullOrWhiteSpace(tbxVehicleRentalRate.Text) ||
                cbxVehicleTransmission.SelectedIndex == -1 ||
                cbxFuelType.SelectedIndex == -1 ||
                cbxVehicleType.SelectedIndex == -1 ||
                cbxVehicleStatus.SelectedIndex == -1 ||
                cbxVehicleInternalStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate Year
            if (!int.TryParse(tbxVehicleYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year + 1)
            {
                MessageBox.Show("Please enter a valid year.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate Rental Rate
            if (!decimal.TryParse(tbxVehicleRentalRate.Text, out decimal rate) || rate < 0)
            {
                MessageBox.Show("Please enter a valid rental rate.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate VehicleStatus has one of the allowed values
            string vehicleStatus = cbxVehicleInternalStatus.Text;
            if (vehicleStatus != "Available" && vehicleStatus != "Under Maintenance")
            {
                MessageBox.Show("Vehicle Status must be one of: Available or Under Maintenance.",
                              "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private void LoadVehicleImage(string carCode)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Get the first image path for the selected vehicle
                    string query = "SELECT TOP 1 ImagePath FROM tblVehicleImages WHERE CarCode = ? ORDER BY ImageID";
                    OleDbCommand cmd = new OleDbCommand(query, connection);
                    cmd.Parameters.AddWithValue("?", carCode);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        currentImagePath = result.ToString();

                        // Check if file exists
                        currentImagePath = currentImagePath.Trim();
                        if (!File.Exists(currentImagePath))
                        {
                            MessageBox.Show("File does not exist: " + currentImagePath, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            try
                            {
                                // Dispose of the old image if it exists
                                if (pbxCarImage.Image != null)
                                {
                                    pbxCarImage.Image.Dispose();
                                    pbxCarImage.Image = null;
                                }

                                // Load the original image
                                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                                string imagePath = Path.Combine(baseDir, currentImagePath);
                                using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                                {
                                    using (Image originalImage = Image.FromStream(stream))
                                    {
                                        // Create a thumbnail
                                        int thumbnailWidth = pbxCarImage.Width;  // Use PictureBox width
                                        int thumbnailHeight = pbxCarImage.Height; // Use PictureBox height
                                        
                                        // Calculate aspect ratio
                                        double ratio = Math.Min(
                                            (double)thumbnailWidth / originalImage.Width,
                                            (double)thumbnailHeight / originalImage.Height
                                        );
                                        
                                        int newWidth = (int)(originalImage.Width * ratio);
                                        int newHeight = (int)(originalImage.Height * ratio);

                                        // Create the thumbnail
                                        using (Bitmap thumbnail = new Bitmap(newWidth, newHeight))
                                        {
                                            using (Graphics g = Graphics.FromImage(thumbnail))
                                            {
                                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                                            }
                                            pbxCarImage.Image = new Bitmap(thumbnail);
                                        }
                                    }
                                }
                                pbxCarImage.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error loading image: {ex.Message}", "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                pbxCarImage.Image = Properties.Resources.NoImageAvailable;
                            }
                        }
                    }
                    else
                    {
                        pbxCarImage.Image = Properties.Resources.NoImageAvailable;
                        currentImagePath = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading vehicle image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbxCarImage.Image = Properties.Resources.NoImageAvailable;
            }
        }

        // Event handler for VehicleInternalStatus changes
        private void cbxVehicleInternalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Automatically update Availability based on VehicleStatus
            if (cbxVehicleInternalStatus.SelectedIndex != -1)
            {
                string selectedStatus = cbxVehicleInternalStatus.Text;

                // Set availability based on selected vehicle status
                if (selectedStatus == "Available")
                {
                    cbxVehicleStatus.Text = "Available";
                }
                else
                {
                    cbxVehicleStatus.Text = "Not Available";
                }
            }
        }



        // The following methods are empty as they're just event handlers for property changes
        private void tbxVehicleMake_TextChanged(object sender, EventArgs e) { }
        private void tbxVehicleModel_TextChanged(object sender, EventArgs e) { }
        private void tbxVehicleRentalRate_TextChanged(object sender, EventArgs e) { }
        private void tbxVehicleYear_TextChanged(object sender, EventArgs e) { }
        private void tbxVehicleLicensePlate_TextChanged(object sender, EventArgs e) { }
        private void cbxVehicleStatus_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cbxFuelType_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cbxVehicleTransmission_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cbxVehicleType_SelectedIndexChanged(object sender, EventArgs e) { }
        private void btnDeleteVehicleImage_Click(object sender, EventArgs e) { }
        private void btnAddCarImage_Click(object sender, EventArgs e) { }
        private void pbxCarImage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCarCode))
            {
                MessageBox.Show("Please select a vehicle first.", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Always create a new gallery for the selected vehicle
            frmVehicleGallery galleryForm = new frmVehicleGallery(selectedCarCode);
            galleryForm.Show();
        }
    }
}