using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace CarRent
{
    public partial class frmUserTransactionHistory : Form
    {
        private readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private string userCode;
        private string fullName;

        public frmUserTransactionHistory(string code, string name)
        {
            InitializeComponent();
            userCode = code;
            fullName = name;
            txtUserName.Text = fullName;
            lblUserID.Text = code;

            // Load user image
            LoadUserImage();

            // Initialize DataGridView properties
            InitializeDataGridView();
            
            // Load the data
            LoadTransactionHistory();
        }

        private void InitializeDataGridView()
        {
            // Basic DataGridView setup
            dgvTransactionHistory.AutoGenerateColumns = true;
            dgvTransactionHistory.AllowUserToAddRows = false;
            dgvTransactionHistory.AllowUserToDeleteRows = false;
            dgvTransactionHistory.ReadOnly = true;
            dgvTransactionHistory.MultiSelect = false;
            dgvTransactionHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactionHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // Style setup
            dgvTransactionHistory.EnableHeadersVisualStyles = false;
            dgvTransactionHistory.BorderStyle = BorderStyle.None;
            dgvTransactionHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTransactionHistory.GridColor = Color.LightGray;
            dgvTransactionHistory.RowHeadersVisible = false;
            
            // Header style
            dgvTransactionHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.Firebrick;
            dgvTransactionHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTransactionHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvTransactionHistory.ColumnHeadersHeight = 40;
            
            // Row style
            dgvTransactionHistory.DefaultCellStyle.SelectionBackColor = Color.RosyBrown;
            dgvTransactionHistory.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvTransactionHistory.RowTemplate.Height = 35;
        }

        private void LoadTransactionHistory(DateTime? start = null, DateTime? end = null)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                        [r].[RentalCode],
                        [c].[FullName] AS [Customer],
                        ([v].[Make] & ' ' & [v].[Model]) AS [Vehicle],
                        [r].[StartDate],
                        [r].[EndDate],
                        [r].[ReturnDate],
                        [r].[DaysRented],
                        [r].[TotalAmount],
                        [r].[RentalStatus],
                        [r].[ReferenceNumber]
                    FROM 
                        ([tblRentals] AS [r]
                        INNER JOIN [tblCustomers] AS [c] ON [r].[CustomerCode] = [c].[CustomerCode])
                        INNER JOIN [tblVehicles] AS [v] ON [r].[CarCode] = [v].[CarCode]
                    WHERE 
                        [r].[ProcessedBy] = ?";

                    if (start.HasValue && end.HasValue)
                    {
                        query += " AND [r].[StartDate] >= ? AND [r].[EndDate] <= ?";
                    }
                    query += " ORDER BY [r].[DateCreated] DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", userCode);
                        if (start.HasValue && end.HasValue)
                        {
                            cmd.Parameters.AddWithValue("?", start.Value.Date);
                            cmd.Parameters.AddWithValue("?", end.Value.Date.AddDays(1).AddSeconds(-1));
                        }

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Clear existing data and binding
                            dgvTransactionHistory.DataSource = null;
                            dgvTransactionHistory.Columns.Clear();
                            dgvTransactionHistory.DataSource = dt;

                            FormatColumns();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transaction history: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatColumns()
        {
            if (dgvTransactionHistory?.Columns == null || dgvTransactionHistory.Columns.Count == 0)
                return;

            try
            {
                // First set AutoSizeMode to None to prevent auto-adjustments during formatting
                dgvTransactionHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                // Calculate total available width
                int totalWidth = dgvTransactionHistory.Width - SystemInformation.VerticalScrollBarWidth;
                
                // Define column proportions (total should be 100)
                var columnProps = new Dictionary<string, (string HeaderText, int WidthProportion, DataGridViewContentAlignment Alignment, string Format)>
                {
                    {"RentalCode", ("Rental Code", 10, DataGridViewContentAlignment.MiddleLeft, null)},
                    {"Customer", ("Customer Name", 15, DataGridViewContentAlignment.MiddleLeft, null)},
                    {"Vehicle", ("Vehicle Details", 15, DataGridViewContentAlignment.MiddleLeft, null)},
                    {"StartDate", ("Start Date & Time", 12, DataGridViewContentAlignment.MiddleCenter, "MMM dd, yyyy HH:mm")},
                    {"EndDate", ("End Date & Time", 12, DataGridViewContentAlignment.MiddleCenter, "MMM dd, yyyy HH:mm")},
                    {"ReturnDate", ("Return Date & Time", 12, DataGridViewContentAlignment.MiddleCenter, "MMM dd, yyyy HH:mm")},
                    {"DaysRented", ("Days", 5, DataGridViewContentAlignment.MiddleCenter, null)},
                    {"TotalAmount", ("Total Amount", 8, DataGridViewContentAlignment.MiddleRight, "₱#,##0.00")},
                    {"RentalStatus", ("Status", 6, DataGridViewContentAlignment.MiddleCenter, null)},
                    {"ReferenceNumber", ("Reference Number", 10, DataGridViewContentAlignment.MiddleLeft, null)}
                };

                // Apply formatting to each column
                foreach (var columnProp in columnProps)
                {
                    if (dgvTransactionHistory.Columns.Contains(columnProp.Key))
                    {
                        var column = dgvTransactionHistory.Columns[columnProp.Key];
                        if (column != null)
                        {
                            // Calculate width based on proportion
                            int width = (totalWidth * columnProp.Value.WidthProportion) / 100;
                            
                            column.HeaderText = columnProp.Value.HeaderText;
                            column.Width = width;
                            column.DefaultCellStyle.Alignment = columnProp.Value.Alignment;
                            
                            if (!string.IsNullOrEmpty(columnProp.Value.Format))
                            {
                                column.DefaultCellStyle.Format = columnProp.Value.Format;
                            }

                            // Add padding
                            column.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
                            
                            // Set minimum width based on header text
                            using (Graphics g = dgvTransactionHistory.CreateGraphics())
                            {
                                SizeF headerSize = g.MeasureString(column.HeaderText, dgvTransactionHistory.ColumnHeadersDefaultCellStyle.Font);
                                int minWidth = (int)headerSize.Width + 20; // Add padding
                                if (column.Width < minWidth)
                                {
                                    column.Width = minWidth;
                                }
                            }
                        }
                    }
                }

                // Apply general grid styling
                dgvTransactionHistory.EnableHeadersVisualStyles = false;
                dgvTransactionHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.Firebrick;
                dgvTransactionHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvTransactionHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                dgvTransactionHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTransactionHistory.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvTransactionHistory.ColumnHeadersHeight = 45;

                // Row styling
                dgvTransactionHistory.DefaultCellStyle.SelectionBackColor = Color.RosyBrown;
                dgvTransactionHistory.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvTransactionHistory.RowTemplate.Height = 35;

                // Grid styling
                dgvTransactionHistory.BorderStyle = BorderStyle.None;
                dgvTransactionHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvTransactionHistory.GridColor = Color.LightGray;
                dgvTransactionHistory.RowHeadersVisible = false;

                // Ensure proper resizing behavior
                dgvTransactionHistory.AllowUserToResizeColumns = true;
                dgvTransactionHistory.AllowUserToResizeRows = false;

                // Auto-adjust column widths if needed
                if (dgvTransactionHistory.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) < totalWidth)
                {
                    dgvTransactionHistory.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error formatting columns: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add a resize handler to maintain column proportions
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            FormatColumns();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTransactionHistory(dtpStart.Value, dtpEnd.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadUserImage()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT UserImage FROM tblUsers WHERE UserCode = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", userCode);
                        var result = cmd.ExecuteScalar();
                        
                        if (result != null && result != DBNull.Value)
                        {
                            string imagePath = result.ToString();
                            if (File.Exists(imagePath))
                            {
                                using (var img = Image.FromFile(imagePath))
                                {
                                    pbxUser.Image = new Bitmap(img);
                                }
                                pbxUser.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            else
                            {
                                // Set default image if user image doesn't exist
                                pbxUser.Image = Properties.Resources.user__1_;
                                pbxUser.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                        }
                        else
                        {
                            // Set default image if no image path in database
                            pbxUser.Image = Properties.Resources.user__1_;
                            pbxUser.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user image: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Set default image on error
                pbxUser.Image = Properties.Resources.user__1_;
                pbxUser.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}