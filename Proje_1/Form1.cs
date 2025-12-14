using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Proje_1
{
    public partial class Form1 : Form
    {
        // Database Variables
        string dbFileName = "P_information.db";
        string connectionString;

        public Form1()
        {
            InitializeComponent();
            connectionString = $"Data Source={dbFileName};Version=3;";
            VeritabaniHazirla();
        }

        // --- DATABASE PREPARATION (ENGLISH TABLE) ---
        private void VeritabaniHazirla()
        {
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // UPDATED: Creating 'Users' table instead of 'Kullanicilar'
                    string sql = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT, Password TEXT)";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection error: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- LOGIN BUTTON (button1) ---
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please fill in username and password fields.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // UPDATED SQL: Using 'Users', 'Username', 'Password'
                    string sql = "SELECT * FROM Users WHERE Username=@user AND Password=@pass";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@user", textBox1.Text.Trim());
                        command.Parameters.AddWithValue("@pass", textBox2.Text.Trim());

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MessageBox.Show("Login Successful! Welcome.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Save logged-in user (Updated column name)
                                Form3.AktifKullanici = reader["Username"].ToString();

                                Form2 form2 = new Form2();
                                form2.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- REGISTER BUTTON (button2) ---
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please fill in all fields to register.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Check if username exists (Updated SQL)
                    string checkSql = "SELECT COUNT(*) FROM Users WHERE Username=@user";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkSql, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@user", textBox3.Text.Trim());
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("This username is already taken. Please choose another.", "Username Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert New User (Updated SQL)
                    string sql = "INSERT INTO Users (Username, Password) VALUES (@user, @pass)";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@user", textBox3.Text.Trim());
                        command.Parameters.AddWithValue("@pass", textBox4.Text.Trim());
                        command.ExecuteNonQuery();

                        MessageBox.Show("Account created successfully! You can now login.", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        textBox3.Clear();
                        textBox4.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- EMPTY EVENTS ---
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}