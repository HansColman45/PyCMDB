using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CMDB.Models
{
    public class Admin
    {
        [Column("Admin_id")]
        [Key]
        public int Adminid { get; set; }
        [Required]
        public Account Account { get; set; }
        [Required]
        public int Level { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime DateSet { get; set; }
    }
}
