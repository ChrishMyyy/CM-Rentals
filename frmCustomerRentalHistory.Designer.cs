namespace CarRent
{
    partial class frmCustomerRentalHistory
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerRentalHistory));
            lblTitle = new Label();
            dgvCustomerRentalHistory = new DataGridView();
            lblCustomerName = new Label();
            txtCustomerName = new TextBox();
            lblDateRange = new Label();
            dtpStart = new DateTimePicker();
            dtpEnd = new DateTimePicker();
            guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(components);
            guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(components);
            pbxCRH = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            label1 = new Label();
            lblCRHID = new Label();
            btnCustomerCloseHistory = new Button();
            btnCustomerFilterHistory = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCustomerRentalHistory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxCRH).BeginInit();
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
            lblTitle.Text = "Customer Rental History";
            // 
            // dgvCustomerRentalHistory
            // 
            dgvCustomerRentalHistory.AllowUserToAddRows = false;
            dgvCustomerRentalHistory.AllowUserToDeleteRows = false;
            dgvCustomerRentalHistory.ColumnHeadersHeight = 29;
            dgvCustomerRentalHistory.Location = new Point(20, 140);
            dgvCustomerRentalHistory.Name = "dgvCustomerRentalHistory";
            dgvCustomerRentalHistory.ReadOnly = true;
            dgvCustomerRentalHistory.RowHeadersWidth = 51;
            dgvCustomerRentalHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomerRentalHistory.Size = new Size(760, 300);
            dgvCustomerRentalHistory.TabIndex = 7;
            // 
            // lblCustomerName
            // 
            lblCustomerName.AutoSize = true;
            lblCustomerName.Location = new Point(20, 63);
            lblCustomerName.Name = "lblCustomerName";
            lblCustomerName.Size = new Size(119, 20);
            lblCustomerName.TabIndex = 1;
            lblCustomerName.Text = "Customer Name:";
            // 
            // txtCustomerName
            // 
            txtCustomerName.Location = new Point(160, 60);
            txtCustomerName.Name = "txtCustomerName";
            txtCustomerName.ReadOnly = true;
            txtCustomerName.Size = new Size(200, 27);
            txtCustomerName.TabIndex = 2;
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
            dtpStart.Location = new Point(110, 100);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(130, 27);
            dtpStart.TabIndex = 4;
            // 
            // dtpEnd
            // 
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.Location = new Point(250, 100);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(130, 27);
            dtpEnd.TabIndex = 5;
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
            // pbxCRH
            // 
            pbxCRH.BackgroundImage = Properties.Resources.profile;
            pbxCRH.BackgroundImageLayout = ImageLayout.Stretch;
            pbxCRH.FillColor = Color.Transparent;
            pbxCRH.ImageRotate = 0F;
            pbxCRH.InitialImage = Properties.Resources.profile;
            pbxCRH.Location = new Point(495, 29);
            pbxCRH.Name = "pbxCRH";
            pbxCRH.ShadowDecoration.CustomizableEdges = customizableEdges1;
            pbxCRH.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            pbxCRH.Size = new Size(58, 58);
            pbxCRH.SizeMode = PictureBoxSizeMode.StretchImage;
            pbxCRH.TabIndex = 12;
            pbxCRH.TabStop = false;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(559, 43);
            label1.Name = "label1";
            label1.Size = new Size(120, 40);
            label1.TabIndex = 13;
            label1.Text = "Customer ID:";
            // 
            // lblCRHID
            // 
            lblCRHID.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCRHID.ForeColor = SystemColors.HotTrack;
            lblCRHID.Location = new Point(668, 43);
            lblCRHID.Name = "lblCRHID";
            lblCRHID.Size = new Size(120, 40);
            lblCRHID.TabIndex = 14;
            lblCRHID.Text = "Customer ID";
            // 
            // btnCustomerCloseHistory
            // 
            btnCustomerCloseHistory.BackColor = Color.RosyBrown;
            btnCustomerCloseHistory.Location = new Point(691, 458);
            btnCustomerCloseHistory.Name = "btnCustomerCloseHistory";
            btnCustomerCloseHistory.Size = new Size(89, 30);
            btnCustomerCloseHistory.TabIndex = 15;
            btnCustomerCloseHistory.Text = "Close";
            btnCustomerCloseHistory.UseVisualStyleBackColor = false;
            btnCustomerCloseHistory.Click += btnCustomerCloseHistory_Click;
            // 
            // btnCustomerFilterHistory
            // 
            btnCustomerFilterHistory.BackColor = SystemColors.ActiveCaption;
            btnCustomerFilterHistory.Location = new Point(396, 100);
            btnCustomerFilterHistory.Name = "btnCustomerFilterHistory";
            btnCustomerFilterHistory.Size = new Size(80, 29);
            btnCustomerFilterHistory.TabIndex = 17;
            btnCustomerFilterHistory.Text = "Filter";
            btnCustomerFilterHistory.UseVisualStyleBackColor = false;
            btnCustomerFilterHistory.Click += btnCustomerFilterHistory_Click;
            // 
            // frmCustomerRentalHistory
            // 
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(800, 500);
            Controls.Add(btnCustomerFilterHistory);
            Controls.Add(btnCustomerCloseHistory);
            Controls.Add(lblCRHID);
            Controls.Add(label1);
            Controls.Add(pbxCRH);
            Controls.Add(lblTitle);
            Controls.Add(lblCustomerName);
            Controls.Add(txtCustomerName);
            Controls.Add(lblDateRange);
            Controls.Add(dtpStart);
            Controls.Add(dtpEnd);
            Controls.Add(dgvCustomerRentalHistory);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmCustomerRentalHistory";
            Text = "Customer Rental History";
            ((System.ComponentModel.ISupportInitialize)dgvCustomerRentalHistory).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxCRH).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblDateRange;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DataGridView dgvCustomerRentalHistory;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2CirclePictureBox pbxCRH;
        private Label label1;
        private Label lblCRHID;
        private Button btnCustomerCloseHistory;
        private Button btnCustomerFilterHistory;
    }
}
