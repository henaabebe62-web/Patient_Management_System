using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Patient_Management_System
{
    public partial class Appointments : Form
    {
        string connString = @"Server=.\SQLEXPRESS; Database=Patient_Managemnt_System; Integrated Security=True";

        public Appointments()
        {
            InitializeComponent();
            SetupCustomUI();
        }

        private void SetupCustomUI()
        {
            dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAppointments.MultiSelect = false;
            dgvAppointments.AllowUserToAddRows = false;

            dgvAppointments.AutoGenerateColumns = false;
            dgvAppointments.Columns.Clear();

            dgvAppointments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "App ID", DataPropertyName = "AppID", Name = "AppID" });
            dgvAppointments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Patient ID", DataPropertyName = "PatientID", Name = "PatientID" });
            dgvAppointments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Doctor ID", DataPropertyName = "DoctorID", Name = "DoctorID" });
            dgvAppointments.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date", DataPropertyName = "AppointmentDate", Name = "AppointmentDate" });

            dgvAppointments.CellClick += new DataGridViewCellEventHandler(this.dgvAppointments_CellClick);
        }

        private void Appointments_Load(object sender, EventArgs e)
        {
            LoadData();

            if (Home.UserRole == "Manager")
            {
                ApplyManagerReadOnlyMode();
            }
            else
            {
                ApplyActiveMode();
            }
        }

        private void ApplyManagerReadOnlyMode()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            txtPatientID.ReadOnly = true;
            txtDoctorID.ReadOnly = true;
            txtDate.ReadOnly = true;

            dgvAppointments.ReadOnly = true;
        }

        private void ApplyActiveMode()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            txtPatientID.ReadOnly = false;
            txtDoctorID.ReadOnly = false;
            txtDate.ReadOnly = false;

            dgvAppointments.ReadOnly = false;
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT AppID, PatientID, DoctorID, AppointmentDate FROM Appointments";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAppointments.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading data: " + ex.Message); }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (string.IsNullOrWhiteSpace(txtPatientID.Text) || string.IsNullOrWhiteSpace(txtDoctorID.Text) || string.IsNullOrWhiteSpace(txtDate.Text))
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }

            string query = "INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate) VALUES (@pid, @did, @date)";
            ExecuteQuery(query, null, "Successfully Added!");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (dgvAppointments.CurrentRow != null)
            {
                string appId = dgvAppointments.CurrentRow.Cells["AppID"].Value.ToString();

                if (!string.IsNullOrEmpty(appId))
                {
                    string query = "UPDATE Appointments SET PatientID=@pid, DoctorID=@did, AppointmentDate=@date WHERE AppID=@id";
                    ExecuteQuery(query, appId, "Successfully Updated!");
                }
            }
            else { MessageBox.Show("Please select a row first."); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (dgvAppointments.CurrentRow != null)
            {
                string appId = dgvAppointments.CurrentRow.Cells["AppID"].Value.ToString();

                if (MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        {
                            string query = "DELETE FROM Appointments WHERE AppID = @id";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@id", appId);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Deleted!");
                            LoadData();
                            ClearFields();
                        }
                    }
                    catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
                }
            }
        }

        private void ExecuteQuery(string query, string id, string successMsg)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@pid", txtPatientID.Text.Trim());
                    cmd.Parameters.AddWithValue("@did", txtDoctorID.Text.Trim());

                    DateTime appDate;
                    if (DateTime.TryParse(txtDate.Text.Trim(), out appDate))
                        cmd.Parameters.AddWithValue("@date", appDate);
                    else
                    {
                        MessageBox.Show("Invalid Date format (YYYY-MM-DD)");
                        return;
                    }

                    if (id != null)
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(successMsg);
                    LoadData();
                    ClearFields();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void ClearFields()
        {
            if (Home.UserRole == "Manager") return;

            txtPatientID.Clear();
            txtDoctorID.Clear();
            txtDate.Clear();
            if (txtSearch != null) txtSearch.Clear();
            txtPatientID.Focus();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvAppointments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dgvAppointments.Rows[e.RowIndex];

                    if (row.Cells["PatientID"].Value != null)
                        txtPatientID.Text = row.Cells["PatientID"].Value.ToString();
                    else
                        txtPatientID.Text = "";

                    if (row.Cells["DoctorID"].Value != null)
                        txtDoctorID.Text = row.Cells["DoctorID"].Value.ToString();
                    else
                        txtDoctorID.Text = "";

                    if (row.Cells["AppointmentDate"].Value != null)
                    {
                        DateTime dt = Convert.ToDateTime(row.Cells["AppointmentDate"].Value);
                        txtDate.Text = dt.ToString("yyyy-MM-dd");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Selection Error: " + ex.Message);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT AppID, PatientID, DoctorID, AppointmentDate FROM Appointments WHERE PatientID LIKE @s", conn);
                    da.SelectCommand.Parameters.AddWithValue("@s", "%" + txtSearch.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAppointments.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 home = new Form1();
            home.Show();
            this.Close();
        }
    }
}