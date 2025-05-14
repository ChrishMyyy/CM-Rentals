using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.Threading.Tasks;

namespace CarRent
{
    public partial class UC_Info : UserControl
    {
        private System.Windows.Forms.Timer typewriterTimer;
        private string fullText;
        private int currentIndex;
        private Guna2HtmlLabel lblInfo;

        public UC_Info()
        {
            InitializeComponent();
            InitializeCustomComponents();
            StartTypewriterAnimation();
        }

        private void InitializeCustomComponents()
        {
            // Remove the profile picture box (pbxProfile)
            // Only create and configure the info label
            lblInfo = new Guna2HtmlLabel();
            lblInfo.Size = new Size(600, 150);
            lblInfo.Location = new Point(40, 100); // Align to the left side
            lblInfo.Font = new Font("ROG Fonts", 40F, FontStyle.Bold | FontStyle.Italic);
            lblInfo.ForeColor = Color.White;
            lblInfo.Text = "";
            lblInfo.TextAlignment = ContentAlignment.TopLeft;
            this.Controls.Add(lblInfo);

            // Initialize typewriter timer
            typewriterTimer = new System.Windows.Forms.Timer();
            typewriterTimer.Interval = 30; // Speed of typing
            typewriterTimer.Tick += TypewriterTimer_Tick;
        }

        private void StartTypewriterAnimation()
        {
            // Use HTML for color and line break
            fullText = "<span>Welcome to</span><br>" +
                       "<span style='color:#B94141; font-weight:bold;'>C</span>hris " +
                       "<span style='color:#B94141; font-weight:bold;'>M</span>amporte<br>" +
                       "<span style='color:#B94141; font-weight:bold;'>Rentals</span>!";
            currentIndex = 0;
            lblInfo.Text = "";
            typewriterTimer.Start();
        }

        private void TypewriterTimer_Tick(object sender, EventArgs e)
        {
            if (currentIndex < fullText.Length)
            {
                lblInfo.Text += fullText[currentIndex];
                currentIndex++;
            }
            else
            {
                typewriterTimer.Stop();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Add any custom painting here if needed
        }
    }
}
