using System;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Proje_1
{
    public partial class Form3 : Form
    {
        string dbFileName = "P_information.db";
        string connectionString;

        // Form1 ve Form4 bu değişkene eriştiği için ismini 'AktifKullanici' olarak bıraktım.
        // Böylece diğer kodlarda hata almazsın.
        public static string AktifKullanici = "";

        public Form3()
        {
            InitializeComponent();
            connectionString = $"Data Source={dbFileName};Version=3;";
        }

        // --- CHANGE PASSWORD BUTTON (button1) ---
        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Check for empty fields
            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please fill in all password fields.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Check Password Match
            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("New passwords do not match. Please check again.", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // UPDATED SQL: Using 'Users', 'Password', 'Username'
                    string sql = "UPDATE Users SET Password=@newPass WHERE Username=@user";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@newPass", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@user", AktifKullanici);

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Password changed successfully! For security, please login again.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Redirect to Login Page (Form1)
                            Form1 form1 = new Form1();
                            form1.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Password could not be changed. User session not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("System Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- BACK BUTTON (button2) ---
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        // --- EMPTY EVENTS ---
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
    }
}