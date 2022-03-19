using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.Entities
{
    public class Desktop : Device
    {
        public string MAC { get; set; }
        [Required(ErrorMessage = "Please select the amount of RAM")]
        public string RAM { get; set; }
    }
}