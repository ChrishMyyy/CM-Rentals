using System.Data.OleDb;

namespace CarRent
{
    partial class frmEditCustomers
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
            components = new System.ComponentModel.Container();
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtFullName = new Guna.UI2.WinForms.Guna2TextBox();
            txtContactNumber = new Guna.UI2.WinForms.Guna2TextBox();
            txtAddress = new Guna.UI2.WinForms.Guna2TextBox();
            cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            pbxProfile = new Guna.UI2.WinForms.Guna2PictureBox();
            btnUploadImage = new Guna.UI2.WinForms.Guna2Button();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(components);
            guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(components);
            ((System.ComponentModel.ISupportInitialize)pbxProfile).BeginInit();
            SuspendLayout();
            // 
            // txtFullName
            // 
            txtFullName.Cursor = Cursors.IBeam;
            txtFullName.CustomizableEdges = customizableEdges1;
            txtFullName.DefaultText = "";
            txtFullName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtFullName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtFullName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtFullName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtFullName.FocusedState.BorderColor = Color.FromArgb(189, 65, 65);
            txtFullName.Font = new Font("Segoe UI", 9F);
            txtFullName.HoverState.BorderColor = Color.FromArgb(189, 65, 65);
            txtFullName.Location = new Point(160, 31);
            txtFullName.Margin = new Padding(4, 6, 4, 6);
            txtFullName.Name = "txtFullName";
            txtFullName.PlaceholderText = "";
            txtFullName.SelectedText = "";
            txtFullName.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtFullName.Size = new Size(267, 46);
            txtFullName.TabIndex = 0;
            // 
            // txtContactNumber
            // 
            txtContactNumber.Cursor = Cursors.IBeam;
            txtContactNumber.CustomizableEdges = customizableEdges3;
            txtContactNumber.DefaultText = "";
            txtContactNumber.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtContactNumber.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtContactNumber.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtContactNumber.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtContactNumber.FocusedState.BorderColor = Color.FromArgb(189, 65, 65);
            txtContactNumber.Font = new Font("Segoe UI", 9F);
            txtContactNumber.HoverState.BorderColor = Color.FromArgb(189, 65, 65);
            txtContactNumber.Location = new Point(160, 92);
            txtContactNumber.Margin = new Padding(4, 6, 4, 6);
            txtContactNumber.Name = "txtContactNumber";
            txtContactNumber.PlaceholderText = "";
            txtContactNumber.SelectedText = "";
            txtContactNumber.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtContactNumber.Size = new Size(267, 46);
            txtContactNumber.TabIndex = 1;
            // 
            // txtAddress
            // 
            txtAddress.Cursor = Cursors.IBeam;
            txtAddress.CustomizableEdges = customizableEdges5;
            txtAddress.DefaultText = "";
            txtAddress.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtAddress.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtAddress.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtAddress.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtAddress.FocusedState.BorderColor = Color.FromArgb(189, 65, 65);
            txtAddress.Font = new Font("Segoe UI", 9F);
            txtAddress.HoverState.BorderColor = Color.FromArgb(189, 65, 65);
            txtAddress.Location = new Point(160, 154);
            txtAddress.Margin = new Padding(4, 6, 4, 6);
            txtAddress.Name = "txtAddress";
            txtAddress.PlaceholderText = "";
            txtAddress.SelectedText = "";
            txtAddress.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtAddress.Size = new Size(267, 46);
            txtAddress.TabIndex = 2;
            // 
            // cmbStatus
            // 
            cmbStatus.BackColor = Color.Transparent;
            cmbStatus.CustomizableEdges = customizableEdges7;
            cmbStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.FocusedColor = Color.FromArgb(189, 65, 65);
            cmbStatus.FocusedState.BorderColor = Color.FromArgb(189, 65, 65);
            cmbStatus.Font = new Font("Segoe UI", 10F);
            cmbStatus.ForeColor = Color.FromArgb(68, 88, 112);
            cmbStatus.ItemHeight = 30;
            cmbStatus.Items.AddRange(new object[] { "Active", "Inactive" });
            cmbStatus.Location = new Point(160, 215);
            cmbStatus.Margin = new Padding(4, 5, 4, 5);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.ShadowDecoration.CustomizableEdges = customizableEdges8;
            cmbStatus.Size = new Size(265, 36);
            cmbStatus.TabIndex = 3;
            // 
            // pbxProfile
            // 
            pbxProfile.BorderStyle = BorderStyle.FixedSingle;
            pbxProfile.CustomizableEdges = customizableEdges9;
            pbxProfile.ImageRotate = 0F;
            pbxProfile.Location = new Point(467, 62);
            pbxProfile.Margin = new Padding(4, 5, 4, 5);
            pbxProfile.Name = "pbxProfile";
            pbxProfile.ShadowDecoration.CustomizableEdges = customizableEdges10;
            pbxProfile.Size = new Size(133, 120);
            pbxProfile.SizeMode = PictureBoxSizeMode.Zoom;
            pbxProfile.TabIndex = 5;
            pbxProfile.TabStop = false;
            // 
            // btnUploadImage
            // 
            btnUploadImage.BorderRadius = 5;
            btnUploadImage.CustomizableEdges = customizableEdges11;
            btnUploadImage.DisabledState.BorderColor = Color.DarkGray;
            btnUploadImage.DisabledState.CustomBorderColor = Color.DarkGray;
            btnUploadImage.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnUploadImage.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnUploadImage.FillColor = Color.FromArgb(189, 65, 65);
            btnUploadImage.Font = new Font("Segoe UI", 9F);
            btnUploadImage.ForeColor = Color.White;
            btnUploadImage.Location = new Point(467, 200);
            btnUploadImage.Margin = new Padding(4, 5, 4, 5);
            btnUploadImage.Name = "btnUploadImage";
            btnUploadImage.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnUploadImage.Size = new Size(133, 46);
            btnUploadImage.TabIndex = 6;
            btnUploadImage.Text = "Upload Image";
            btnUploadImage.Click += btnUploadImage_Click;
            // 
            // btnSave
            // 
            btnSave.BorderRadius = 5;
            btnSave.CustomizableEdges = customizableEdges13;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.FillColor = Color.FromArgb(189, 65, 65);
            btnSave.Font = new Font("Segoe UI", 9F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(160, 277);
            btnSave.Margin = new Padding(4, 5, 4, 5);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnSave.Size = new Size(133, 46);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 5;
            btnCancel.CustomizableEdges = customizableEdges15;
            btnCancel.DisabledState.BorderColor = Color.DarkGray;
            btnCancel.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancel.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancel.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancel.FillColor = Color.FromArgb(189, 65, 65);
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(307, 277);
            btnCancel.Margin = new Padding(4, 5, 4, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnCancel.Size = new Size(120, 46);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.Location = new Point(27, 38);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(76, 20);
            label1.TabIndex = 9;
            label1.Text = "Full Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.Location = new Point(27, 100);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(118, 20);
            label2.TabIndex = 10;
            label2.Text = "Contact Number";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F);
            label3.Location = new Point(27, 162);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(62, 20);
            label3.TabIndex = 11;
            label3.Text = "Address";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F);
            label4.Location = new Point(27, 223);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 12;
            label4.Text = "Status";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F);
            label5.Location = new Point(467, 31);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(48, 20);
            label5.TabIndex = 13;
            label5.Text = "Photo";
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
            // frmEditCustomers
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(645, 350);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnUploadImage);
            Controls.Add(pbxProfile);
            Controls.Add(cmbStatus);
            Controls.Add(txtAddress);
            Controls.Add(txtContactNumber);
            Controls.Add(txtFullName);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmEditCustomers";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Customer";
            ((System.ComponentModel.ISupportInitialize)pbxProfile).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Guna.UI2.WinForms.Guna2TextBox txtFullName;
        private Guna.UI2.WinForms.Guna2TextBox txtContactNumber;
        private Guna.UI2.WinForms.Guna2TextBox txtAddress;
        private Guna.UI2.WinForms.Guna2ComboBox cmbStatus;
        private Guna.UI2.WinForms.Guna2PictureBox pbxProfile;
        private Guna.UI2.WinForms.Guna2Button btnUploadImage;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            // TODO: Implement image upload logic
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tblCustomers SET FullName = ?, ContactNumber = ?, Address = ?, Status = ?, LicenseImage = ? WHERE CustomerCode = ?";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        command.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text.Trim());
                        command.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        command.Parameters.AddWithValue("@Status", cmbStatus.Text.Trim());
                        command.Parameters.AddWithValue("@LicenseImage", currentImagePath ?? "");
                        command.Parameters.AddWithValue("@CustomerCode", customerCode);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving customer: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
    }
}
#endregion