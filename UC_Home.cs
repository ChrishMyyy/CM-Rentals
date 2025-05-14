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

namespace CarRent
{
    public partial class UC_Home : UserControl
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";

        private int targetSizeIncrease = 35;
        private int stepSize = 2;
        private bool isZoomingIn = false;
        private Dictionary<PictureBox, (int width, int height, Point location)> originalSizes;

        public UC_Home()
        {
            InitializeComponent();
            originalSizes = new Dictionary<PictureBox, (int, int, Point)>
    {
        { guna2PictureBox1, (guna2PictureBox1.Width, guna2PictureBox1.Height, guna2PictureBox1.Location) },
        { guna2PictureBox2, (guna2PictureBox2.Width, guna2PictureBox2.Height, guna2PictureBox2.Location) },
        { guna2PictureBox3, (guna2PictureBox3.Width, guna2PictureBox3.Height, guna2PictureBox3.Location) },
        { guna2PictureBox4, (guna2PictureBox4.Width, guna2PictureBox4.Height, guna2PictureBox4.Location) }
    };
        }
        private void PictureBox_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is not PictureBox pb || !originalSizes.ContainsKey(pb)) return;

            isZoomingIn = true;
            timerZoom.Tag = pb;
            timerZoom.Start();
        }

        private void PictureBox_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is not PictureBox pb || !originalSizes.ContainsKey(pb)) return;

            isZoomingIn = false;
            timerZoom.Tag = pb;
            timerZoom.Start();
        }

        private void UC_Home_Load(object sender, EventArgs e)
        {

            guna2PictureBox1.MouseEnter += PictureBox_MouseEnter;
            guna2PictureBox1.MouseLeave += PictureBox_MouseLeave;

            guna2PictureBox2.MouseEnter += PictureBox_MouseEnter;
            guna2PictureBox2.MouseLeave += PictureBox_MouseLeave;

            guna2PictureBox3.MouseEnter += PictureBox_MouseEnter;
            guna2PictureBox3.MouseLeave += PictureBox_MouseLeave;

            guna2PictureBox4.MouseEnter += PictureBox_MouseEnter;
            guna2PictureBox4.MouseLeave += PictureBox_MouseLeave;

            LoadDashboardData();
            LoadRecentRentals();

        }

        private void LoadDashboardData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    // Count total vehicles
                    string vehiclesQuery = "SELECT COUNT(*) FROM tblVehicles";
                    using (OleDbCommand command = new OleDbCommand(vehiclesQuery, connection))
                    {
                        int totalVehicles = Convert.ToInt32(command.ExecuteScalar());
                        lblHomeVehicles.Text = totalVehicles.ToString("00");
                    }
                    // Count total customers
                    string customersQuery = "SELECT COUNT(*) FROM tblCustomers WHERE Status = 'Active'";
                    using (OleDbCommand command = new OleDbCommand(customersQuery, connection))
                    {
                        int totalCustomers = Convert.ToInt32(command.ExecuteScalar());
                        lblHomeCustomers.Text = totalCustomers.ToString("00");
                    }
                    // Count active rentals
                    string activeRentalsQuery = "SELECT COUNT(*) FROM tblRentals WHERE RentalStatus = 'Active'";
                    using (OleDbCommand command = new OleDbCommand(activeRentalsQuery, connection))
                    {
                        int activeRentals = Convert.ToInt32(command.ExecuteScalar());
                        lblHomeActiveRentals.Text = activeRentals.ToString("00");
                    }
                    // Count available vehicles
                    string availableVehiclesQuery = "SELECT COUNT(*) FROM tblVehicles WHERE VehicleStatus = 'Available'";
                    using (OleDbCommand command = new OleDbCommand(availableVehiclesQuery, connection))
                    {
                        int availableVehicles = Convert.ToInt32(command.ExecuteScalar());
                        lblHomeAvailableVehicles.Text = availableVehicles.ToString("00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void RefreshData()
        {
            LoadDashboardData();
            LoadRecentRentals();
        }
        private void LoadRecentRentals()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT TOP 5 
                            [r].[RentalCode] AS [Rental ID], 
                            [c].[FullName] AS [Customer],
                            ([v].[Make] & ' ' & [v].[Model]) AS [Vehicle],
                            [r].[StartDate] AS [Start Date],
                            [r].[EndDate] AS [End Date],
                            [r].[RentalStatus] AS [Status],
                            Format([r].[TotalAmount], 'Currency') AS [Amount]
                        FROM 
                            ([tblRentals] AS [r] 
                            INNER JOIN [tblCustomers] AS [c] ON [r].[CustomerCode] = [c].[CustomerCode])
                            INNER JOIN [tblVehicles] AS [v] ON [r].[CarCode] = [v].[CarCode]
                        WHERE 
                            [r].[ProcessedBy] = ?
                        ORDER BY 
                            [r].[DateCreated] DESC";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", Session.CurrentUserCode);
                        DataTable rentalsTable = new DataTable();
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                        adapter.Fill(rentalsTable);
                        
                        // Always set the DataSource
                        dgvHomeTable.DataSource = rentalsTable;
                        
                        // Format the grid even if there's no data
                        FormatRentalsDataGridView();
                        
                        // If no data, show a friendly message in the grid
                        if (rentalsTable.Rows.Count == 0)
                        {
                            dgvHomeTable.BackgroundColor = Color.White;
                            if (!dgvHomeTable.Controls.OfType<Label>().Any())
                            {
                                Label noDataLabel = new Label
                                {
                                    Text = "No recent rentals to display",
                                    AutoSize = false,
                                    Dock = DockStyle.Fill,
                                    TextAlign = ContentAlignment.MiddleCenter,
                                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                                    ForeColor = Color.Gray
                                };
                                dgvHomeTable.Controls.Add(noDataLabel);
                            }
                        }
                        else
                        {
                            // Remove the "no data" label if it exists
                            var existingLabel = dgvHomeTable.Controls.OfType<Label>().FirstOrDefault();
                            if (existingLabel != null)
                            {
                                dgvHomeTable.Controls.Remove(existingLabel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recent rentals: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatRentalsDataGridView()
        {
            // Set border style and radius
            dgvHomeTable.BorderStyle = BorderStyle.None;
            dgvHomeTable.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHomeTable.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHomeTable.EnableHeadersVisualStyles = false;
            dgvHomeTable.BackgroundColor = Color.White;
            dgvHomeTable.GridColor = Color.FromArgb(231, 229, 255);

            // Header styling
            dgvHomeTable.ColumnHeadersDefaultCellStyle.BackColor = Color.Firebrick;
            dgvHomeTable.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHomeTable.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            dgvHomeTable.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgvHomeTable.ColumnHeadersHeight = 40;
            dgvHomeTable.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Row styling
            dgvHomeTable.DefaultCellStyle.BackColor = Color.White;
            dgvHomeTable.DefaultCellStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvHomeTable.DefaultCellStyle.SelectionBackColor = Color.RosyBrown;
            dgvHomeTable.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvHomeTable.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvHomeTable.RowTemplate.Height = 35;

            // Alternating row style
            dgvHomeTable.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvHomeTable.AlternatingRowsDefaultCellStyle.Font = new Font("Segoe UI", 9F);

            // Format date columns
            dgvHomeTable.Columns["Start Date"].DefaultCellStyle.Format = "MMM dd, yyyy";
            dgvHomeTable.Columns["End Date"].DefaultCellStyle.Format = "MMM dd, yyyy";

            // Format amount column
            dgvHomeTable.Columns["Amount"].DefaultCellStyle.Format = "₱#,##0.00";
            dgvHomeTable.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Center align date columns
            dgvHomeTable.Columns["Start Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHomeTable.Columns["End Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHomeTable.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set column widths
            dgvHomeTable.Columns["Rental ID"].Width = 90;
            dgvHomeTable.Columns["Customer"].Width = 150;
            dgvHomeTable.Columns["Vehicle"].Width = 150;
            dgvHomeTable.Columns["Start Date"].Width = 100;
            dgvHomeTable.Columns["End Date"].Width = 100;
            dgvHomeTable.Columns["Status"].Width = 80;
            dgvHomeTable.Columns["Amount"].Width = 100;

            // Add padding to all columns
            foreach (DataGridViewColumn column in dgvHomeTable.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
            }

            // Hide row headers
            dgvHomeTable.RowHeadersVisible = false;

            // Set selection mode
            dgvHomeTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void guna2PictureBox1_MouseEnter(object? sender, EventArgs e)
        {
            isZoomingIn = true;
            timerZoom.Start();
        }

        private void guna2PictureBox1_MouseLeave(object? sender, EventArgs e)
        {
            isZoomingIn = false;
            timerZoom.Start();
        }

        private void timerZoom_Tick(object? sender, EventArgs e)
        {
            if (timerZoom.Tag is PictureBox pb && originalSizes.ContainsKey(pb))
            {
                int change = isZoomingIn ? stepSize : -stepSize;

                // Get original size & position
                var (originalWidth, originalHeight, originalLocation) = originalSizes[pb];

                // Calculate new size
                int newWidth = pb.Width + change;
                int newHeight = pb.Height + change;

                // Stop animation when limits are reached
                if (newWidth >= originalWidth + targetSizeIncrease || newWidth <= originalWidth)
                {
                    timerZoom.Stop();
                    return;
                }

                pb.Location = new Point(
                    pb.Location.X - (change / 2),
                    pb.Location.Y - (change / 2)
                );

                pb.Size = new Size(newWidth, newHeight);
            }
            else
            {
                timerZoom.Stop();
            }
        }

        private void guna2PictureBox1_Click(object? sender, EventArgs e)
        {

        }

        private void btnHomeRentCar_Click(object? sender, EventArgs e)
        {
            Form? mainForm = this.FindForm();

            if (mainForm != null)
            {
                Panel? pnlMain = mainForm.Controls["pnlMain"] as Panel;

                if (pnlMain != null)
                {
                    pnlMain.Controls.Clear();

                    UC_ManageRentals uc = new UC_ManageRentals();
                    uc.Dock = DockStyle.Fill;
                    pnlMain.Controls.Add(uc);
                }
            }
        }

        private void btnHomeManageCustomer_Click(object sender, EventArgs e)
        {
            Form mainForm = this.FindForm();

            if (mainForm != null)
            {
                Panel pnlMain = mainForm.Controls["pnlMain"] as Panel;

                if (pnlMain != null)
                {
                    pnlMain.Controls.Clear();

                    UC_ManageCustomers uc = new UC_ManageCustomers();
                    uc.Dock = DockStyle.Fill;
                    pnlMain.Controls.Add(uc);
                }
            }
        }

        private void lblHomeVehicles_Click(object sender, EventArgs e)
        {

        }

        private void lblHomeCustomers_Click(object sender, EventArgs e)
        {

        }

        private void lblHomeActiveRentals_Click(object sender, EventArgs e)
        {

        }

        private void lblHomeAvailableVehicles_Click(object sender, EventArgs e)
        {

        }
    }
}

