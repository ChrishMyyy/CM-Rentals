using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarRent;

namespace CarRent
{
    public partial class frmDashboard : Form
    {
        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.Timer typewriterTimer;

        public frmDashboard()
        {
            InitializeComponent();
            // Load default user control
            LoadUserControl(new UC_Home());
            InitializeClock();
            btnInfo.Click += btnInfo_Click;
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

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // Set profile name
            lblProfileName.Text = Session.CurrentUserFullName;
            
            // Set profile image
            if (!string.IsNullOrEmpty(Session.CurrentUserImagePath) && System.IO.File.Exists(Session.CurrentUserImagePath))
            {
                pbxProfile.Image = Image.FromFile(Session.CurrentUserImagePath);
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

        private void guna2ControlBox1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnManageVehicles_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_ManageVehicles());
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

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_ManageCustomers());
        }

        private void btnManageRentals_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_ManageRentals());
        }

        private void btnAnalytics_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_ReportAnalytics());
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblProfileName_Click(object sender, EventArgs e)
        {

        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UC_Info());
        }
    }
}
