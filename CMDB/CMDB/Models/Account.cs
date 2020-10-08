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
        public string UserID { get; set; }
        public AccountType Type { get; set; }
        public Application Application { get; set; }

        public ICollection<IdenAccount> Identities { get; set; }
    }
}