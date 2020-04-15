using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.OleDb;
using Microsoft.Win32;
namespace BilgisayarTakipProgramı
{
    public partial class Giriş : Form
    {
        public Giriş()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'");
        OleDbCommand sorgu = new OleDbCommand();
        BilgisayarTakipProgramı.VeriTabaniniCek cek = new BilgisayarTakipProgramı.VeriTabaniniCek();
        string sifreCek;
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SifreDegistirme frm = new SifreDegistirme();
            frm.Show();
            this.Hide();  
        }
        void cek1()
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.Text = "Şifreyi Göster";
                textBox1.PasswordChar = '*';
            }
            else
            {
                checkBox1.Text = "Şifreyi Gizle";
                textBox1.PasswordChar = '\0';
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cek1(); 
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EpostaDegisim frm = new EpostaDegisim();
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Trim();
            //try
            //{
                if (sifreCek == textBox1.Text)
                {
                    MessageBox.Show("Şifre Doğru");
                    KayıtFormu frm = new KayıtFormu();
                    notifyIcon1.Visible = false;
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Şifre Yanlış");
                }
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Lütfen Şifreyi Giriniz");
            //}
        }

        private void Giriş_Load(object sender, EventArgs e)
        {
            this.Show();
            Mutex Mtx = new Mutex(false, "SINGLE_INSTANCE_APP_MUTEX");
            if (Mtx.WaitOne(0, false) == false)
            {

                Mtx.Close();

                Mtx = null;

                MessageBox.Show("Program Zaten Açık");
                Application.Exit();
            }
            else
            {
                this.Opacity = 100;
            }
            checkBox1.Text = "Şifreyi Göster";
            checkBox1.Checked = true;
            goruntuAl frm2 = new goruntuAl();
            frm2.Show();
            sifreCek = cek.CekSifEp(sifreCek, 0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            YardımVeSorun frm = new YardımVeSorun();
            frm.ShowDialog();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
