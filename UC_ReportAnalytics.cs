using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Data.OleDb;
using LiveCharts;
using LiveCharts.Wpf;
using System.IO;
using System.Windows.Forms.Integration;

namespace CarRent
{
    public partial class UC_ReportAnalytics : UserControl
    {
        private LiveCharts.Wpf.CartesianChart monthlyRentalsWpfChart;
        private LiveCharts.Wpf.PieChart carCategoriesWpfChart;
        private LiveCharts.Wpf.CartesianChart topCustomersWpfChart;
        private LiveCharts.Wpf.CartesianChart revenueWpfChart;
        private ElementHost monthlyRentalsHost;
        private ElementHost carCategoriesHost;
        private ElementHost topCustomersHost;
        private ElementHost revenueHost;
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";

        // Define theme colors
        private readonly System.Windows.Media.Color darkRed = System.Windows.Media.Color.FromRgb(153, 27, 27);
        private readonly System.Windows.Media.Color lightRed = System.Windows.Media.Color.FromRgb(220, 53, 69);
        private readonly System.Windows.Media.Color bgColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
        private readonly System.Windows.Media.Color gridColor = System.Windows.Media.Color.FromRgb(222, 226, 230);
        private readonly System.Windows.Media.Color textColor = System.Windows.Media.Color.FromRgb(33, 37, 41);

        public UC_ReportAnalytics()
        {
            InitializeComponent();
            InitializeCharts();
            LoadChartData();
        }

        private void InitializeCharts()
        {
            try
            {
                // Monthly Rentals Chart
                monthlyRentalsWpfChart = new LiveCharts.Wpf.CartesianChart
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
                    DisableAnimations = true
                };
                monthlyRentalsHost = new ElementHost
                {
                    Location = new Point(10, 40),
                    Size = new Size(530, 200),
                    Child = monthlyRentalsWpfChart
                };
                panelMonthlyRentals.Controls.Add(monthlyRentalsHost);

                // Car Categories Chart
                carCategoriesWpfChart = new LiveCharts.Wpf.PieChart
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
                    DisableAnimations = true
                };
                carCategoriesHost = new ElementHost
                {
                    Location = new Point(10, 40),
                    Size = new Size(530, 200),
                    Child = carCategoriesWpfChart
                };
                panelCarCategories.Controls.Add(carCategoriesHost);

                // Top Customers Chart
                topCustomersWpfChart = new LiveCharts.Wpf.CartesianChart
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
                    DisableAnimations = true
                };
                topCustomersHost = new ElementHost
                {
                    Location = new Point(10, 40),
                    Size = new Size(530, 200),
                    Child = topCustomersWpfChart
                };
                panelTopCustomers.Controls.Add(topCustomersHost);

