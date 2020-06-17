using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace FindTrip_Web.Models
{
    public class Utility
    {
        #region "密碼加密"
        public const int DefaultSaltSize = 5;
        /// <summary>
        /// 產生Salt
        /// </summary>
        /// <returns>Salt</returns>
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[DefaultSaltSize];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
        ///// <summary>
        ///// 密碼加密
        ///// </summary>
        ///// <param name="password">密碼明碼</param>
        ///// <returns>Hash後密碼</returns>
        //public static string CreateHash(string password)
        //{
        //    string salt = CreateSalt();
        //    string saltAndPassword = String.Concat(password, salt);
        //    string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, "SHA1");
        //    hashedPassword = string.Concat(hashedPassword, salt);
        //    return hashedPassword;
        //}

        /// <summary>
        /// Computes a salted hash of the password and salt provided and returns as a base64 encoded string.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use in the hash.</param>
        public static string GenerateHashWithSalt(string password, string salt)
        {
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            HashAlgorithm algorithm = new SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // return the has as a base 64 encoded string
            return Convert.ToBase64String(hash);
        }

        #endregion

        #region "將使用者資料寫入cookie,產生AuthenTicket"
        /// <summary>
        /// 將使用者資料寫入cookie,產生AuthenTicket
        /// </summary>
        /// <param name="userData">使用者資料</param>
        /// <param name="userId">UserAccount</param>
        static public void SetAuthenTicket(string userData, string userId)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立Cookie
            HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將Cookie寫入回應

            HttpContext.Current.Response.Cookies.Add(authenticationcookie);

        }
        #endregion

        #region"儲存用戶上傳圖片"
        /// <summary>
        /// 儲存上傳圖片
        /// </summary>
        /// <param name="upfile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        static public string SaveUpImage(HttpPostedFile upfile)
        {
            //取得副檔名
            string extension = upfile.FileName.Split('.')[upfile.FileName.Split('.').Length - 1];
            //新檔案名稱
            //string fileName = String.Format("{0:yyyyMMddhhmmsss}.{1}", DateTime.Now, extension);
            string fileName = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.{extension}";
            string savedName = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Userimg"), fileName);
            upfile.SaveAs(savedName);
            return fileName;
        }
        #endregion

        #region"儲存背景上傳圖片"
        /// <summary>
        /// 儲存上傳圖片
        /// </summary>
        /// <param name="upFile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        public static string UploadImage(HttpPostedFile upFile)
        {
            string fileName = Path.GetFileName(upFile.FileName);
            //取得副檔名
            string extension = fileName.Split('.')[fileName.Split('.').Length - 1];
            //新檔案名稱
            fileName = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.{extension}";
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/BGimg"), fileName);
            upFile.SaveAs(path);
            return fileName;
        }
        #endregion

        #region"儲存旅行計畫背景上傳圖片"
        /// <summary>
        /// 儲存上傳圖片
        /// </summary>
        /// <param name="upFile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        public static string UploadPlanBGImage(HttpPostedFile upFile)
        {
            string fileName = Path.GetFileName(upFile.FileName);
            //取得副檔名
            string extension = fileName.Split('.')[fileName.Split('.').Length - 1];
            //新檔案名稱
            fileName = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.{extension}";
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/TPBGimg"), fileName);
            upFile.SaveAs(path);
            return fileName;
        }
        #endregion

        #region"儲存旅行計畫國家上傳圖片"
        /// <summary>
        /// 儲存上傳圖片
        /// </summary>
        /// <param name="upFile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        public static string UploadPlanCYImage(HttpPostedFile upFile)
        {
            string fileName = Path.GetFileName(upFile.FileName);
            //取得副檔名
            string extension = fileName.Split('.')[fileName.Split('.').Length - 1];
            //新檔案名稱
            fileName = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.{extension}";
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/TPCimg"), fileName);
            upFile.SaveAs(path);
            return fileName;
        }
        #endregion


        #region 寄送系統通知信
        public static void SendEmail(string Email)
        {
            string MessageBody = "尋旅 FINDTRIP 找回密碼，<br>" +
                                 "主旨：" + "<br>" +
                                 "維修單編號：" + "<br>" +
                                 "<br>敬請撥冗查看，謝謝!";
            //寄件者及名稱，可隨便填
            MailAddress from = new MailAddress("goodbye.n.hello.again@gmail.com", "找回密碼");
            //收件者，可寫多人，用逗號隔開
            MailAddress to = new MailAddress(Email);
            MailMessage message = new MailMessage(from, to);
            message.Subject = $"[監視器報修系統自動通知信]";
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = MessageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true; //是否為html信件
            //msg.Attachments.Add(new Attachment(@"D:\test2.docx"));  //附件
            //msg.Priority = MailPriority.High;//郵件優先級
            //設定smtp sever及port
            SmtpClient myMail = new SmtpClient("smtp.gmail.com", 587);
            myMail.Credentials = new System.Net.NetworkCredential("goodbye.n.hello.again@gmail.com", "Tobey1011"); //填入帳密
            myMail.EnableSsl = true; //ssl打開，寄信時加密(gmail預設開啟驗證)
            myMail.Send(message); //寄信
            myMail.Dispose(); //傳送結束訊息給smtp
            message.Dispose(); //釋放Mailmessage所使用的所有資源
        }
        #endregion


    }
}





