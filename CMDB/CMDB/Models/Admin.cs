using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text;
using System.Security.Cryptography;

namespace CMDB.Models
{
    public class Admin
    {
        [Column("Admin_id")]
        [Key]
        public int Adminid { get; set; }
        public Account Account { get; set; }
        public int Level { get; set; }
        public string Password { get; set; }
        public DateTime DateSet { get; set; }

        public Admin Login(string userID, string pwd, MySqlConnection connection)
        {
            using MySqlConnection conn = connection;
            conn.Open();
            string sql = "Select Acc_ID from Account a join application ap on a.application = ap.App_ID where UserID = @userid and ap.name='CMDB'";
            int accID = 0; 
            int level = 0;
            string Password ="";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userid", userID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accID = Convert.ToInt32(reader["Acc_ID"]);
            }
            conn.Close();
            conn.Open();
            sql = "Select Password, Level from admin where Account = @userid";
            cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userid", accID);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                level = Convert.ToInt32(reader["Level"]);
                Password = reader["Password"].ToString();
            }
            conn.Close();
            if (String.Equals(Password, MD5Hash(pwd)))
            {
                return new Admin()
                {
                    Account = new Account()
                    {
                        AccID = accID,
                        UserID = userID
                    },
                    Password = Password,
                    Level = level
                };
            }
            else
            {
                return new Admin();
            }
        }
        private static string MD5Hash(string text)
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
    }
}
