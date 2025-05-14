using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace CarRent
{
    public partial class UC_ManageClients : UserControl
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private FlowLayoutPanel flowPanelStaff;
        private FlowLayoutPanel flowPanelCustomers;

        public UC_ManageClients()
        {
            InitializeComponent();
            InitializeFlowPanels();
            LoadStaffData();
            LoadCustomerData();
        }

        private void InitializeFlowPanels()
        {
            // Initialize Staff Flow Panel
            flowPanelStaff = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                BackColor = Color.Transparent
            };
            tabStaff.Controls.Add(flowPanelStaff);

            // Initialize Customers Flow Panel
            flowPanelCustomers = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10),
                BackColor = Color.Transparent
            };
            tabPageCustomers.Controls.Add(flowPanelCustomers);
        }

        private void LoadStaffData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT UserCode, FullName, Username, Role, Status, DateCreated, UserImage 
                                   FROM tblUsers 
                                   WHERE Role = 'Staff' 
                                   ORDER BY DateCreated DESC";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CreateStaffCard(reader);
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

        private void LoadCustomerData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT CustomerCode, FullName, ContactNumber, Address, Status, DateRegistered, LicenseImage 
                                   FROM tblCustomers 
                                   ORDER BY DateRegistered DESC";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CreateCustomerCard(reader);
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

        private void CreateStaffCard(OleDbDataReader reader)
        {
            var card = new RoundedPanel
            {
                Width = 370,
                Height = 200,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderRadius = 12,
                BorderThickness = 2,
                BorderColor = Color.FromArgb(189, 65, 65)
            };

            // Picture Box for Staff Image
            var pbxStaff = new Guna.UI2.WinForms.Guna2CirclePictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(240, 15),
                SizeMode = PictureBoxSizeMode.Zoom,
               
                FillColor = Color.White
            };

            // Try to load staff image if exists, otherwise use default
            try
            {
                string imagePath = reader["UserImage"]?.ToString();
                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    pbxStaff.Image = Image.FromFile(imagePath);
                }
                else
                {
                    pbxStaff.Image = Properties.Resources.profile; // Default profile image
                }
            }
            catch
            {
                pbxStaff.Image = Properties.Resources.profile; // Default profile image
            }
            card.Controls.Add(pbxStaff);

            // Staff Code Label
            var lblCode = new Label
            {
                Text = reader["UserCode"].ToString(),
                Location = new Point(15, 15),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(189, 65, 65),
                AutoSize = true
            };
            card.Controls.Add(lblCode);

            // Staff Name Label
            var lblName = new Label
            {
                Text = reader["FullName"].ToString(),
                Location = new Point(15, 40),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };
            card.Controls.Add(lblName);

            // Username Label
            var lblUsername = new Label
            {
                Text = $"Username: {reader["Username"].ToString()}",
                Location = new Point(15, 70),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            card.Controls.Add(lblUsername);

            // Role Label
            var lblRole = new Label
            {
                Text = $"Role: {reader["Role"].ToString()}",
                Location = new Point(15, 95),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            card.Controls.Add(lblRole);

            // Status Label
            var lblStatus = new Label
            {
                Text = $"Status: {reader["Status"].ToString()}",
                Location = new Point(15, 120),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.CadetBlue,
                AutoSize = true
            };
            card.Controls.Add(lblStatus);

            // Date Created Label
            var lblDate = new Label
            {
                Text = $"Joined: {Convert.ToDateTime(reader["DateCreated"]).ToString("MMM dd, yyyy")}",
                Location = new Point(15, 145),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            card.Controls.Add(lblDate);

            // Edit Button
            var btnEdit = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Edit",
                Size = new Size(120, 30),
                BorderRadius = 5,
                FillColor = Color.FromArgb(189, 65, 65),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(card.Width - 135, card.Height - 45), // 15px from right, 15px from bottom
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            string userCode = reader["UserCode"].ToString();
            btnEdit.Click += (s, e) => EditStaff(userCode);
            card.Controls.Add(btnEdit);
            btnEdit.BringToFront();

            flowPanelStaff.Controls.Add(card);
        }

        private void CreateCustomerCard(OleDbDataReader reader)
        {
            var card = new RoundedPanel
            {
                Width = 360,
                Height = 250,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderRadius = 12,
                BorderThickness = 2,
                BorderColor = Color.FromArgb(189, 65, 65)
            };

            // Picture Box for Customer Image
            var pbxCustomer = new Guna.UI2.WinForms.Guna2CirclePictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(240, 15),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.FixedSingle,
                FillColor = Color.White

            };

            // Try to load customer image if exists, otherwise use default
            try
            {
                string imagePath = reader["LicenseImage"]?.ToString();
                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                {
                    pbxCustomer.Image = Image.FromFile(imagePath);
                }
                else
                {
                    pbxCustomer.Image = Properties.Resources.profile; // Default profile image
                }
            }
            catch
            {
                pbxCustomer.Image = Properties.Resources.profile; // Default profile image
            }
            card.Controls.Add(pbxCustomer);

            // Customer Code Label
            var lblCode = new Label
            {
                Text = reader["CustomerCode"].ToString(),
                Location = new Point(15, 15),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(189, 65, 65),
                AutoSize = true
            };
            card.Controls.Add(lblCode);

            // Customer Name Label
            var lblName = new Label
            {
                Text = reader["FullName"].ToString(),
                Location = new Point(15, 40),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };
            card.Controls.Add(lblName);

            // Contact Label
            var lblContact = new Label
            {
                Text = $"Contact: {reader["ContactNumber"].ToString()}",
                Location = new Point(15, 70),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            card.Controls.Add(lblContact);

  
            // Address Label
            var lblAddress = new Label
            {
                Text = $"Address: {reader["Address"].ToString()}",
                Location = new Point(15, 120),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            card.Controls.Add(lblAddress);

            // Status Label
            var lblStatus = new Label
            {
                Text = $"Status: {reader["Status"].ToString()}",
                Location = new Point(15, 145),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.CadetBlue,
                AutoSize = true
            };
            card.Controls.Add(lblStatus);

            // Date Created Label
            var lblDate = new Label
            {
            
                Text = $"Joined: {Convert.ToDateTime(reader["DateRegistered"]).ToString("MMM dd, yyyy")}",
                Location = new Point(15, 170),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            card.Controls.Add(lblDate);

            // Edit Button
            var btnEdit = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Edit",
                Location = new Point(15, 200),
                Size = new Size(120, 30),
                BorderRadius = 5,
                FillColor = Color.FromArgb(189, 65, 65),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            string customerCode = reader["CustomerCode"].ToString();
            btnEdit.Click += (s, e) => EditCustomer(customerCode);
            card.Controls.Add(btnEdit);

            flowPanelCustomers.Controls.Add(card);
        }

        private void EditStaff(string userCode)
        {
            using (frmEditStaff editForm = new frmEditStaff(userCode))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Clear existing cards
                    flowPanelStaff.Controls.Clear();
                    // Reload staff data
                    LoadStaffData();
                }
            }
        }

        private void EditCustomer(string customerCode)
        {
            using (frmEditCustomers editForm = new frmEditCustomers(customerCode))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Clear existing cards
                    flowPanelCustomers.Controls.Clear();
                    // Reload customer data
                    LoadCustomerData();
                }
            }
        }
    }

    // Custom rounded panel for cards
    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 12;
        public int BorderThickness { get; set; } = 2;
        public Color BorderColor { get; set; } = Color.FromArgb(189, 65, 65);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                int r = BorderRadius;
                int t = BorderThickness;
                int w = Width - 1 - t;
                int h = Height - 1 - t;
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
}
