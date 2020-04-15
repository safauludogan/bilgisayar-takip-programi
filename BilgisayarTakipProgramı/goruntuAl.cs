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
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Net;
using System.Net.Mail;
namespace BilgisayarTakipProgramı
{
    public partial class goruntuAl : Form
    {
        public goruntuAl()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'");
        OleDbCommand sorgu = new OleDbCommand();
        public static string _resimsakla,acKapa;
        DataSet al = new DataSet();
        OleDbDataReader dr;
        DataView goster = new DataView();
        int i, sure2;
        string saat, saatiDuzenle;
        void cek()
        {
            timer1.Start();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from Kayit";
            dr = sorgu.ExecuteReader();
            dr.Read();
            sure2 = int.Parse(dr[0].ToString());
            baglan.Close();
        }
        void cek2()
        {
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from VeriTabani";
            dr = sorgu.ExecuteReader();
            dr.Read();
            acKapa = dr[3].ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sure2 -= 1;
            if (sure2 <= 0)
            {
                timer1.Stop();
                button1_Click(button1, new EventArgs());
                cek();
            }
        }

        private void goruntuAl_Load(object sender, EventArgs e)
        {
            Mutex Mtx = new Mutex(false, "SINGLE_INSTANCE_APP_MUTEX");
            if (Mtx.WaitOne(0, false) == false)
            {

                Mtx.Close();

                Mtx = null;

                Application.Exit();

            }
            //
            this.ShowInTaskbar = false;
            timer1.Start();
            timer2.Start();
            cek();
            Directory.CreateDirectory(@"C:\\" + "Windows2");
            saat = DateTime.Now.ToLongTimeString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(@"C:\\" + "Windows2");
            saat = DateTime.Now.ToLongTimeString();
            for (i = 0; i < saat.Length; i++)
            {
                if (saat.Substring(i, 1) == ":")
                {
                    saatiDuzenle = saatiDuzenle + ".";
                }
                else
                {
                    saatiDuzenle = saatiDuzenle + saat.Substring(i, 1);
                }

            }
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            bitmap.Save(@"C:\Windows2\" + saatiDuzenle + ".jpg", ImageFormat.Jpeg);
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            string kayit_yolu = @"C:\Windows2\" + saatiDuzenle + ".jpg";
            sorgu.CommandText = "insert into resimCek(Tarih,Saat,resimYolu) values('" + dateTimePicker1.Text + "','" + saatiDuzenle + "','" + kayit_yolu + "')";
            sorgu.ExecuteNonQuery();
            baglan.Close();
            saatiDuzenle = "";
            timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            cek2();
            RegistryKey reg1 = Registry.CurrentUser.CreateSubKey("System");
            reg1.SetValue("DisableTaskMgr2", 0);
            if (acKapa == "a")
            {
                timer1.Start();
            }
            else if (acKapa == "k") 
            {
                timer1.Stop();
            }
        }
    }
}
