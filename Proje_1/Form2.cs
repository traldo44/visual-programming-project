using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proje_1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        // --- BUTTON 1: Open Form3 (Change Password) ---
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        // --- BUTTON 2: Open Form4 (Transactions) ---
        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        // --- BUTTON 4: Open Form5 (Analysis/Graphs) ---
        private void button4_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }

        // --- BUTTON 5: Logout / Back to Form1 ---
        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        // --- LABEL CLICKS (Triggers Buttons) ---
        private void label3_Click(object sender, EventArgs e) { button1.PerformClick(); }
        private void label4_Click(object sender, EventArgs e) { button1.PerformClick(); }
        private void label5_Click(object sender, EventArgs e) { button2.PerformClick(); }
        private void label6_Click(object sender, EventArgs e) { button2.PerformClick(); }
        private void label7_Click(object sender, EventArgs e) { button4.PerformClick(); }
        private void label8_Click(object sender, EventArgs e) { button4.PerformClick(); }

        // --- EMPTY METHODS ---
        private void button3_Click(object sender, EventArgs e) { }
    }
}