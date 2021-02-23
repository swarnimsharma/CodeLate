using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Portal.Common
{
    public static class Utilities
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null)
            {
                return "";
            }
            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        public static int? GetColumnIndexFromName(string columnName)
        {

            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }

        public static bool SendEmail(string fromAddress, string toAddress, string subject, string body, int Port, bool EnableSsl, string smtpServer, string password, string[] cc = null, string[] bcc = null)
        {
         string server_email = ConfigurationManager.AppSettings["ServerEmailID"];
          string server_password = ConfigurationManager.AppSettings["ServerEmailPassword"];

            if (string.IsNullOrEmpty(fromAddress))
            {
                fromAddress = server_email;
            }

            if (string.IsNullOrEmpty(password))
            {
                password =server_password;
            }

            try
            {
                MailAddress to = new MailAddress(toAddress);
                MailAddress from = new MailAddress(fromAddress);
                MailMessage mail = new MailMessage(from, to);
                mail.Subject = subject;
                mail.Body = body;
                if (cc != null)
                    foreach (var ccAdd in cc)
                    {
                        try
                        {
                            MailAddress ccAddMail = new MailAddress(ccAdd);
                            mail.CC.Add(ccAddMail);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                if (bcc != null)
                    foreach (var bccAdd in bcc)
                    {
                        try
                        {
                            MailAddress bccAddMail = new MailAddress(bccAdd);
                            mail.CC.Add(bccAddMail);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpServer;
                smtp.Port = Port;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential(fromAddress, password);
                smtp.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string ConvertCommaSeperateToBadge(string str)
        {
            string temp_string = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                var data = str.Split(',');
                foreach(var item in data)
                {
                    temp_string += "<span class='badge badge-secondary' style='padding:2px 5px !important;margin-left:5px;font-size:10px !important'>" + item + "</span>";
                }
            }
            return temp_string;
        }
    }
}