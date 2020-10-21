using Microsoft.AspNetCore.Builder;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace CMDB.Models
{
    public class Account: Model
    {
        [Key]
        public int AccID { get; set; }
        [Required(ErrorMessage = "Please fill in a UserID")]
        public string UserID { get; set; }
        [Required(ErrorMessage = "Please select a type")]
        public AccountType Type { get; set; }
        [Required(ErrorMessage = "Please select an application")]
        public Application Application { get; set; }

        public ICollection<IdenAccount> Identities { get; set; }
    }
}