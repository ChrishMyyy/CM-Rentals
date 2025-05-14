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
using System.Runtime.InteropServices;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CarRent
{
    public partial class frmOnlinePayment : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";
        private string customerCode;
        private decimal dueAmount;
        private int loyaltyPoints;
        private decimal discountAmount;
        private decimal totalAmount;
        private string userCode;
        private string carCode = "";
        private DateTime startDate;
        private DateTime endDate;
        private int daysRented;
        private string originalRentalCode;

        public frmOnlinePayment(string customerCode, decimal dueAmount, string userCode, string carCode, DateTime startDate, DateTime endDate, int daysRented, string originalRentalCode)
        {
            InitializeComponent();
            this.customerCode = customerCode;
            this.dueAmount = dueAmount;
            this.userCode = userCode;
            this.carCode = carCode;
            this.startDate = startDate;
            this.endDate = endDate;
            this.daysRented = daysRented;
            this.originalRentalCode = originalRentalCode;
            LoadCustomerLoyaltyPoints();
            CalculateDiscount();
            UpdateLabels();
        }

        private void LoadCustomerLoyaltyPoints()
        {
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT LoyaltyPoints FROM tblCustomers WHERE CustomerCode = ?";
                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", customerCode);
                        var result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int points))
                        {
                            loyaltyPoints = points;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loyalty points: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateDiscount()
        {
            try
            {
                // If late return, no discount
                if (!string.IsNullOrEmpty(originalRentalCode))
                {
                    discountAmount = 0;
                }
                else
                {
                    discountAmount = loyaltyPoints * 5;
                }
                totalAmount = dueAmount - discountAmount;
                if (totalAmount < 0)
                {
                    totalAmount = 0;
                    discountAmount = dueAmount;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating discount: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateLabels()
        {
            try
            {
                if (lblOnlineDueAmount != null)
                    lblOnlineDueAmount.Text = $"PHP {dueAmount:N2}";
                if (lblOnlineLP != null)
                    lblOnlineLP.Text = $"PHP {discountAmount:N2}";
                if (lblOnlineTA != null)
                    lblOnlineTA.Text = $"PHP {totalAmount:N2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pbxGoBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOlConfirmPayment_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(
                "Are you sure you want to confirm this payment?",
                "Confirm Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

                string rentalCode = GenerateRentalCode();
                string referenceNumber = GenerateUniqueTransactionCode();
                string processedBy = Session.CurrentUserCode;
                DateTime dateCreated = DateTime.Now;
            string rentalStatus = string.IsNullOrEmpty(originalRentalCode) ? "Active" : "Returned";
            string customerStatus = string.IsNullOrEmpty(originalRentalCode) ? "Ongoing" : "Returned";
            decimal totalDiscount = 0;
            if (string.IsNullOrEmpty(originalRentalCode))
                totalDiscount = loyaltyPoints * 5;
            // else, late return: totalDiscount stays 0

            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insert new rental record
                            string insertRentalQuery = @"
                                INSERT INTO tblRentals 
                                (RentalCode, ReferenceNumber, CustomerCode, CarCode, 
                                 StartDate, EndDate, ReturnDate, DaysRented, 
                                 TotalAmount, RentalStatus, ProcessedBy, DateCreated, TotalDiscount) 
                                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                            using (var cmd = new OleDbCommand(insertRentalQuery, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("?", rentalCode);
                                cmd.Parameters.AddWithValue("?", referenceNumber);
                                cmd.Parameters.AddWithValue("?", customerCode);
                                cmd.Parameters.AddWithValue("?", carCode);
                                cmd.Parameters.AddWithValue("?", startDate.ToString("M/d/yyyy h:mm:ss tt"));
                                cmd.Parameters.AddWithValue("?", endDate.ToString("M/d/yyyy h:mm:ss tt"));
                                cmd.Parameters.AddWithValue("?", endDate.ToString("M/d/yyyy h:mm:ss tt"));
                                cmd.Parameters.AddWithValue("?", daysRented);
                                cmd.Parameters.AddWithValue("?", (double)totalAmount);
                                cmd.Parameters.AddWithValue("?", rentalStatus);
                                cmd.Parameters.AddWithValue("?", processedBy);
                                cmd.Parameters.AddWithValue("?", dateCreated.ToString("M/d/yyyy h:mm:ss tt"));
                                cmd.Parameters.AddWithValue("?", (double)totalDiscount);

                                int rentalRowsAffected = cmd.ExecuteNonQuery();
                                if (rentalRowsAffected == 0)
                                {
                                    throw new Exception("Failed to insert rental record.");
                                }
                            }

                            // Update customer status
                            string customerQuery = @"
                                UPDATE tblCustomers 
                                SET AccumulatedAmount = AccumulatedAmount + ?,
                                    CustomerStatus = ?
                                WHERE CustomerCode = ?";
                            using (var cmd = new OleDbCommand(customerQuery, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("?", dueAmount);
                                cmd.Parameters.AddWithValue("?", customerStatus);
                                cmd.Parameters.AddWithValue("?", customerCode);
                                int customerRowsAffected = cmd.ExecuteNonQuery();
                                if (customerRowsAffected == 0)
                                {
                                    throw new Exception("Failed to update customer record.");
                                }
                            }

                            // Update vehicle status
                            string updateVehicleQuery = @"
                                UPDATE tblVehicles 
                                SET VehicleStatus = 'Rented',
                                    Availability = 'Not Available'
                                WHERE CarCode = ?";
                            using (var cmd = new OleDbCommand(updateVehicleQuery, conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("?", carCode);
                                int vehicleRowsAffected = cmd.ExecuteNonQuery();
                                if (vehicleRowsAffected == 0)
                                {
                                    throw new Exception("Failed to update vehicle status.");
                                }
                            }

                            if (!string.IsNullOrEmpty(originalRentalCode))
                            {
                                // Update the original rental record to mark it as returned
                                string updateOriginalRentalQuery = @"
                                    UPDATE tblRentals 
                                    SET RentalStatus = 'Returned',
                                        ReturnDate = ?
                                    WHERE RentalCode = ?";
                                using (var cmd = new OleDbCommand(updateOriginalRentalQuery, conn))
                                {
                                    cmd.Transaction = transaction;
                                    cmd.Parameters.AddWithValue("?", endDate.ToString("M/d/yyyy h:mm:ss tt"));
                                    cmd.Parameters.AddWithValue("?", originalRentalCode);
                                    int rentalRowsAffected = cmd.ExecuteNonQuery();
                                    if (rentalRowsAffected == 0)
                                    {
                                        throw new Exception("Failed to update original rental record.");
                                    }
                                }
                            }

                            // Commit the transaction if all updates succeeded
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction if any update fails
                            transaction.Rollback();
                            throw new Exception($"Transaction failed: {ex.Message}");
                        }
                    }
                }

                // --- PDF RECEIPT GENERATION (after transaction/connection are closed) ---
                MessageBox.Show("Payment processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string receiptDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CMRentalsReceipts");
                if (!Directory.Exists(receiptDir)) Directory.CreateDirectory(receiptDir);
                string filePath = Path.Combine(receiptDir, $"Receipt_{referenceNumber}.pdf");

                // Calculate the discount for the receipt
                decimal receiptDiscount = 0;
                if (string.IsNullOrEmpty(originalRentalCode))
                    receiptDiscount = loyaltyPoints * 5;
                // else, late return: receiptDiscount stays 0

                string receiptCustomerName = GetCustomerName(customerCode);
                GenerateReceiptPdf(
                    filePath,
                    "CM RENTALS",
                    receiptCustomerName,
                    customerCode,
                    GetVehicleName(carCode),
                    GetVehiclePlate(carCode),
                    startDate,
                    endDate,
                    daysRented,
                    rentalStatus,
                    dueAmount,
                    receiptDiscount,
                    totalAmount,
                    "Online",
                    referenceNumber,
                    processedBy,
                    dateCreated
                );
                // Optionally open the PDF
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                // Now close the form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateRentalCode()
        {
            string prefix = "RENT";
            int nextNumber = 1;
            string newCode = "";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    while (true)
                    {
                        newCode = $"{prefix}{nextNumber:D4}";
                        string query = "SELECT COUNT(*) FROM tblRentals WHERE RentalCode = ?";
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("?", newCode);
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            if (count == 0)
                                break;
                        }
                        nextNumber++;
                    }
                }
            }
            catch
            {
                newCode = $"{prefix}{DateTime.Now.Ticks % 10000:D4}"; // fallback
            }
            return newCode;
        }

        private string GenerateUniqueTransactionCode()
        {
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string prefix = "TXN";
            string code;
            Random rnd = new Random();

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                while (true)
                {
                    int randomNum = rnd.Next(0, 10000); // 0 to 9999
                    string randomPart = randomNum.ToString("D4");
                    code = $"{prefix}-{datePart}-{randomPart}";

                    // Check uniqueness in the database
                    string query = "SELECT COUNT(*) FROM tblRentals WHERE ReferenceNumber = ?";
                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", code);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            // Unique code found
                            break;
                        }
                    }
                }
            }
            return code;
        }

        public void SetPaymentDetails(decimal dueAmount, decimal loyaltyPoints, decimal totalAmount, int daysRented, string processedBy)
        {
            decimal discountAmount = 0;
            decimal finalTotal = dueAmount;
            if (string.IsNullOrEmpty(originalRentalCode))
            {
                discountAmount = loyaltyPoints * 5;
                finalTotal = dueAmount - discountAmount;
                if (finalTotal < 0) finalTotal = 0;
            }
            // else, late return: discountAmount stays 0, finalTotal = dueAmount

            if (lblOnlineDueAmount != null)
            lblOnlineDueAmount.Text = $"PHP {dueAmount:N2}";
            if (lblOnlineLP != null)
                lblOnlineLP.Text = $"PHP {discountAmount:N2}";
            if (lblOnlineTA != null)
                lblOnlineTA.Text = $"PHP {finalTotal:N2}";
        }

        private string GetCustomerName(string code)
        {
            string name = code;
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT FullName FROM tblCustomers WHERE CustomerCode = ?";
                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", code);
                        var result = cmd.ExecuteScalar();
                        if (result != null) name = result.ToString();
                    }
                }
            }
            catch { }
            return name;
        }

        private string GetVehicleName(string code)
        {
            string name = code;
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Make, Model FROM tblVehicles WHERE CarCode = ?";
                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", code);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                name = reader["Make"] + " " + reader["Model"];
                        }
                    }
                }
            }
            catch { }
            return name;
        }

        private string GetVehiclePlate(string code)
        {
            string plate = code;
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT PlateNumber FROM tblVehicles WHERE CarCode = ?";
                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", code);
                        var result = cmd.ExecuteScalar();
                        if (result != null) plate = result.ToString();
                    }
                }
            }
            catch { }
            return plate;
        }

        private decimal GetLoyaltyPointsDiscount(string customerCode)
        {
            decimal points = 0;
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT LoyaltyPoints FROM tblCustomers WHERE CustomerCode = ?";
                    using (var cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", customerCode);
                        var result = cmd.ExecuteScalar();
                        if (result != null && decimal.TryParse(result.ToString(), out decimal pts))
                        {
                            points = pts;
                        }
                    }
                }
            }
            catch { }
            return points * 5;
        }

        private void GenerateReceiptPdf(
            string filePath,
            string companyName,
            string customerName,
            string customerCode,
            string vehicle,
            string plate,
            DateTime startDate,
            DateTime endDate,
            int daysRented,
            string rentalStatus,
            decimal amountDue,
            decimal discount,
            decimal totalAmount,
            string paymentMethod,
            string referenceNumber,
            string processedBy,
            DateTime dateCreated
        )
        {
            // Firebrick theme colors
            iTextSharp.text.BaseColor primaryColor = new iTextSharp.text.BaseColor(178, 34, 34);    // Firebrick
            iTextSharp.text.BaseColor accentColor = new iTextSharp.text.BaseColor(205, 92, 92);     // Indian Red (lighter firebrick)
            iTextSharp.text.BaseColor highlightColor = new iTextSharp.text.BaseColor(220, 20, 60);  // Crimson (brighter red)
            iTextSharp.text.BaseColor textColor = new iTextSharp.text.BaseColor(43, 43, 43);        // Dark gray for text
            iTextSharp.text.BaseColor lightGray = new iTextSharp.text.BaseColor(253, 245, 245);     // Very soft pink/red for alternating rows
            iTextSharp.text.BaseColor white = iTextSharp.text.BaseColor.WHITE;
            iTextSharp.text.BaseColor divider = new iTextSharp.text.BaseColor(222, 184, 184);       // Soft pink/red for dividers

            iTextSharp.text.Rectangle receiptSize = new iTextSharp.text.Rectangle(226.8f, 700f);
            Document doc = new Document(receiptSize, 10, 10, 10, 10);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            float headerFontSize = 12f;
            float subtitleFontSize = 9f;
            float normalFontSize = 7f;
            float smallFontSize = 6f;
            float cellPadding = 3f;

            Paragraph header = new Paragraph();
            header.Alignment = Element.ALIGN_CENTER;
            Chunk companyNameChunk = new Chunk(companyName, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, headerFontSize, primaryColor));
            header.Add(companyNameChunk);
            header.Add(Chunk.NEWLINE);
            Chunk receiptTypeChunk = new Chunk("RENTAL RECEIPT", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, subtitleFontSize, accentColor));
            header.Add(receiptTypeChunk);
            header.Add(Chunk.NEWLINE);
            header.Add(new Chunk("Date: " + dateCreated.ToString("MM/dd/yyyy HH:mm"), FontFactory.GetFont(FontFactory.HELVETICA, normalFontSize)));
            header.Add(Chunk.NEWLINE);
            header.Add(new Chunk("Reference: " + referenceNumber, FontFactory.GetFont(FontFactory.HELVETICA, normalFontSize)));
            header.Add(Chunk.NEWLINE);
            header.Add(new Chunk("Processed by: " + processedBy, FontFactory.GetFont(FontFactory.HELVETICA, normalFontSize)));
            header.SpacingAfter = 5f;
            doc.Add(header);

            PdfPTable dividerLine = new PdfPTable(1);
            dividerLine.WidthPercentage = 100;
            PdfPCell dividerCell = new PdfPCell(new Phrase(" "));
            dividerCell.BorderColor = divider;
            dividerCell.BorderWidthBottom = 1f;
            dividerCell.BorderWidthTop = 0;
            dividerCell.BorderWidthLeft = 0;
            dividerCell.BorderWidthRight = 0;
            dividerCell.FixedHeight = 1f;
            dividerCell.Padding = 0;
            dividerLine.AddCell(dividerCell);
            doc.Add(dividerLine);

            PdfPTable infoTable = new PdfPTable(2);
            infoTable.WidthPercentage = 100;
            infoTable.SpacingBefore = 5f;
            infoTable.SpacingAfter = 5f;
            infoTable.SetWidths(new float[] { 50, 50 });
            PdfPCell customerHeaderCell = new PdfPCell(new Phrase("CUSTOMER", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, normalFontSize, accentColor)));
            customerHeaderCell.Border = PdfPCell.NO_BORDER;
            customerHeaderCell.Padding = cellPadding;
            infoTable.AddCell(customerHeaderCell);
            PdfPCell vehicleHeaderCell = new PdfPCell(new Phrase("VEHICLE", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, normalFontSize, accentColor)));
            vehicleHeaderCell.Border = PdfPCell.NO_BORDER;
            vehicleHeaderCell.Padding = cellPadding;
            infoTable.AddCell(vehicleHeaderCell);
            PdfPCell customerDetailsCell = new PdfPCell();
            customerDetailsCell.Border = PdfPCell.NO_BORDER;
            customerDetailsCell.Padding = cellPadding;
            Paragraph customerDetails = new Paragraph();
            customerDetails.Add(new Chunk("Name: " + customerName, FontFactory.GetFont(FontFactory.HELVETICA, normalFontSize)));
            customerDetails.Add(Chunk.NEWLINE);
            customerDetails.Add(new Chunk("Code: " + customerCode, FontFactory.GetFont(FontFactory.HELVETICA, normalFontSize)));
            customerDetailsCell.AddElement(customerDetails);
            infoTable.AddCell(customerDetailsCell);
            PdfPCell vehicleDetailsCell = new PdfPCell();
            vehicleDetailsCell.Border = PdfPCell.NO_BORDER;
            vehicleDetailsCell.Padding = cellPadding;
            Paragraph vehicleDetails = new Paragraph();
            vehicleDetails.Add(new Chunk("Model: " + vehicle, FontFactory.GetFont(FontFactory.HELVETICA, normalFontSize)));
            vehicleDetails.Add(Chunk.NEWLINE);
            vehicleDetails.Add(new Chunk("Plate: " + plate, FontFactory.GetFont(FontFactory.HELVETICA, normalFontSize)));
            vehicleDetailsCell.AddElement(vehicleDetails);
            infoTable.AddCell(vehicleDetailsCell);
            doc.Add(infoTable);

            PdfPTable rentalTable = new PdfPTable(2);
            rentalTable.WidthPercentage = 100;
            rentalTable.SpacingBefore = 5f;
            rentalTable.SpacingAfter = 5f;
            rentalTable.SetWidths(new float[] { 50, 50 });
            PdfPCell headerCell = new PdfPCell(new Phrase("RENTAL DETAILS", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, normalFontSize, white)));
            headerCell.BackgroundColor = primaryColor;
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.Padding = cellPadding;
            headerCell.Colspan = 2;
            rentalTable.AddCell(headerCell);
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font labelFont = new iTextSharp.text.Font(baseFont, normalFontSize, iTextSharp.text.Font.NORMAL, textColor);
            iTextSharp.text.Font valueFont = new iTextSharp.text.Font(baseFont, normalFontSize, iTextSharp.text.Font.BOLD, textColor);
            void AddTableRow(string label, string value, bool alternateColor)
            {
                PdfPCell labelCell = new PdfPCell(new Phrase(label, labelFont));
                labelCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                labelCell.BorderColor = divider;
                labelCell.PaddingTop = cellPadding;
                labelCell.PaddingBottom = cellPadding;
                labelCell.BackgroundColor = alternateColor ? lightGray : white;
                PdfPCell valueCell = new PdfPCell(new Phrase(value, valueFont));
                valueCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                valueCell.BorderColor = divider;
                valueCell.PaddingTop = cellPadding;
                valueCell.PaddingBottom = cellPadding;
                valueCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                valueCell.BackgroundColor = alternateColor ? lightGray : white;
                rentalTable.AddCell(labelCell);
                rentalTable.AddCell(valueCell);
            }
            AddTableRow("Start Date", startDate.ToString("MM/dd/yyyy"), false);
            AddTableRow("End Date", endDate.ToString("MM/dd/yyyy"), true);
            AddTableRow("Days Rented", daysRented.ToString(), false);
            PdfPCell statusLabelCell = new PdfPCell(new Phrase("Status", labelFont));
            statusLabelCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            statusLabelCell.BorderColor = divider;
            statusLabelCell.PaddingTop = cellPadding;
            statusLabelCell.PaddingBottom = cellPadding;
            statusLabelCell.BackgroundColor = lightGray;
            rentalTable.AddCell(statusLabelCell);
            BaseColor statusColor = rentalStatus == "Returned" ? accentColor :
                                    rentalStatus == "Active" ? new BaseColor(46, 139, 87) : highlightColor;
            PdfPCell statusValueCell = new PdfPCell(new Phrase(rentalStatus,
                new iTextSharp.text.Font(baseFont, normalFontSize, iTextSharp.text.Font.BOLD, statusColor)));
            statusValueCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            statusValueCell.BorderColor = divider;
            statusValueCell.PaddingTop = cellPadding;
            statusValueCell.PaddingBottom = cellPadding;
            statusValueCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            statusValueCell.BackgroundColor = lightGray;
            rentalTable.AddCell(statusValueCell);
            AddTableRow("Payment Method", paymentMethod, false);
            doc.Add(rentalTable);
            PdfPTable paymentTable = new PdfPTable(2);
            paymentTable.WidthPercentage = 100;
            paymentTable.SpacingBefore = 5f;
            paymentTable.SpacingAfter = 5f;
            paymentTable.SetWidths(new float[] { 50, 50 });
            PdfPCell paymentHeaderCell = new PdfPCell(new Phrase("PAYMENT SUMMARY", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, normalFontSize, white)));
            paymentHeaderCell.BackgroundColor = primaryColor;
            paymentHeaderCell.HorizontalAlignment = Element.ALIGN_CENTER;
            paymentHeaderCell.Padding = cellPadding;
            paymentHeaderCell.Colspan = 2;
            paymentTable.AddCell(paymentHeaderCell);
            void AddPaymentRow(string label, string value, bool alternateColor)
            {
                PdfPCell labelCell = new PdfPCell(new Phrase(label, labelFont));
                labelCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                labelCell.BorderColor = divider;
                labelCell.PaddingTop = cellPadding;
                labelCell.PaddingBottom = cellPadding;
                labelCell.BackgroundColor = alternateColor ? lightGray : white;
                PdfPCell valueCell = new PdfPCell(new Phrase(value, valueFont));
                valueCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                valueCell.BorderColor = divider;
                valueCell.PaddingTop = cellPadding;
                valueCell.PaddingBottom = cellPadding;
                valueCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                valueCell.BackgroundColor = alternateColor ? lightGray : white;
                paymentTable.AddCell(labelCell);
                paymentTable.AddCell(valueCell);
            }
            AddPaymentRow("Amount Due", $"₱{amountDue:N2}", false);
            AddPaymentRow("Discount", $"₱{discount:N2}", true);
            PdfPCell totalLabelCell = new PdfPCell(new Phrase("TOTAL PAID",
                new iTextSharp.text.Font(baseFont, normalFontSize, iTextSharp.text.Font.BOLD, white)));
            totalLabelCell.BackgroundColor = accentColor;
            totalLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            totalLabelCell.PaddingTop = cellPadding;
            totalLabelCell.PaddingBottom = cellPadding;
            paymentTable.AddCell(totalLabelCell);
            PdfPCell totalValueCell = new PdfPCell(new Phrase($"₱{totalAmount:N2}",
                new iTextSharp.text.Font(baseFont, normalFontSize, iTextSharp.text.Font.BOLD, white)));
            totalValueCell.BackgroundColor = accentColor;
            totalValueCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            totalValueCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalValueCell.PaddingTop = cellPadding;
            totalValueCell.PaddingBottom = cellPadding;
            paymentTable.AddCell(totalValueCell);
            doc.Add(paymentTable);
            Paragraph footer = new Paragraph();
            footer.Alignment = Element.ALIGN_CENTER;
            footer.SpacingBefore = 5f;
            Chunk thankYouChunk = new Chunk("Thank you for choosing " + companyName + "!",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, smallFontSize, iTextSharp.text.Font.ITALIC, accentColor));
            footer.Add(thankYouChunk);
            footer.Add(Chunk.NEWLINE);
            footer.Add(new Chunk("For inquiries: support@cmrentals.com",
                FontFactory.GetFont(FontFactory.HELVETICA, smallFontSize)));
            footer.Add(Chunk.NEWLINE);
            footer.Add(new Chunk("This is a computer-generated receipt and does not require a signature.",
                    FontFactory.GetFont(FontFactory.HELVETICA, smallFontSize - 1, iTextSharp.text.Font.ITALIC)));
            doc.Add(footer);
            doc.Close();
        }
    }
}
