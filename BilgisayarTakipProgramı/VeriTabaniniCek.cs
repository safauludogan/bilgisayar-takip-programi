using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Data.OleDb;
namespace BilgisayarTakipProgramı
{
    class VeriTabaniniCek
    {
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='pcTakip.accdb'"); 
        OleDbCommand sorgu = new OleDbCommand();
        OleDbDataReader dr;
        string yol;
        public string CekSifEp(string cek, int hangisi)
        {
            baglan.Close();
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = "select * from VeriTabani";
            dr = sorgu.ExecuteReader();
            dr.Read();
            if (hangisi == 0)
            {
                cek = dr[0].ToString();
            }
            if (hangisi == 1)
            {
                cek = dr[1].ToString();
            }
            if (hangisi == 2) 
            {
                cek = dr[3].ToString();
            }
            return cek;
        }
        public string Yenile(string EpSif, int sayac)
        {

            if (sayac == 1)
            {
                yol = "update VeriTabani set sifre='" + EpSif + "'";
            }
            if (sayac == 2)
            {
                yol = "update VeriTabani set eposta='" + EpSif + "'";
            }
            baglan.Open();
            sorgu.Connection = baglan;
            sorgu.CommandText = yol;
            sorgu.ExecuteNonQuery();
            baglan.Close();
            return EpSif;
        }
    }
}
