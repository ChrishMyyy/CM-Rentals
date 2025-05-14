using System;
using System.Data.OleDb;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using System.Drawing;
using Guna.UI2.WinForms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using FontWeight = System.Windows.FontWeight;

namespace CarRent
{
    public partial class UC_AdminAnalytics : UserControl
    {
        // Chart objects
        private CartesianChart revenueWpfChart;
        private CartesianChart vehicleUtilizationWpfChart;
        private PieChart customerDemographicsWpfChart;
        private CartesianChart rentalTrendsWpfChart;
        private CartesianChart rentalStatusWpfChart;

        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\chris aldrin\source\repos\CarRent\CarRentDatabase.accdb";

        // Theme colors
        private SolidColorBrush themeDarkRed = new SolidColorBrush(System.Windows.Media.Color.FromRgb(155, 34, 38));
        private SolidColorBrush themeWhite = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        private SolidColorBrush themeBg = new SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 30, 30));

        public UC_AdminAnalytics()
        {
            InitializeComponent();

            // Initialize and assign all charts
            InitializeCharts();
            LoadAnalyticsData();
            SetupTabSwitching();
        }

        private void InitializeCharts()
        {
            // Revenue Chart
            revenueWpfChart = new CartesianChart { Background = System.Windows.Media.Brushes.Transparent };
            revenueHost.Child = revenueWpfChart;

            // Vehicle Utilization Chart
            vehicleUtilizationWpfChart = new CartesianChart { Background = System.Windows.Media.Brushes.Transparent };
            vehicleUtilizationHost.Child = vehicleUtilizationWpfChart;

            // Customer Demographics Chart (smaller size)
            customerDemographicsWpfChart = new PieChart
            {
                Background = System.Windows.Media.Brushes.Transparent,
                Width = 250,
                Height = 250,
                LegendLocation = LegendLocation.Right
            };
            customerDemographicsHost.Child = customerDemographicsWpfChart;

            // Rental Trends Chart
            rentalTrendsWpfChart = new CartesianChart { Background = System.Windows.Media.Brushes.Transparent };
            rentalTrendsHost.Child = rentalTrendsWpfChart;

            // Rental Status Chart
            rentalStatusWpfChart = new CartesianChart { Background = System.Windows.Media.Brushes.Transparent };
            rentalStatusHost.Child = rentalStatusWpfChart;
        }

        private void SetupTabSwitching()
        {
            // Hide all panels initially
            guna2PanelRevenue.Visible = false;
            guna2PanelVehicleUtilization.Visible = false;
            guna2PanelCustomerDemographics.Visible = false;
            guna2PanelRentalTrends.Visible = false;
            guna2PanelRentalStatus.Visible = false;

            // Show first tab by default
            guna2PanelRevenue.Visible = true;

            // Wire up click events
            btnRevenue.Click += (s, e) => SwitchTab(guna2PanelRevenue);
            btnVehicleUtilization.Click += (s, e) => SwitchTab(guna2PanelVehicleUtilization);
            btnCustomerDemographics.Click += (s, e) => SwitchTab(guna2PanelCustomerDemographics);
            btnRentalTrends.Click += (s, e) => SwitchTab(guna2PanelRentalTrends);
            btnRentalStatus.Click += (s, e) => SwitchTab(guna2PanelRentalStatus);
        }

        private void SwitchTab(Guna2Panel panelToShow)
        {
            // Hide all panels
            guna2PanelRevenue.Visible = false;
            guna2PanelVehicleUtilization.Visible = false;
            guna2PanelCustomerDemographics.Visible = false;
            guna2PanelRentalTrends.Visible = false;
            guna2PanelRentalStatus.Visible = false;

            // Show selected panel
            panelToShow.Visible = true;
        }

        private void LoadAnalyticsData()
        {
            LoadRevenueData();
            LoadVehicleUtilizationData();
            LoadCustomerDemographicsData();
            LoadRentalTrendsData();
            LoadRentalStatusData();
        }

        private void LoadRevenueData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT 
    Month([StartDate]) AS [Month], 
    Year([StartDate]) AS [Year], 
    SUM([TotalAmount]) AS [Revenue]
FROM tblRentals
GROUP BY Year([StartDate]), Month([StartDate])
ORDER BY Year([StartDate]), Month([StartDate])";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            var series = new SeriesCollection();
                            var values = new ChartValues<double>();
                            var labels = new System.Collections.Generic.List<string>();

                            while (reader.Read())
                            {
                                values.Add(Convert.ToDouble(reader["Revenue"]));
                                int month = Convert.ToInt32(reader["Month"]);
                                int year = Convert.ToInt32(reader["Year"]);
                                labels.Add(new DateTime(year, month, 1).ToString("MMM yyyy"));
                            }

                            series.Add(new LineSeries
                            {
                                Title = "Revenue",
                                Values = values,
                                PointGeometry = DefaultGeometries.Circle,
                                PointGeometrySize = 10,
                                Stroke = themeDarkRed,
                                Fill = System.Windows.Media.Brushes.Transparent
                            });

                            revenueWpfChart.Series = series;
                            revenueWpfChart.AxisX.Clear();
                            revenueWpfChart.AxisY.Clear();
                            revenueWpfChart.AxisX.Add(new Axis
                            {
                                Labels = labels,
                                Foreground = themeWhite,
                                Separator = new Separator { Stroke = themeWhite }
                            });
                            revenueWpfChart.AxisY.Add(new Axis
                            {
                                Title = "Revenue",
                                Foreground = themeWhite,
                                Separator = new Separator { Stroke = themeWhite }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading revenue data: " + ex.Message);
            }
        }

        private void LoadVehicleUtilizationData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT 
    v.[Model], 
    COUNT(r.[RentalID]) AS [RentalCount]
