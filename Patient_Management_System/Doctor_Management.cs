using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Patient_Management_System
{
    public partial class Doctor_Management : Form
    {
        string connString = @"Server=.\SQLEXPRESS; Database=Patient_Managemnt_System; Integrated Security=True";

        public Doctor_Management()
        {
            InitializeComponent();

            btnUpdate.BackColor = Color.Green;
            btnUpdate.ForeColor = Color.White;
            btnDelete.BackColor = Color.Red;
            btnDelete.ForeColor = Color.White;

            txtDoctorID.ReadOnly = true;
            txtDoctorID.BackColor = Color.LightGray;

            txtPhone.MaxLength = 13;

            txtPhone.KeyPress += PhoneNumbers_KeyPress;
            txtPhone.Leave += ValidateEthiopianPhone_Leave;

            txtDoctorName.KeyPress += OnlyLetters_KeyPress;
        }

        private void Doctor_Management_Load(object sender, EventArgs e)
        {
            SetupGrid();
            LoadDoctorData();
            GetNextDoctorID();

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

            txtDoctorName.ReadOnly = true;
            txtPhone.ReadOnly = true;

            cmbSpecialty.Enabled = false;

            dgvDoctors.ReadOnly = true;
        }

        private void ApplyActiveMode()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            txtDoctorName.ReadOnly = false;
            txtPhone.ReadOnly = false;

            cmbSpecialty.Enabled = true;

            dgvDoctors.ReadOnly = false;
        }

        private bool IsValidEthiopianPhone(string phone)
        {
            string pattern = @"^\+251\d{9}$";
            return Regex.IsMatch(phone, pattern);
        }

        private void ValidateEthiopianPhone_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !IsValidEthiopianPhone(txtPhone.Text))
            {
                MessageBox.Show("Invalid Format! Use +251 followed by 9 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
            }
        }

        private void PhoneNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '+')
            {
                e.Handled = true;
            }
        }

        private void OnlyLetters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SetupGrid()
        {
            dgvDoctors.Columns.Clear();
            dgvDoctors.AutoGenerateColumns = false;
            dgvDoctors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDoctors.MultiSelect = false;
            dgvDoctors.AllowUserToAddRows = false;
            dgvDoctors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Doctor ID", DataPropertyName = "DoctorID", Name = "DoctorID" });
            dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Doctor Name", DataPropertyName = "DoctorName", Name = "DoctorName" });
            dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Specialty", DataPropertyName = "Specialty", Name = "Specialty" });
            dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Phone Number", DataPropertyName = "Phone", Name = "Phone" });
        }

        private void LoadDoctorData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT DoctorID, DoctorName, Specialty, Phone FROM Doctors", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDoctors.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private void GetNextDoctorID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT ISNULL(MAX(DoctorID), 100) + 1 FROM Doctors";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    txtDoctorID.Text = cmd.ExecuteScalar().ToString();
                }
            }
            catch { txtDoctorID.Text = "101"; }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDoctorName.Text) || string.IsNullOrWhiteSpace(cmbSpecialty.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter all required information!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEthiopianPhone(txtPhone.Text))
            {
                MessageBox.Show("Please enter a valid Ethiopian phone number (+251XXXXXXXXX).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO Doctors (DoctorName, Specialty, Phone) VALUES (@name, @spec, @phone)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtDoctorName.Text.Trim());
                    cmd.Parameters.AddWithValue("@spec", cmbSpecialty.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Doctor successfully added!");
                    ClearFields();
                    LoadDoctorData();
                    GetNextDoctorID();
                }
            }
            catch (Exception ex) { MessageBox.Show("Add Error: " + ex.Message); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDoctorID.Text)) return;

            if (!IsValidEthiopianPhone(txtPhone.Text))
            {
                MessageBox.Show("Please enter a valid Ethiopian phone number (+251XXXXXXXXX).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "UPDATE Doctors SET DoctorName=@name, Specialty=@spec, Phone=@phone WHERE DoctorID=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", txtDoctorID.Text);
                    cmd.Parameters.AddWithValue("@name", txtDoctorName.Text.Trim());
                    cmd.Parameters.AddWithValue("@spec", cmbSpecialty.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Updated successfully!");
                    LoadDoctorData();
                    ClearFields();
                    GetNextDoctorID();
                }
            }
            catch (Exception ex) { MessageBox.Show("Update Error: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDoctorID.Text)) return;

            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "DELETE FROM Doctors WHERE DoctorID=@id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", txtDoctorID.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Deleted successfully!");
                        LoadDoctorData();
                        ClearFields();
                        GetNextDoctorID();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Delete Error: " + ex.Message); }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                LoadDoctorData();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT * FROM Doctors WHERE DoctorID = @id";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@id", textBox4.Text.Trim());
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDoctors.DataSource = dt;

                    if (dt.Rows.Count == 0) MessageBox.Show("No doctor found with this ID.");
                }
            }
            catch (Exception ex) { MessageBox.Show("Search Error: " + ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
            GetNextDoctorID();
            LoadDoctorData();
        }

        private void dgvDoctors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDoctors.Rows[e.RowIndex];
                txtDoctorID.Text = Convert.ToString(row.Cells["DoctorID"].Value);
                txtDoctorName.Text = Convert.ToString(row.Cells["DoctorName"].Value);
                cmbSpecialty.Text = Convert.ToString(row.Cells["Specialty"].Value);
                txtPhone.Text = Convert.ToString(row.Cells["Phone"].Value);
            }
        }

        private void ClearFields()
        {
            if (Home.UserRole == "Manager") return;

            txtDoctorName.Clear();
            txtPhone.Clear();
            cmbSpecialty.SelectedIndex = -1;
            textBox4.Clear();
            dgvDoctors.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }
    }
}