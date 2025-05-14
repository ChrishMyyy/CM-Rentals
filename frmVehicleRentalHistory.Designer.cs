namespace CarRent

{
    partial class frmVehicleRentalHistory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            dgvVehicleRentalHistory = new DataGridView();
            lblVehicleInfo = new Label();
            txtVehicleInfo = new TextBox();
            lblDateRange = new Label();
            dtpStart = new DateTimePicker();
            dtpEnd = new DateTimePicker();
            btnFilter = new Button();
            guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(components);
            guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(components);
            btnVehicleCloseRental = new Button();
            lblVLP = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvVehicleRentalHistory).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Firebrick;
            lblTitle.Location = new Point(20, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(400, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Vehicle Rental History";
            // 
            // dgvVehicleRentalHistory
            // 
            dgvVehicleRentalHistory.AllowUserToAddRows = false;
            dgvVehicleRentalHistory.AllowUserToDeleteRows = false;
            dgvVehicleRentalHistory.ColumnHeadersHeight = 29;
            dgvVehicleRentalHistory.Location = new Point(20, 140);
            dgvVehicleRentalHistory.Name = "dgvVehicleRentalHistory";
            dgvVehicleRentalHistory.ReadOnly = true;
            dgvVehicleRentalHistory.RowHeadersWidth = 51;
            dgvVehicleRentalHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehicleRentalHistory.Size = new Size(760, 300);
            dgvVehicleRentalHistory.TabIndex = 7;
            // 
            // lblVehicleInfo
            // 
            lblVehicleInfo.AutoSize = true;
            lblVehicleInfo.Location = new Point(20, 63);
            lblVehicleInfo.Name = "lblVehicleInfo";
            lblVehicleInfo.Size = new Size(157, 20);
            lblVehicleInfo.TabIndex = 1;
            lblVehicleInfo.Text = "Vehicle (Model & Plate):";
            // 
            // txtVehicleInfo
            // 
            txtVehicleInfo.Location = new Point(183, 60);
            txtVehicleInfo.Name = "txtVehicleInfo";
            txtVehicleInfo.ReadOnly = true;
            txtVehicleInfo.Size = new Size(250, 27);
            txtVehicleInfo.TabIndex = 2;
            // 
            // lblDateRange
            // 
            lblDateRange.Location = new Point(20, 100);
            lblDateRange.Name = "lblDateRange";
            lblDateRange.Size = new Size(100, 23);
            lblDateRange.TabIndex = 3;
            lblDateRange.Text = "Date Range:";
            // 
            // dtpStart
            // 
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.Location = new Point(126, 98);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(129, 27);
            dtpStart.TabIndex = 4;
            // 
            // dtpEnd
            // 
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.Location = new Point(270, 98);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(127, 27);
            dtpEnd.TabIndex = 5;
            // 
            // btnFilter
            // 
            btnFilter.BackColor = SystemColors.ActiveCaption;
            btnFilter.Location = new Point(413, 96);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(80, 34);
            btnFilter.TabIndex = 6;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = false;
            // 
            // guna2Elipse1
            // 
            guna2Elipse1.TargetControl = this;
            // 
            // guna2DragControl1
            // 
            guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            guna2DragControl1.TargetControl = this;
            guna2DragControl1.UseTransparentDrag = true;
            // 
            // btnVehicleCloseRental
            // 
            btnVehicleCloseRental.BackColor = Color.RosyBrown;
            btnVehicleCloseRental.Location = new Point(700, 458);
            btnVehicleCloseRental.Name = "btnVehicleCloseRental";
            btnVehicleCloseRental.Size = new Size(80, 30);
            btnVehicleCloseRental.TabIndex = 9;
            btnVehicleCloseRental.Text = "Close";
            btnVehicleCloseRental.UseVisualStyleBackColor = false;
            btnVehicleCloseRental.Click += btnVehicleCloseRental_Click;
            // 
            // lblVLP
            // 
            lblVLP.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblVLP.ForeColor = SystemColors.HotTrack;
            lblVLP.Location = new Point(608, 43);
            lblVLP.Name = "lblVLP";
            lblVLP.Size = new Size(120, 26);
            lblVLP.TabIndex = 16;
            lblVLP.Text = "Vehicle ID";
            lblVLP.Click += lblVLP_Click;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(485, 43);
            label1.Name = "label1";
            label1.Size = new Size(128, 24);
            label1.TabIndex = 15;
            label1.Text = "Plate Number:";
            // 
            // frmVehicleRentalHistory
            // 
            ClientSize = new Size(800, 500);
            Controls.Add(lblVLP);
            Controls.Add(label1);
            Controls.Add(btnVehicleCloseRental);
            Controls.Add(lblTitle);
            Controls.Add(lblVehicleInfo);
            Controls.Add(txtVehicleInfo);
            Controls.Add(lblDateRange);
            Controls.Add(dtpStart);
            Controls.Add(dtpEnd);
            Controls.Add(btnFilter);
            Controls.Add(dgvVehicleRentalHistory);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmVehicleRentalHistory";
            Text = "Vehicle Rental History";
            ((System.ComponentModel.ISupportInitialize)dgvVehicleRentalHistory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtVehicleInfo;
        private System.Windows.Forms.Label lblVehicleInfo;
        private System.Windows.Forms.Label lblDateRange;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.DataGridView dgvVehicleRentalHistory;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Button btnVehicleCloseRental;
        private Label lblVLP;
        private Label label1;
    }
}
