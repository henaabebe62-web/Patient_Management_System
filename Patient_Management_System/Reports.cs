using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;

namespace Patient_Management_System
{
    public partial class Reports : Form
    {
        string connString = @"Server=.\SQLEXPRESS; Database=Patient_Managemnt_System; Integrated Security=True";

        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            LoadDatabaseAnalytics();
        }

        private void LoadDatabaseAnalytics()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    int p = ExecuteCount("SELECT COUNT(*) FROM Patients", conn);
                    int d = ExecuteCount("SELECT COUNT(*) FROM Doctors", conn);
                    int a = ExecuteCount("SELECT COUNT(*) FROM Appointments", conn);
                    int m = ExecuteCount("SELECT COUNT(*) FROM MedicalRecords", conn);
                    int b = ExecuteCount("SELECT COUNT(*) FROM Billing", conn);

                    double paid = ExecuteSum("SELECT SUM(Amount) FROM Billing WHERE PaymentStatus = 'Paid'", conn);
                    double unpaid = ExecuteSum("SELECT SUM(Amount) FROM Billing WHERE PaymentStatus = 'Unpaid'", conn);

                    UpdateUI(p, d, a, m, b, paid, unpaid);
                    UpdateChart(p, d, a, m, b, paid, unpaid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int ExecuteCount(string query, SqlConnection conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                return (int)cmd.ExecuteScalar();
            }
            catch { return 0; }
        }

        private double ExecuteSum(string query, SqlConnection conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                object res = cmd.ExecuteScalar();
                return res != DBNull.Value ? Convert.ToDouble(res) : 0;
            }
            catch { return 0; }
        }

        private void UpdateUI(int p, int d, int a, int m, int b, double paid, double unpaid)
        {
            lblTotalPatients.Text = "Total Patients: " + p;
            lblTotalDoctors.Text = "Total Doctors: " + d;
            lblTotalAppointment.Text = "Appointments: " + a;
            lblTotalMedicalRecords.Text = "Medical Records: " + m;
            lblTotalBills.Text = "Total Bills: " + b;
            lblTotalPaidAmount.Text = "Total Paid: $" + paid.ToString("N2");
            lblTotalUnpaid.Text = "Total Unpaid: $" + unpaid.ToString("N2");
        }

        private void UpdateChart(int p, int d, int a, int m, int b, double paid, double unpaid)
        {
            chartFinancials.Series.Clear();
            chartFinancials.Titles.Clear();

            chartFinancials.ChartAreas[0].AxisY.IsLogarithmic = true;
            chartFinancials.ChartAreas[0].AxisY.LogarithmBase = 10;

            Title title = new Title("Patient Management System - Comprehensive Analytics");
            title.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            chartFinancials.Titles.Add(title);

            Series mainSeries = new Series("Overall Statistics");
            mainSeries.ChartType = SeriesChartType.Column;
            mainSeries.IsValueShownAsLabel = true;
            mainSeries.Font = new Font("Arial", 9, FontStyle.Bold);

            AddPoint(mainSeries, "Patients", p, Color.DodgerBlue);
            AddPoint(mainSeries, "Doctors", d, Color.Gold);
            AddPoint(mainSeries, "Appts", a, Color.MediumPurple);
            AddPoint(mainSeries, "Records", m, Color.DarkOrange);
            AddPoint(mainSeries, "Bills", b, Color.Teal);
            AddPoint(mainSeries, "Paid ($)", paid, Color.ForestGreen);
            AddPoint(mainSeries, "Unpaid ($)", unpaid, Color.Crimson);

            mainSeries["PointWidth"] = "0.7";
            chartFinancials.Series.Add(mainSeries);

            chartFinancials.ChartAreas[0].AxisX.Interval = 1;
            chartFinancials.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartFinancials.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartFinancials.ChartAreas[0].AxisY.Title = "Scale (Logarithmic View)";
        }

        private void AddPoint(Series series, string name, double value, Color color)
        {
            double displayValue = value > 0 ? value : 0.1;
            int idx = series.Points.AddXY(name, displayValue);
            series.Points[idx].Color = color;
            series.Points[idx].Label = value.ToString();
        }

        private void grpSystemReports_Enter(object sender, EventArgs e) { }
        private void chartFinancials_Click(object sender, EventArgs e) { }

        private void chartFinancials_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Close();
        }
    }
}