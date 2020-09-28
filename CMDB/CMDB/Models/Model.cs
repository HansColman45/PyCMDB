using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace CMDB.Models
{
    public class Model
    {
        private string LogText;
        public ICollection<Log> Logs { get; set; }
        public string Active { get; set; }
        [Column("Deactivate_reason")]
        public string DeactivateReason { get; set; }
        public void GetLogs(string table, int ID, MySqlConnection connection)
        {
            using MySqlConnection conn = connection;
            string Sql = "";
            conn.Open();
            switch (table)
            {
                case "identity":
                    Sql = "select Log_Text, Log_Date from log where Identity=@uuid";
                    break;
                case "identitytype":
                    Sql = "select Log_Text, Log_Date from log where IdentityType=@uuid";
                    break;
                case "account":
                    Sql = "select Log_Text, Log_Date from log where Account=@uuid";
                    break;
                case "accounttype":
                    Sql = "select Log_Text, Log_Date from log where AccountType=@uuid";
                    break;
                case "role":
                    Sql = "select Log_Text, Log_Date from log where Role=@uuid";
                    break;
                case "roletype":
                    Sql = "select Log_Text, Log_Date from log where RoleType=@uuid";
                    break;
                case "assettype":
                    Sql = "select Log_Text, Log_Date from log where AssetType=@uuid";
                    break;
                case "menu":
                    Sql = "select Log_Text, Log_Date from log where menu=@uuid";
                    break;
                case "permissions":
                    Sql = "select Log_Text, Log_Date from log where permissions=@uuid";
                    break;
                case "role_perm":
                    Sql = "select Log_Text, Log_Date from log where role_perm_id=@uuid";
                    break;
                case "application":
                    Sql = "select Log_Text, Log_Date from log where Application=@uuid";
                    break;
                case "kensington":
                    Sql = "select Log_Text, Log_Date from log where Kensington=@uuid";
                    break;
                case "admin":
                    Sql = "select Log_Text, Log_Date from log where Admin=@uuid";
                    break;
                case "mobile":
                    Sql = "select Log_Text, Log_Date from log where IMEI=@uuid";
                    break;
                case "subscriptiontype":
                    Sql = "select Log_Text, Log_Date from log where SubscriptionType=@uuid";
                    break;
                case "subscription":
                    Sql = "select Log_Text, Log_Date from LOG where Subscription=@uuid";
                    break;
            }
            MySqlCommand cmd = new MySqlCommand(Sql, conn);
            cmd.Parameters.AddWithValue("@uuid", ID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                this.Logs.Add(new Log()
                {
                    LogDate = DateTime.Parse(reader["Log_Date"].ToString()),
                    LogText = reader["Log_Text"].ToString()
                });
            }
            conn.Close();
        }
        public void GetLogs(string table, string AssetTag, MySqlConnection connection)
        {
            using MySqlConnection conn = connection;
            string Sql = "";
            conn.Open();
            switch (table)
            {
                case "devices":
                    Sql = "select Log_Text, Log_Date from log where AssetTag=@uuid";
                    break;
                case "token":
                    Sql = "select Log_Text, Log_Date from log where AssetTag=@uuid";
                    break;
            }
            MySqlCommand cmd = new MySqlCommand(Sql, conn);
            cmd.Parameters.AddWithValue("@uuid", AssetTag);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Logs.Add(new Log()
                {
                    LogDate = DateTime.Parse(reader["Log_Date"].ToString()),
                    LogText = reader["Log_Text"].ToString()
                });
            }
            conn.Close();
        }
        protected void LogCreate(string table, int ID, string Value, string AdminName, MySqlConnection connection)
        {
            this.LogText = "The "+ Value + " is created by "+ AdminName+" in table "+ table;
            DoLog(connection, table, ID);
        }
        protected void LogCreate(string table, string AssetTag, string Value, string AdminName, MySqlConnection connection)
        {
            this.LogText = "The " + Value + " is created by " + AdminName + " in table " + table;
            DoLog(connection, table, AssetTag);
        }

        private void DoLog(MySqlConnection connection, string table, int ID)
        {
            using MySqlConnection conn = connection;
            string Sql = "";
            conn.Open();
            switch (table)
            {
                case "identity":
                    Sql = "INSERT INTO log (Identity,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "identitytype":
                    Sql = "INSERT INTO log (IdentityType,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "account":
                    Sql = "INSERT INTO log (Account,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "accounttype":
                    Sql = "INSERT INTO log (AccountType,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "role":
                    Sql = "INSERT INTO log (Role,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "roletype":
                    Sql = "INSERT INTO log (RoleType,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "assettype":
                    Sql = "INSERT INTO log (AssetType,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "menu":
                    Sql = "INSERT INTO log (menu,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "permissions":
                    Sql = "INSERT INTO log (permissions,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "role_perm":
                    Sql = "INSERT INTO log (role_perm_id,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "application":
                    Sql = "INSERT INTO log (Application,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "kensington":
                    Sql = "INSERT INTO log (Kensington,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "admin":
                	Sql = "INSERT INTO log (Admin,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "mobile":
                    Sql = "INSERT INTO log (IMEI,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "subscriptiontype":
                    Sql = "INSERT INTO log (SubscriptionType,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "subscription":
                    Sql = "INSERT INTO LOG (Subscription,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
            }
            MySqlCommand cmd = new MySqlCommand(Sql, conn);
            cmd.Parameters.AddWithValue("@UUID", ID);
            cmd.Parameters.AddWithValue("@log_text", this.LogText);
            cmd.Parameters.AddWithValue("@log_date", new DateTime());
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
        }
        private void DoLog(MySqlConnection connection, string table, string AssetTag)
        {
            using MySqlConnection conn = connection;
            string Sql = "";
            conn.Open();
            switch (table)
            {
                case "devices":
                    Sql = "INSERT INTO log (AssetTag,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
                case "token":
                    Sql = "INSERT INTO log (AssetTag,Log_Text,Log_Date) values(@uuid, @log_text, @log_date)";
                    break;
            }
            MySqlCommand cmd = new MySqlCommand(Sql, conn);
            cmd.Parameters.AddWithValue("@UUID", AssetTag);
            cmd.Parameters.AddWithValue("@log_text", this.LogText);
            cmd.Parameters.AddWithValue("@log_date", new DateTime());
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
        }
    }
}
