using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CMDB.Models
{
    public class Admin:Model
    {
        [Column("Admin_id")]
        [Key]
        public int Adminid { get; set; }
        [Required(ErrorMessage = "Please select an account")]
        public Account Account { get; set; }
        [Required(ErrorMessage = "Please select an level")]
        public int Level { get; set; }
        public string Password { get; set; }
        public DateTime DateSet { get; set; }
    }
}
