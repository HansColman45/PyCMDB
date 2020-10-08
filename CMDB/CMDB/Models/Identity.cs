using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Models
{
    [Table("identity")]
    public class Identity : Model
    {
        [Key]
        [Column("Iden_ID")]
        public int IdenID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName
        {
            get
            {
                if (String.IsNullOrEmpty(Name))
                    return "";
                else
                    return Name.Split(",")[1];
            }
            set => Name = FirstName + ", " + value;
        }
        [Required]
        public string FirstName
        {
            get
            {
                if (String.IsNullOrEmpty(Name))
                    return "";
                else
                    return Name.Split(",")[0];
            }
            set => Name = value + ", " + LastName;
        }
        [Required]
        [Column("E_Mail")]
        [EmailAddress]
        public string EMail { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public Language Language { get; set; }
        [Required]
        public IdentityType Type { get; set; }

        public ICollection<Device> Devices { get; set; }
        public ICollection<IdenAccount> Accounts { get; set; }
    }
}