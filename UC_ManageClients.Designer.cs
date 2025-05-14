namespace CarRent
{
    partial class UC_ManageClients
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            lblHeader = new Label();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            tabPageCustomers = new TabPage();
            tabStaff = new TabPage();
            tabClients = new Guna.UI2.WinForms.Guna2TabControl();
            tabPageVehicles = new TabPage();
            tabClients.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("ROG Fonts", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(1, 0);
            label1.Name = "label1";
            label1.Size = new Size(297, 60);
            label1.TabIndex = 7;
            label1.Text = "manage";
            // 
            // lblHeader
            // 
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("ROG Fonts", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblHeader.ForeColor = Color.Firebrick;
            lblHeader.Location = new Point(303, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(306, 60);
            lblHeader.TabIndex = 6;
            lblHeader.Text = "clients";
            // 
            // guna2Panel1
            // 
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.Location = new Point(-1, 63);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(1208, 585);
            guna2Panel1.TabIndex = 0;
            // 
            // tabPageCustomers
            // 
            tabPageCustomers.Location = new Point(4, 44);
            tabPageCustomers.Name = "tabPageCustomers";
            tabPageCustomers.Padding = new Padding(3);
            tabPageCustomers.Size = new Size(1200, 537);
            tabPageCustomers.TabIndex = 1;
            tabPageCustomers.Text = "CUSTOMERS";
            tabPageCustomers.UseVisualStyleBackColor = true;
            // 
            // tabStaff
            // 
            tabStaff.BackColor = Color.Transparent;
            tabStaff.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabStaff.Location = new Point(4, 44);
            tabStaff.Name = "tabStaff";
            tabStaff.Padding = new Padding(3);
            tabStaff.Size = new Size(1200, 537);
            tabStaff.TabIndex = 0;
            tabStaff.Text = "STAFF";
            // 
            // tabClients
            // 
            tabClients.Controls.Add(tabStaff);
            tabClients.Controls.Add(tabPageCustomers);
            tabClients.Controls.Add(tabPageVehicles);
            tabClients.ItemSize = new Size(180, 40);
            tabClients.Location = new Point(-1, 63);
            tabClients.Name = "tabClients";
            tabClients.Padding = new Point(6, 4);
            tabClients.SelectedIndex = 0;
            tabClients.Size = new Size(1208, 585);
            tabClients.TabButtonHoverState.BorderColor = Color.Empty;
            tabClients.TabButtonHoverState.FillColor = Color.FromArgb(40, 52, 70);
            tabClients.TabButtonHoverState.Font = new Font("Segoe UI Semibold", 10F);
            tabClients.TabButtonHoverState.ForeColor = Color.White;
            tabClients.TabButtonHoverState.InnerColor = Color.FromArgb(40, 52, 70);
            tabClients.TabButtonIdleState.BorderColor = Color.Empty;
            tabClients.TabButtonIdleState.FillColor = Color.FromArgb(33, 42, 57);
            tabClients.TabButtonIdleState.Font = new Font("Segoe UI Semibold", 10F);
            tabClients.TabButtonIdleState.ForeColor = Color.FromArgb(156, 160, 167);
            tabClients.TabButtonIdleState.InnerColor = Color.FromArgb(33, 42, 57);
            tabClients.TabButtonSelectedState.BorderColor = Color.Empty;
            tabClients.TabButtonSelectedState.FillColor = Color.FromArgb(29, 37, 49);
            tabClients.TabButtonSelectedState.Font = new Font("Segoe UI Semibold", 10F);
            tabClients.TabButtonSelectedState.ForeColor = Color.White;
            tabClients.TabButtonSelectedState.InnerColor = Color.FromArgb(76, 132, 255);
            tabClients.TabButtonSize = new Size(180, 40);
            tabClients.TabIndex = 8;
            tabClients.TabMenuBackColor = Color.FromArgb(33, 42, 57);
            tabClients.TabMenuOrientation = Guna.UI2.WinForms.TabMenuOrientation.HorizontalTop;
            // 
            // tabPageVehicles
            // 
            tabPageVehicles.Location = new Point(4, 44);
            tabPageVehicles.Name = "tabPageVehicles";
            tabPageVehicles.Padding = new Padding(3);
            tabPageVehicles.Size = new Size(1200, 537);
            tabPageVehicles.TabIndex = 2;
            tabPageVehicles.Text = "VEHICLES";
            tabPageVehicles.UseVisualStyleBackColor = true;
            // 
            // UC_ManageClients
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(tabClients);
            Controls.Add(label1);
            Controls.Add(lblHeader);
            Controls.Add(guna2Panel1);
            Name = "UC_ManageClients";
            Size = new Size(1206, 647);
            tabClients.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lblHeader;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private TabPage tabPage3;
        private TabPage tabPageCustomers;
        private TabPage tabStaff;
        private Guna.UI2.WinForms.Guna2TabControl tabClients;
        private TabPage tabPageVehicles;
    }
}