                // Revenue Chart
                revenueWpfChart = new LiveCharts.Wpf.CartesianChart
                {
                    Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White),
                    DisableAnimations = true
                };
                revenueHost = new ElementHost
                {
                    Location = new Point(10, 40),
                    Size = new Size(530, 200),
                    Child = revenueWpfChart
                };
                panelRevenue.Controls.Add(revenueHost);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing charts: " + ex.Message);
            }
        }

        private void LoadChartData()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    LoadMonthlyRentalsData(conn);
                    LoadCarCategoriesData(conn);
                    LoadTopCustomersData(conn);
                    LoadRevenueData(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void LoadMonthlyRentalsData(OleDbConnection conn)
        {
            var values = new ChartValues<double>();
            var labels = new List<string>();

            using (OleDbCommand cmd = new OleDbCommand(
                @"SELECT Format([DateCreated],'mmm-yyyy') AS [Month], COUNT(*) AS [Count] 
                FROM tblRentals 
                GROUP BY Format([DateCreated],'mmm-yyyy') 
                ORDER BY Min([DateCreated])", conn))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        values.Add(Convert.ToDouble(reader["Count"]));
                        labels.Add(reader["Month"].ToString());
                    }
                }
            }

            monthlyRentalsWpfChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Monthly Rentals",
                    Values = values,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 8,
                    LineSmoothness = 0.5,
                    StrokeThickness = 2,
                    Stroke = new System.Windows.Media.SolidColorBrush(darkRed),
                    Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(30, lightRed.R, lightRed.G, lightRed.B))
                }
            };

            monthlyRentalsWpfChart.AxisX.Add(new Axis
            {
                Title = "Month",
                Labels = labels,
                Separator = new Separator
                {
                    Stroke = new System.Windows.Media.SolidColorBrush(gridColor),
                    StrokeThickness = 1
                },
                Foreground = new System.Windows.Media.SolidColorBrush(textColor)
            });

            monthlyRentalsWpfChart.AxisY.Add(new Axis
            {
                Title = "Number of Rentals",
                Foreground = new System.Windows.Media.SolidColorBrush(textColor),
                Separator = new Separator
                {
                    Stroke = new System.Windows.Media.SolidColorBrush(gridColor),
                    StrokeThickness = 1
                }
            });
        }

        private void LoadCarCategoriesData(OleDbConnection conn)
        {
            var series = new SeriesCollection();
            var colors = new[] 
            {
                System.Windows.Media.Color.FromRgb(220, 53, 69),    // Main red
                System.Windows.Media.Color.FromRgb(255, 107, 107),  // Lighter red
                System.Windows.Media.Color.FromRgb(153, 27, 27),    // Dark red
                System.Windows.Media.Color.FromRgb(255, 148, 148),  // Very light red
                System.Windows.Media.Color.FromRgb(139, 0, 0)       // Deep red
            };
            int colorIndex = 0;

            using (OleDbCommand cmd = new OleDbCommand(
                @"SELECT [VehicleType] AS [Category], COUNT(*) AS [Count] 
                FROM tblVehicles 
                GROUP BY [VehicleType]", conn))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        series.Add(new PieSeries
                        {
                            Title = reader["Category"].ToString(),
                            Values = new ChartValues<double> { Convert.ToDouble(reader["Count"]) },
                            DataLabels = true,
                            LabelPoint = point => $"{point.SeriesView.Title}: {point.Participation:P0}",
                            Fill = new System.Windows.Media.SolidColorBrush(colors[colorIndex % colors.Length]),
                            Foreground = new System.Windows.Media.SolidColorBrush(textColor)
                        });
                        colorIndex++;
                    }
                }
            }

            carCategoriesWpfChart.Series = series;
            carCategoriesWpfChart.LegendLocation = LegendLocation.Right;
        }

        private void LoadTopCustomersData(OleDbConnection conn)
        {
            var values = new ChartValues<double>();
            var labels = new List<string>();

            using (OleDbCommand cmd = new OleDbCommand(
                @"SELECT TOP 5 c.[FullName], COUNT(r.[RentalCode]) AS [RentalCount] 
                FROM tblCustomers c 
                INNER JOIN tblRentals r ON c.[CustomerCode] = r.[CustomerCode] 
                GROUP BY c.[FullName] 
                ORDER BY COUNT(r.[RentalCode]) DESC", conn))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        values.Add(Convert.ToDouble(reader["RentalCount"]));
                        labels.Add(reader["FullName"].ToString());
                    }
                }
            }

            topCustomersWpfChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Top Customers",
                    Values = values,
                    Fill = new System.Windows.Media.SolidColorBrush(lightRed),
                    MaxColumnWidth = 50
                }
            };

            topCustomersWpfChart.AxisX.Add(new Axis
            {
                Title = "Customer Name",
                Labels = labels,
                Separator = new Separator
                {
                    Stroke = new System.Windows.Media.SolidColorBrush(gridColor),
                    StrokeThickness = 1
                },
                Foreground = new System.Windows.Media.SolidColorBrush(textColor)
            });

            topCustomersWpfChart.AxisY.Add(new Axis
            {
                Title = "Number of Rentals",
                Foreground = new System.Windows.Media.SolidColorBrush(textColor),
                Separator = new Separator
                {
                    Stroke = new System.Windows.Media.SolidColorBrush(gridColor),
                    StrokeThickness = 1
                }
            });
        }

        private void LoadRevenueData(OleDbConnection conn)
        {
            var values = new ChartValues<double>();
            var labels = new List<string>();

            using (OleDbCommand cmd = new OleDbCommand(
                @"SELECT Format([DateCreated],'mmm-yyyy') AS [Month], SUM([TotalAmount]) AS [Revenue] 
                FROM tblRentals 
                WHERE ProcessedBy = ?
                GROUP BY Format([DateCreated],'mmm-yyyy') 
                ORDER BY Min([DateCreated])", conn))
            {
                cmd.Parameters.AddWithValue("?", Session.CurrentUserCode);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        values.Add(Convert.ToDouble(reader["Revenue"]));
                        labels.Add(reader["Month"].ToString());
                    }
                }
            }

            revenueWpfChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "My Revenue",
                    Values = values,
                    PointGeometry = DefaultGeometries.Diamond,
                    PointGeometrySize = 8,
                    LineSmoothness = 0.5,
                    StrokeThickness = 2,
                    Stroke = new System.Windows.Media.SolidColorBrush(darkRed),
                    Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(30, lightRed.R, lightRed.G, lightRed.B))
                }
            };

            revenueWpfChart.AxisX.Add(new Axis
            {
                Title = "Month",
                Labels = labels,
                Separator = new Separator
                {
                    Stroke = new System.Windows.Media.SolidColorBrush(gridColor),
                    StrokeThickness = 1
                },
                Foreground = new System.Windows.Media.SolidColorBrush(textColor)
            });

            revenueWpfChart.AxisY.Add(new Axis
            {
                Title = "Revenue Amount",
                Foreground = new System.Windows.Media.SolidColorBrush(textColor),
                LabelFormatter = value => value.ToString("C0"),
                Separator = new Separator
                {
                    Stroke = new System.Windows.Media.SolidColorBrush(gridColor),
                    StrokeThickness = 1
                }
            });

            lblRevenue.Text = "My Revenue Analysis";
        }
    }
}