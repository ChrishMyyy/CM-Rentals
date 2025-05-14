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
using System.IO;

namespace CarRent
{
    public partial class frmCustomerRentalHistory : Form
    {
        private readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private string customerCode;
        public frmCustomerRentalHistory(string code, string name, string licenseImagePath)
        {
            InitializeComponent();
            customerCode = code;
            txtCustomerName.Text = name;
            lblCRHID.Text = code;

            if (!string.IsNullOrEmpty(licenseImagePath) && File.Exists(licenseImagePath))
                pbxCRH.Image = Image.FromFile(licenseImagePath);
            else
                pbxCRH.Image = null;

            LoadRentalHistory();
        }

        private void btnCustomerCloseHistory_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnCustomerFilterHistory_Click(object sender, EventArgs e)
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
                                 WHERE CustomerCode = ?";
                if (start.HasValue && end.HasValue)
                    query += " AND StartDate >= ? AND EndDate <= ?";
                query += " ORDER BY StartDate DESC";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", customerCode);
                    if (start.HasValue && end.HasValue)
                    {
                        cmd.Parameters.AddWithValue("?", start.Value.Date);
                        cmd.Parameters.AddWithValue("?", end.Value.Date.AddDays(1).AddSeconds(-1));
                    }
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCustomerRentalHistory.DataSource = dt;
                }
            }
            StyleRentalHistoryGrid();
        }

        private void StyleRentalHistoryGrid()
        {
            var dgv = dgvCustomerRentalHistory;
            if (dgv.Columns.Count == 0) return;

            dgv.Columns["RentalCode"].HeaderText = "Rental Code";
            dgv.Columns["StartDate"].HeaderText = "Start Date";
            dgv.Columns["EndDate"].HeaderText = "End Date";
            dgv.Columns["TotalAmount"].HeaderText = "Amount";
            dgv.Columns["RentalStatus"].HeaderText = "Status";

            dgv.Columns["StartDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgv.Columns["EndDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Optional: Apply alternating row colors, header style, etc. to match your design
        }
    }
}
