using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace CMDB.Models
{
    public class CMDBContext : DbContext
    {
        private string LogText;
        public string ConnectionString { get; set; }
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
        public CMDBContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Identity>().ToTable("identity");
        }
        public ICollection<Identity> ListAllIdenties()
        {
            List<Identity> list = new List<Identity>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Iden_Id, Name, UserID, E_Mail, Language, it.Type, if(i.active=1,\"Active\",\"Inactive\") as Active, "+
                "i.Deactivate_reason from Identity i join identitytype it on i.type = it.type_id", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Identity()
                {
                    IdenID = Convert.ToInt32(reader["Iden_Id"]),
                    Name = reader["Name"].ToString(),
                    EMail = reader["E_Mail"].ToString(),
                    Language = reader["Language"].ToString(),
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
            MySqlCommand cmd = new MySqlCommand("Select Iden_Id, Language,Name, UserID, E_Mail, Company, it.Type_ID, it.Type, if(i.active=1,\"Active\",\"Inactive\") as Active, i.Deactivate_reason "
                + "from Identity i join identitytype it on i.type = it.type_id where Iden_Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Identity()
                {
                    IdenID = Convert.ToInt32(reader["Iden_Id"]),
                    Name = reader["Name"].ToString(),
                    EMail = reader["E_Mail"].ToString(),
                    Language = reader["Language"].ToString(),
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
        public List<IdentityType> ListActiveIdentityTypes()
        {
            List<IdentityType> identityTypes = new List<IdentityType>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Type_ID, Type, Description from identitytype it where active = 1", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                identityTypes.Add(new IdentityType()
                {
                    TypeID = Convert.ToInt32(reader["Type_ID"]),
                    Type = reader["Type"].ToString(),
                    Description = reader["Description"].ToString()
                });
            }
            return identityTypes;
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
                + "where ia.Identity = @uuid";
            MySqlCommand cmd = new MySqlCommand(SQL, conn);
            cmd.Parameters.AddWithValue("@uuid", identity.IdenID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DateTime validUntil = new DateTime(9999,12,12);
                if (!String.IsNullOrEmpty(reader["ValidEnd"].ToString()))
                    validUntil = DateTime.Parse(reader["ValidEnd"].ToString());
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
                    ValidUntil = validUntil
                });
            }
            conn.Close();
        }
        public void CreateNewIdentity(string firstName,string LastName, int type, string UserID, string Company, string EMail, string Language, Admin admin, string Table)
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
            LogCreate(Table, ID, Value, admin.Account.UserID);
        }
        public void EditIdentity(Identity identity, string firstName, string LastName, int type, string UserID, string Company, string EMail, string Language, Admin admin, string Table)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            string OldFirst = identity.Name.Split(",")[0];
            string OldLast = identity.Name.Split(",")[1];
            if(String.Compare(OldFirst,firstName) != 0)
            {
                LogUpdate(Table, identity.IdenID, "FirstName", OldFirst, firstName, admin.Account.UserID);
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
            if(String.Compare(OldLast,LastName) != 0)
            {
                LogUpdate(Table, identity.IdenID, "LastName", OldLast, LastName, admin.Account.UserID);
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
                LogUpdate(Table, identity.IdenID, "Company", identity.Company, Company, admin.Account.UserID);
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
            if(String.Compare(identity.Language,Language) != 0)
            {
                LogUpdate(Table, identity.IdenID, "Language", identity.Language, Language, admin.Account.UserID);
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
                LogUpdate(Table, identity.IdenID, "EMail", identity.EMail, EMail, admin.Account.UserID);
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
                LogUpdate(Table, identity.IdenID, "UserID", identity.UserID, UserID, admin.Account.UserID);
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
                LogUpdate(Table, identity.IdenID, "Type", identity.Type.Type, newType.Type, admin.Account.UserID);
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
                        Name = reader["Name"].ToString()
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
        public ICollection<Account> GetAccountByID(int ID)
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
                        Name = reader["Name"].ToString()
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
        public List<Account> ListAllFreeAccounts()
        {
            List<Account> accounts = new List<Account>();
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
            conn.Open();
            string SQL = "select a.Acc_ID,a.UserID,app.App_ID, app.Name Application " +
                "from account a "+
                "join application app on a.Application = app.App_ID "+
                "where a.Acc_ID not in (select Account from idenaccount ia where now() between ia.ValidFrom and IFNULL(ia.ValidEnd,now()+1)) " +
                "and a.Active = 1";
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
                        Name = reader["Name"].ToString()
                    }
                }) ;
            }
            conn.Close();
            return accounts;
        }
        public ICollection<AccountType> ListAllAccountTypes()
        {
            List<AccountType> accountTypes = new List<AccountType>();

            return accountTypes;
        }
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
        public void GetLogs(string table, int ID, Model model)
        {
            MySqlConnection Connection = new MySqlConnection(ConnectionString);
            using MySqlConnection conn = Connection;
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
                model.Logs.Add(new Log()
                {
                    LogDate = DateTime.Parse(reader["Log_Date"].ToString()),
                    LogText = reader["Log_Text"].ToString()
                });
            }
            conn.Close();
        }
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
        private void LogCreate(string table, int ID, string Value, string AdminName)
        {
            this.LogText = "The " + Value + " is created by " + AdminName + " in table " + table;
            DoLog(table, ID);
        }
        private void LogCreate(string table, string AssetTag, string Value, string AdminName)
        {
            this.LogText = "The " + Value + " is created by " + AdminName + " in table " + table;
            DoLog(table, AssetTag);
        }
        private void LogUpdate(string table, int ID, string field, string oldValue, string newValue, string AdminName)
        {
            if (String.IsNullOrEmpty(oldValue))
                oldValue = "Empty";
            if (String.IsNullOrEmpty(newValue))
                newValue = "Empty";
            LogText = "The "+ field +" in table "+ table +" has been changed from "+ oldValue +" to "+ newValue + " by "+AdminName;
            DoLog(table, ID);
        }
        private void LogUpdate(string table, string AssetTag, string field, string oldValue, string newValue, string AdminName)
        {
            if (String.IsNullOrEmpty(oldValue))
                oldValue = "Empty";
            if (String.IsNullOrEmpty(newValue))
                newValue = "Empty";
            LogText = "The " + field + " in table " + table + " has been changed from " + oldValue + " to " + newValue + " by " + AdminName;
            DoLog(table, AssetTag);
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
            cmd.Parameters.AddWithValue("@log_date", new DateTime());
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
            cmd.Parameters.AddWithValue("@log_date", new DateTime());
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
        }
    }
}