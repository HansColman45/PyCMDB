using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    [Table("identity")]
    public class Identity:Model
    {
        private static readonly string Table = "identity";
        [Key]
        [Column("Iden_ID")]
        public int IdenID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Column("E_Mail")]
        public string EMail { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public IdentityType Type { get; set; }
        public int TypeID { get; set; }
        

        public ICollection<Device>Devices { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Identity> ListAll(MySqlConnection connection)
        {
            List<Identity> list = new List<Identity>();
            using MySqlConnection conn = connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Iden_Id, Name, UserID, E_Mail, Language, it.Type, if(i.active=1,\"Active\",\"Inactive\") as Active, i.Deactivate_reason from Identity i join identitytype it on i.type = it.type_id", conn);
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
                    Type = new IdentityType() { 
                        Description = reader["Type"].ToString()
                    }
                });
            }
            conn.Close();
            return list;
        }
        public List<IdentityType> ActiveIdentityTypes(MySqlConnection connection)
        {
            List <IdentityType> identityTypes= new List<IdentityType>();
            using MySqlConnection conn = connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Type_ID, Type, Description from identitytype it where active = 1", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                identityTypes.Add(new IdentityType() { 
                    TypeID = Convert.ToInt32(reader["Type_ID"]),
                    Type = reader["Type"].ToString(),
                    Description = reader["Description"].ToString()
                });
            }
            return identityTypes;
        }
        public ICollection<Identity> ListByID(MySqlConnection connection, int id)
        {
            List<Identity> list = new List<Identity>();
            using MySqlConnection conn = connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Select Iden_Id, Name, UserID, E_Mail, Language, it.Type_ID, it.Type, if(i.active=1,\"Active\",\"Inactive\") as Active, i.Deactivate_reason "
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
                    Type = new IdentityType()
                    {
                        TypeID = Convert.ToInt32(reader["Type_ID"]),
                        Description = reader["Type"].ToString()
                    },
                    Devices = new List<Device>(),
                    Accounts= new List<Account>(),
                    Logs = new List<Log>()
                });
            }
            conn.Close();
            return list;
        }
        public void Create(MySqlConnection connection, string AdminName)
        {
            using MySqlConnection conn = connection;
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO Identity (Name,UserID,Type,Language,Company,E_Mail) values(@name, @userid, @type, @language, @company, @email)", conn);
            cmd.Parameters.AddWithValue("@name", this.Name);
            cmd.Parameters.AddWithValue("@userID", this.UserID);
            cmd.Parameters.AddWithValue("@Type", this.TypeID);
            cmd.Parameters.AddWithValue("@language", this.Language);
            cmd.Parameters.AddWithValue("@company", this.Company);
            cmd.Parameters.AddWithValue("@email", this.EMail);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
            }
            conn.Close();
            conn.Open();
            string Value = "Identity width name: "+this.Name;
            cmd = new MySqlCommand("Select Iden_ID from Identity order by Iden_ID desc limit 1", conn);
            reader = cmd.ExecuteReader();
            int ID = 0;
            while (reader.Read())
            {
                ID = Convert.ToInt32(reader["Iden_Id"]);
            }
            conn.Close();
            LogCreate(Table, ID, Value, AdminName, connection);
        }
        public void GetAssignedDevices(MySqlConnection connection, int IdenID)
        {
            using MySqlConnection conn = connection;
            conn.Open();
            string SQL= "select c.Category, a.AssetTag, a.SerialNumber, at.Vendor, at.Type, if(a.Active=1,\"Active\",\"Inactive\") as Active, a.Identity "
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
            cmd.Parameters.AddWithValue("@uuid", IdenID);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                this.Devices.Add(new Device()
                {
                    AssetTag = reader["AssetTag"].ToString(),
                    SerialNumber = reader["SerialNumber"].ToString(),
                    Category = new AssetCategory(){
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
    }
}
