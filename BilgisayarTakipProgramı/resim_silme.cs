using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
namespace BilgisayarTakipProgramı
{
    public partial class resim_silme : Form
    {
        public resim_silme()
        {
            InitializeComponent();
        }
        BilgisayarTakipProgramı.VeriTabaniniCek cek3 = new BilgisayarTakipProgramı.VeriTabaniniCek();

        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'");
        OleDbCommand sorgu = new OleDbCommand();
        OleDbDataReader dr;
        int kayit_adedi;
        string saat_ismi;
        string resim_yolu;
        void listUstSecim()
        {
            try
            {
                listBox1.SetSelected(0, true);
            }
            catch
            { }
        }
        void cek()
        {
            kayit_adedi = 0;           
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
                saat_ismi = listBox1.Text;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seçili Resmi Silmek İstediğinize Eminmisiniz?", "Dikkat!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.BackgroundImage = null;
                    listBox1.Items.Clear();
                    string kayit_yolu = @"C:\Windows2\" + saat_ismi + ".jpg";             
                    baglan.Close();
                    baglan.Open();
                    sorgu.Connection = baglan;
                    sorgu.CommandText = "delete * from resimCek where Saat='" + saat_ismi + "'";
                    sorgu.ExecuteNonQuery();
                    baglan.Close();
                    pictureBox1.Image = null;
                    kayit_yolu = "";
                    cek();
                    saat_ismi = "";
                    resim_silme frm = new resim_silme();
                    this.Hide();
                    frm.Show();
                   
                }
                catch
                {
                    string kayit_yolu = @"C:\Windows2\" + saat_ismi + ".jpg";
                    Image.FromFile(kayit_yolu).Dispose();  
                }
                finally
                {
                    string kayit_yolu = @"C:\Windows2\" + saat_ismi + ".jpg";
                    File.Delete(kayit_yolu);            
                }
            }
        }
        string resim_yolu2;
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
                Image.FromFile(resim_yolu2.ToString()).Dispose();
                saat_ismi = "";
            }
        }

        private void resim_silme_FormClosed(object sender, FormClosedEventArgs e)
        {
            KayıtFormu frm = new KayıtFormu();
            frm.Show();
            this.Hide();
        }

        private void resim_silme_Load(object sender, EventArgs e)
        {
            cek();
            timer1.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {                
                cek();              
                
            }
            catch
            { }
        }

        public Microsoft.Win32.SafeHandles.SafeFileHandle server { get; set; }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (kayit_adedi <= 0) 
            {                
                KayıtFormu frm = new KayıtFormu();
                frm.Show();
                this.Close();
            }
        }
    }
}
