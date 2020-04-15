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
namespace BilgisayarTakipProgramı
{
    public partial class EpostaDegisim : Form
    {
        public EpostaDegisim()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'");
        OleDbCommand sorgu = new OleDbCommand();
        string eposta, sifre, kodcek, tEposta;
        BilgisayarTakipProgramı.MailAt at = new BilgisayarTakipProgramı.MailAt();
        BilgisayarTakipProgramı.Sifre at2 = new BilgisayarTakipProgramı.Sifre();
        BilgisayarTakipProgramı.VeriTabaniniCek Epsif = new BilgisayarTakipProgramı.VeriTabaniniCek();
        BilgisayarTakipProgramı.VeriTabaniniCek cek = new BilgisayarTakipProgramı.VeriTabaniniCek();

        private void EpostaDegisim_Load(object sender, EventArgs e)
        {
            Mutex Mtx = new Mutex(false, "SINGLE_INSTANCE_APP_MUTEX");
            if (Mtx.WaitOne(0, false) == false)
            {

                Mtx.Close();

                Mtx = null;

                MessageBox.Show("Program Zaten Açık");

                Application.Exit();

            }
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            comboBox1.Items.Add("@gmail.com");
            comboBox1.Items.Add("@hotmail.com");
            comboBox1.Items.Add("@hotmail.com.tr");
            comboBox1.Items.Add("@windowslive.com");
            comboBox1.Items.Add("@yandex.com");
            comboBox1.Items.Add("@yahoo.com");
            //
            comboBox2.Items.Add("@gmail.com");
            comboBox2.Items.Add("@hotmail.com");
            comboBox2.Items.Add("@hotmail.com.tr");
            comboBox2.Items.Add("@windowslive.com");
            comboBox2.Items.Add("@yandex.com");
            comboBox2.Items.Add("@yahoo.com");
            timer1.Start();
            kodcek = at2.GenerateNewPassword();
            sifre = cek.CekSifEp(sifre, 0);
            eposta = cek.CekSifEp(eposta, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Lütfen E Posta Türünüzü Seçiniz");
            }
            else
            {
                if (textBox1.Text + comboBox1.Text == eposta && textBox2.Text == sifre)
                {
                    textBox1.Text = at.mail(textBox1.Text + comboBox1.Text, 1, kodcek);
                    tEposta = eposta;
                    textBox1.Clear();
                    comboBox1.Text = "";
                    textBox2.Clear();
                    MessageBox.Show("Onaylandı");
                    MessageBox.Show("E postanıza Sıfırlama Kodunuz Gönderilmiştir");
                    groupBox1.Visible = false;
                    groupBox1.Location = new Point(800, 800);
                    groupBox2.Location = new Point(106, 71);
                    groupBox2.Visible = true;

                }
                else
                {
                    MessageBox.Show("E Posta veya Şifre Yanlış");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.PasswordChar = '*';

            }
            else
            {
                textBox2.PasswordChar = '\0';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == kodcek)
            {
                MessageBox.Show("Kod Doğrulandı");
                groupBox2.Visible = false;
                groupBox2.Location = new Point(800, 800);
                groupBox3.Location = new Point(31, 64);
                groupBox3.Visible = true;
            }
            else
            {
                MessageBox.Show("Kod Yanlış");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("E Posta Türünüz veya E Postanız Girilmemiş");
            }
            else
            {
                textBox4.Text = Epsif.Yenile(textBox4.Text + comboBox2.Text, 2);
                MessageBox.Show("E Posta Değiştirildi");
                textBox4.Text = "";
                Giriş frm = new Giriş();
                this.Hide();
                frm.Show();
            }
        }

        private void EpostaDegisim_FormClosed(object sender, FormClosedEventArgs e)
        {
            Giriş frm = new Giriş();
            this.Hide();
            frm.Show();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                if (textBox1.Text.Substring(i, 1) == "@")
                {
                    pictureBox1.Visible = true;

                }
                else
                {
                    pictureBox1.Visible = false;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            kodcek = at2.GenerateNewPassword();
            tEposta = at.mail(tEposta, 1, kodcek);
            MessageBox.Show("Tekrar Gönderildi");
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox4.Text == "@")
            {
                button3.Enabled = false;
                pictureBox2.Visible = true;
            }
            else
            {
                button3.Enabled = true;
                pictureBox2.Visible = false;
            }
        }

    }
}
