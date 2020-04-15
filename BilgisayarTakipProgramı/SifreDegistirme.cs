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
    public partial class SifreDegistirme : Form
    {
        public SifreDegistirme()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'");
        OleDbCommand sorgu = new OleDbCommand();
        string gmail, kodcek;
        BilgisayarTakipProgramı.MailAt at = new BilgisayarTakipProgramı.MailAt();
        BilgisayarTakipProgramı.Sifre at2 = new BilgisayarTakipProgramı.Sifre();
        BilgisayarTakipProgramı.VeriTabaniniCek epsif = new BilgisayarTakipProgramı.VeriTabaniniCek();
        BilgisayarTakipProgramı.VeriTabaniniCek cek = new BilgisayarTakipProgramı.VeriTabaniniCek();
        void sifredegis()
        {
            gmail = cek.CekSifEp(gmail, 1);
            gmail = at.mail(gmail, 2, kodcek);
            MessageBox.Show("E postanıza Sıfırlama Kodunuz Gönderilmiştir");
            baglan.Close();
        }
        private void SifreDegistirme_Load(object sender, EventArgs e)
        {
            Mutex Mtx = new Mutex(false, "SINGLE_INSTANCE_APP_MUTEX");
            if (Mtx.WaitOne(0, false) == false)
            {

                Mtx.Close();

                Mtx = null;

                MessageBox.Show("Program Zaten Açık");

                Application.Exit();

            }
            button2.Location = new Point(106, 82);            
            textBox1.Visible = false;
            timer1.Start();
            button3.Enabled = false; 
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            kodcek = at2.GenerateNewPassword();
            sifredegis();
            button2.Location = new Point(149, 118);
            textBox1.Visible = true;
            label1.Text = "E Postanıza Gönderilen Kodu Giriniz";
            button2.Text = "Tekrar Gönder";
            button1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == kodcek)
            {
                MessageBox.Show("Kodlar Uyuştu");
                groupBox1.Location = new Point(800, 800);
                groupBox2.Location = new Point(49, 38);
                groupBox2.Visible = true;
            }
            else
            {
                MessageBox.Show("Kodlar Uyuşmuyor");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Lütfen E Postanızı Yazınız");
            }
            else
            {
                textBox2.Text = epsif.Yenile(textBox2.Text, 1);
                MessageBox.Show("Şifreniz Yenilendi");
                Giriş frm = new Giriş();
                frm.Show();
                this.Hide();
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox2.Text == textBox3.Text && textBox2.Text != "")
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox2.Text == textBox3.Text && textBox3.Text != "")
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Enabled == true)
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.PasswordChar = '*';
                textBox3.PasswordChar = '*';
            }
            else
            {
                textBox2.PasswordChar = '\0';
                textBox3.PasswordChar = '\0';
            }
        }

        private void SifreDegistirme_FormClosed(object sender, FormClosedEventArgs e)
        {
            Giriş frm = new Giriş();
            frm.Show();
            this.Hide();
        }
    }
}
