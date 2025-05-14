using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRent
{
    public partial class frmAdminDashboard : Form
    {
        private System.Windows.Forms.Timer clockTimer;

        public frmAdminDashboard()
        {
            InitializeComponent();
            // Load default user control
            LoadUserControl(new UC_Home());
            InitializeClock();
        }

        private void InitializeClock()
        {
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 1000; // 1 second
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
            UpdateClockLabel();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateClockLabel();
        }

        private void UpdateClockLabel()
        {
            if (lblHours != null)
            {
                lblHours.Text = DateTime.Now.ToString("MMM dd, yyyy") + " | " + DateTime.Now.ToString("hh:mm:ss tt");
            }
        }

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            // Set profile name
            lblProfileName.Text = Session.CurrentUserFullName;
            lblRole.Text = Session.CurrentUserRole;

            // Set profile image
            if (!string.IsNullOrEmpty(Session.CurrentUserImagePath) && System.IO.File.Exists(Session.CurrentUserImagePath))
            {
                try
                {
                    pbxProfile.Image = Image.FromFile(Session.CurrentUserImagePath);
                }
                catch
                {
                    pbxProfile.Image = Properties.Resources.profile; // fallback/default image
                }
            }
            else
            {
                pbxProfile.Image = Properties.Resources.profile; // fallback/default image
            }
        }

        private void LoadUserControl(UserControl uc)
        {
            pnlMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(uc);
            uc.BringToFront();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_Home());
        }

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_ManageCustomers());
        }

        private void btnManageVehicles_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_ManageVehicles());
        }

        private void btnManageRentals_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_ManageRentals());
        }

        private void btnManageStaff_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_ManageClients());
        }

        private void btnAnalytics_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_AdminAnalytics());
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            // Clear any existing controls
            pnlMain.Controls.Clear();

            // Create and add the UC_ManageRequest control
            UC_ManageRequest manageRequest = new UC_ManageRequest();
            manageRequest.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(manageRequest);


        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                frmLogin loginForm = new frmLogin();
                loginForm.Show();
                this.Close();
            }
        }

        private void guna2ControlBox1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
