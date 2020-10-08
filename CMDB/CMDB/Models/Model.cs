using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace CMDB.Models
{
    public class Model
    {
        public ICollection<Log> Logs { get; set; }
        public string Active { get; set; }
        [Column("Deactivate_reason")]
        public string DeactivateReason { get; set; }
        
    }
}
