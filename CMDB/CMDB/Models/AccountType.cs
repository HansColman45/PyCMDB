using System.ComponentModel.DataAnnotations;

namespace CMDB.Models
{
    public class AccountType : Model
    {
        [Key]
        public int TypeID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}