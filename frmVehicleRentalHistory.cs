using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CarRent
{
    public partial class frmVehicleRentalHistory : Form
    {
        private string carCode;
        private string model;
        private string plateNumber;
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";

        public frmVehicleRentalHistory(string carCode, string model, string plateNumber)
        {
            InitializeComponent();
            this.carCode = carCode;
            this.model = model;
            this.plateNumber = plateNumber;

            txtVehicleInfo.Text = $"{model}";
            lblVLP.Text = plateNumber;

            LoadRentalHistory();
            btnFilter.Click += BtnFilter_Click;
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            LoadRentalHistory(dtpStart.Value, dtpEnd.Value);
        }

        private void LoadRentalHistory(DateTime? start = null, DateTime? end = null)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT RentalCode, StartDate, EndDate, TotalAmount, RentalStatus
                                 FROM tblRentals
                                 WHERE CarCode = ?";
                if (start.HasValue && end.HasValue)
                    query += " AND StartDate >= ? AND EndDate <= ?";
                query += " ORDER BY StartDate DESC";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", carCode);
                    if (start.HasValue && end.HasValue)
                    {
                        cmd.Parameters.AddWithValue("?", start.Value.Date);
                        cmd.Parameters.AddWithValue("?", end.Value.Date.AddDays(1).AddSeconds(-1));
                    }
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvVehicleRentalHistory.DataSource = dt;
                }
            }
            StyleRentalHistoryGrid();
        }

        private void StyleRentalHistoryGrid()
        {
            var dgv = dgvVehicleRentalHistory;
            if (dgv.Columns.Count == 0) return;

            // Set header text
            dgv.Columns["RentalCode"].HeaderText = "Rental Code";
            dgv.Columns["StartDate"].HeaderText = "Start Date";
            dgv.Columns["EndDate"].HeaderText = "End Date";
            dgv.Columns["TotalAmount"].HeaderText = "Amount";
            dgv.Columns["RentalStatus"].HeaderText = "Status";

            // Format date columns
            dgv.Columns["StartDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgv.Columns["EndDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgv.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";

            // Header style
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Firebrick;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Alternating row style
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightCoral;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // Grid style
            dgv.GridColor = Color.LightGray;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnVehicleCloseRental_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblVLP_Click(object sender, EventArgs e)
        {

        }
    }
}
