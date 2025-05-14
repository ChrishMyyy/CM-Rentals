namespace CarRent
{
    partial class frmUserTransactionHistory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblTitle = new Label();
            dgvTransactionHistory = new Guna.UI2.WinForms.Guna2DataGridView();
            lblUserName = new Label();
            txtUserName = new TextBox();
            lblDateRange = new Label();
            dtpStart = new DateTimePicker();
            dtpEnd = new DateTimePicker();
            guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(components);
            guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(components);
            pbxUser = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            label1 = new Label();
            lblUserID = new Label();
            btnClose = new Button();
            btnFilter = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvTransactionHistory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxUser).BeginInit();
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
            lblTitle.Text = "Transaction History";
            // 
            // dgvTransactionHistory
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvTransactionHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvTransactionHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvTransactionHistory.ColumnHeadersHeight = 29;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvTransactionHistory.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTransactionHistory.GridColor = Color.FromArgb(231, 229, 255);
            dgvTransactionHistory.Location = new Point(20, 140);
            dgvTransactionHistory.Name = "dgvTransactionHistory";
            dgvTransactionHistory.ReadOnly = true;
            dgvTransactionHistory.RowHeadersVisible = false;
            dgvTransactionHistory.RowHeadersWidth = 51;
            dgvTransactionHistory.Size = new Size(760, 300);
            dgvTransactionHistory.TabIndex = 6;
            dgvTransactionHistory.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvTransactionHistory.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvTransactionHistory.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvTransactionHistory.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvTransactionHistory.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvTransactionHistory.ThemeStyle.BackColor = Color.White;
            dgvTransactionHistory.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvTransactionHistory.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvTransactionHistory.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvTransactionHistory.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvTransactionHistory.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvTransactionHistory.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTransactionHistory.ThemeStyle.HeaderStyle.Height = 29;
            dgvTransactionHistory.ThemeStyle.ReadOnly = true;
            dgvTransactionHistory.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvTransactionHistory.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTransactionHistory.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvTransactionHistory.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvTransactionHistory.ThemeStyle.RowsStyle.Height = 29;
            dgvTransactionHistory.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvTransactionHistory.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // lblUserName
            // 
            lblUserName.BackColor = Color.Transparent;
            lblUserName.Location = new Point(20, 63);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(100, 20);
            lblUserName.TabIndex = 1;
            lblUserName.Text = "Staff Name:";
            // 
            // txtUserName
            // 
            txtUserName.Location = new Point(160, 60);
            txtUserName.Name = "txtUserName";
            txtUserName.ReadOnly = true;
            txtUserName.Size = new Size(200, 27);
            txtUserName.TabIndex = 2;
            // 
            // lblDateRange
            // 
            lblDateRange.BackColor = Color.Transparent;
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
            guna2Elipse1.BorderRadius = 15;
            guna2Elipse1.TargetControl = this;
            // 
            // guna2DragControl1
            // 
            guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            guna2DragControl1.TargetControl = this;
            guna2DragControl1.UseTransparentDrag = true;
            // 
            // pbxUser
            // 
            pbxUser.BackgroundImage = Properties.Resources.profile;
            pbxUser.BackgroundImageLayout = ImageLayout.Zoom;
            pbxUser.ImageRotate = 0F;
            pbxUser.Location = new Point(495, 29);
            pbxUser.Name = "pbxUser";
            pbxUser.ShadowDecoration.CustomizableEdges = customizableEdges1;
            pbxUser.Size = new Size(58, 58);
            pbxUser.SizeMode = PictureBoxSizeMode.Zoom;
            pbxUser.TabIndex = 7;
            pbxUser.TabStop = false;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label1.Location = new Point(559, 43);
            label1.Name = "label1";
            label1.Size = new Size(100, 40);
            label1.TabIndex = 8;
            label1.Text = "User ID:";
            // 
            // lblUserID
            // 
            lblUserID.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            lblUserID.ForeColor = SystemColors.HotTrack;
            lblUserID.Location = new Point(632, 43);
            lblUserID.Name = "lblUserID";
            lblUserID.Size = new Size(120, 40);
            lblUserID.TabIndex = 9;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.RosyBrown;
            btnClose.Location = new Point(691, 458);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(89, 30);
            btnClose.TabIndex = 10;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnFilter
            // 
            btnFilter.BackColor = SystemColors.ActiveCaption;
            btnFilter.Location = new Point(396, 100);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(80, 29);
            btnFilter.TabIndex = 11;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = false;
            btnFilter.Click += btnFilter_Click;
            // 
            // frmUserTransactionHistory
            // 
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(800, 500);
            Controls.Add(lblUserID);
            Controls.Add(dtpStart);
            Controls.Add(lblTitle);
            Controls.Add(lblUserName);
            Controls.Add(txtUserName);
            Controls.Add(lblDateRange);
            Controls.Add(dtpEnd);
            Controls.Add(dgvTransactionHistory);
            Controls.Add(pbxUser);
            Controls.Add(label1);
            Controls.Add(btnClose);
            Controls.Add(btnFilter);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmUserTransactionHistory";
            ((System.ComponentModel.ISupportInitialize)dgvTransactionHistory).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxUser).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Guna.UI2.WinForms.Guna2DataGridView dgvTransactionHistory;
        private Label lblUserName;
        private TextBox txtUserName;
        private Label lblDateRange;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2CirclePictureBox pbxUser;
        private Label label1;
        private Label lblUserID;
        private Button btnClose;
        private Button btnFilter;
    }
}