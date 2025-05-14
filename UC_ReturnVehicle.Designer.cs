namespace CarRent
{
    partial class UC_ReturnVehicle
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblHeader;
        private Guna.UI2.WinForms.Guna2Panel pnlFilter;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDateRange;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpFrom;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpTo;
        private Guna.UI2.WinForms.Guna2Button btnFilter;
        private Guna.UI2.WinForms.Guna2TextBox tbxSearch;

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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblHeader = new Label();
            label1 = new Label();
            guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(components);
            tbxSearch = new Guna.UI2.WinForms.Guna2TextBox();
            guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(components);
            pnlFilter = new Guna.UI2.WinForms.Guna2Panel();
            lblDateRange = new Guna.UI2.WinForms.Guna2HtmlLabel();
            dtpFrom = new Guna.UI2.WinForms.Guna2DateTimePicker();
            dtpTo = new Guna.UI2.WinForms.Guna2DateTimePicker();
            btnFilter = new Guna.UI2.WinForms.Guna2Button();
            pnlChartPlaceholder = new Guna.UI2.WinForms.Guna2Panel();
            guna2Elipse3 = new Guna.UI2.WinForms.Guna2Elipse(components);
            pnlFilter.SuspendLayout();
            SuspendLayout();
            // 
            // lblHeader
            // 
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("ROG Fonts", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblHeader.ForeColor = Color.Firebrick;
            lblHeader.Location = new Point(323, 0);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(302, 60);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "Vehicle";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("ROG Fonts", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(21, 0);
            label1.Name = "label1";
            label1.Size = new Size(315, 60);
            label1.TabIndex = 5;
            label1.Text = "Return ";
            // 
            // guna2Elipse1
            // 
            guna2Elipse1.BorderRadius = 20;
            guna2Elipse1.TargetControl = tbxSearch;
            // 
            // tbxSearch
            // 
            tbxSearch.BackColor = Color.Transparent;
            tbxSearch.BorderColor = Color.Black;
            tbxSearch.BorderRadius = 20;
            tbxSearch.BorderThickness = 2;
            tbxSearch.Cursor = Cursors.IBeam;
            tbxSearch.CustomizableEdges = customizableEdges1;
            tbxSearch.DefaultText = "";
            tbxSearch.DisabledState.BorderColor = Color.DarkGray;
            tbxSearch.DisabledState.FillColor = Color.FromArgb(228, 116, 117);
            tbxSearch.DisabledState.ForeColor = Color.Gainsboro;
            tbxSearch.DisabledState.PlaceholderForeColor = Color.Gainsboro;
            tbxSearch.FillColor = Color.RosyBrown;
            tbxSearch.FocusedState.BorderColor = Color.FromArgb(228, 116, 117);
            tbxSearch.Font = new Font("Segoe UI", 11F);
            tbxSearch.ForeColor = Color.Gainsboro;
            tbxSearch.HoverState.BorderColor = Color.FromArgb(228, 116, 117);
            tbxSearch.IconLeft = Properties.Resources.search;
            tbxSearch.IconLeftOffset = new Point(10, 0);
            tbxSearch.Location = new Point(131, 77);
            tbxSearch.Margin = new Padding(0);
            tbxSearch.Name = "tbxSearch";
            tbxSearch.PlaceholderForeColor = Color.Gainsboro;
            tbxSearch.PlaceholderText = "Search Keyword";
            tbxSearch.SelectedText = "";
            tbxSearch.ShadowDecoration.CustomizableEdges = customizableEdges2;
            tbxSearch.Size = new Size(201, 51);
            tbxSearch.TabIndex = 10;
            tbxSearch.TextOffset = new Point(7, 0);
            // 
            // guna2Elipse2
            // 
            guna2Elipse2.BorderRadius = 30;
            guna2Elipse2.TargetControl = pnlFilter;
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.FromArgb(228, 116, 117);
            pnlFilter.BorderColor = Color.Black;
            pnlFilter.BorderRadius = 12;
            pnlFilter.BorderThickness = 2;
            pnlFilter.Controls.Add(lblDateRange);
            pnlFilter.Controls.Add(dtpFrom);
            pnlFilter.Controls.Add(dtpTo);
            pnlFilter.Controls.Add(btnFilter);
            pnlFilter.CustomizableEdges = customizableEdges9;
            pnlFilter.FillColor = Color.FromArgb(228, 116, 117);
            pnlFilter.Location = new Point(335, 77);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.ShadowDecoration.CustomizableEdges = customizableEdges10;
            pnlFilter.Size = new Size(733, 50);
            pnlFilter.TabIndex = 12;
            // 
            // lblDateRange
            // 
            lblDateRange.BackColor = Color.Transparent;
            lblDateRange.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            lblDateRange.ForeColor = Color.White;
            lblDateRange.Location = new Point(10, 13);
            lblDateRange.Name = "lblDateRange";
            lblDateRange.Size = new Size(97, 25);
            lblDateRange.TabIndex = 0;
            lblDateRange.Text = "Date Range:";
            // 
            // dtpFrom
            // 
            dtpFrom.BorderRadius = 8;
            dtpFrom.Checked = true;
            dtpFrom.CustomizableEdges = customizableEdges3;
            dtpFrom.FillColor = Color.White;
            dtpFrom.Font = new Font("Segoe UI", 10F);
            dtpFrom.Format = DateTimePickerFormat.Long;
            dtpFrom.Location = new Point(105, 10);
            dtpFrom.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpFrom.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.ShadowDecoration.CustomizableEdges = customizableEdges4;
            dtpFrom.Size = new Size(251, 30);
            dtpFrom.TabIndex = 1;
            dtpFrom.TextAlign = HorizontalAlignment.Center;
            dtpFrom.Value = new DateTime(2025, 4, 30, 20, 15, 22, 980);
            // 
            // dtpTo
            // 
            dtpTo.BorderRadius = 8;
            dtpTo.Checked = true;
            dtpTo.CustomizableEdges = customizableEdges5;
            dtpTo.FillColor = Color.White;
            dtpTo.Font = new Font("Segoe UI", 10F);
            dtpTo.Format = DateTimePickerFormat.Long;
            dtpTo.Location = new Point(362, 10);
            dtpTo.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpTo.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpTo.Name = "dtpTo";
            dtpTo.ShadowDecoration.CustomizableEdges = customizableEdges6;
            dtpTo.Size = new Size(251, 30);
            dtpTo.TabIndex = 2;
            dtpTo.TextAlign = HorizontalAlignment.Center;
            dtpTo.Value = new DateTime(2025, 4, 30, 20, 15, 23, 13);
            // 
            // btnFilter
            // 
            btnFilter.BorderRadius = 8;
            btnFilter.CustomizableEdges = customizableEdges7;
            btnFilter.FillColor = Color.FromArgb(100, 149, 237);
            btnFilter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnFilter.ForeColor = Color.White;
            btnFilter.Location = new Point(619, 10);
            btnFilter.Name = "btnFilter";
            btnFilter.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnFilter.Size = new Size(100, 30);
            btnFilter.TabIndex = 3;
            btnFilter.Text = "Filter";
            // 
            // pnlChartPlaceholder
            // 
            pnlChartPlaceholder.BackColor = Color.FromArgb(207, 120, 120);
            pnlChartPlaceholder.BorderColor = Color.Black;
            pnlChartPlaceholder.BorderRadius = 20;
            pnlChartPlaceholder.BorderThickness = 2;
            pnlChartPlaceholder.CustomizableEdges = customizableEdges11;
            pnlChartPlaceholder.Location = new Point(131, 142);
            pnlChartPlaceholder.Name = "pnlChartPlaceholder";
            pnlChartPlaceholder.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnlChartPlaceholder.Size = new Size(937, 453);
            pnlChartPlaceholder.TabIndex = 11;
            pnlChartPlaceholder.Paint += pnlChartPlaceholder_Paint;
            // 
            // guna2Elipse3
            // 
            guna2Elipse3.BorderRadius = 40;
            guna2Elipse3.TargetControl = pnlChartPlaceholder;
            // 
            // UC_ReturnVehicle
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(pnlChartPlaceholder);
            Controls.Add(tbxSearch);
            Controls.Add(label1);
            Controls.Add(pnlFilter);
            Controls.Add(lblHeader);
            Name = "UC_ReturnVehicle";
            Size = new Size(1208, 649);
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse2;
        private Guna.UI2.WinForms.Guna2Panel pnlChartPlaceholder;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse3;
    }
}
