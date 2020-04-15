using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
namespace BilgisayarTakipProgramı
{
    class Sifre
    {
        int sifre = 6;
        string tut;
        public string GenerateNewPassword()
        {
            char[] cr = "0123456789AbCdEfGhijKlMnopqRstuVwXyz".ToCharArray();
            string result = string.Empty;
            Random r = new Random();
            for (int i = 0; i < sifre; i++)
            {
                result += cr[r.Next(0, cr.Length - 1)].ToString();
                tut += result;
            }

            return result;
        }
    }
    class MailAt
    {

        string konu, icerik;

        public string mail(string eposta, int sayac, string kod)
        {
            if (sayac == 1)
            {
                konu = "E Posta Değiştirme";
                icerik = "E Posta Değiştirme Kodunuz : ";
            }
            else if (sayac == 2)
            {
                konu = "Şifre Değiştirme";
                icerik = "Şifre Sıfırlama Kodunuz : ";
            }
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("suludogan1995@gmail.com");
            mail.To.Add(eposta);
            mail.Subject = konu;
            mail.Body = icerik + kod;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("suludogan1995@gmail.com", "cd21600b"); 
            smtp.EnableSsl = true;
            smtp.Send(mail);
            kod = "";
            return eposta;
        }
    }
}