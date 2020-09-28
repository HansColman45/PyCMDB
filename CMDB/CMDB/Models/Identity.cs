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

        public ICollection<Device> Devices { get; set; }
        public ICollection<IdenAccount> Accounts { get; set; }
    }
}
