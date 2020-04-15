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
using System.Net.Mail;
using System.Net;
namespace BilgisayarTakipProgramı
{
    public partial class SifreOlustur : Form
    {
        public SifreOlustur()
        {
            InitializeComponent();
        }
        public static string text3;
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'");
        OleDbCommand sorgu = new OleDbCommand();
        OleDbDataReader dr;
        string sql, sifreGirildi;
        int sifreAyni, sifreAyni2, eposta;

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Lütfen E Posta Türünüzü Seçiniz");
            }
            else
            {
                MessageBox.Show("Kullanıcı Oluşturuldu");
                timer1.Start();
                baglan.Close();
                sifreGirildi = "a";
                baglan.Open();
                sorgu.Connection = baglan;
                sql = "insert into VeriTabani(sifre,eposta,sifregirildi) values('" + textBox1.Text.Trim() + "','" + textBox3.Text.Trim() + comboBox1.Text + "','" + sifreGirildi + "')";
                sorgu.CommandText = sql;
                sorgu.ExecuteNonQuery();
                baglan.Close();
                
            }
        }

        private void SifreOlustur_Load(object sender, EventArgs e)
        {
            Mutex Mtx = new Mutex(false, "SINGLE_INSTANCE_APP_MUTEX");
            if (Mtx.WaitOne(0, false) == false)
            {

                Mtx.Close();

                Mtx = null;

                MessageBox.Show("Program Zaten Açık");

                Application.Exit();

            }
            comboBox1.Items.Add("@gmail.com");
            comboBox1.Items.Add("@hotmail.com");
            comboBox1.Items.Add("@hotmail.com.tr");
            comboBox1.Items.Add("@windowslive.com");
            comboBox1.Items.Add("@yandex.com");
            comboBox1.Items.Add("@yahoo.com");
            timer1.Start();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from VeriTabani";
            dr = sorgu.ExecuteReader();
            dr.Read();
            sifreGirildi = dr[2].ToString();
            baglan.Close();
            //

            //
            label5.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            button1.Enabled = false;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            textBox1.Text = textBox1.Text.Trim();
            if (textBox1.Text == textBox2.Text && textBox1.Text != "")
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
                sifreAyni2 = 1;
                sifreAyni = 1;
            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
                sifreAyni2 = 0;
                sifreAyni = 0;
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            textBox2.Text = textBox2.Text.Trim();
            if (textBox1.Text == textBox2.Text && textBox2.Text != "")
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
                sifreAyni = 1;
                sifreAyni2 = 1;
            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
                sifreAyni = 0;
                sifreAyni2 = 0;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.PasswordChar = '*';
                textBox2.PasswordChar = '*';
            }
            else
            {
                textBox1.PasswordChar = '\0';
                textBox2.PasswordChar = '\0';
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            int i;
            textBox3.Text = textBox3.Text.Trim();
            for (i = 0; i < textBox3.Text.Length; i++)
            {


                if (textBox3.Text.Substring(i, 1) == "@")
                {
                    pictureBox3.Visible = true;
                    eposta = 0;
                    label5.Visible = true;
                    label5.Text = "Lütfen e postanızı düzgün biçimde giriniz.";

                }
                else
                {
                    pictureBox3.Visible = false;
                    eposta = 1;
                    label5.Visible = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sifreGirildi == "a")
            {
                timer1.Stop();
                this.Hide();
                Giriş frm = new Giriş();
                frm.Show();
            }
            else
            {
                this.Opacity = 100;
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || sifreAyni == 0 || sifreAyni2 == 0 || eposta == 0)
                {
                    button1.Enabled = false;
                }
                else
                {
                    button1.Enabled = true;
                }
            }  
        }

        private void SifreOlustur_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
