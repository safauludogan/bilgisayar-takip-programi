using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Drawing.Imaging;
using Microsoft.VisualBasic;
using System.Diagnostics;
namespace BilgisayarTakipProgramı
{
    public partial class KayıtFormu : Form
    {
        public KayıtFormu()
        {
            InitializeComponent();
        }
        BilgisayarTakipProgramı.VeriTabaniniCek cek3 = new BilgisayarTakipProgramı.VeriTabaniniCek();
        BilgisayarTakipProgramı.VeriTabaniniCek acKapatCek = new BilgisayarTakipProgramı.VeriTabaniniCek();
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'");
        OleDbCommand sorgu = new OleDbCommand();
        public static string _resimsakla, acKapat, resim_yolu, resim_yolu2;
        DataSet al = new DataSet();
        DataView goster = new DataView();
        OleDbDataReader dr;
        int kayit_adedi;
        string saat_ismi, acik_kapali;
        string resme_bak, saat_ismi2,  resim_yolu3;
        void listUstSecim()
        {
            try
            {
                listBox1.SetSelected(0, true);
            }
            catch
            { }
        }
        void cek2()
        {
            for (int j = 0; j < kayit_adedi; j++)
                {
                    resme_bak = listBox1.Items[j].ToString();
                    listBox1_SelectedIndexChanged(resme_bak, new EventArgs());
                    saat_ismi2 = (resme_bak[30].ToString() + resme_bak[31].ToString() + resme_bak[32].ToString() + resme_bak[33].ToString() + resme_bak[34].ToString() + resme_bak[35].ToString() + resme_bak[36].ToString() + resme_bak[37].ToString()).ToString();
                    baglan.Close();
                    baglan.Open();
                    sorgu.Connection = baglan;
                    sorgu.CommandText = "select * from resimCek where Saat='" + saat_ismi2 + "'";
                    dr = sorgu.ExecuteReader();
                    dr.Read();
                    resim_yolu3 = dr[3].ToString();
                    try
                    {
                       Image.FromFile(resim_yolu3.ToString()).Dispose();
                       
                    }
                    catch
                    {
                        baglan.Close();
                        baglan.Open();
                        sorgu.Connection = baglan;
                        sorgu.CommandText = "delete * from resimCek where Saat='" + saat_ismi2 + "'";
                        sorgu.ExecuteNonQuery();
                    }
                    listUstSecim();
            }
        }
        void cek()
        {
            kayit_adedi = 0;
            saat_ismi = "";
            listBox1.Items.Clear();
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from resimCek";
            dr = sorgu.ExecuteReader();
            while (dr.Read())
            {
                listBox1.Items.Add("Tarih : " + dr[1] + " --- Saat : " + dr[2]);
                kayit_adedi = kayit_adedi + 1;

                if (kayit_adedi == 0)
                {
                    label1.Text = "Hiç Kayıt Yok";
                }
                else
                {
                    label1.Text = kayit_adedi + " Adet Kayıt Bulundu.";
                }
            }
            listUstSecim();

        }
        private void öneriVeyaSorunBildirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YardımVeSorun frm = new YardımVeSorun();
            frm.ShowDialog();
        }
        string sifreCek, sifre;
        private void seçeneklerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sifreCek = cek3.CekSifEp(sifreCek, 0);
            sifre = Interaction.InputBox("Yönetici İzni", "Şifrenizi Giriniz", "", 0, 0);
            if (sifreCek == sifre)
            {
               
                pictureBox1.Image = null;
                Ayarlar frm = new Ayarlar();
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Şifre Yanlış!");
            }
        }
        public string server { get; set; }

        private void KayıtFormu_Load(object sender, EventArgs e)
        {
            timer2.Start();
            cek();
            cek2();
            button1_Click(button1, new EventArgs());
            timer1.Start();
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
            listUstSecim();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {            
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from VeriTabani";
            dr = sorgu.ExecuteReader();
            dr.Read();
            acik_kapali = dr[3].ToString();
            if (acik_kapali == "a")
            {
                listBox1.Items.Clear();
                cek();
                cek2();
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from resimCek where Tarih='" + dateTimePicker1.Text + "'";
            dr = sorgu.ExecuteReader();
            listBox1.Text = "";
            kayit_adedi = 0;
            while (dr.Read())
            {
                listBox1.Items.Add("Tarih : " + dr["Tarih"] + " --- Saat : " + dr["Saat"]);
                kayit_adedi = kayit_adedi + 1;
            }
            label1.Text = kayit_adedi + " Adet Kayıt Bulundu.";
            listUstSecim();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {           
            try
            {
                try
                {
                   
                    saat_ismi = (listBox1.Text[30].ToString() + listBox1.Text[31].ToString() + listBox1.Text[32].ToString() + listBox1.Text[33].ToString() + listBox1.Text[34].ToString() + listBox1.Text[35].ToString() + listBox1.Text[36].ToString() + listBox1.Text[37].ToString()).ToString();
                    baglan.Close();
                    baglan.Open();
                    sorgu.Connection = baglan;
                    sorgu.CommandText = "select * from resimCek where Saat='" + saat_ismi + "'";
                    dr = sorgu.ExecuteReader();
                    dr.Read();
                    resim_yolu2 = dr[3].ToString();
                    baglan.Close();
                    baglan.Close();
                    resim_yolu = listBox1.Text;                    
                    if (listBox1.Text != "")
                    {
                        try
                        {

                            pictureBox1.Image = Image.FromFile(resim_yolu2);
                            Image.FromFile(resim_yolu2.ToString()).Dispose();
                        }
                        catch
                        {
                            Image.FromFile(resim_yolu2.ToString()).Dispose();
                        }
                    }
                }
                catch (InvalidOperationException)
                { Image.FromFile(resim_yolu2.ToString()).Dispose(); }

            }
            catch (IndexOutOfRangeException)
            {               
                saat_ismi = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Image = null;
                listBox1.Items.Clear();
                cek();
                cek2();                           
            }
            catch
            { }
           
        }
        int kapanma_silme_ac;
        private void KayıtFormu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (kapanma_silme_ac != 1) 
            {
                Giriş frm = new Giriş();
                frm.Show();
                this.Hide();
            }            
        }
        private void resimSilmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kayit_adedi > 0) 
            {
                kapanma_silme_ac = 1;
                sifreCek = cek3.CekSifEp(sifreCek, 0);
                sifre = Interaction.InputBox("Yönetici İzni", "Şifrenizi Giriniz", "", 0, 0);
                if (sifreCek == sifre)
                {
                    this.Hide();
                    pictureBox1.Image = null;
                    Image.FromFile(resim_yolu3.ToString()).Dispose();
                    resim_silme frm = new resim_silme();
                    frm.Show();

                }
                else
                {
                    MessageBox.Show("Şifre Yanlış!");
                }
            }
            else
            {
                MessageBox.Show("Hiç kayıt yok");
            }
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                listUstSecim();
                timer2.Stop();
            }
            catch
            { }
        }

    }
}