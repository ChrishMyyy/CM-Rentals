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
using System.IO;

namespace CarRent
{
    public partial class UC_ManageRentals : UserControl
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private string selectedCustomerCode = "";
        private string selectedCarCode = "";
        private string userCode = Session.CurrentUserCode;
        private string currentUserCode = Session.CurrentUserCode;

        public UC_ManageRentals()
        {
            InitializeComponent();
            LoadAvailableCustomers();
            LoadAvailableVehicles();
            SetVehicleDetailsReadOnly();
        }

        private void LoadAvailableCustomers()
        {
            cbxRentalCustomerName.Items.Clear();
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT CustomerCode, FullName 
                    FROM tblCustomers 
                    WHERE Status = 'Active' 
                    AND CustomerStatus = 'Returned' 
                    AND CustomerCode NOT IN (SELECT CustomerCode FROM tblRentals WHERE RentalStatus = 'Active')";
                using (var cmd = new OleDbCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbxRentalCustomerName.Items.Add(new ComboBoxItem
                        {
                            Text = reader["FullName"].ToString(),
                            Value = reader["CustomerCode"].ToString()
                        });
                    }
                }
            }
        }

        private void LoadAvailableVehicles()
        {
            cbxRentalAvailableVehicles.Items.Clear();
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CarCode, Model FROM tblVehicles WHERE Availability = 'Available'";
                using (var cmd = new OleDbCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbxRentalAvailableVehicles.Items.Add(new ComboBoxItem
                        {
                            Text = reader["Model"].ToString(),
                            Value = reader["CarCode"].ToString()
                        });
                    }
                }
            }
        }

        private void cbxRentalCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = cbxRentalCustomerName.SelectedItem as ComboBoxItem;
            if (selected == null) return;
            selectedCustomerCode = selected.Value;

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT LicenseImage FROM tblCustomers WHERE CustomerCode = ?";
                using (var cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", selectedCustomerCode);
                    var result = cmd.ExecuteScalar();
                    if (result != null && File.Exists(result.ToString()))
                    {
                        pbxRentalLicense.Image = Image.FromFile(result.ToString());
                    }
                    else
                    {
                        pbxRentalLicense.Image = null;
                    }
                }
            }
        }

        private void cbxRentalAvailableVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = cbxRentalAvailableVehicles.SelectedItem as ComboBoxItem;
            if (selected == null) return;
            selectedCarCode = selected.Value;

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM tblVehicles WHERE CarCode = ?";
                using (var cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", selectedCarCode);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tbxMake.Text = reader["Make"].ToString();
                            tbxCarCode.Text = reader["CarCode"].ToString();
                            tbxModel.Text = reader["Model"].ToString();
                            tbxRentalRate.Text = reader["RentalPrice"].ToString();
                            tbxYear.Text = reader["Year_"].ToString();
                            tbxLicensePlate.Text = reader["PlateNumber"].ToString();
                            cbxVehicleStatus.Text = reader["Availability"].ToString();
                            cbxFuelType.Text = reader["FuelType"].ToString();
                            cbxTransmission.Text = reader["Transmission"].ToString();
                            cbxVehicleType.Text = reader["VehicleType"].ToString();
                        }
                    }
                }
                // Load vehicle image
                string imgQuery = "SELECT TOP 1 ImagePath FROM tblVehicleImages WHERE CarCode = ? ORDER BY ImageID";
                using (var imgCmd = new OleDbCommand(imgQuery, conn))
                {
                    imgCmd.Parameters.AddWithValue("?", selectedCarCode);
                    var imgResult = imgCmd.ExecuteScalar();
                    if (imgResult != null && File.Exists(imgResult.ToString()))
                    {
                        pbxCarImage.Image = Image.FromFile(imgResult.ToString());
                    }
                    else
                    {
                        pbxCarImage.Image = null;
                    }
                }
            }
            UpdateRentalCalculation();
        }

        private void dtpScheduledReturnDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateRentalCalculation();
        }

        private void UpdateRentalCalculation()
        {
            DateTime start = dtpRentalDate.Value;
            DateTime end = dtpScheduledReturnDate.Value;

            // Calculate the total time difference in hours
            double totalHours = (end - start).TotalHours;

            int days;

            // For 24 hours or less, count as 1 day
            if (totalHours <= 24.0)
            {
                days = 1;
            }
            else
            {
                // For rentals exactly at multiples of 24 hours (with small tolerance)
                double mod24 = totalHours % 24.0;
                if (mod24 < 0.01 || mod24 > 23.99)
                {
                    days = (int)Math.Round(totalHours / 24.0);
                }
                else
                {
                    // For other periods, round up partially used days
                    days = (int)Math.Ceiling(totalHours / 24.0);
                }
            }

            // Update the UI
            tbxRentalDuration.Text = days.ToString();

            if (decimal.TryParse(tbxRentalRate.Text, out decimal rate))
            {
                decimal totalAmount = days * rate;
                tbxRentalAmount.Text = totalAmount.ToString("F2");
            }
            else
            {
                tbxRentalAmount.Text = "0.00";
            }
        }

        private void btnRentalProcessPayment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCustomerCode) || string.IsNullOrEmpty(selectedCarCode))
            {
                MessageBox.Show("Please select both a customer and a vehicle.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!rbnGcash.Checked && !rbnCash.Checked)
            {
                MessageBox.Show("Please select a payment method.", "Payment Method Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbnGcash.Checked)
            {
                if (decimal.TryParse(tbxRentalAmount.Text, out decimal dueAmount))
                {
                    DateTime startDate = dtpRentalDate.Value;
                    DateTime endDate = dtpScheduledReturnDate.Value;
                    int daysRented = int.Parse(tbxRentalDuration.Text); // Use calculated days instead of recalculating
                    frmOnlinePayment onlinePaymentForm = new frmOnlinePayment(selectedCustomerCode, dueAmount, currentUserCode, selectedCarCode, startDate, endDate, daysRented, "");
                    var result = onlinePaymentForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        ClearRentalFields();
                        LoadAvailableCustomers();
                        LoadAvailableVehicles();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid rental amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (rbnCash.Checked)
            {
                decimal dueAmount = 0;
                decimal loyaltyPoints = 0;
                decimal totalAmount = 0;
                int daysRented = int.Parse(tbxRentalDuration.Text); // Use calculated days

                decimal.TryParse(tbxRentalAmount.Text, out dueAmount);

                DateTime startDate = dtpRentalDate.Value;
                DateTime endDate = dtpScheduledReturnDate.Value;

                // Fetch loyalty points
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT LoyaltyPoints FROM tblCustomers WHERE CustomerCode = ?";
                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", selectedCustomerCode);
                        var dbResult = cmd.ExecuteScalar();
                        if (dbResult != null && decimal.TryParse(dbResult.ToString(), out decimal points))
                        {
                            loyaltyPoints = points;
                        }
                    }
                }

                decimal discountAmount = loyaltyPoints * 10;
                totalAmount = dueAmount - discountAmount;
                if (totalAmount < 0) totalAmount = 0;

                var paymentForm = new frmProcessPayment(
                    selectedCustomerCode,
                    selectedCarCode,
                    startDate,
                    endDate,
                    daysRented,
                    dueAmount,
                    totalAmount,
                    ""
                );

                paymentForm.SetPaymentDetails(dueAmount, loyaltyPoints, totalAmount, daysRented, currentUserCode);
                var result = paymentForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ClearRentalFields();
                    LoadAvailableCustomers();
                    LoadAvailableVehicles();
                }
            }
        }

        private void SetVehicleDetailsReadOnly()
        {
            tbxMake.ReadOnly = true;
            tbxCarCode.ReadOnly = true;
            tbxModel.ReadOnly = true;
            tbxRentalRate.ReadOnly = true;
            tbxYear.ReadOnly = true;
            tbxLicensePlate.ReadOnly = true;
            cbxVehicleStatus.Enabled = false;
            cbxFuelType.Enabled = false;
            cbxTransmission.Enabled = false;
            cbxVehicleType.Enabled = false;
        }

        private void UC_ManageRentals_Load(object sender, EventArgs e)
        {
            LoadAvailableCustomers();
            LoadAvailableVehicles();

            // Set the date pickers to the current date and time
            DateTime now = DateTime.Now;
            dtpRentalDate.Value = now;

            // Set return date to exactly 24 hours later (not just 1 day later)
            dtpScheduledReturnDate.Value = now.AddHours(24);

            // Make sure to calculate the initial values
            UpdateRentalCalculation();
        }

        private string GenerateRentalCode()
        {
            string newCode = "RENT0001";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MAX(RentalCode) FROM tblRentals WHERE RentalCode LIKE 'RENT%'";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string highestCode = result.ToString();
                            if (highestCode.Length >= 8)
                            {
                                string numberPart = highestCode.Substring(4);
                                if (int.TryParse(numberPart, out int highestNumber))
                                {
                                    newCode = $"RENT{(highestNumber + 1):D4}";
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Optionally handle error or fallback to default
            }
            return newCode;
        }

        // Helper class for ComboBox items
        public class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public override string ToString() => Text;
        }

        private void btnCustomerClear_Click_1(object sender, EventArgs e)
        {
            cbxRentalCustomerName.SelectedIndex = -1;
            cbxRentalAvailableVehicles.SelectedIndex = -1;
            pbxRentalLicense.Image = null;
            pbxCarImage.Image = null;
            tbxMake.Text = "";
            tbxCarCode.Text = "";
            tbxModel.Text = "";
            tbxRentalRate.Text = "";
            tbxYear.Text = "";
            tbxLicensePlate.Text = "";
            cbxVehicleStatus.SelectedIndex = -1;
            cbxFuelType.SelectedIndex = -1;
            cbxTransmission.SelectedIndex = -1;
            cbxVehicleType.SelectedIndex = -1;
            tbxRentalDuration.Text = "";
            tbxRentalAmount.Text = "";
            rbnGcash.Checked = false;
            rbnCash.Checked = false;
        }

        private void dtpRentalDate_ValueChanged_1(object sender, EventArgs e)
        {
            UpdateRentalCalculation();
        }

        private void btnRentalsReturn_Click(object sender, EventArgs e)
        {
            // Get the parent form
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                // Find the main panel in the parent form
                Panel mainPanel = parentForm.Controls.Find("pnlMain", true).FirstOrDefault() as Panel;
                if (mainPanel != null)
                {
                    // Clear the main panel
                    mainPanel.Controls.Clear();

                    // Create and add the return vehicle control
                    UC_ReturnVehicle returnVehicle = new UC_ReturnVehicle();
                    returnVehicle.Dock = DockStyle.Fill;
                    mainPanel.Controls.Add(returnVehicle);
                }
            }
        }

        private void ReturnCar(string rentalCode)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string updateRental = "UPDATE tblRentals SET RentalStatus = 'Returned' WHERE RentalCode = ?";
                using (var cmd = new OleDbCommand(updateRental, conn))
                {
                    cmd.Parameters.Add("?", OleDbType.DBTimeStamp).Value = DateTime.Now;
                    cmd.Parameters.Add("?", OleDbType.VarChar).Value = rentalCode;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnRentalsTransacHistory_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current user's name from the database
                string userFullName = "";
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT FullName FROM tblUsers WHERE UserCode = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", Session.CurrentUserCode);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            userFullName = result.ToString();
                        }
                    }
                }

                // Create and show the transaction history form
                frmUserTransactionHistory historyForm = new frmUserTransactionHistory(Session.CurrentUserCode, userFullName);
                historyForm.ShowDialog();

                // Refresh the rentals data after viewing history (if needed)
                LoadAvailableCustomers();
                LoadAvailableVehicles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transaction history: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearRentalFields()
        {
            cbxRentalCustomerName.SelectedIndex = -1;
            cbxRentalAvailableVehicles.SelectedIndex = -1;
            pbxRentalLicense.Image = null;
            pbxCarImage.Image = null;
            tbxMake.Text = "";
            tbxCarCode.Text = "";
            tbxModel.Text = "";
            tbxRentalRate.Text = "";
            tbxYear.Text = "";
            tbxLicensePlate.Text = "";
            cbxVehicleStatus.SelectedIndex = -1;
            cbxFuelType.SelectedIndex = -1;
            cbxTransmission.SelectedIndex = -1;
            cbxVehicleType.SelectedIndex = -1;
            tbxRentalDuration.Text = "";
            tbxRentalAmount.Text = "";
            rbnGcash.Checked = false;
            rbnCash.Checked = false;
        }
    }
}