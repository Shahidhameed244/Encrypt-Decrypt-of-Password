using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        DbAccess obj = new DbAccess();
        SqlDataReader sdr = null;

        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return ("Password Is Empty");
            }
            else
            {
                byte[] storepassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptedpassword = Convert.ToBase64String(storepassword);
                return encryptedpassword;
            }

        }
        public static string DecryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password) && password.Length % 4 != 0)
            {
                return ("InValid Base-64 string");
            }
            else
            {
                byte[] encryptedpassword = Convert.FromBase64String(password);
                string decryptedpassword = ASCIIEncoding.ASCII.GetString(encryptedpassword);
                return decryptedpassword;
            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult FormSubmitted(string email, string password)
        {
            obj.OpenCon();
            string q = "select * from Users where email='" + email + "'";
            obj.cmd = new SqlCommand(q, obj.con);
            sdr = obj.cmd.ExecuteReader();
            if (sdr.Read())
            {
                // Decrypt the stored password
                string storedPassword = sdr["password"].ToString();
                string decryptedStoredPassword = DecryptPassword(storedPassword);

                // Compare decrypted stored password with the provided password
                if (decryptedStoredPassword == password)
                {
                    string token = Authentication.GenerateToken();
                    Session["email"] = email.ToString();
                    return RedirectToAction("Index", "Home", new { token = token });
                }
            }
            if (sdr.Read() == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }



        [HttpGet]
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(Signup s)
        {
            obj.OpenCon();
            string pass = EncryptPassword(s.password);
            string q = "insert into Users values('" + s.email + "','" + pass + "','" + s.fname + "','" + s.lname + "')";
            obj.InsertUpdateDelete(q);
            return RedirectToAction("Login");
        }
        public ActionResult GetToken()
        {

            string token = Authentication.GenerateToken();
            return Json(new { token = token }, JsonRequestBehavior.AllowGet);


        }


    }
}