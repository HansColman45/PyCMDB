using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    public class Model
    {
        public int active { get; set; }
        public Model()
        {
            Logs = new List<Log>();
        }
        public virtual ICollection<Log> Logs { get; set; }
        [NotMapped]
        public virtual string Active
        {
            get
            {
                return active switch
                {
                    1 => "Active",
                    0 => "Inactive",
                    _ => "",
                };
                ;
            }
            set
            {
                active = value switch
                {
                    "Active" => 1,
                    "Inactive" => 0,
                    _ => 1,
                };
            }
        }
        [Column("Deactivate_reason")]
        public string DeactivateReason { get; set; }

    }
}
