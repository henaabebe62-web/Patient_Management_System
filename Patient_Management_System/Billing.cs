using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Patient_Management_System
{
    public partial class Billing : Form
    {
        string connString = @"Server=.\SQLEXPRESS; Database=Patient_Managemnt_System; Integrated Security=True";

        public Billing()
        {
            InitializeComponent();
            SetupGrid();
            StyleButtons();
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void StyleButtons()
        {
            btnUpdate.BackColor = Color.Green;
            btnUpdate.ForeColor = Color.White;
            btnDelete.BackColor = Color.Red;
            btnDelete.ForeColor = Color.White;
        }

        private void SetupGrid()
        {
            dgvBilling.AutoGenerateColumns = false;
            dgvBilling.Columns.Clear();
            dgvBilling.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBilling.MultiSelect = false;
            dgvBilling.AllowUserToAddRows = false;
            dgvBilling.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvBilling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Bill ID", DataPropertyName = "BillID", Name = "BillID" });
            dgvBilling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Patient ID", DataPropertyName = "PatientID", Name = "PatientID" });
            dgvBilling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Amount", DataPropertyName = "Amount", Name = "Amount" });
            dgvBilling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Status", DataPropertyName = "PaymentStatus", Name = "PaymentStatus" });
            dgvBilling.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date", DataPropertyName = "BillDate", Name = "BillDate" });
        }

        private void Billing_Load(object sender, EventArgs e)
        {
            LoadBillingData();

            if (Home.UserRole == "Manager")
            {
                ApplyManagerReadOnlyMode();
            }
            else
            {
                ApplyBillingStaffActiveMode();
            }
        }

        private void ApplyManagerReadOnlyMode()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            txtPatientID.ReadOnly = true;
            txtPayment.ReadOnly = true;
            cmbStatus.Enabled = false;

            dgvBilling.ReadOnly = true;
        }

        private void ApplyBillingStaffActiveMode()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            txtPatientID.ReadOnly = false;
            txtPayment.ReadOnly = false;
            cmbStatus.Enabled = true;

            dgvBilling.ReadOnly = false;
        }

        private void LoadBillingData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT * FROM Billing";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvBilling.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
        }

        private bool IsPatientExists(string id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT COUNT(*) FROM Patients WHERE PatientID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id.Trim());
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (!ValidateInputs()) return;

            if (!IsPatientExists(txtPatientID.Text))
            {
                MessageBox.Show("This Patient ID was not found in the system! Please register the patient first.", "Unregistered Patient", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO Billing (PatientID, Amount, PaymentStatus, BillDate) VALUES (@pid, @amt, @status, @date)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@pid", txtPatientID.Text.Trim());
                    cmd.Parameters.AddWithValue("@amt", decimal.Parse(txtPayment.Text.Trim()));
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Payment successfully registered!");
                    LoadBillingData();
                    ClearFields();
                }
            }
            catch (Exception ex) { MessageBox.Show("Add Error: " + ex.Message); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (dgvBilling.CurrentRow == null) return;
            if (!ValidateInputs()) return;

            string billId = dgvBilling.CurrentRow.Cells["BillID"].Value.ToString();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "UPDATE Billing SET PatientID=@pid, Amount=@amt, PaymentStatus=@status WHERE BillID=@bid";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@pid", txtPatientID.Text.Trim());
                    cmd.Parameters.AddWithValue("@amt", decimal.Parse(txtPayment.Text.Trim()));
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@bid", billId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Information updated!");
                    LoadBillingData();
                    ClearFields();
                }
            }
            catch (Exception ex) { MessageBox.Show("Update Error: " + ex.Message); }
        }

        private void dgvBilling_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBilling.Rows[e.RowIndex];
                txtPatientID.Text = row.Cells["PatientID"].Value.ToString();
                txtPayment.Text = row.Cells["Amount"].Value.ToString();
                cmbStatus.Text = row.Cells["PaymentStatus"].Value.ToString();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtPatientID.Text) ||
                string.IsNullOrWhiteSpace(txtPayment.Text) ||
                cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in the Patient ID, payment amount, and status correctly!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            if (Home.UserRole == "Manager") return;

            txtPatientID.Clear();
            txtPayment.Clear();
            cmbStatus.SelectedIndex = -1;
            txtPatientID.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e) { ClearFields(); }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Home.UserRole == "Manager") return;

            if (dgvBilling.CurrentRow == null) return;

            if (MessageBox.Show("Are you sure you want to delete this payment information?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string billId = dgvBilling.CurrentRow.Cells["BillID"].Value.ToString();
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string query = "DELETE FROM Billing WHERE BillID=@bid";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@bid", billId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Information deleted!");
                        LoadBillingData();
                        ClearFields();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Delete Error: " + ex.Message); }
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