using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;

using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace MD5Encryption.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string textToHash)
        {
            string hashString = null;

            // Creating an MD5 hash object
            using (MD5Cng md5Hash = new MD5Cng())
            {
                // Computing the hash of the input string
                byte[] hashBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(textToHash));

                // Converting the hashed bytes to a string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                hashString = sb.ToString();
            }

            // Saving the hashed string to the database
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString);
            {
                conn.Open();
                SqlCommand command = new SqlCommand("hass", conn);
                command.CommandType=CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TextToHash", textToHash);
                command.Parameters.AddWithValue("@HashedText", hashString);
                command.ExecuteNonQuery();
                conn.Close();
            }

            ViewBag.TextToHash = textToHash; // Passing the original text to the view
            ViewBag.Hash = hashString; // Passing the hash string to the view

            return View();
        }
    }
}
