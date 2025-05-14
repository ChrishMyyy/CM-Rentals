using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CarRent
{
    public partial class frmVehicleGallery : Form
    {
        public string CarCodeToDisplay { get; set; }
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";

        public frmVehicleGallery(string carCode)
        {
            InitializeComponent();
            this.CarCodeToDisplay = carCode;
            btnAddImage.Click += btnAddImage_Click;
            btnAddImage.FlatStyle = FlatStyle.Flat;
            btnAddImage.FlatAppearance.BorderSize = 0;
            this.FormClosing += FrmVehicleGallery_FormClosing;
            LoadVehicleImages(CarCodeToDisplay);
        }

        private bool TestDatabaseConnection()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed: {ex.Message}", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LoadVehicleImages(string carCode)
        {
            flowLayoutVehicleImages.Controls.Clear();
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT ImageID, ImagePath, ImageLabel FROM tblVehicleImages WHERE CarCode = @CarCode";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarCode", carCode);
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            bool hasImages = false;
                            while (reader.Read())
                            {
                                hasImages = true;
                                int imageId = Convert.ToInt32(reader["ImageID"]);
                                string imagePath = reader["ImagePath"].ToString();
                                string imageLabel = reader["ImageLabel"].ToString();

                                if (File.Exists(imagePath))
                                {
                                    try
                                    {
                                        // Panel to hold everything
                                        Panel panel = new Panel();
                                        panel.Width = 250;  // Increased from 150
                                        panel.Height = 250; // Increased from 150
                                        panel.Margin = new Padding(10);
                                        panel.Tag = imageId;
                                        panel.BackColor = Color.White;
                                        panel.BorderStyle = BorderStyle.FixedSingle;

                                        // Delete Button
                                        Button btnDelete = new Button();
                                        btnDelete.Text = "🗑️";
                                        btnDelete.Width = 40;  // Increased from 30
                                        btnDelete.Height = 40; // Increased from 30
                                        btnDelete.Location = new Point(panel.Width - 45, 0);
                                        btnDelete.BackColor = Color.Red;
                                        btnDelete.ForeColor = Color.White;
                                        btnDelete.FlatStyle = FlatStyle.Flat;
                                        btnDelete.Tag = imageId;
                                        btnDelete.Click += (s, e) => DeleteImage((int)((Button)s).Tag, imagePath);

                                        // Edit Button
                                        Button btnEdit = new Button();
                                        btnEdit.Text = "✏️";
                                        btnEdit.Width = 40;  // Increased from 30
                                        btnEdit.Height = 40; // Increased from 30
                                        btnEdit.Location = new Point(panel.Width - 90, 0);
                                        btnEdit.BackColor = Color.Blue;
                                        btnEdit.ForeColor = Color.White;
                                        btnEdit.FlatStyle = FlatStyle.Flat;
                                        btnEdit.Tag = new Tuple<int, string, string>(imageId, imagePath, imageLabel);
                                        btnEdit.Click += (s, e) => EditImage((Tuple<int, string, string>)((Button)s).Tag);

                                        // PictureBox for the image
                                        PictureBox pb = new PictureBox();
                                        using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                                        {
                                            pb.Image = Image.FromStream(stream);
                                        }
                                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                                        pb.Width = 250;  // Increased from 150
                                        pb.Height = 200; // Increased from 100
                                        pb.Location = new Point(0, 40); // Adjusted for larger buttons
                                        pb.Tag = imagePath;
                                        pb.DoubleClick += (s, e) => ShowFullScreenImage(imagePath, imageLabel);

                                        // Label for the image
                                        Label lbl = new Label();
                                        lbl.Text = imageLabel;
                                        lbl.Dock = DockStyle.Bottom;
                                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                                        lbl.Height = 30; // Increased from 20
                                        lbl.Font = new Font(lbl.Font.FontFamily, 10); // Increased font size

                                        // Add buttons first, then PictureBox, then label
                                        panel.Controls.Add(btnDelete);
                                        panel.Controls.Add(btnEdit);
                                        panel.Controls.Add(pb);
                                        panel.Controls.Add(lbl);

                                        flowLayoutVehicleImages.Controls.Add(panel);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error loading image {imagePath}: {ex.Message}", "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }

                            if (!hasImages)
                            {
                                Label noImagesLabel = new Label();
                                noImagesLabel.Text = "No images found for this vehicle.";
                                noImagesLabel.AutoSize = true;
                                noImagesLabel.Font = new Font(noImagesLabel.Font.FontFamily, 12);
                                flowLayoutVehicleImages.Controls.Add(noImagesLabel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteImage(int imageId, string imagePath)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this image?\nThis action cannot be undone.",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        // Use direct SQL again for consistency
                        string query = "DELETE FROM tblVehicleImages WHERE ImageID = " + imageId;
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            conn.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                try
                                {
                                    // Ask if the user also wants to delete the file from disk
                                    DialogResult fileDeleteResult = MessageBox.Show(
                                        "Do you also want to delete the image file from disk?",
                                        "Delete File",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);

                                    if (fileDeleteResult == DialogResult.Yes && File.Exists(imagePath))
                                    {
                                        File.Delete(imagePath);
                                    }

                                    MessageBox.Show("Image was successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadVehicleImages(CarCodeToDisplay); // Refresh the gallery
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Database record deleted, but there was a problem deleting the file: {ex.Message}",
                                        "File Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    LoadVehicleImages(CarCodeToDisplay); // Still refresh the gallery
                                }
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the image from database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting image: {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EditImage(Tuple<int, string, string> imageDetails)
        {
            int imageId = imageDetails.Item1;
            string currentPath = imageDetails.Item2;
            string imageLabel = imageDetails.Item3;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Select a New Image"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string newImagePath = openFileDialog.FileName;

                // Ask if user wants to use the original path or the new path
                DialogResult pathChoice = MessageBox.Show(
                    "Do you want to:\n" +
                    "Yes - Copy the new image to the original location\n" +
                    "No - Update the database to point to the new image location",
                    "Image Path Options",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                string finalPath;
                if (pathChoice == DialogResult.Yes)
                {
                    // Copy the new image to the original location, overwriting the old one
                    try
                    {
                        File.Copy(newImagePath, currentPath, true);
                        finalPath = currentPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to copy the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    // Use the new image path
                    finalPath = newImagePath;
                }

                try
                {
                    // Update the database with the new path using direct SQL
                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        string query = "UPDATE tblVehicleImages SET ImagePath = '" + finalPath.Replace("'", "''") + "' WHERE ImageID = " + imageId;
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            conn.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Image was successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadVehicleImages(CarCodeToDisplay); // Refresh the gallery
                            }
                            else
                            {
                                MessageBox.Show("Failed to update the image in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating image: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void flowLayoutVehicleImages_Paint(object sender, PaintEventArgs e)
        {
            // Empty event handler
        }

        private void FrmVehicleGallery_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void frmVehicleGallery_Load_1(object sender, EventArgs e)
        {
            try
            {
                // Test database connection first
                TestDatabaseConnection();

                if (!string.IsNullOrEmpty(CarCodeToDisplay))
                {
                    LoadVehicleImages(CarCodeToDisplay);
                }
                else
                {
                    MessageBox.Show("No Car Code was provided.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CarCodeToDisplay))
            {
                MessageBox.Show("No Car Code is set for this gallery.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Select an Image to Add"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                string defaultName = Path.GetFileNameWithoutExtension(selectedImagePath);
                
                // Prompt user for image name
                string imageLabel = Microsoft.VisualBasic.Interaction.InputBox(
                    "Enter a name for this image:",
                    "Image Name",
                    defaultName);

                // If user cancels the input box, return
                if (string.IsNullOrEmpty(imageLabel))
                {
                    return;
                }

                // Optionally, copy the image to a project folder
                string destDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VehicleImages");
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);
                string destPath = Path.Combine(destDir, Path.GetFileName(selectedImagePath));
                try
                {
                    File.Copy(selectedImagePath, destPath, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to copy image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Insert into database
                try
                {
                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        string query = "INSERT INTO tblVehicleImages (CarCode, ImagePath, ImageLabel) VALUES (?, ?, ?)";
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CarCode", CarCodeToDisplay);
                            cmd.Parameters.AddWithValue("@ImagePath", destPath);
                            cmd.Parameters.AddWithValue("@ImageLabel", imageLabel);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Image added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVehicleImages(CarCodeToDisplay);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add image to database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowFullScreenImage(string imagePath, string imageLabel)
        {
            Form fullScreenForm = new Form();
            fullScreenForm.WindowState = FormWindowState.Maximized;
            fullScreenForm.FormBorderStyle = FormBorderStyle.None;
            fullScreenForm.BackColor = Color.Black;

            PictureBox fullScreenPictureBox = new PictureBox();
            fullScreenPictureBox.Dock = DockStyle.Fill;
            fullScreenPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            fullScreenPictureBox.Image = Image.FromFile(imagePath);

            Label titleLabel = new Label();
            titleLabel.Text = imageLabel;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 40;
            titleLabel.BackColor = Color.Black;
            titleLabel.ForeColor = Color.White;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Font = new Font(titleLabel.Font.FontFamily, 14, FontStyle.Bold);

            Button closeButton = new Button();
            closeButton.Text = "✕";
            closeButton.Size = new Size(40, 40);
            closeButton.Location = new Point(fullScreenForm.Width - 50, 0);
            closeButton.BackColor = Color.Red;
            closeButton.ForeColor = Color.White;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => fullScreenForm.Close();

            fullScreenForm.Controls.Add(fullScreenPictureBox);
            fullScreenForm.Controls.Add(titleLabel);
            fullScreenForm.Controls.Add(closeButton);

            fullScreenForm.KeyPreview = true;
            fullScreenForm.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) fullScreenForm.Close(); };

            fullScreenForm.Show();
        }
    }
}