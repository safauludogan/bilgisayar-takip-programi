using Microsoft.Win32;
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
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
namespace BilgisayarTakipProgramı
{
    public partial class Ayarlar : Form
    {        
        public Ayarlar()
        {
            InitializeComponent();
        }   
 
        BilgisayarTakipProgramı.VeriTabaniniCek acKapat = new BilgisayarTakipProgramı.VeriTabaniniCek();
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int vKey);
        public static int sure;
        int radioButton,   goruntusuresi;
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'");
        OleDbCommand sorgu = new OleDbCommand();
        public static int baslat = 1, Durdur = 0;
        OleDbDataReader dr;
        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true);
        RegistryKey reg1 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
        string mgr, acKapa;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                acKapa = "a";
            }
            if (checkBox1.Checked == false)
            {
                acKapa = "k";
            }
        }

        private void Ayarlar_Load(object sender, EventArgs e)
        {
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from VeriTabani";
            dr = sorgu.ExecuteReader();
            dr.Read();
            acKapa = dr[3].ToString();
            //////////////////////////
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from Kayit";
            dr = sorgu.ExecuteReader();
            dr.Read();
            goruntusuresi = int.Parse(dr[0].ToString());
            if (acKapa == "a")
            {
                checkBox1.Checked = true;
            }
            else if (acKapa == "k")
            {
                checkBox1.Checked = false;
            }
            if (goruntusuresi == 5)
            {
                radioButton1.Checked = true;
            }
            if (goruntusuresi == 10)
            {
                radioButton2.Checked = true;
            }
            if (goruntusuresi == 30)
            {
                radioButton3.Checked = true;
            }
            if (goruntusuresi == 60)
            {
                radioButton4.Checked = true;
            }
            if (goruntusuresi == 300)
            {
                radioButton5.Checked = true;
            }
            if (goruntusuresi == 900)
            {
                radioButton6.Checked = true;
            }
            if (goruntusuresi == 1800)
            {
                radioButton7.Checked = true;
            }
            if (goruntusuresi == 3600)
            {
                radioButton8.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }
            //////////////////////////////
            Mutex Mtx = new Mutex(false, "SINGLE_INSTANCE_APP_MUTEX");
            if (Mtx.WaitOne(0, false) == false)
            {

                Mtx.Close();

                Mtx = null;

                MessageBox.Show("Program Zaten Açık");

                Application.Exit();

            }
            acKapa = acKapat.CekSifEp(acKapa, 2);

            if (key != null)
            {
                RegistryKey key2 = key.CreateSubKey("System");
                key2.SetValue("", "");
            }
            mgr = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System").GetValue("DisableTaskMgr").ToString();
            if (mgr == "1")
            {
                checkBox2.Checked = true;
                label2.Text = "Görev Yöneticisi Kapalı";
            }
            else
            {
                checkBox2.Checked = false;
                label2.Text = "Görev Yöneticisi Açık";
            }          
            
        }
        void ackapat()
        {
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "update VeriTabani set acKapa='" + acKapa + "'";
            sorgu.ExecuteNonQuery();
            baglan.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {           
            if (radioButton1.Checked == true)
            {
                radioButton = 5;
            }
            if (radioButton2.Checked == true)
            {
                radioButton = 10;
            }
            if (radioButton3.Checked == true)
            {
                radioButton = 30;
            }
            if (radioButton4.Checked == true)
            {
                radioButton = 60;
            }
            if (radioButton5.Checked == true)
            {
                radioButton = 300;
            }
            if (radioButton6.Checked == true)
            {
                radioButton = 900;
            }
            if (radioButton7.Checked == true)
            {
                radioButton = 1800;
            }
            if (radioButton8.Checked == true)
            {
                radioButton = 3600;
            }
            sure = radioButton;
            //
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "update Kayit set goruntuSuresi=" + radioButton + "";
            sorgu.ExecuteNonQuery();
            baglan.Close();
            //
            ackapat();
            KayıtFormu frm = new KayıtFormu();
            frm.Show();
            this.Hide();
        }
      
        private void button2_Click(object sender, EventArgs e)
        {
            ackapat();
            if (MessageBox.Show("Bütün Kayıtları Silmek İstediğinize Eminmisiniz?", "Dikkat!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ////////////
                Process[] p;
                p = Process.GetProcessesByName("BilgisayarTakipProgramı");
                if (p.Length > 0)
                {
                    foreach (Process process in p)
                    {
                        process.Kill();
                    }
                }
                /////////////
                baglan.Close();
                baglan.Open();
                sorgu.Connection = baglan;
                sorgu.CommandText = "delete * from resimCek";
                sorgu.ExecuteNonQuery();
                baglan.Close();
                Directory.Delete(@"C:\Windows2", true);
                MessageBox.Show("Bütün Kayıtlar Silindi.");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
                if (checkBox2.Checked == true)
                {
                    label2.Text = "Görev Yöneticisi Kapalı";
                    reg1.SetValue("DisableTaskMgr", 1);
                }
                else
                {
                    label2.Text = "Görev Yöneticisi Açık";
                    reg1.SetValue("DisableTaskMgr", 0);
                }           
        }

        private void Ayarlar_FormClosed(object sender, FormClosedEventArgs e)
        {
            KayıtFormu frm = new KayıtFormu();
            frm.Show();
            this.Hide();
        }

    }
}
