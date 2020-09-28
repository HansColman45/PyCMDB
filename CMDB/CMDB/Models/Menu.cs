using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Models
{
    public class Menu : Model
    {
        [Column("Menu_id")]
        [Key]
        public int MenuId { get; set; }
        [Column("label")]
        public string Label { get; set; }
        [Column("link_url")]
        public string URL { get; set; }
        [Column("parent_id")]
        public Menu Parent { get; set; }
        public List<Menu> Children { get; set; }
    }
}
