using System;
using System.Windows.Forms;

namespace Patient_Management_System
{
    public partial class Home : Form
    {
        public static string UserRole = "";

        public Home()
        {
            InitializeComponent();
        }

        public Home(string role)
        {
            InitializeComponent();
            UserRole = role;
        }

        private void Home_Load(object sender, EventArgs e)
        {
        }

        private void btnManagePatients_Click_1(object sender, EventArgs e)
        {
            Patient_Management patient = new Patient_Management();
            patient.Show();
        }

        private void btnManageDoctors_Click_1(object sender, EventArgs e)
        {
            Doctor_Management doctorPage = new Doctor_Management();
            doctorPage.Show();
        }

        private void btnAppointments_Click_1(object sender, EventArgs e)
        {
            Appointments appointmentPage = new Appointments();
            appointmentPage.Show();
        }

        private void btnRecords_Click_1(object sender, EventArgs e)
        {
            Reports recordPage = new Reports();
            recordPage.Show();
        }

        private void btnBilling_Click_1(object sender, EventArgs e)
        {
            Billing billingPage = new Billing();
            billingPage.Show();
        }

        private void btnReports_Click_1(object sender, EventArgs e)
        {
            Medical_Records reportPage = new Medical_Records();
            reportPage.Show();
        }

        private void btnExitApp_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
        }
    }
}