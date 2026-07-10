using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Patient_Management_System
{
    public partial class Form1 : Form
    {
        string connString = @"Server=.\SQLEXPRESS; Database=Patient_Managemnt_System; Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT Role FROM Registration WHERE Username=@user AND Password=@pass";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string userRole = result.ToString().Trim();
                        MessageBox.Show("Welcome! Your role is: " + userRole, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        switch (userRole)
                        {
                            case "Secretary":
                                Patient_Management patientPage = new Patient_Management();
                                patientPage.Show();
                                break;

                            case "Billing Staff":
                                Billing billingPage = new Billing();
                                billingPage.Show();
                                break;

                            case "Doctor Manager":
                                Doctor_Management Page = new Doctor_Management();
                                Page.Show();
                                break;

                            case "Doctor":
                                DialogResult choice = MessageBox.Show(
                                    "Which page would you like to open?\n\n" +
                                    "• Click 'Yes' - for Appointments\n" +
                                    "• Click 'No' - for Medical Records\n" +
                                    "• Click 'Cancel' - to Stay Here",
                                    "Doctor Choice",
                                    MessageBoxButtons.YesNoCancel,
                                    MessageBoxIcon.Question);

                                if (choice == DialogResult.Yes)
                                {
                                    Appointments appointmentsPage = new Appointments();
                                    appointmentsPage.Show();
                                }
                                else if (choice == DialogResult.No)
                                {
                                    Medical_Records reportPage = new Medical_Records();
                                    reportPage.Show();
                                }
                                else
                                {
                                    return;
                                }
                                break;

                            case "Manager":
                                Home homePage = new Home(userRole);
                                homePage.Show();
                                break;

                            default:
                                Home defaultHome = new Home("Guest");
                                defaultHome.Show();
                                break;
                        }

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Registration registrationForm = new Registration();
            registrationForm.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}