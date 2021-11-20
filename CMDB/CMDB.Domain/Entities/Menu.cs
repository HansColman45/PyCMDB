using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMDB.Domain.Entities
{
    public class Menu
    {
        public Menu()
        {
            Logs = new List<Log>();
        }

        [Key]
        public int MenuId { get; set; }
        [Required(ErrorMessage = "Please fill in a label")]
        public string Label { get; set; }
        public string URL { get; set; }
        public Menu Parent { get; set; }
        public int? ParentId { get; set; }
        public ICollection<Menu> Children { get; set; }
        public ICollection<RolePerm> Permissions { get; set; }
        public ICollection<Log> Logs { get; set; }
    }
}