FROM tblVehicles AS v
LEFT JOIN tblRentals AS r ON v.[CarCode] = r.[CarCode]
GROUP BY v.[Model]
ORDER BY COUNT(r.[RentalID]) DESC";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            var series = new SeriesCollection();
                            var values = new ChartValues<double>();
                            var labels = new System.Collections.Generic.List<string>();

                            while (reader.Read())
                            {
                                values.Add(Convert.ToDouble(reader["RentalCount"]));
                                labels.Add(reader["Model"].ToString());
                            }

                            series.Add(new ColumnSeries
                            {
                                Title = "Rental Count",
                                Values = values,
                                Fill = themeDarkRed
                            });

                            vehicleUtilizationWpfChart.Series = series;
                            vehicleUtilizationWpfChart.AxisX.Clear();
                            vehicleUtilizationWpfChart.AxisY.Clear();
                            vehicleUtilizationWpfChart.AxisX.Add(new Axis
                            {
                                Labels = labels,
                                Foreground = themeWhite,
                                Separator = new Separator { Stroke = themeWhite }
                            });
                            vehicleUtilizationWpfChart.AxisY.Add(new Axis
                            {
                                Title = "Number of Rentals",
                                Foreground = themeWhite,
                                Separator = new Separator { Stroke = themeWhite }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading vehicle utilization data: " + ex.Message);
            }
        }

        private void LoadCustomerDemographicsData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT [Gender], COUNT(*) AS [Count]
FROM tblCustomers
GROUP BY [Gender]";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            var series = new SeriesCollection();
                            // Define a palette of red shades
                            var redPalette = new System.Windows.Media.Brush[] {
                                new SolidColorBrush(System.Windows.Media.Color.FromRgb(220, 53, 69)), // Bootstrap Red
                                new SolidColorBrush(System.Windows.Media.Color.FromRgb(155, 34, 38)), // Dark Red
                                new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 99, 132)), // Light Red
                                new SolidColorBrush(System.Windows.Media.Color.FromRgb(200, 35, 51)), // Medium Red
                                new SolidColorBrush(System.Windows.Media.Color.FromRgb(139, 0, 0)),   // Deep Red
                            };
                            int colorIndex = 0;

                            while (reader.Read())
                            {
                                series.Add(new PieSeries
                                {
                                    Title = reader["Gender"].ToString(),
                                    Values = new ChartValues<double> { Convert.ToDouble(reader["Count"]) },
                                    DataLabels = true,
                                    Fill = redPalette[colorIndex % redPalette.Length],
                                    Foreground = themeWhite
                                });
                                colorIndex++;
                            }

                            customerDemographicsWpfChart.Series = series;
                            customerDemographicsWpfChart.LegendLocation = LegendLocation.Right;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer demographics data: " + ex.Message);
            }
        }

        private void LoadRentalTrendsData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT 
    Month([StartDate]) AS [Month], 
    Year([StartDate]) AS [Year], 
    COUNT(*) AS [RentalCount]
FROM tblRentals
GROUP BY Year([StartDate]), Month([StartDate])
ORDER BY Year([StartDate]), Month([StartDate])";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            var series = new SeriesCollection();
                            var values = new ChartValues<double>();
                            var labels = new System.Collections.Generic.List<string>();

                            while (reader.Read())
                            {
                                values.Add(Convert.ToDouble(reader["RentalCount"]));
                                int month = Convert.ToInt32(reader["Month"]);
                                int year = Convert.ToInt32(reader["Year"]);
                                labels.Add(new DateTime(year, month, 1).ToString("MMM yyyy"));
                            }

                            series.Add(new LineSeries
                            {
                                Title = "Rental Count",
                                Values = values,
                                PointGeometry = DefaultGeometries.Circle,
                                PointGeometrySize = 10,
                                Stroke = themeDarkRed,
                                Fill = System.Windows.Media.Brushes.Transparent
                            });

                            rentalTrendsWpfChart.Series = series;
                            rentalTrendsWpfChart.AxisX.Clear();
                            rentalTrendsWpfChart.AxisY.Clear();
                            rentalTrendsWpfChart.AxisX.Add(new Axis
                            {
                                Labels = labels,
                                Foreground = themeWhite,
                                Separator = new Separator { Stroke = themeWhite }
                            });
                            rentalTrendsWpfChart.AxisY.Add(new Axis
                            {
                                Title = "Number of Rentals",
                                Foreground = themeWhite,
                                Separator = new Separator { Stroke = themeWhite }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rental trends data: " + ex.Message);
            }
        }

        private void LoadRentalStatusData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT 
    [RentalStatus], 
    COUNT(*) AS [Count]
FROM tblRentals
GROUP BY [RentalStatus]";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            var series = new SeriesCollection();
                            var values = new ChartValues<double>();
                            var labels = new System.Collections.Generic.List<string>();

                            while (reader.Read())
                            {
                                values.Add(Convert.ToDouble(reader["Count"]));
                                labels.Add(reader["RentalStatus"].ToString());
                            }

                            series.Add(new ColumnSeries
                            {
                                Title = "Rental Status",
                                Values = values,
                                Fill = themeDarkRed
                            });

                            rentalStatusWpfChart.Series = series;
                            rentalStatusWpfChart.AxisX.Clear();
                            rentalStatusWpfChart.AxisY.Clear();
                            rentalStatusWpfChart.AxisX.Add(new Axis
                            {
                                Labels = labels,
                                Foreground = themeWhite,
                                Separator = new Separator { Stroke = themeWhite }
                            });
                            rentalStatusWpfChart.AxisY.Add(new Axis
                            {
                                Title = "Count",
                                Foreground = themeWhite,
                                Separator = new Separator { Stroke = themeWhite }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rental status data: " + ex.Message);
            }
        }
    }
}