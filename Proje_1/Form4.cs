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
    public partial class Form4 : Form
    {
        string dbFileName = "P_information.db";
        string connectionString;

        public Form4()
        {
            InitializeComponent();
            connectionString = $"Data Source={dbFileName};Version=3;";

            button3.BackgroundImageLayout = ImageLayout.Stretch;

            InitialSettings();
        }

        private void InitialSettings()
        {
            label8.Text = "0.00";
            label9.Text = "0.00"; 
            label10.Text = "0.00"; 

            PrepareTable();

            ListData();
        }

        private void PrepareTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = @"CREATE TABLE IF NOT EXISTS Transactions (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                                    Username TEXT, 
                                    Date TEXT, 
                                    Type TEXT, 
                                    Category TEXT, 
                                    Description TEXT, 
                                    Amount TEXT)";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Table creation error: " + ex.Message);
                }
            }
        }

        private void ListData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT * FROM Transactions WHERE Username=@user";

                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, connection))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@user", Form3.AktifKullanici);

                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }

                    CalculateAndPrint();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Data listing error: " + ex.Message);
                }
            }
        }

        private void CalculateAndPrint()
        {
            double totalIncome = 0;
            double totalExpense = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Type"].Value != null && row.Cells["Amount"].Value != null)
                {
                    string type = row.Cells["Type"].Value.ToString();
                    string amountStr = row.Cells["Amount"].Value.ToString();
                    double amount;

                    if (double.TryParse(amountStr, out amount))
                    {
                        if (type == "Income")
                            totalIncome += amount;
                        else if (type == "Expense")
                            totalExpense += amount;
                    }
                }
            }

            label8.Text = totalIncome.ToString("N2");
            label9.Text = totalExpense.ToString("N2");
            double balance = totalIncome - totalExpense;
            label10.Text = balance.ToString("N2");

            if (balance < 0) label10.ForeColor = Color.Red;
            else label10.ForeColor = Color.Green;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrepareTable();

            if (string.IsNullOrWhiteSpace(maskedTextBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                comboBox1.SelectedIndex == -1 ||
                comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double checkNumber;
            if (!double.TryParse(textBox2.Text, out checkNumber))
            {
                
                MessageBox.Show("The Amount field must contain only numbers!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Transactions (Username, Date, Type, Category, Description, Amount) VALUES (@user, @date, @type, @cat, @desc, @amount)";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@user", Form3.AktifKullanici);
                        command.Parameters.AddWithValue("@date", maskedTextBox1.Text);
                        command.Parameters.AddWithValue("@type", comboBox1.Text);
                        command.Parameters.AddWithValue("@cat", comboBox2.Text);
                        command.Parameters.AddWithValue("@desc", textBox1.Text);
                        command.Parameters.AddWithValue("@amount", textBox2.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Record Saved Successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ListData(); 
                        ClearFields(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save error: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(maskedTextBox2.Text))
            {
                MessageBox.Show("Please enter the ID of the record to delete.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult response = MessageBox.Show($"Are you sure you want to delete record ID: {maskedTextBox2.Text}?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (response == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "DELETE FROM Transactions WHERE Id = @id AND Username = @user";

                        using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", maskedTextBox2.Text);
                            command.Parameters.AddWithValue("@user", Form3.AktifKullanici);

                            int affectedRows = command.ExecuteNonQuery();

                            if (affectedRows > 0)
                            {
                                MessageBox.Show("Record deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ListData();
                                maskedTextBox2.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Record not found or you don't have permission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            maskedTextBox1.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void Form4_Load(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
    }
}