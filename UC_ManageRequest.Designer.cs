namespace CarRent
{
    partial class UC_ManageRequest
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
            label29 = new Label();
            label28 = new Label();
            pnlRequest = new Guna.UI2.WinForms.Guna2GradientPanel();
            SuspendLayout();
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Font = new Font("ROG Fonts", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label29.ForeColor = Color.Firebrick;
            label29.Location = new Point(288, 0);
            label29.Name = "label29";
            label29.RightToLeft = RightToLeft.No;
            label29.Size = new Size(347, 60);
            label29.TabIndex = 57;
            label29.Text = "request";
            label29.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Font = new Font("ROG Fonts", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label28.ForeColor = Color.WhiteSmoke;
            label28.Location = new Point(0, 0);
            label28.Name = "label28";
            label28.RightToLeft = RightToLeft.No;
            label28.Size = new Size(297, 60);
            label28.TabIndex = 56;
            label28.Text = "manage";
            label28.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlRequest
            // 
            pnlRequest.CustomizableEdges = customizableEdges1;
            pnlRequest.Dock = DockStyle.Bottom;
            pnlRequest.Location = new Point(0, 95);
            pnlRequest.Name = "pnlRequest";
            pnlRequest.ShadowDecoration.CustomizableEdges = customizableEdges2;
            pnlRequest.Size = new Size(1206, 552);
            pnlRequest.TabIndex = 58;
            // 
            // UC_ManageRequest
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(label28);
            Controls.Add(label29);
            Controls.Add(pnlRequest);
            Name = "UC_ManageRequest";
            Size = new Size(1206, 647);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label29;
        private Label label28;
        private Guna.UI2.WinForms.Guna2GradientPanel pnlRequest;
    }
}
