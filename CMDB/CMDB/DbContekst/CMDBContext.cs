using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using CMDB.Models;
using System.Text;
using System.Security.Cryptography;

namespace CMDB.DbContekst
{
    public class CMDBContext : DbContext
    {
        private string LogText;
        private readonly DateTime LogDate = DateTime.Now;
        private readonly string ConnectionString;
        public Admin Admin { get; set; }
        public string LogDateFormat {
            get
            {
                string format = "dd/MM/yyyy";
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                using MySqlConnection conn = Connection;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select TEXT from configuration where Code = 'General' and SUB_CODE = 'LogDateFormat'", conn) ;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    format = reader["TEXT"].ToString();
                }
                conn.Close();
                return format;
            }
        }
        public string DateFormat
        {
            get
            {
                string format = "dd/MM/yyyy";
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                using MySqlConnection conn = Connection;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select TEXT from configuration where Code = 'General' and SUB_CODE = 'DateFormat'", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    format = reader["TEXT"].ToString();
                }
                conn.Close();
                return format;
            }
        }
        public string Company
        {
            get
            {
                string format = "";
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                using MySqlConnection conn = Connection;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select TEXT from configuration where Code = 'General' and SUB_CODE = 'Company'", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    format = reader["TEXT"].ToString();
                }
                conn.Close();
                return format;
            }
        }
        public CMDBContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Identity>().ToTable("identity");
            modelBuilder.Entity<Account>().ToTable("account");
            modelBuilder.Entity<AccountType>().ToTable("accounttype");
            modelBuilder.Entity<IdentityType>().ToTable("identitytype");
            modelBuilder.Entity<Laptop>().ToTable("asset");
            modelBuilder.Entity<Desktop>().ToTable("asset");
            modelBuilder.Entity<Monitor>().ToTable("asset");
            modelBuilder.Entity<Docking>().ToTable("asset");
            modelBuilder.Entity<IdenAccount>().ToTable("idenaccount");
            modelBuilder.Entity<Language>().ToTable("language");
            modelBuilder.Entity<Admin>().ToTable("admin");
        }
        #region Identity
        public ICollection<Identity> ListAllIdenties()
        {
            List<Identity> list = new List<Identity>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Iden_Id, Name, UserID, E_Mail, L.code, L.Description, it.Type, if(i.active=1,\"Active\",\"Inactive\") as Active, " +
                "i.Deactivate_reason from Identity i join identitytype it on i.type = it.type_id "+
                "join Language l on i.Language = l.code ", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Identity()
                {
                    IdenID = Convert.ToInt32(reader["Iden_Id"]),
                    Name = reader["Name"].ToString(),
                    EMail = reader["E_Mail"].ToString(),
                    Language = new Language()
                    {
                        Code = reader["code"].ToString(),
                        Description = reader["Description"].ToString()
                    },
                    Active = reader["Active"].ToString(),
                    DeactivateReason = reader["Deactivate_reason"].ToString(),
                    UserID = reader["UserID"].ToString(),
                    Type = new IdentityType()
                    {
                        Description = reader["Type"].ToString()
                    }
                });
            }
            conn.Close();
            return list;
        }
        public ICollection<Identity> GetIdentityByID(int id)
        {
            List<Identity> list = new List<Identity>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Iden_Id, L.code, L.Description,Name, UserID, E_Mail, Company, it.Type_ID, it.Type, if(i.active=1,\"Active\",\"Inactive\") as Active, i.Deactivate_reason "
                + "from Identity i join identitytype it on i.type = it.type_id "+
                "join Language l on i.Language = l.code where Iden_Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Identity()
                {
                    IdenID = Convert.ToInt32(reader["Iden_Id"]),
                    Name = reader["Name"].ToString(),
                    EMail = reader["E_Mail"].ToString(),
                    Language = new Language()
                    {
                        Code = reader["code"].ToString(),
                        Description = reader["Description"].ToString()
                    },
                    Active = reader["Active"].ToString(),
                    DeactivateReason = reader["Deactivate_reason"].ToString(),
                    UserID = reader["UserID"].ToString(),
                    Company = reader["Company"].ToString(),
                    Type = new IdentityType()
                    {
                        TypeID = Convert.ToInt32(reader["Type_ID"]),
                        Description = reader["Type"].ToString()
                    },
                    Devices = new List<Device>(),
                    Accounts = new List<IdenAccount>(),
                    Logs = new List<Log>()
                });
            }
            conn.Close();
            return list;
        }
        public List<SelectListItem> ListActiveIdentityTypes()
        {
            List<SelectListItem> identityTypes = new List<SelectListItem>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Type_ID, Type, Description from identitytype it where active = 1", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                identityTypes.Add(new SelectListItem(reader["Type"].ToString() + " " + reader["Description"].ToString(), reader["Type_ID"].ToString()));
            }
            return identityTypes;
        }
        public List<SelectListItem> ListAllActiveLanguages()
        {
            List<SelectListItem> langs = new List<SelectListItem>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Code, Description from Language it where active = 1", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                langs.Add(new SelectListItem(reader["Description"].ToString(), reader["Code"].ToString()));
            }
            return langs;
        }
        public List<IdenAccount> GetIdenAccountByID(int id)
        {
            List<IdenAccount> idenAccounts = new List<IdenAccount>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select ia.ID ,a.Acc_ID, a.UserID accUserID, app.App_ID, app.Name Application, ia.ValidFrom, ia.ValidEnd, " 
                + "i.Name, i.Iden_Id, i.UserID idenUserID, E_Mail, L.code, L.Description, it.type_id, it.Type, at.Type AccountType, at.Type_id AccountTypeID "
                + "from account a "
                + "join application app on a.Application = app.App_ID "
                + "join accounttype at on a.Type = at.Type_id "
                + "join idenaccount ia on ia.Account= a.Acc_ID " 
                + "join identity i on ia.Identity = i.Iden_ID "
                + "join identitytype it on i.type = it.type_id "
                + "join Language l on i.Language = l.code "
                + "where ia.ID = @uuid";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@uuid", id);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                idenAccounts.Add(new IdenAccount()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Account = new Account()
                    {
                        AccID = Convert.ToInt32(reader["Acc_ID"]),
                        UserID = reader["accUserID"].ToString(),
                        Application = new Application()
                        {
                            AppID = Convert.ToInt32(reader["App_ID"]),
                            Name = reader["Application"].ToString()
                        },
                        Type = new AccountType()
                        {
                            TypeID = Convert.ToInt32(reader["AccountTypeID"]),
                            Type = reader["AccountType"].ToString()
                        }
                    },
                    Identity = new Identity() 
                    { 
                        IdenID = Convert.ToInt32(reader["Iden_Id"]),
                        Name = reader["Name"].ToString(),
                        EMail = reader["E_Mail"].ToString(),
                        UserID = reader["idenUserID"].ToString(),
                        Language = new Language()
                        {
                            Code = reader["code"].ToString(),
                            Description = reader["Description"].ToString()
                        },
                        Type = new IdentityType()
                        {
                            TypeID = Convert.ToInt32(reader["Type_Id"]),
                            Type = reader["Type"].ToString()
                        }
                    },
                    ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString()),
                    ValidUntil = DateTime.Parse(reader["ValidEnd"].ToString())
                });
            }
            conn.Close();
            return idenAccounts;
        }
        public void GetAssingedDevicesForIdentity(Identity identity)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select c.Category, a.AssetTag, a.SerialNumber, at.Vendor, at.Type, if(a.Active=1,\"Active\",\"Inactive\") as Active, a.Identity "
                + "FROM asset a "
                + "join assettype at on a.Type = at.Type_ID "
                + "join category c on a.Category = c.ID "
                + "where a.Identity = @uuid "
                + "UNION "
                + "SELECT c.Category,m.IMEI AssetTag, m.IMEI SerialNumber, at.Vendor, at.Type, "
                + "if(m.Active=1,\"Active\",\"Inactive\") as Active, m.Identity "
                + "from mobile m "
                + "join assettype at on m.MobileType = at.Type_ID "
                + "join category c on at.Category = c.ID "
                + "where m.Identity = @uuid ";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                identity.Devices.Add(new Device()
                {
                    AssetTag = reader["AssetTag"].ToString(),
                    SerialNumber = reader["SerialNumber"].ToString(),
                    Category = new AssetCategory()
                    {
                        Category = reader["Category"].ToString()
                    },
                    Type = new AssetType()
                    {
                        Type = reader["Type"].ToString(),
                        Vendor = reader["Vendor"].ToString()
                    },
                    Active = reader["Active"].ToString()
                });
            }
            conn.Close();
        }
        public void GetAssignedAccountsForIdentity(Identity identity)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select ia.ID ,a.Acc_ID, a.UserID, app.App_ID, app.Name Application, ia.ValidFrom, ia.ValidEnd "
                +"from account a "
                +"join application app on a.`Application` = app.`App_ID` "
                +"join idenaccount ia on ia.Account= a.Acc_ID "
                + "where ia.Identity = @uuid order by ia.ValidFrom desc";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                identity.Accounts.Add(new IdenAccount() 
                { 
                    ID = Convert.ToInt32(reader["ID"]),
                    Account = new Account()
                    {
                        AccID = Convert.ToInt32(reader["Acc_ID"]),
                        UserID = reader["UserID"].ToString(),
                        Application = new Application()
                        {
                            AppID = Convert.ToInt32(reader["App_ID"]),
                            Name = reader["Application"].ToString()
                        }
                    },
                    Identity = identity,
                    ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString()),
                    ValidUntil = DateTime.Parse(reader["ValidEnd"].ToString())
                });
            }
            conn.Close();
        }
        public void CreateNewIdentity(string firstName,string LastName, int type, string UserID, string Company, string EMail, string Language, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Identity (Name,UserID,Type,Language,Company,E_Mail) values(@name, @userid, @type, @language, @company, @email)", conn);
            cmd.Parameters.AddWithValue("@name", firstName + ", " + LastName);
            cmd.Parameters.AddWithValue("@userID", UserID);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@language", Language);
            cmd.Parameters.AddWithValue("@company", Company);
            cmd.Parameters.AddWithValue("@email", EMail);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            conn.Open();
            string Value = "Identity width name: " + firstName + ", " + LastName;
            cmd = new MySqlCommand("Select Iden_ID from Identity order by Iden_ID desc limit 1", conn);
            reader = cmd.ExecuteReader();
            int ID = 0;
            while (reader.Read())
            {
                ID = Convert.ToInt32(reader["Iden_Id"]);
            }
            conn.Close();
            LogCreate(Table, ID, Value, Admin.Account.UserID);
        }
        public void EditIdentity(Identity identity, string firstName, string LastName, int type, string UserID, string Company, string EMail, string Language, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            if(String.Compare(identity.FirstName, firstName) != 0)
            {
                LogUpdate(Table, identity.IdenID, "FirstName", identity.FirstName, firstName, Admin.Account.UserID);
                string FullName = firstName + ", " + LastName;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Identity Name = @name where Iden_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
                cmd.Parameters.AddWithValue("@name", FullName);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if(String.Compare(identity.LastName, LastName) != 0)
            {
                LogUpdate(Table, identity.IdenID, "LastName", identity.LastName, LastName, Admin.Account.UserID);
                string FullName = firstName + ", " + LastName;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Identity Name = @name where Iden_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
                cmd.Parameters.AddWithValue("@name", FullName);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if(String.Compare(identity.Company,Company) != 0)
            {
                LogUpdate(Table, identity.IdenID, "Company", identity.Company, Company, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Identity Company = @Company where Iden_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
                cmd.Parameters.AddWithValue("@Company", Company);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if (String.Compare(identity.Language.Code,Language) != 0)
            {
                LogUpdate(Table, identity.IdenID, "Language", identity.Language.Code, Language, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Identity Language = @Language where Iden_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
                cmd.Parameters.AddWithValue("@Language", Language);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if(string.Compare(identity.EMail,EMail) != 0)
            {
                LogUpdate(Table, identity.IdenID, "EMail", identity.EMail, EMail, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Identity E_Mail = @EMail where Iden_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
                cmd.Parameters.AddWithValue("@EMail", EMail);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if (String.Compare(identity.UserID,UserID) != 0)
            {
                LogUpdate(Table, identity.IdenID, "UserID", identity.UserID, UserID, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Identity set UserID = @userid where Iden_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
                cmd.Parameters.AddWithValue("@userID", UserID);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if(identity.Type.TypeID != type)
            {
                var Type = GetIdenityTypeByID(type);
                IdentityType newType = Type.ElementAt<IdentityType>(0);
                LogUpdate(Table, identity.IdenID, "Type", identity.Type.Type, newType.Type, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Identity set Type = @type where Iden_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
                cmd.Parameters.AddWithValue("@Type", type);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
        }
        public void DeactivateIdenity(Identity identity, string Reason,string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update Identity set Deactivate_reason = @Reason, Active=0 where Iden_ID = @uuid", conn);
            cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
            cmd.Parameters.AddWithValue("@Reason", Reason);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            string value = "Identity width name: " + identity.FirstName + ", " + identity.LastName;
            LogDeactivate(Table, identity.IdenID, value, Reason);
        }
        public void ActivateIdentity(Identity identity, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update Identity set Deactivate_reason = NULL, Active=1 where Iden_ID = @uuid", conn);
            cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            string value = "Identity width name: " + identity.FirstName + ", " + identity.LastName;
            LogActivate(Table, identity.IdenID, value);
        } 
        public void AssignAccount2Idenity(Identity identity, int AccID, DateTime ValidFrom, DateTime ValidUntil,string Table)
        {
            var Account = GetAccountByID(AccID);
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "Insert into idenaccount (Identity, Account, ValidFrom, ValidEnd) VALUE (@idenID,@accId,@validFrom,@validUntil)";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@idenID", identity.IdenID);
            cmd.Parameters.AddWithValue("@accId", AccID);
            cmd.Parameters.AddWithValue("@validFrom", ValidFrom.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@validUntil", ValidUntil.ToString("yyyy-MM-dd HH:mm:ss"));
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            LogAssignIden2Account(Table, identity.IdenID, identity, Account.ElementAt<Account>(0));
            LogAssignAccount2Identity("account", AccID, Account.ElementAt<Account>(0), identity);
        }
        public void ReleaseAccount4Identity(Identity identity, Account account, int idenAccountID, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "update idenaccount set ValidEnd = DATE_SUB(NOW(), INTERVAL 1 DAY) where ID = @ID";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@ID", idenAccountID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            LogReleaseAccountFromIdentity(Table, identity.IdenID, identity, account);
            LogReleaseIdentity4Account("account", account.AccID, identity, account);
        }
        public List<SelectListItem> ListAllFreeIdentities()
        {
            List<SelectListItem> accounts = new List<SelectListItem>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select i.Iden_ID, i.Name, i.UserID " +
                "from Identity i " +
                "where i.Iden_ID not in (select Identity from idenaccount ia where now() between ia.ValidFrom and IFNULL(ia.ValidEnd,now()+1)) " +
                "and i.Active = 1 and i.Iden_ID !=1";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new SelectListItem(reader["UserID"].ToString() + " " + reader["Name"].ToString(), reader["Iden_ID"].ToString()));
            }
            conn.Close();
            return accounts;
        }
        #endregion
        #region Account
        public ICollection<Account> ListAllAccounts()
        {
            List<Account> accounts = new List<Account>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select a.Acc_ID,a.UserID,app.App_ID, app.Name Application, at.Type_ID, at.Type, at.Description, if(a.active=1,\"Active\",\"Inactive\") as Active, a.Deactivate_reason " +
                "from account a " +
                "join application app on a.Application = app.App_ID " +
                "join accounttype at on a.Type = at.Type_id";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new Account()
                {
                    AccID = Convert.ToInt32(reader["Acc_ID"]),
                    UserID = reader["UserID"].ToString(),
                    Application = new Application()
                    {
                        AppID = Convert.ToInt32(reader["App_ID"]),
                        Name = reader["Application"].ToString()
                    },
                    Type = new AccountType()
                    {
                        TypeID = Convert.ToInt32(reader["Type_ID"]),
                        Type = reader["Type"].ToString(),
                        Description = reader["Description"].ToString()
                    },
                    Active = reader["Active"].ToString(),
                    DeactivateReason = reader["Deactivate_reason"].ToString()
                });
            }
            conn.Close();
            return accounts;
        }
        public List<Account> GetAccountByID(int ID)
        {
            List<Account> accounts = new List<Account>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select a.Acc_ID,a.UserID,app.App_ID, app.Name Application, at.Type_ID, at.Type, at.Description, if(a.active=1,\"Active\",\"Inactive\") as Active, a.Deactivate_reason " +
                "from account a " +
                "join application app on a.Application = app.App_ID " +
                "join accounttype at on a.Type = at.Type_id where Acc_ID=@id";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@id", ID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new Account()
                {
                    AccID = Convert.ToInt32(reader["Acc_ID"]),
                    UserID = reader["UserID"].ToString(),
                    Application = new Application()
                    {
                        AppID = Convert.ToInt32(reader["App_ID"]),
                        Name = reader["Application"].ToString()
                    },
                    Type = new AccountType()
                    {
                        TypeID = Convert.ToInt32(reader["Type_ID"]),
                        Type = reader["Type"].ToString(),
                        Description = reader["Description"].ToString()
                    },
                    Active = reader["Active"].ToString(),
                    DeactivateReason = reader["Deactivate_reason"].ToString(),
                    Identities = new List<IdenAccount>(),
                    Logs = new List<Log>()
                });
            }
            conn.Close();
            return accounts;
        }
        public void AssignIdentity2Account(Account account, int IdenID, DateTime ValidFrom, DateTime ValidUntil, string Table)
        {
            var Identity = GetIdentityByID(IdenID);
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "Insert into idenaccount (Identity, Account, ValidFrom, ValidEnd) VALUE (@idenID,@accId,@validFrom,@validUntil)";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@idenID", IdenID);
            cmd.Parameters.AddWithValue("@accId", account.AccID);
            cmd.Parameters.AddWithValue("@validFrom", ValidFrom.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@validUntil", ValidUntil.ToString("yyyy-MM-dd HH:mm:ss"));
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            LogAssignAccount2Identity(Table, account.AccID, account, Identity.ElementAt<Identity>(0));
            LogAssignIden2Account("identity", IdenID, Identity.ElementAt<Identity>(0), account);
        }        
        public void ReleaseIdentity4Acount(Account account, Identity identity, int idenAccountID, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "update idenaccount set ValidEnd = DATE_SUB(NOW(), INTERVAL 1 DAY) where ID = @ID";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@ID", idenAccountID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            LogReleaseAccountFromIdentity(Table, identity.IdenID, identity, account);
            LogReleaseIdentity4Account("identity", account.AccID, identity, account);
        }
        public void CreateNewAccount(string UserID, int type, int application, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Account (UserID,Type,Application) values(@userid, @Type, @Application)", conn);
            cmd.Parameters.AddWithValue("@userID", UserID);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@Application", application);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            conn.Open();
            cmd = new MySqlCommand("Select Acc_ID from Account order by Acc_ID desc limit 1", conn);
            int ID = 0;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ID = Convert.ToInt32(reader["Acc_ID"]);
            }
            conn.Close();
            List<AccountType> accountTypes= GetAccountTypeByID(type);
            List<Application> applications = GetApplicationByID(application);
            string Value = "Account width UserID: " + UserID+ " with type "+accountTypes.ElementAt<AccountType>(0).Type+" for application "+ applications.ElementAt<Application>(0).Name;
            LogCreate(Table, ID, Value, Admin.Account.UserID);
        }
        public void EditAccount(Account account, string UserID, int type, int application, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            List<AccountType> accountTypes = GetAccountTypeByID(type);
            List<Application> applications = GetApplicationByID(application);
            if (String.Compare(account.UserID,UserID) != 0)
            {
                LogUpdate(Table, account.AccID, "UserID", account.UserID, UserID, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Account set UserID = @userid where Acc_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", account.AccID);
                cmd.Parameters.AddWithValue("@userID", UserID);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if(account.Type.TypeID != type) {

                LogUpdate(Table, account.AccID, "Type", account.Type.Type, accountTypes.ElementAt<AccountType>(0).Type, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Account set type = @userid where Acc_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", account.AccID);
                cmd.Parameters.AddWithValue("@userID", type);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if(account.Application.AppID != application)
            {
                LogUpdate(Table, account.AccID, "Application", account.Application.Name, applications.ElementAt<Application>(0).Name, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Account set application = @userid where Acc_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", account.AccID);
                cmd.Parameters.AddWithValue("@userID", application);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
        }
        public void DeactivateAccount(Account account, string Reason, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update Account set Deactivate_reason = @Reason, Active=0 where Acc_ID = @uuid", conn);
            cmd.Parameters.AddWithValue("@uuid", account.AccID);
            cmd.Parameters.AddWithValue("@Reason", Reason);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            string value = "Account width UserID: " + account.UserID + " and type " + account.Type.Description;
            LogDeactivate(Table, account.AccID, value, Reason);
        }
        public void ActivateAccount(Account account, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update Account set Deactivate_reason = NULL, Active=1 where Acc_ID = @uuid", conn);
            cmd.Parameters.AddWithValue("@uuid", account.AccID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            string value = "Account width UserID: " + account.UserID + " and type " + account.Type.Description;
            LogActivate(Table, account.AccID, value);
        }
        public void GetAssignedIdentitiesForAccount(Account account)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select ia.ID, Iden_Id, Name, UserID, E_Mail, L.code, L.Description, it.Type_ID, it.Type, if(i.active=1,\"Active\",\"Inactive\") as Active , ia.ValidFrom, ia.ValidEnd "
                + "from Identity i "
                + "join identitytype it on i.type = it.type_id "
                + "join Language l on i.Language = l.code "
                + "join idenaccount ia on ia.Identity = i.Iden_ID "
                + "where ia.Account = @uuid order by ia.ValidFrom desc";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@uuid", account.AccID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DateTime validUntil = new DateTime(9999, 12, 12);
                if (!String.IsNullOrEmpty(reader["ValidEnd"].ToString()))
                    validUntil = DateTime.Parse(reader["ValidEnd"].ToString());
                account.Identities.Add(new IdenAccount()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Identity = new Identity()
                    {
                        IdenID = Convert.ToInt32(reader["Iden_Id"]),
                        UserID = reader["UserID"].ToString(),
                        Name = reader["Name"].ToString(),
                        EMail = reader["E_Mail"].ToString(),
                        Active = reader["Active"].ToString(),
                        Type = new IdentityType()
                        {
                            TypeID = Convert.ToInt32(reader["Iden_Id"]),
                            Type = reader["Type"].ToString()
                        },
                        Language = new Language()
                        {
                            Code = reader["code"].ToString(),
                            Description = reader["Description"].ToString()
                        }
                    },
                    ValidFrom = DateTime.Parse(reader["ValidFrom"].ToString()),
                    ValidUntil = validUntil
                });
            }
        }
        public List<SelectListItem> ListAllFreeAccounts()
        {
            List<SelectListItem> accounts = new List<SelectListItem>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select a.Acc_ID,a.UserID,app.App_ID, app.Name Application " +
                "from account a " +
                "join application app on a.Application = app.App_ID " +
                "where a.Acc_ID not in (select Account from idenaccount ia where now() between ia.ValidFrom and IFNULL(ia.ValidEnd,now()+1)) " +
                "and a.Active = 1";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new SelectListItem(reader["UserID"].ToString() + " " + reader["Application"].ToString(), reader["Acc_ID"].ToString()));
            }
            conn.Close();
            return accounts;
        }
        #endregion
        public bool IsPeriodOverlapping(int? IdenID, int? AccID, DateTime ValidFrom, DateTime ValidUntil)
        {
            bool result = false;
            string SQL ="";
            if (IdenID == null && AccID == null)
                throw new Exception("Missing required id's");
            else { 
                if (IdenID != null)
                    SQL = "Select * from idenaccount where Identity = @id and ValidFrom >= @ValidFrom and ValidEnd <=@ValidUntil";
                else if (AccID != null)
                    SQL = "Select * from idenaccount where Account = @id and ValidFrom >= @ValidFrom and ValidEnd <=@ValidUntil";
                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                using MySqlConnection conn = Connection;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                if (IdenID != null)
                    cmd.Parameters.AddWithValue("@id", IdenID);
                else if (AccID != null)
                    cmd.Parameters.AddWithValue("@id", AccID);
                cmd.Parameters.AddWithValue("@ValidFrom", ValidFrom.ToString("yyyy-MM-dd H:mm:ss"));
                cmd.Parameters.AddWithValue("@ValidUntil", ValidUntil.ToString("yyyy-MM-dd H:mm:ss"));
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = true;
                }
                conn.Clone();
            }
            return result;
        }
        #region IdenityType
        public ICollection<IdentityType> ListAllIdentyTypes()
        {
            List<IdentityType> list = new List<IdentityType>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Type_ID, Type, Description, if(active=1,\"Active\",\"Inactive\") as Active, Deactivate_reason from identitytype", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new IdentityType() 
                {
                    TypeID = Convert.ToInt32(reader["Type_ID"]),
                    Description = reader["Description"].ToString(),
                    Type = reader["Type"].ToString(),
                    Active = reader["Active"].ToString(),
                    DeactivateReason = reader["Deactivate_reason"].ToString()
                });
            }
            conn.Close();
            return list;
        }
        public ICollection<IdentityType> GetIdenityTypeByID(int id)
        {
            List<IdentityType> list = new List<IdentityType>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Type_ID, Type, Description, if(active=1,\"Active\",\"Inactive\") as Active, Deactivate_reason from identitytype where Type_ID=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new IdentityType()
                {
                    TypeID = Convert.ToInt32(reader["Type_ID"]),
                    Description = reader["Description"].ToString(),
                    Type = reader["Type"].ToString(),
                    Active = reader["Active"].ToString(),
                    DeactivateReason = reader["Deactivate_reason"].ToString()
                });
            }
            conn.Close();
            return list;
        }
        #endregion
        #region AccountType
        public List<AccountType> GetAccountTypeByID(int ID)
        {
            List <AccountType> accountTypes= new List<AccountType>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Type_ID, Type, Description, if(active=1,\"Active\",\"Inactive\") as Active from accounttype it where Type_ID= @id", conn);
            cmd.Parameters.AddWithValue("@id", ID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accountTypes.Add(new AccountType()
                {
                    TypeID = Convert.ToInt32(reader["Type_ID"]),
                    Type = reader["Type"].ToString(),
                    Description = reader["Description"].ToString(),
                    Active = reader["Active"].ToString(),
                    Logs = new List<Log>()
                }) ;
            }
            conn.Close();
            return accountTypes;
        }
        public void CreateNewAccountType(AccountType accountType ,string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO AccountType (Type,Description) values(@Type, @Description)", conn);
            cmd.Parameters.AddWithValue("@Type", accountType.Type);
            cmd.Parameters.AddWithValue("@Description", accountType.Description);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            string Value = "Account type created with type: "+accountType.Type+" and description: "+accountType.Description;
            conn.Open();
            cmd = new MySqlCommand("Select Type_ID from AccountType order by Type_ID desc limit 1", conn);
            int ID = 0;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ID = Convert.ToInt32(reader["Type_ID"]);
            }
            conn.Close();
            LogCreate(Table, ID, Value, Admin.Account.UserID);
        }
        public void UpdateAccountType(AccountType accountType, string Type, string Description, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            if (String.Compare(accountType.Type, Type) !=0) 
            {
                LogUpdate(Table, accountType.TypeID, "Type", accountType.Type, Type, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Accounttype set Type = @type where Type_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", accountType.TypeID);
                cmd.Parameters.AddWithValue("@type", Type);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
            if(String.Compare(accountType.Description, Description) != 0)
            {
                LogUpdate(Table, accountType.TypeID, "Description", accountType.Description, Description, Admin.Account.UserID);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Update Accounttype set Description = @Description where Type_ID = @uuid", conn);
                cmd.Parameters.AddWithValue("@uuid", accountType.TypeID);
                cmd.Parameters.AddWithValue("@Description", Description);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                }
                conn.Close();
            }
        }
        public void DeactivateAccountType(AccountType accountType, string Reason, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update AccountType set Deactivate_reason = @Reason, Active=0 where Type_ID = @uuid", conn);
            cmd.Parameters.AddWithValue("@uuid", accountType.TypeID);
            cmd.Parameters.AddWithValue("@Reason", Reason);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            string Value = "Account type created with type: " + accountType.Type + " and description: " + accountType.Description;
            LogDeactivate(Table, accountType.TypeID, Value, Reason);
        }
        public void ActivateAccountType(AccountType accountType, string Table) 
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update AccountType set Deactivate_reason = Null, Active=1 where Type_ID = @uuid", conn);
            cmd.Parameters.AddWithValue("@uuid", accountType.TypeID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            string Value = "Account type created with type: " + accountType.Type + " and description: " + accountType.Description;
            LogActivate(Table, accountType.TypeID, Value);
        }
        public ICollection<AccountType> ListAllAccountTypes()
        {
            List<AccountType> accountTypes = new List<AccountType>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Type_ID, Type, Description, if(active=1,\"Active\",\"Inactive\") as Active from accounttype it", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accountTypes.Add(new AccountType()
                {
                    TypeID = Convert.ToInt32(reader["Type_ID"]),
                    Type = reader["Type"].ToString(),
                    Description = reader["Description"].ToString(),
                    Active = reader["Active"].ToString()
                });
            }
            conn.Close();
            return accountTypes;
        }
        public List<SelectListItem> ListActiveAccountTypes()
        {
            List<SelectListItem> accounts = new List<SelectListItem>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Type_ID, Type, Description from accounttype where Active = 1", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new SelectListItem(reader["Type"].ToString() + " " + reader["Description"].ToString(), reader["Type_ID"].ToString()));
            }
            conn.Close();
            return accounts;
        }
        #endregion
        #region Application
        public List<Application> GetApplicationByID(int ID)
        {
            List<Application> accountTypes = new List<Application>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select App_ID, Name, if(active=1,\"Active\",\"Inactive\") Active from Application where App_ID = @id", conn);
            cmd.Parameters.AddWithValue("@id", ID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accountTypes.Add(new Application() 
                { 
                    AppID= Convert.ToInt32(reader["App_ID"]),
                    Name = reader["Name"].ToString(),
                    Active = reader["Active"].ToString(),
                    Logs = new List<Log>()
                });
            }
            conn.Close();
            return accountTypes;
        }
        public List<SelectListItem> ListActiveApplications()
        {
            List<SelectListItem> accounts = new List<SelectListItem>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select App_ID, Name from Application where Active = 1", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new SelectListItem(reader["Name"].ToString(), reader["App_ID"].ToString()));
            }
            conn.Close();
            return accounts;
        }
        #endregion
        #region Menu
        public ICollection<Menu> ListFirstMenuLevel()
        {
            List<Menu> list = new List<Menu>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "SELECT * from Menu where parent_id is null ORDER BY parent_id, Menu_id ASC";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Menu()
                {
                    MenuId = Convert.ToInt32(reader["Menu_id"]),
                    Label = reader["label"].ToString(),
                    URL = reader["link_url"].ToString()
                });
            }
            conn.Close();
            return list;
        }
        public ICollection<Menu> ListSecondMenuLevel(int menuID)
        {
            List<Menu> list = new List<Menu>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "SELECT * from Menu where parent_id = @menu_id ORDER BY parent_id, Menu_id ASC";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@menu_id", menuID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Menu()
                {
                    MenuId = Convert.ToInt32(reader["Menu_id"]),
                    Label = reader["label"].ToString(),
                    URL = reader["link_url"].ToString()
                });
            }
            conn.Close();
            return list;
        }
        public ICollection<Menu> ListPersonalMenu(int level, int menuID)
        {
            List<Menu> list = new List<Menu>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "Select m1.* " +
                    "from role_perm rp " +
                    "join permissions p on rp.perm_id = p.perm_id " +
                    "join menu m on rp.menu_id = m.Menu_id " +
                    "join menu m1 on m1.parent_id = m.Menu_id " +
                    "where p.permission = 'Read' and rp.level= @level and m.menu_id = @menu_id";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@level", level);
            cmd.Parameters.AddWithValue("@menu_id", menuID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Menu()
                {
                    MenuId = Convert.ToInt32(reader["Menu_id"]),
                    Label = reader["label"].ToString(),
                    URL = reader["link_url"].ToString()
                });
            }
            conn.Close();
            return list;
        }
        #endregion
        #region GetLogs
        public void GetLogs(string table, int ID, Model model)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            string Sql = "";
            conn.Open();
            switch (table)
            {
                case "identity":
                    Sql = "select Log_Text, Log_Date from log where Identity=@uuid order by Log_date desc";
                    break;
                case "identitytype":
                    Sql = "select Log_Text, Log_Date from log where IdentityType=@uuid order by Log_date desc";
                    break;
                case "account":
                    Sql = "select Log_Text, Log_Date from log where Account=@uuid order by Log_date desc";
                    break;
                case "accounttype":
                    Sql = "select Log_Text, Log_Date from log where AccountType=@uuid order by Log_date desc";
                    break;
                case "role":
                    Sql = "select Log_Text, Log_Date from log where Role=@uuid order by Log_date desc";
                    break;
                case "roletype":
                    Sql = "select Log_Text, Log_Date from log where RoleType=@uuid order by Log_date desc";
                    break;
                case "assettype":
                    Sql = "select Log_Text, Log_Date from log where AssetType=@uuid order by Log_date desc";
                    break;
                case "menu":
                    Sql = "select Log_Text, Log_Date from log where menu=@uuid order by Log_date desc";
                    break;
                case "permissions":
                    Sql = "select Log_Text, Log_Date from log where permissions=@uuid order by Log_date desc";
                    break;
                case "role_perm":
                    Sql = "select Log_Text, Log_Date from log where role_perm_id=@uuid order by Log_date desc";
                    break;
                case "application":
                    Sql = "select Log_Text, Log_Date from log where Application=@uuid order by Log_date desc";
                    break;
                case "kensington":
                    Sql = "select Log_Text, Log_Date from log where Kensington=@uuid order by Log_date desc";
                    break;
                case "admin":
                    Sql = "select Log_Text, Log_Date from log where Admin=@uuid order by Log_date desc";
                    break;
                case "mobile":
                    Sql = "select Log_Text, Log_Date from log where IMEI=@uuid order by Log_date desc";
                    break;
                case "subscriptiontype":
                    Sql = "select Log_Text, Log_Date from log where SubscriptionType=@uuid order by Log_date desc";
                    break;
                case "subscription":
                    Sql = "select Log_Text, Log_Date from LOG where Subscription=@uuid order by Log_date desc";
                    break;
            }
            MySqlCommand cmd = new MySqlCommand(Sql, conn);
            cmd.Parameters.AddWithValue("@uuid", ID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                model.Logs.Add(new Log()
                {
                    LogDate = DateTime.Parse(reader["Log_Date"].ToString()),
                    LogText = reader["Log_Text"].ToString()
                });
            }
            conn.Close();
        }
        public void GetLogs(string table, string AssetTag, Model model)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            string Sql = "";
            conn.Open();
            switch (table)
            {
                case "devices":
                    Sql = "select Log_Text, Log_Date from log where AssetTag=@uuid order by Log_date desc";
                    break;
                case "token":
                    Sql = "select Log_Text, Log_Date from log where AssetTag=@uuid order by Log_date desc";
                    break;
            }
            MySqlCommand cmd = new MySqlCommand(Sql, conn);
            cmd.Parameters.AddWithValue("@uuid", AssetTag);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                model.Logs.Add(new Log()
                {
                    LogDate = DateTime.Parse(reader["Log_Date"].ToString()),
                    LogText = reader["Log_Text"].ToString()
                });
            }
            conn.Close();
        }
        #endregion
        public bool HasAdminAccess(Admin admin, string site, string action)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            string SQL = "Select rp.role_perm_id, Level, M.label Menu, p.permission "
                + "from role_perm rp "
                + "join Menu m on rp.menu_id = m.Menu_id "
                + "join permissions p on rp.perm_id = p.perm_id "
                + "where rp.level=@level and m.label=@part and p.permission=@action";
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(SQL, conn);            
            cmd.Parameters.AddWithValue("@level", admin.Level);
            cmd.Parameters.AddWithValue("@part", site);
            cmd.Parameters.AddWithValue("@action", action);
            var reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            int i = 0;
            while (reader.Read())
            {
                i += 1;
            }
            if (i > 0)
                return true;
            else
                return false;
        }
        public Admin Login(string userID, string pwd)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string sql = "Select Acc_ID from Account a join application ap on a.application = ap.App_ID where UserID = @userid and ap.name='CMDB'";
            int accID = 0;
            int level = 0;
            string Password = "";
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
        #region log functions
        protected void LogCreate(string table, int ID, string Value, string AdminName)
        {
            this.LogText = "The " + Value + " is created by " + AdminName + " in table " + table;
            DoLog(table, ID);
        }
        protected void LogCreate(string table, string AssetTag, string Value, string AdminName)
        {
            this.LogText = "The " + Value + " is created by " + AdminName + " in table " + table;
            DoLog(table, AssetTag);
        }
        protected void LogUpdate(string table, int ID, string field, string oldValue, string newValue, string AdminName)
        {
            if (String.IsNullOrEmpty(oldValue))
                oldValue = "Empty";
            if (String.IsNullOrEmpty(newValue))
                newValue = "Empty";
            LogText = "The "+ field +" in table "+ table +" has been changed from "+ oldValue +" to "+ newValue + " by "+AdminName;
            DoLog(table, ID);
        }
        protected void LogUpdate(string table, string AssetTag, string field, string oldValue, string newValue, string AdminName)
        {
            if (String.IsNullOrEmpty(oldValue))
                oldValue = "Empty";
            if (String.IsNullOrEmpty(newValue))
                newValue = "Empty";
            LogText = "The " + field + " in table " + table + " has been changed from " + oldValue + " to " + newValue + " by " + AdminName;
            DoLog(table, AssetTag);
        }
        protected void LogDeactivate(string table, int ID, string value, string reason)
        {
            LogText = "The "+value+" in table "+table+" is deleted due to "+reason+" by "+Admin.Account.UserID;
            DoLog(table, ID);
        }
        protected void LogActivate(string table, int ID, string value)
        {
            LogText = "The " + value + " in table " + table + " is activated by " + Admin.Account.UserID;
            DoLog(table, ID);
        }
        protected void LogAssignIden2Account(string table, int ID, Identity identity, Account account)
        {
            LogText = "The Identity width name:" + identity.Name + " in table " + table + " is assigned to Account with UserID" + account.UserID + " by " + Admin.Account.UserID;
            DoLog(table, ID);
        }
        protected void LogAssignAccount2Identity(string table, int ID, Account account, Identity identity)
        {
            LogText = "The Account with UserID " + account.UserID + " in table " + table + " is assigned to Identity width name:" + identity.Name + " by " + Admin.Account.UserID;
            DoLog(table, ID);
        }
        protected void LogReleaseAccountFromIdentity(string table, int IdenId, Identity identity, Account account)
        {
            LogText = "Identity with Name " + identity.Name + " in table " + table + " is released from Account with UserID " + account.UserID + " in appliction " + account.Application.Name + " by " + Admin.Account.UserID;
            DoLog(table, IdenId);
        }
        protected void LogReleaseIdentity4Account(string table, int AccId, Identity identity, Account account)
        {
            LogText = "Account with UserID " + account.UserID + " in appliction " + account.Application.Name + " in table " + table + " is released from Identity with Name " + identity.Name + " by " + Admin.Account.UserID;
            DoLog(table, AccId);
        }
        private void DoLog(string table, int ID)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
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
            cmd.Parameters.AddWithValue("@log_date", this.LogDate.ToString("yyyy-MM-dd H:mm:ss"));
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
        }
        private void DoLog(string table, string AssetTag)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
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
            cmd.Parameters.AddWithValue("@log_date", this.LogDate.ToString("yyyy-MM-dd H:mm:ss"));
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
        }
        #endregion
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