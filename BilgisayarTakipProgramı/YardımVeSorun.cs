using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
namespace BilgisayarTakipProgramı
{
    public partial class YardımVeSorun : Form
    {
        public YardımVeSorun()
        {
            InitializeComponent();
        }
        BilgisayarTakipProgramı.VeriTabaniniCek veriCek = new BilgisayarTakipProgramı.VeriTabaniniCek();

        private void YardımVeSorun_Load(object sender, EventArgs e)
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Alanları Doğru Doldurunuz");
            }
            else
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("suludogan1995@gmail.com");
                mail.To.Add("suludogan1995@gmail.com");
                mail.Subject = textBox1.Text;
                mail.Body = textBox3.Text + " E postası : " + textBox2.Text + comboBox1.Text;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("suludogan1995@gmail.com", "cd21600b"); smtp.EnableSsl = true;
                smtp.Send(mail);
                MessageBox.Show("Mesajınız İletildi En Kısa Süre İçinde Geri Döneceğiz");
                this.Hide();
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < textBox2.Text.Length; i++)
            {

                if (textBox2.Text.Substring(i, 1) == "@")
                {
                    pictureBox1.Visible = true;

                }
                else
                {
                    pictureBox1.Visible = false;
                }
            }
        }

    }
}
