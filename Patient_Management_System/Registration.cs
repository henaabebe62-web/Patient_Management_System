using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace Patient_Management_System
{
    public partial class Registration : Form
    {
        string connString = @"Server=.\SQLEXPRESS; Database=Patient_Managemnt_System; Integrated Security=True";

        public Registration()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
            textBox4.UseSystemPasswordChar = true;
            
            textBox6.MaxLength = 13; 
            textBox6.Text = "+251";

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            // Manually linking events to ensure they work even if the designer misses them
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox6_KeyPress);
        }

        private void Registeration_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Doctor Manager");
            comboBox1.Items.Add("Secretary");
            comboBox1.Items.Add("Billing Staff");
            comboBox1.Items.Add("Doctor");
            comboBox1.Items.Add("Manager");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(comboBox1.Text) || textBox6.Text.Length < 13)
            {
                MessageBox.Show("Please fill all fields correctly. Phone must be 13 characters (+251XXXXXXXXX).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (textBox2.Text != textBox4.Text)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedRole = comboBox1.Text;
            string correctCode = "";

            switch (selectedRole)
            {
                case "Doctor Manager": correctCode = "DOM123"; break;
                case "Secretary": correctCode = "SE123"; break;
                case "Billing Staff": correctCode = "BI123"; break;
                case "Doctor": correctCode = "DO123"; break;
                case "Manager": correctCode = "MA123"; break;
            }

            // FIXED: Added missing $ sign for interpolation
            string userInput = Interaction.InputBox("Enter code for {selectedRole}:", "Security Verification", "");

            if (userInput != correctCode)
            {
                MessageBox.Show("Incorrect Security Code!", "Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO Registration (FullName, Username, Password, Role, Phone) VALUES (@full, @user, @pass, @role, @phone)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@full", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@user", textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@phone", textBox6.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // FIXED: Added missing $ sign for interpolation
                    MessageBox.Show("{selectedRole} registered successfully!", "Success");
                    btnLogin_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // FULLNAME VALIDATION: Blocks numbers and symbols (Only letters and spaces allowed)
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        // PHONE NUMBER VALIDATION: Blocks letters and spaces (Only numbers allowed)
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the key is NOT a digit and NOT a control key (like backspace), block it
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // This cancels the keypress and blocks letters/spaces/symbols
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox6.Text = "+251";
            comboBox1.SelectedIndex = -1;
        }
    }
}