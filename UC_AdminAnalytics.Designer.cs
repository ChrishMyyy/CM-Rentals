using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.Windows.Forms.Integration;

namespace CarRent
{
    partial class UC_AdminAnalytics
    {
        private Guna2Panel guna2PanelRevenue;
        private Guna2Panel guna2PanelVehicleUtilization;
        private Guna2Panel guna2PanelCustomerDemographics;
        private Guna2Panel guna2PanelRentalTrends;
        private Guna2Panel guna2PanelRentalStatus;
        private Guna2Panel guna2PanelTabButtons;
        private Guna2Button btnRevenue;
        private Guna2Button btnVehicleUtilization;
        private Guna2Button btnCustomerDemographics;
        private Guna2Button btnRentalTrends;
        private Guna2Button btnRentalStatus;
        private ElementHost revenueHost;
        private ElementHost vehicleUtilizationHost;
        private ElementHost customerDemographicsHost;
        private ElementHost rentalTrendsHost;
        private ElementHost rentalStatusHost;

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2PanelRevenue = new Guna2Panel();
            revenueHost = new ElementHost();
            guna2PanelVehicleUtilization = new Guna2Panel();
            vehicleUtilizationHost = new ElementHost();
            guna2PanelCustomerDemographics = new Guna2Panel();
            customerDemographicsHost = new ElementHost();
            guna2PanelRentalTrends = new Guna2Panel();
            rentalTrendsHost = new ElementHost();
            guna2PanelRentalStatus = new Guna2Panel();
            rentalStatusHost = new ElementHost();
            guna2PanelTabButtons = new Guna2Panel();
            btnRevenue = new Guna2Button();
            btnVehicleUtilization = new Guna2Button();
            btnCustomerDemographics = new Guna2Button();
            btnRentalTrends = new Guna2Button();
            btnRentalStatus = new Guna2Button();
            guna2PanelRevenue.SuspendLayout();
            guna2PanelVehicleUtilization.SuspendLayout();
            guna2PanelCustomerDemographics.SuspendLayout();
            guna2PanelRentalTrends.SuspendLayout();
            guna2PanelRentalStatus.SuspendLayout();
            guna2PanelTabButtons.SuspendLayout();
            SuspendLayout();
            // 
            // guna2PanelRevenue
            // 
            guna2PanelRevenue.Controls.Add(revenueHost);
            guna2PanelRevenue.CustomizableEdges = customizableEdges1;
            guna2PanelRevenue.Dock = DockStyle.Fill;
            guna2PanelRevenue.FillColor = Color.Transparent;
            guna2PanelRevenue.Location = new Point(0, 60);
            guna2PanelRevenue.Name = "guna2PanelRevenue";
            guna2PanelRevenue.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PanelRevenue.Size = new Size(1208, 589);
            guna2PanelRevenue.TabIndex = 0;
            // 
            // revenueHost
            // 
            revenueHost.Dock = DockStyle.Fill;
            revenueHost.Location = new Point(0, 0);
            revenueHost.Name = "revenueHost";
            revenueHost.Size = new Size(1208, 589);
            revenueHost.TabIndex = 0;
            // 
            // guna2PanelVehicleUtilization
            // 
            guna2PanelVehicleUtilization.Controls.Add(vehicleUtilizationHost);
            guna2PanelVehicleUtilization.CustomizableEdges = customizableEdges3;
            guna2PanelVehicleUtilization.Dock = DockStyle.Fill;
            guna2PanelVehicleUtilization.Location = new Point(0, 60);
            guna2PanelVehicleUtilization.Name = "guna2PanelVehicleUtilization";
            guna2PanelVehicleUtilization.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2PanelVehicleUtilization.Size = new Size(1208, 589);
            guna2PanelVehicleUtilization.TabIndex = 1;
            // 
            // vehicleUtilizationHost
            // 
            vehicleUtilizationHost.Dock = DockStyle.Fill;
            vehicleUtilizationHost.Location = new Point(0, 0);
            vehicleUtilizationHost.Name = "vehicleUtilizationHost";
            vehicleUtilizationHost.Size = new Size(1208, 589);
            vehicleUtilizationHost.TabIndex = 0;
            // 
            // guna2PanelCustomerDemographics
            // 
            guna2PanelCustomerDemographics.Controls.Add(customerDemographicsHost);
            guna2PanelCustomerDemographics.CustomizableEdges = customizableEdges5;
            guna2PanelCustomerDemographics.Dock = DockStyle.Fill;
            guna2PanelCustomerDemographics.Location = new Point(0, 60);
            guna2PanelCustomerDemographics.Name = "guna2PanelCustomerDemographics";
            guna2PanelCustomerDemographics.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2PanelCustomerDemographics.Size = new Size(1208, 589);
            guna2PanelCustomerDemographics.TabIndex = 2;
            // 
            // customerDemographicsHost
            // 
            customerDemographicsHost.Dock = DockStyle.Fill;
            customerDemographicsHost.Location = new Point(0, 0);
            customerDemographicsHost.Name = "customerDemographicsHost";
            customerDemographicsHost.Size = new Size(1208, 589);
            customerDemographicsHost.TabIndex = 0;
            // 
            // guna2PanelRentalTrends
            // 
            guna2PanelRentalTrends.Controls.Add(rentalTrendsHost);
            guna2PanelRentalTrends.CustomizableEdges = customizableEdges7;
            guna2PanelRentalTrends.Dock = DockStyle.Fill;
            guna2PanelRentalTrends.Location = new Point(0, 60);
            guna2PanelRentalTrends.Name = "guna2PanelRentalTrends";
            guna2PanelRentalTrends.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2PanelRentalTrends.Size = new Size(1208, 589);
            guna2PanelRentalTrends.TabIndex = 3;
            // 
            // rentalTrendsHost
            // 
            rentalTrendsHost.Dock = DockStyle.Fill;
            rentalTrendsHost.Location = new Point(0, 0);
            rentalTrendsHost.Name = "rentalTrendsHost";
            rentalTrendsHost.Size = new Size(1208, 589);
            rentalTrendsHost.TabIndex = 0;
            // 
            // guna2PanelRentalStatus
            // 
            guna2PanelRentalStatus.Controls.Add(rentalStatusHost);
            guna2PanelRentalStatus.CustomizableEdges = customizableEdges9;
            guna2PanelRentalStatus.Dock = DockStyle.Fill;
            guna2PanelRentalStatus.Location = new Point(0, 60);
            guna2PanelRentalStatus.Name = "guna2PanelRentalStatus";
            guna2PanelRentalStatus.ShadowDecoration.CustomizableEdges = customizableEdges10;
            guna2PanelRentalStatus.Size = new Size(1208, 589);
            guna2PanelRentalStatus.TabIndex = 4;
            // 
            // rentalStatusHost
            // 
            rentalStatusHost.BackColor = Color.Transparent;
            rentalStatusHost.Dock = DockStyle.Fill;
            rentalStatusHost.Location = new Point(0, 0);
            rentalStatusHost.Name = "rentalStatusHost";
            rentalStatusHost.Size = new Size(1208, 589);
            rentalStatusHost.TabIndex = 0;
            // 
            // guna2PanelTabButtons
            // 
            guna2PanelTabButtons.BackColor = Color.Transparent;
            guna2PanelTabButtons.Controls.Add(btnRevenue);
            guna2PanelTabButtons.Controls.Add(btnVehicleUtilization);
            guna2PanelTabButtons.Controls.Add(btnCustomerDemographics);
            guna2PanelTabButtons.Controls.Add(btnRentalTrends);
            guna2PanelTabButtons.Controls.Add(btnRentalStatus);
            guna2PanelTabButtons.CustomizableEdges = customizableEdges21;
            guna2PanelTabButtons.Dock = DockStyle.Top;
            guna2PanelTabButtons.ForeColor = SystemColors.ActiveCaptionText;
            guna2PanelTabButtons.Location = new Point(0, 0);
            guna2PanelTabButtons.Name = "guna2PanelTabButtons";
            guna2PanelTabButtons.ShadowDecoration.CustomizableEdges = customizableEdges22;
            guna2PanelTabButtons.Size = new Size(1208, 60);
            guna2PanelTabButtons.TabIndex = 5;
            // 
            // btnRevenue
            // 
            btnRevenue.BackColor = Color.Transparent;
            btnRevenue.BorderRadius = 5;
            btnRevenue.BorderThickness = 1;
            btnRevenue.CustomizableEdges = customizableEdges11;
            btnRevenue.FillColor = Color.White;
            btnRevenue.Font = new Font("Segoe UI", 9F);
            btnRevenue.ForeColor = Color.Firebrick;
            btnRevenue.Location = new Point(27, 10);
            btnRevenue.Name = "btnRevenue";
            btnRevenue.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnRevenue.Size = new Size(180, 40);
            btnRevenue.TabIndex = 0;
            btnRevenue.Text = "Revenue";
            // 
            // btnVehicleUtilization
            // 
            btnVehicleUtilization.BackColor = Color.Transparent;
            btnVehicleUtilization.BorderRadius = 5;
            btnVehicleUtilization.BorderThickness = 1;
            btnVehicleUtilization.CustomizableEdges = customizableEdges13;
            btnVehicleUtilization.FillColor = Color.White;
            btnVehicleUtilization.Font = new Font("Segoe UI", 9F);
            btnVehicleUtilization.ForeColor = Color.Firebrick;
            btnVehicleUtilization.Location = new Point(248, 10);
            btnVehicleUtilization.Name = "btnVehicleUtilization";
            btnVehicleUtilization.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnVehicleUtilization.Size = new Size(180, 40);
            btnVehicleUtilization.TabIndex = 1;
            btnVehicleUtilization.Text = "Vehicle Utilization";
            // 
            // btnCustomerDemographics
            // 
            btnCustomerDemographics.BackColor = Color.Transparent;
            btnCustomerDemographics.BorderRadius = 5;
            btnCustomerDemographics.BorderThickness = 1;
            btnCustomerDemographics.CustomizableEdges = customizableEdges15;
            btnCustomerDemographics.FillColor = Color.White;
            btnCustomerDemographics.Font = new Font("Segoe UI", 9F);
            btnCustomerDemographics.ForeColor = Color.Firebrick;
            btnCustomerDemographics.Location = new Point(476, 10);
            btnCustomerDemographics.Name = "btnCustomerDemographics";
            btnCustomerDemographics.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnCustomerDemographics.Size = new Size(220, 40);
            btnCustomerDemographics.TabIndex = 2;
            btnCustomerDemographics.Text = "Customer Demographics";
            // 
            // btnRentalTrends
            // 
            btnRentalTrends.BorderRadius = 5;
            btnRentalTrends.BorderThickness = 1;
            btnRentalTrends.CustomizableEdges = customizableEdges17;
            btnRentalTrends.FillColor = Color.White;
            btnRentalTrends.Font = new Font("Segoe UI", 9F);
            btnRentalTrends.ForeColor = Color.Firebrick;
            btnRentalTrends.Location = new Point(740, 10);
            btnRentalTrends.Name = "btnRentalTrends";
            btnRentalTrends.ShadowDecoration.CustomizableEdges = customizableEdges18;
            btnRentalTrends.Size = new Size(180, 40);
            btnRentalTrends.TabIndex = 3;
            btnRentalTrends.Text = "Rental Trends";
            // 
            // btnRentalStatus
            // 
            btnRentalStatus.BorderRadius = 5;
            btnRentalStatus.BorderThickness = 1;
            btnRentalStatus.CustomizableEdges = customizableEdges19;
            btnRentalStatus.FillColor = Color.White;
            btnRentalStatus.Font = new Font("Segoe UI", 9F);
            btnRentalStatus.ForeColor = Color.Firebrick;
            btnRentalStatus.Location = new Point(954, 10);
            btnRentalStatus.Name = "btnRentalStatus";
            btnRentalStatus.ShadowDecoration.CustomizableEdges = customizableEdges20;
            btnRentalStatus.Size = new Size(180, 40);
            btnRentalStatus.TabIndex = 4;
            btnRentalStatus.Text = "Rental Status";
            // 
            // UC_AdminAnalytics
            // 
            BackColor = Color.Transparent;
            Controls.Add(guna2PanelRevenue);
            Controls.Add(guna2PanelVehicleUtilization);
            Controls.Add(guna2PanelCustomerDemographics);
            Controls.Add(guna2PanelRentalTrends);
            Controls.Add(guna2PanelRentalStatus);
            Controls.Add(guna2PanelTabButtons);
            Name = "UC_AdminAnalytics";
            Size = new Size(1208, 649);
            guna2PanelRevenue.ResumeLayout(false);
            guna2PanelVehicleUtilization.ResumeLayout(false);
            guna2PanelCustomerDemographics.ResumeLayout(false);
            guna2PanelRentalTrends.ResumeLayout(false);
            guna2PanelRentalStatus.ResumeLayout(false);
            guna2PanelTabButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}

