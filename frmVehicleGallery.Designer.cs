namespace CarRent
{
    partial class frmVehicleGallery
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            flowLayoutVehicleImages = new FlowLayoutPanel();
            btnAddImage = new Button();
            guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            flowLayoutVehicleImages.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutVehicleImages
            // 
            flowLayoutVehicleImages.BackColor = Color.FromArgb(207, 120, 120);
            flowLayoutVehicleImages.Controls.Add(guna2ControlBox1);
            flowLayoutVehicleImages.Dock = DockStyle.Fill;
            flowLayoutVehicleImages.Location = new Point(0, 0);
            flowLayoutVehicleImages.Name = "flowLayoutVehicleImages";
            flowLayoutVehicleImages.Size = new Size(876, 613);
            flowLayoutVehicleImages.TabIndex = 0;
            flowLayoutVehicleImages.Paint += flowLayoutVehicleImages_Paint;
            // 
            // btnAddImage
            // 
            btnAddImage.BackColor = Color.FromArgb(207, 120, 120);
            btnAddImage.BackgroundImage = Properties.Resources.add_image__1_;
            btnAddImage.BackgroundImageLayout = ImageLayout.Zoom;
            btnAddImage.FlatAppearance.BorderSize = 0;
            btnAddImage.FlatStyle = FlatStyle.Flat;
            btnAddImage.Location = new Point(815, 5);
            btnAddImage.Name = "btnAddImage";
            btnAddImage.Size = new Size(51, 47);
            btnAddImage.TabIndex = 1;
            btnAddImage.UseVisualStyleBackColor = false;
            // 
            // guna2ControlBox1
            // 
            guna2ControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guna2ControlBox1.CustomizableEdges = customizableEdges1;
            guna2ControlBox1.FillColor = Color.FromArgb(139, 152, 166);
            guna2ControlBox1.IconColor = Color.White;
            guna2ControlBox1.Location = new Point(3, 3);
            guna2ControlBox1.Name = "guna2ControlBox1";
            guna2ControlBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2ControlBox1.Size = new Size(45, 29);
            guna2ControlBox1.TabIndex = 0;
            // 
            // frmVehicleGallery
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(876, 613);
            Controls.Add(btnAddImage);
            Controls.Add(flowLayoutVehicleImages);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmVehicleGallery";
            Text = "Vehicle Gallery";
            Load += frmVehicleGallery_Load_1;
            flowLayoutVehicleImages.ResumeLayout(false);
            ResumeLayout(false);

            // Create and configure the close button
            var btnClose = new Guna.UI2.WinForms.Guna2Button();
            btnClose.Text = "Close";
            btnClose.Size = new Size(120, 45);
            btnClose.FillColor = Color.Firebrick;
            btnClose.ForeColor = Color.White;
            btnClose.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            // Place it at the bottom center
            btnClose.Location = new Point((this.ClientSize.Width - btnClose.Width) / 2, this.ClientSize.Height - btnClose.Height - 20);
            btnClose.Anchor = AnchorStyles.Bottom;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
            btnClose.BringToFront();
        }

        #endregion

        private FlowLayoutPanel flowLayoutVehicleImages;
        private Button btnAddImage;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
    }
}