using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    public class Admin : Model
    {
        [Column("Admin_id")]
        [Key]
        public int AdminId { get; set; }
        [Required(ErrorMessage = "Please select an account")]
        public Account Account { get; set; }
        public int? AccountId { get; set; }
        [Required(ErrorMessage = "Please select an level")]
        public int Level { get; set; }
        public string Password { get; set; }
        public DateTime DateSet { get; set; }
    }
}
