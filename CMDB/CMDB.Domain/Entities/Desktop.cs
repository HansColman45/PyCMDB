using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class Desktop : Device
    {
        public Desktop()
        {
            Keys = new List<Kensington>();
        }
        public string MAC { get; set; }
        [Required(ErrorMessage = "Please select the amount of RAM")]
        public string RAM { get; set; }
        public virtual List<Kensington> Keys { get; set; }
    }
}