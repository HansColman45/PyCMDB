using System;

namespace CMDB.Domain.Entities
{
    public class Configuration
    {
        public string Code { get; set; }
        public string SubCode { get; set; }
        public DateTime? CFN_Date { get; set; }
        public int? CFN_Number { get; set; }
        public string CFN_Tekst { get; set; }
        public string Description { get; set; }
    }
}
