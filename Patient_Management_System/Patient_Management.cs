using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Patient_Management_System
{
    public partial class Patient_Management : Form
    {
        string connString = @"Server=.\SQLEXPRESS; Database=Patient_Managemnt_System; Integrated Security=True";

        public Patient_Management()
        {
            InitializeComponent();

            btnUpdate.BackColor = Color.Green;
            btnUpdate.ForeColor = Color.White;
            btnDelete.BackColor = Color.Red;
            btnDelete.ForeColor = Color.White;

            txtPatientID.ReadOnly = true;
            txtPatientID.BackColor = Color.LightGray;

            cmbphone.MaxLength = 13;

            txtAge.KeyPress += OnlyNumbers_KeyPress;
            cmbphone.KeyPress += PhoneNumbers_KeyPress;
            cmbphone.Leave += ValidateEthiopianPhone_Leave;
            txtPatientName.KeyPress += OnlyLetters_KeyPress;
        }

        private void Patient_Management_Load(object sender, EventArgs e)
        {
            dgvPatients.AutoGenerateColumns = false;
            dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPatients.MultiSelect = false;

            if (dgvPatients.Columns.Count >= 6)
            {
                dgvPatients.Columns[0].DataPropertyName = "PatientID";
                dgvPatients.Columns[1].DataPropertyName = "PatientName";
                dgvPatients.Columns[2].DataPropertyName = "Age";
                dgvPatients.Columns[3].DataPropertyName = "Gender";
                dgvPatients.Columns[4].DataPropertyName = "Phone";
                dgvPatients.Columns[5].DataPropertyName = "Address";
            }

            LoadData();
            GetNextID();

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

            txtPatientName.ReadOnly = true;
            txtAge.ReadOnly = true;
            textBox1.ReadOnly = true;

            comboBox1.Enabled = false;
            cmbphone.Enabled = false;

            dgvPatients.ReadOnly = true;
        }

        private void ApplyActiveMode()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            txtPatientName.ReadOnly = false;
            txtAge.ReadOnly = false;
            textBox1.ReadOnly = false;

            comboBox1.Enabled = true;
            cmbphone.Enabled = true;

            dgvPatients.ReadOnly = false;
        }

        private void OnlyLetters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void PhoneNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '+')
            {
                e.Handled = true;
            }
        }

        private bool IsValidEthiopianPhone(string phone)
        {
            string pattern = @"^\+251\d{9}$";
            return Regex.IsMatch(phone, pattern);
        }

        private void ValidateEthiopianPhone_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbphone.Text) && !IsValidEthiopianPhone(cmbphone.Text))
            {
                MessageBox.Show("Invalid Format! Phone must be +251 followed by exactly 9 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbphone.Focus();
            }
        }

        private void GetNextID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT ISNULL(MAX(PatientID), 0) + 1 FROM Patients";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    txtPatientID.Text = result != null ? result.ToString() : "1";
                }
            }
            catch (Exception ex) { MessageBox.Show("ID Error: " + ex.Message); }
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT PatientID, PatientName, Age, Gender, Phone, Address FROM Patients";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPatients.DataSource = null;
                    dgvPatients.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPatientName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(cmbphone.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please fill in all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEthiopianPhone(cmbphone.Text))
            {
                MessageBox.Show("Please enter a valid Ethiopian phone number (+251XXXXXXXXX).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO Patients (PatientName, Age, Phone, Gender, Address) VALUES (@name, @age, @ph, @gen, @add)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtPatientName.Text.Trim());
                    cmd.Parameters.AddWithValue("@age", txtAge.Text.Trim());
                    cmd.Parameters.AddWithValue("@ph", cmbphone.Text.Trim());
                    cmd.Parameters.AddWithValue("@gen", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@add", textBox1.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Patient registered successfully!");
                    ClearInputs();
                    LoadData();
                }
            }
            catch (Exception ex) { MessageBox.Show("Add Error: " + ex.Message); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPatientID.Text)) return;

            if (!IsValidEthiopianPhone(cmbphone.Text))
            {
                MessageBox.Show("Please enter a valid Ethiopian phone number (+251XXXXXXXXX).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "UPDATE Patients SET PatientName=@name, Age=@age, Phone=@ph, Gender=@gen, Address=@add WHERE PatientID=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtPatientID.Text);
                    cmd.Parameters.AddWithValue("@name", txtPatientName.Text.Trim());
                    cmd.Parameters.AddWithValue("@age", txtAge.Text.Trim());
                    cmd.Parameters.AddWithValue("@ph", cmbphone.Text.Trim());
                    cmd.Parameters.AddWithValue("@gen", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@add", textBox1.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record updated successfully!");
                    LoadData();
                    ClearInputs();
                }
            }
            catch (Exception ex) { MessageBox.Show("Update Error: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPatientID.Text)) return;

            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "DELETE FROM Patients WHERE PatientID=@id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", txtPatientID.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record deleted successfully!");
                        LoadData();
                        ClearInputs();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Delete Error: " + ex.Message); }
            }
        }

        private void ClearInputs(object sender = null, EventArgs e = null)
        {
            if (Home.UserRole == "Manager") return;

            txtPatientName.Clear();
            txtAge.Clear();
            cmbphone.Clear();
            textBox1.Clear();
            textBoxSearch.Clear();
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = -1;
            GetNextID();
        }

        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPatients.Rows[e.RowIndex];
                txtPatientID.Text = Convert.ToString(row.Cells[0].Value);
                txtPatientName.Text = Convert.ToString(row.Cells[1].Value);
                txtAge.Text = Convert.ToString(row.Cells[2].Value);
                comboBox1.Text = Convert.ToString(row.Cells[3].Value);
                cmbphone.Text = Convert.ToString(row.Cells[4].Value);
                textBox1.Text = Convert.ToString(row.Cells[5].Value);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearInputs();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                LoadData();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT PatientID, PatientName, Age, Gender, Phone, Address FROM Patients WHERE PatientID = @id";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@id", textBoxSearch.Text.Trim());

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPatients.DataSource = dt;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No patient found with this ID.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: Please ensure you entered a valid numeric ID. " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }
    }
}