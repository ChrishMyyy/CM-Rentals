using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using Guna.UI2.WinForms;
using System.Drawing.Drawing2D;

namespace CarRent
{
    public partial class UC_ManageRequest : UserControl
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private FlowLayoutPanel flowPanel;

        public UC_ManageRequest()
        {
            InitializeComponent();
            InitializeCustomComponents();
            LoadPendingRequests();
        }

        private void InitializeCustomComponents()
        {
            // Initialize FlowLayoutPanel
            flowPanel = new FlowLayoutPanel();
            flowPanel.AutoScroll = true;
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.Padding = new Padding(20);
            flowPanel.BackColor = Color.Transparent;
            flowPanel.WrapContents = true;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.Visible = true;

            // Configure pnlRequest
            pnlRequest.Dock = DockStyle.Fill;
            pnlRequest.Padding = new Padding(10);
            pnlRequest.Visible = true;

            // Add FlowLayoutPanel to the panel
            pnlRequest.Controls.Clear();
            pnlRequest.Controls.Add(flowPanel);
        }

        private void LoadPendingRequests()
        {
            try
            {
                flowPanel.Controls.Clear();
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT UserCode, Fullname, Username, Role, DateCreated FROM tblUsers WHERE Status = 'Pending'";
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            // Show message if no pending requests
                            Label lblNoRequests = new Label();
                            lblNoRequests.Text = "No pending requests";
                            lblNoRequests.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                            lblNoRequests.ForeColor = Color.White;
                            lblNoRequests.AutoSize = true;
                            lblNoRequests.Location = new Point(20, 20);
                            flowPanel.Controls.Add(lblNoRequests);
                            return;
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            CreateRequestCard(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading requests: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateRequestCard(DataRow row)
        {
            // Create card panel
            Guna2GradientPanel card = new Guna2GradientPanel();
            card.Size = new Size(300, 200);
            card.Margin = new Padding(10, 50, 10, 10);
            card.BorderRadius = 15;
            card.FillColor = Color.FromArgb(189, 65, 65);
            card.FillColor2 = Color.FromArgb(189, 65, 65);
            card.BorderColor = Color.White;
            card.BorderThickness = 2;
            card.ShadowDecoration.Enabled = true;
            card.ShadowDecoration.Depth = 10;
            card.ShadowDecoration.Color = Color.FromArgb(0, 0, 0, 100);
            card.Visible = true;

            // Create labels for user information
            Label lblName = new Label();
            lblName.Text = row["Fullname"].ToString();
            lblName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblName.ForeColor = Color.White;
            lblName.Location = new Point(15, 15);
            lblName.AutoSize = true;
            lblName.Visible = true;

            Label lblUsername = new Label();
            lblUsername.Text = "Username: " + row["Username"].ToString();
            lblUsername.Font = new Font("Segoe UI", 10F);
            lblUsername.ForeColor = Color.LightGray;
            lblUsername.Location = new Point(15, 45);
            lblUsername.AutoSize = true;
            lblUsername.Visible = true;

            Label lblRole = new Label();
            lblRole.Text = "Role: " + row["Role"].ToString();
            lblRole.Font = new Font("Segoe UI", 10F);
            lblRole.ForeColor = Color.LightGray;
            lblRole.Location = new Point(15, 70);
            lblRole.AutoSize = true;
            lblRole.Visible = true;

            Label lblDate = new Label();
            lblDate.Text = "Date: " + Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
            lblDate.Font = new Font("Segoe UI", 10F);
            lblDate.ForeColor = Color.LightGray;
            lblDate.Location = new Point(15, 95);
            lblDate.AutoSize = true;
            lblDate.Visible = true;

            // Create approve button
            Guna2Button btnApprove = new Guna2Button();
            btnApprove.Text = "Approve";
            btnApprove.Size = new Size(120, 35);
            btnApprove.Location = new Point(15, 140);
            btnApprove.BorderRadius = 10;
            btnApprove.FillColor = Color.FromArgb(0, 192, 0);
            btnApprove.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnApprove.ForeColor = Color.White;
            btnApprove.Tag = row["UserCode"].ToString();
            btnApprove.Visible = true;
            btnApprove.Click += (s, e) => {
                string userCode = ((Guna2Button)s).Tag.ToString();
                UpdateUserStatus(userCode, "Active", "User approved successfully!");
            };

            // Create reject button
            Guna2Button btnReject = new Guna2Button();
            btnReject.Text = "Reject";
            btnReject.Size = new Size(120, 35);
            btnReject.Location = new Point(145, 140);
            btnReject.BorderRadius = 10;
            btnReject.FillColor = Color.FromArgb(192, 0, 0);
            btnReject.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnReject.ForeColor = Color.White;
            btnReject.Tag = row["UserCode"].ToString();
            btnReject.Visible = true;
            btnReject.Click += (s, e) => {
                string userCode = ((Guna2Button)s).Tag.ToString();
                UpdateUserStatus(userCode, "Inactive", "User flagged as inactive. Only admin can view/manage inactive customers.");
            };

            // Add controls to card
            card.Controls.Add(lblName);
            card.Controls.Add(lblUsername);
            card.Controls.Add(lblRole);
            card.Controls.Add(lblDate);
            card.Controls.Add(btnApprove);
            card.Controls.Add(btnReject);

            // Add card to flow panel
            flowPanel.Controls.Add(card);
        }

        private void UpdateUserStatus(string userCode, string status, string successMessage)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    if (status == "Active" || status == "Inactive" || status == "Pending")
                    {
                        string query = "UPDATE tblUsers SET Status = @Status WHERE UserCode = @UserCode";
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
                            cmd.Parameters.AddWithValue("@UserCode", userCode);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (status == "Void")
                    {
                        string query = "DELETE FROM tblUsers WHERE UserCode = @UserCode";
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserCode", userCode);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Request voided and removed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    LoadPendingRequests(); // Refresh the cards
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
            
        
    }
}
