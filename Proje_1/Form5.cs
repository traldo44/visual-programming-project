using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite; // SQLite Kütüphanesi
using System.Windows.Forms.DataVisualization.Charting; // Grafik Kütüphanesi

namespace Proje_1
{
    public partial class Form5 : Form
    {
        // Veritabanı Yolu
        string dbFileName = "P_information.db";
        string connectionString;

        public Form5()
        {
            InitializeComponent();
            connectionString = $"Data Source={dbFileName};Version=3;";

            // Form yüklenirken grafikleri ve istatistikleri doldur
            GrafikleriDoldur();
            IstatistikleriHesapla();
        }

        // --- 1. GRAFİKLERİ DOLDURMA (YÜZDELİKLİ) ---
        private void GrafikleriDoldur()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // ---------------------------------------------------------
                    // CHART 1: INCOME (GELİR) -> PIE CHART
                    // ---------------------------------------------------------
                    chart1.Series.Clear();
                    Series seriesIncome = new Series("Incomes");
                    seriesIncome.ChartType = SeriesChartType.Pie;

                    // Yüzdelik Ayarı: #VALX (İsim), #PERCENT{P1} (Yüzde)
                    seriesIncome.Label = "#VALX\n#PERCENT{P1}";
                    seriesIncome.LegendText = "#VALX";

                    chart1.Series.Add(seriesIncome);

                    // Gelir verilerini çek
                    string sqlIncome = "SELECT Category, SUM(CAST(Amount AS REAL)) as Total FROM Transactions WHERE Username=@user AND Type='Income' GROUP BY Category";

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlIncome, connection))
                    {
                        cmd.Parameters.AddWithValue("@user", Form3.AktifKullanici);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string cat = dr["Category"].ToString();
                                double amt = Convert.ToDouble(dr["Total"]);
                                seriesIncome.Points.AddXY(cat, amt);
                            }
                        }
                    }

                    // ---------------------------------------------------------
                    // CHART 2: EXPENSE (GİDER) -> PIE CHART
                    // ---------------------------------------------------------
                    chart2.Series.Clear();
                    Series seriesExpense = new Series("Expenses");
                    seriesExpense.ChartType = SeriesChartType.Pie;

                    // Yüzdelik Ayarı
                    seriesExpense.Label = "#VALX\n#PERCENT{P1}";
                    seriesExpense.LegendText = "#VALX";

                    chart2.Series.Add(seriesExpense);

                    // Gider verilerini çek
                    string sqlExpense = "SELECT Category, SUM(CAST(Amount AS REAL)) as Total FROM Transactions WHERE Username=@user AND Type='Expense' GROUP BY Category";

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlExpense, connection))
                    {
                        cmd.Parameters.AddWithValue("@user", Form3.AktifKullanici);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string cat = dr["Category"].ToString();
                                double amt = Convert.ToDouble(dr["Total"]);
                                seriesExpense.Points.AddXY(cat, amt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chart Error: " + ex.Message);
                }
            }
        }

        // --- 2. İSTATİSTİKLER (EN ÇOK / EN AZ) ---
        private void IstatistikleriHesapla()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // En Çok Harcanan Kategori (Expense, DESC)
                    label8.Text = GetCategoryStat(connection, "Expense", "DESC");

                    // En Yüksek Gelir Kaynağı (Income, DESC)
                    label9.Text = GetCategoryStat(connection, "Income", "DESC");

                    // En Az Harcanan Kategori (Expense, ASC)
                    label10.Text = GetCategoryStat(connection, "Expense", "ASC");

                    // En Az Gelir Kaynağı (Income, ASC)
                    label11.Text = GetCategoryStat(connection, "Income", "ASC");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Stats Error: " + ex.Message);
                }
            }
        }

        // Yardımcı Fonksiyon
        private string GetCategoryStat(SQLiteConnection conn, string type, string order)
        {
            string sql = $@"SELECT Category FROM Transactions 
                            WHERE Username=@user AND Type='{type}' 
                            GROUP BY Category 
                            ORDER BY SUM(CAST(Amount AS REAL)) {order} 
                            LIMIT 1";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@user", Form3.AktifKullanici);
                object result = cmd.ExecuteScalar();

                if (result != null)
                    return result.ToString();
                else
                    return "None";
            }
        }

        // --- BUTON 1: MENÜYE DÖN (FORM 2) ---
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        // --- BOŞ EVENTLER ---
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void chart1_Click(object sender, EventArgs e) { }
        private void chart2_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
    }
}