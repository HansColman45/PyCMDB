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
        [Required(ErrorMessage = "Please fill in a Lastname")]
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
        [Required(ErrorMessage = "Please fill in a firstname")]
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
        [Required(ErrorMessage = "Please fill in a E-Mail address")]
        [Column("E_Mail")]
        [EmailAddress]
        public string EMail { get; set; }
        [Required(ErrorMessage = "Please fill in a UserID")]
        public string UserID { get; set; }
        [Required(ErrorMessage = "Please fill in a Company")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Please select a Language")]
        public Language Language { get; set; }
        [Required(ErrorMessage = "Please select a Type")]
        public IdentityType Type { get; set; }

        public ICollection<Device> Devices { get; set; }
        public ICollection<IdenAccount> Accounts { get; set; }
    }
}