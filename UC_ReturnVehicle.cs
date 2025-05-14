using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using System.Drawing;
using System.IO;

namespace CarRent
{
    public partial class UC_ReturnVehicle : UserControl
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private DataTable rentalsTable;

        public UC_ReturnVehicle()
        {
            InitializeComponent();
            LoadActiveRentals();
            btnFilter.Click += BtnFilter_Click;
            tbxSearch.TextChanged += (s, e) => PopulateRentalProgressBars();
        }

        private void LoadActiveRentals(DateTime? from = null, DateTime? to = null)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT r.RentalCode, r.CustomerCode, c.FullName AS Customer, v.Make + ' ' + v.Model AS Vehicle, 
                               v.PlateNumber, r.StartDate, r.EndDate, r.TotalAmount, r.RentalStatus, r.CarCode
                               FROM (tblRentals r
                               INNER JOIN tblCustomers c ON r.CustomerCode = c.CustomerCode)
                               INNER JOIN tblVehicles v ON r.CarCode = v.CarCode
                               WHERE r.RentalStatus = 'Active'";
                if (from.HasValue && to.HasValue)
                {
                    query += " AND r.StartDate >= ? AND r.EndDate <= ?";
                }
                query += " ORDER BY r.StartDate DESC";
                using (var cmd = new OleDbCommand(query, conn))
                {
                    if (from.HasValue && to.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@from", from.Value);
                        cmd.Parameters.AddWithValue("@to", to.Value);
                    }
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    rentalsTable = new DataTable();
                    adapter.Fill(rentalsTable);
                }
            }
            PopulateRentalProgressBars();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // include the whole end day
            LoadActiveRentals(from, to);
        }

        private void PopulateRentalProgressBars()
        {
            pnlChartPlaceholder.Controls.Clear();
            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
            };
            pnlChartPlaceholder.Controls.Add(flowPanel);

            // Apply search filter if any
            DataTable filteredTable = rentalsTable.Clone();
            string filter = tbxSearch.Text.Trim();
            foreach (DataRow row in rentalsTable.Rows)
            {
                bool matches = true;
                if (!string.IsNullOrWhiteSpace(filter) && filter != "Search Keyword")
                {
                    matches = false;
                    foreach (DataColumn col in rentalsTable.Columns)
                    {
                        if (row[col].ToString().IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            matches = true;
                            break;
                        }
                    }
                }
                if (matches)
                    filteredTable.ImportRow(row);
            }

            int columns = 2; // or calculate based on available width
            int totalPadding = flowPanel.Padding.Left + flowPanel.Padding.Right + (columns - 1) * 20; // 20 is margin between cards
            int cardWidth = (pnlChartPlaceholder.Width - totalPadding) / columns;

            foreach (DataRow row in filteredTable.Rows)
            {
                DateTime start = Convert.ToDateTime(row["StartDate"]);
                DateTime end = Convert.ToDateTime(row["EndDate"]);
                double total = (end - start).TotalDays;
                double elapsed = (DateTime.Now - start).TotalDays;
                double percent = Math.Max(0, Math.Min(1, elapsed / (total > 0 ? total : 1)));

                bool isLate = DateTime.Now > end;
                string statusText = isLate ? "Overdue" : "Ongoing";
                Color statusColor = isLate ? Color.Firebrick : SystemColors.ActiveCaption;

                // Card panel with rounded corners and thick border
                var card = new RoundedPanel
                {
                    Width = 420,
                    Height = 130,
                    Margin = new Padding(10),
                    BackColor = Color.White,
                    BorderRadius = 18,
                    BorderThickness = 4,
                    BorderColor = Color.Firebrick,
                    Cursor = Cursors.Hand,
                    Tag = row["RentalCode"],
                };

                // --- Vehicle Image Carousel ---
                string carCode = row.Table.Columns.Contains("CarCode") ? row["CarCode"].ToString() : "";
                List<string> imagePaths = new List<string>();
                if (!string.IsNullOrEmpty(carCode))
                {
                    using (var conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        string imgQuery = "SELECT ImagePath FROM tblVehicleImages WHERE CarCode = ? ORDER BY ImageID";
                        using (var cmd = new OleDbCommand(imgQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("?", carCode);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string imgPath = reader["ImagePath"].ToString();
                                    if (File.Exists(imgPath))
                                        imagePaths.Add(imgPath);
                                }
                            }
                        }
                    }
                }
                int imgIndex = 0;
                PictureBox pbVehicle = new PictureBox
                {
                    Width = 80,
                    Height = 80,
                    Location = new Point(18, 25), // Padding from left and top
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.FixedSingle,
                    Image = imagePaths.Count > 0 ? Image.FromFile(imagePaths[0]) : null
                };
                card.Controls.Add(pbVehicle);

                // All other content starts to the right of the image
                int contentLeft = 18 + pbVehicle.Width + 12; // 18px left padding + image width + 12px gap

                // CustomerCode label
                var lblCustomerCode = new Label
                {
                    Text = row["CustomerCode"].ToString(),
                    Location = new Point(contentLeft, 10),
                    Width = 120,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.DimGray,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                card.Controls.Add(lblCustomerCode);

                // Progress bar below CustomerCode
                var progressBar = new Guna.UI2.WinForms.Guna2ProgressBar
                {
                    Width = 160,
                    Height = 24,
                    Location = new Point(contentLeft, 38),
                    Value = (int)(percent * 100),
                    Maximum = 100,
                    Minimum = 0,
                    FillColor = Color.FromArgb(246, 208, 209),
                    ProgressColor = statusColor,
                    ProgressColor2 = statusColor,
                    BorderRadius = 8,
                };
                card.Controls.Add(progressBar);

                // Status label (overlaid on progress bar)
                var lblStatus = new Label
                {
                    Text = statusText,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Width = progressBar.Width,
                    Height = progressBar.Height,
                    Location = new Point(0, 0), // relative to progressBar
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                };
                progressBar.Controls.Add(lblStatus); // Add as child of progressBar
                lblStatus.BringToFront();

                // Vehicle info label below progress bar
                var lblInfo = new Label
                {
                    Text = $"{row["Vehicle"]} | {row["PlateNumber"]}",
                    Location = new Point(contentLeft, 68),
                    Width = 220,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.Firebrick,
                };
                card.Controls.Add(lblInfo);

                // Dates label below vehicle info
                var lblDates = new Label
                {
                    Text = $"{((DateTime)row["StartDate"]).ToString("MM/dd/yyyy")} - {((DateTime)row["EndDate"]).ToString("MM/dd/yyyy")}",
                    Location = new Point(contentLeft, 90),
                    Width = 220,
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.Gray,
                };
                card.Controls.Add(lblDates);

                // --- Modern Action Buttons ---
                // Return Vehicle button
                var btnReturn = new Button
                {
                    Text = "RETURN",
                    Width = 110,
                    Height = 32,
                    BackColor = Color.Firebrick,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnReturn.Location = new Point(card.Width - btnReturn.Width - 18, 30);
                btnReturn.FlatAppearance.BorderSize = 0;
                btnReturn.Click += (s, e) => ReturnCar(row["RentalCode"].ToString());
                card.Controls.Add(btnReturn);

                // Info button for remaining time
                var btnInfo = new Button
                {
                    Text = "INFO",
                    Width = 110,
                    Height = 32,
                    BackColor = Color.FromArgb(100, 149, 237), // CornflowerBlue, more vibrant
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Enabled = true
                };
                btnInfo.Location = new Point(card.Width - btnInfo.Width - 18, 70);
                btnInfo.FlatAppearance.BorderSize = 0;
                btnInfo.Click += (s, e) =>
                {
                    TimeSpan remaining = end - DateTime.Now;
                    string timeText, timeLabel;
                    Color highlightColor;
                    if (remaining.TotalSeconds > 0)
                    {
                        timeLabel = "Time Remaining";
                        timeText = $"{remaining.Days} days, {remaining.Hours} hours, {remaining.Minutes} minutes";
                        highlightColor = Color.FromArgb(100, 149, 237); // CornflowerBlue
                    }
                    else
                    {
                        timeLabel = "Overdue By";
                        timeText = $"{Math.Abs(remaining.Days)} days, {Math.Abs(remaining.Hours)} hours, {Math.Abs(remaining.Minutes)} minutes";
                        highlightColor = Color.Firebrick;
                    }

                    // Create a custom info form
                    Form infoForm = new Form
                    {
                        Text = "Rental Information",
                        Size = new Size(420, 370),
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        BackColor = Color.White,
                        MaximizeBox = false,
                        MinimizeBox = false
                    };

                    Label lblTitle = new Label
                    {
                        Text = "Rental Details",
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.Firebrick,
                        Location = new Point(20, 15),
                        AutoSize = true
                    };
                    infoForm.Controls.Add(lblTitle);

                    Label lblName = new Label
                    {
                        Text = $"Customer: {row["Customer"]}",
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        ForeColor = Color.Black,
                        Location = new Point(20, 55),
                        AutoSize = true
                    };
                    infoForm.Controls.Add(lblName);

                    Label lblVehicle = new Label
                    {
                        Text = $"Vehicle: {row["Vehicle"]} ({row["PlateNumber"]})",
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        ForeColor = Color.Black,
                        Location = new Point(20, 85),
                        AutoSize = true
                    };
                    infoForm.Controls.Add(lblVehicle);

                    Label lblPeriod = new Label
                    {
                        Text = $"Rental Period: {((DateTime)row["StartDate"]).ToString("MMM dd, yyyy")} - {((DateTime)row["EndDate"]).ToString("MMM dd, yyyy")}",
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        ForeColor = Color.Black,
                        Location = new Point(20, 115),
                        AutoSize = true
                    };
                    infoForm.Controls.Add(lblPeriod);

                    Label lblStatus = new Label
                    {
                        Text = $"Status: {(remaining.TotalSeconds > 0 ? "Ongoing" : "Overdue")}",
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = (remaining.TotalSeconds > 0 ? Color.FromArgb(100, 149, 237) : Color.Firebrick),
                        Location = new Point(20, 145),
                        AutoSize = true
                    };
                    infoForm.Controls.Add(lblStatus);

                    Panel highlightPanel = new Panel
                    {
                        BackColor = highlightColor,
                        Location = new Point(20, 185),
                        Size = new Size(370, 60),
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    infoForm.Controls.Add(highlightPanel);

                    Label lblTimeLabel = new Label
                    {
                        Text = timeLabel,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        ForeColor = Color.White,
                        Location = new Point(10, 8),
                        AutoSize = true
                    };
                    highlightPanel.Controls.Add(lblTimeLabel);

                    Label lblTime = new Label
                    {
                        Text = timeText,
                        Font = new Font("Segoe UI", 13, FontStyle.Bold),
                        ForeColor = Color.White,
                        Location = new Point(10, 28),
                        AutoSize = true
                    };
                    highlightPanel.Controls.Add(lblTime);

                    // Add highlightPanel and all other controls first
                    infoForm.Controls.Add(highlightPanel);

                    // Add Close button after all other controls and anchor it
                    Button btnClose = new Button
                    {
                        Text = "Close",
                        Height = 40,
                        Dock = DockStyle.Bottom,
                        BackColor = Color.Firebrick,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 11, FontStyle.Bold)
                    };
                    btnClose.FlatAppearance.BorderSize = 0;
                    btnClose.Click += (sender2, e2) => infoForm.Close();
                    infoForm.Controls.Add(btnClose);

                    infoForm.ShowDialog();
                };
                card.Controls.Add(btnInfo);
                btnInfo.BringToFront();

                // Add card to flow panel
                flowPanel.Controls.Add(card);
            }

            // Adjust FlowLayoutPanel for less void space
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.AutoScroll = true;
            flowPanel.WrapContents = true; // Fill each row, then wrap to next row
            flowPanel.FlowDirection = FlowDirection.LeftToRight; // Arrange cards in rows
            flowPanel.Padding = new Padding(10);
        }

        private void ReturnCar(string rentalCode)
        {
            // Show confirmation dialog
            DialogResult result = MessageBox.Show(
                "Are you sure you want to return this vehicle?",
                "Confirm Return",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
            {
                return;
            }

            // Get the CarCode, CustomerCode, and EndDate for this rental
            string carCode = null;
            string customerCode = null;
            DateTime endDate = DateTime.MinValue;
            decimal dailyRate = 0;

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string selectQuery = @"SELECT r.CarCode, r.CustomerCode, r.EndDate, v.RentalPrice 
                                     FROM tblRentals r
                                     INNER JOIN tblVehicles v ON r.CarCode = v.CarCode
                                     WHERE r.RentalCode = ?";
                using (var cmd = new OleDbCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("?", rentalCode);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            carCode = reader["CarCode"].ToString();
                            customerCode = reader["CustomerCode"].ToString();
                            endDate = Convert.ToDateTime(reader["EndDate"]);
                            dailyRate = Convert.ToDecimal(reader["RentalPrice"]);
                        }
                    }
                }
            }

            if (carCode == null || customerCode == null)
            {
                MessageBox.Show("Could not find rental details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime actualReturnTime = DateTime.Now;
            DateTime graceEnd = endDate.AddHours(1);
            bool isLateReturn = actualReturnTime > graceEnd;

            if (isLateReturn)
            {
                // Calculate late days after 1-hour grace period
                TimeSpan lateSpan = actualReturnTime - graceEnd;
                int lateDays = (int)Math.Ceiling(lateSpan.TotalDays);
                if (lateDays < 1) lateDays = 1;
                decimal lateFee = lateDays * dailyRate;

                // Show payment options dialog
                var paymentForm = new Form
                {
                    Text = "Late Return Payment",
                    Size = new Size(400, 300),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                var lblMessage = new Label
                {
                    Text = $"This vehicle is being returned {lateDays} day(s) late.\nAdditional amount due: ₱{lateFee:N2}\n\nPlease select payment method:",
                    Location = new Point(20, 20),
                    Size = new Size(340, 80),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                var rbnCash = new RadioButton
                {
                    Text = "Cash",
                    Location = new Point(40, 120),
                    Size = new Size(100, 20),
                    Checked = true
                };

                var rbnGcash = new RadioButton
                {
                    Text = "GCash",
                    Location = new Point(40, 150),
                    Size = new Size(100, 20)
                };

                var btnConfirm = new Button
                {
                    Text = "Confirm Payment",
                    Location = new Point(120, 200),
                    Size = new Size(150, 30),
                    DialogResult = DialogResult.OK
                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    Location = new Point(280, 200),
                    Size = new Size(80, 30),
                    DialogResult = DialogResult.Cancel
                };

                paymentForm.Controls.AddRange(new Control[] { lblMessage, rbnCash, rbnGcash, btnConfirm, btnCancel });

                if (paymentForm.ShowDialog() == DialogResult.OK)
                {
                    if (rbnGcash.Checked)
                    {
                        frmOnlinePayment onlinePaymentForm = new frmOnlinePayment(
                            customerCode,
                            lateFee,
                            "SYSTEM",
                            carCode,
                            endDate,
                            actualReturnTime,
                            lateDays,
                            rentalCode
                        );
                        if (onlinePaymentForm.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }
                    else
                    {
                        var cashPaymentForm = new frmProcessPayment(
                            customerCode,
                            carCode,
                            endDate,
                            actualReturnTime,
                            lateDays,
                            lateFee,
                            lateFee,
                            rentalCode
                        );
                        cashPaymentForm.SetPaymentDetails(lateFee, 0, lateFee, lateDays, "SYSTEM");
                        if (cashPaymentForm.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    // After successful payment, update records
                    using (var conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                // Update original rental
                                string updateRental = "UPDATE tblRentals SET RentalStatus = 'Returned', ReturnDate = ? WHERE RentalCode = ?";
                                using (var cmd = new OleDbCommand(updateRental, conn))
                                {
                                    cmd.Transaction = transaction;
                                    cmd.Parameters.AddWithValue("?", actualReturnTime.ToString("M/d/yyyy h:mm:ss tt"));
                                    cmd.Parameters.AddWithValue("?", rentalCode);
                                    cmd.ExecuteNonQuery();
                                }
                                // Update customer status
                                string updateCustomer = "UPDATE tblCustomers SET CustomerStatus = 'Returned' WHERE CustomerCode = ?";
                                using (var cmd = new OleDbCommand(updateCustomer, conn))
                                {
                                    cmd.Transaction = transaction;
                                    cmd.Parameters.AddWithValue("?", customerCode);
                                    cmd.ExecuteNonQuery();
                                }
                                // Update vehicle status
                                string updateVehicle = "UPDATE tblVehicles SET Availability = 'Available', VehicleStatus = 'Available' WHERE CarCode = ?";
                                using (var cmd = new OleDbCommand(updateVehicle, conn))
                                {
                                    cmd.Transaction = transaction;
                                    cmd.Parameters.AddWithValue("?", carCode);
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                                MessageBox.Show("Car returned and late payment processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadActiveRentals();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Error updating rental status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                // On-time return (within 1 hour grace period)
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update original rental
                string updateRental = "UPDATE tblRentals SET RentalStatus = 'Returned', ReturnDate = ? WHERE RentalCode = ?";
                using (var cmd = new OleDbCommand(updateRental, conn))
                {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("?", actualReturnTime.ToString("M/d/yyyy h:mm:ss tt"));
                    cmd.Parameters.AddWithValue("?", rentalCode);
                    cmd.ExecuteNonQuery();
                }
                            // Update customer status
                string updateCustomer = "UPDATE tblCustomers SET CustomerStatus = 'Returned' WHERE CustomerCode = ?";
                using (var cmd = new OleDbCommand(updateCustomer, conn))
                {
                                cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("?", customerCode);
                    cmd.ExecuteNonQuery();
                }
                            // Update vehicle status
                string updateVehicle = "UPDATE tblVehicles SET Availability = 'Available', VehicleStatus = 'Available' WHERE CarCode = ?";
                using (var cmd = new OleDbCommand(updateVehicle, conn))
                {
                                cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("?", carCode);
                    cmd.ExecuteNonQuery();
                }
                            transaction.Commit();
                            MessageBox.Show("Car returned on time!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadActiveRentals();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error updating rental status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // Custom rounded panel for cards
        public class RoundedPanel : Panel
        {
            public int BorderRadius { get; set; } = 12;
            public int BorderThickness { get; set; } = 2;
            public Color BorderColor { get; set; } = Color.Firebrick;
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    int r = BorderRadius;
                    int t = BorderThickness;
                    int w = Width - 1 - t;
                    int h = Height - 1 - t;
                    // Offset the border by half the thickness to keep it inside
                    Rectangle rect = new Rectangle(t / 2, t / 2, w, h);
                    path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                    path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                    path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                    path.CloseAllFigures();
                    this.Region = new Region(path);
                    using (var pen = new Pen(BorderColor, t))
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }
        }

        private void pnlChartPlaceholder_Paint(object sender, PaintEventArgs e)
        {
            // You can add custom drawing code here if needed
        }
    }
}
