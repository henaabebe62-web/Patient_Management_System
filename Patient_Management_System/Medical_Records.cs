using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Patient_Management_System
{
    public partial class Medical_Records : Form
    {
        string connString = @"Server=.\SQLEXPRESS; Database=Patient_Managemnt_System; Integrated Security=True";

        public Medical_Records()
        {
            InitializeComponent();
            SetupCustomUI();

            btnUpdate.BackColor = Color.Green;
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FlatStyle = FlatStyle.Flat;

            btnDelete.BackColor = Color.Red;
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
        }

        private void SetupCustomUI()
        {
            dgvReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReports.MultiSelect = false;
            dgvReports.AllowUserToAddRows = false;
            dgvReports.AutoGenerateColumns = false;

            dgvReports.Columns["Column1"].DataPropertyName = "PatientID";
            dgvReports.Columns["Column3"].DataPropertyName = "Diagnosis";
            dgvReports.Columns["Column4"].DataPropertyName = "Treatment";
            dgvReports.Columns["Column2"].DataPropertyName = "RecordDate";
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            LoadData();

            if (Home.UserRole == "Manager")
            {
                ApplyManagerReadOnlyMode();
            }
            else
            {
                ApplyDoctorActiveMode();
            }
        }

        private void ApplyManagerReadOnlyMode()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            txtPatientID.ReadOnly = true;
            txtDiagnosis.ReadOnly = true;
            txtTreatment.ReadOnly = true;

            dgvReports.ReadOnly = true;
        }

        private void ApplyDoctorActiveMode()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            txtPatientID.ReadOnly = false;
            txtDiagnosis.ReadOnly = false;
            txtTreatment.ReadOnly = false;

            dgvReports.ReadOnly = false;
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT PatientID, Diagnosis, Treatment, RecordDate FROM Medical_Records ORDER BY RecordDate DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReports.DataSource = null;
                    dgvReports.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private void ClearFields()
        {
            if (Home.UserRole == "Manager") return;

            txtPatientID.Clear();
            txtDiagnosis.Clear();
            txtTreatment.Clear();
            txtPatientID.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (string.IsNullOrWhiteSpace(txtPatientID.Text) ||
                string.IsNullOrWhiteSpace(txtDiagnosis.Text) ||
                string.IsNullOrWhiteSpace(txtTreatment.Text))
            {
                MessageBox.Show("Please fill in all medical record information (ID, Diagnosis, and Treatment) before adding!", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO Medical_Records (PatientID, Diagnosis, Treatment, RecordDate) VALUES (@pid, @diag, @treat, GETDATE())";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@pid", txtPatientID.Text.Trim());
                    cmd.Parameters.AddWithValue("@diag", txtDiagnosis.Text.Trim());
                    cmd.Parameters.AddWithValue("@treat", txtTreatment.Text.Trim());
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Record successfully registered!");
                    LoadData();
                    ClearFields();
                }
            }
            catch (Exception ex) { MessageBox.Show("Add Error: " + ex.Message); }
        }

        private void dgvReports_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvReports.Rows[e.RowIndex];

                txtPatientID.Text = Convert.ToString(row.Cells["Column1"].Value);
                txtDiagnosis.Text = Convert.ToString(row.Cells["Column3"].Value);
                txtTreatment.Text = Convert.ToString(row.Cells["Column4"].Value);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvReports_CellClick(sender, e);
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (string.IsNullOrWhiteSpace(txtPatientID.Text))
            {
                MessageBox.Show("Please select a record from the list to update first!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "UPDATE Medical_Records SET Diagnosis=@diag, Treatment=@treat, RecordDate=GETDATE() WHERE PatientID=@pid";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@pid", txtPatientID.Text.Trim());
                    cmd.Parameters.AddWithValue("@diag", txtDiagnosis.Text.Trim());
                    cmd.Parameters.AddWithValue("@treat", txtTreatment.Text.Trim());

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Updated successfully!");
                        LoadData();
                        ClearFields();
                    }
                    else { MessageBox.Show("No record found to update."); }
                }
            }
            catch (Exception ex) { MessageBox.Show("Update Error: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (dgvReports.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            string selectedID = Convert.ToString(dgvReports.CurrentRow.Cells["Column1"].Value);

            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "DELETE FROM Medical_Records WHERE PatientID=@pid";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@pid", selectedID);
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Deleted successfully!");
                        LoadData();
                        ClearFields();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Delete Error: " + ex.Message); }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 home = new Form1();
            home.Show();
            this.Close();
        }
    }
}