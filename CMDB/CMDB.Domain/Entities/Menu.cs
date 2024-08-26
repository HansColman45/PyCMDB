using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


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
        [JsonIgnore]
        public Menu Parent { get; set; }
        public int? ParentId { get; set; }

        [JsonIgnore]
        public ICollection<Menu> Children { get; set; }
        [JsonIgnore]
        public ICollection<RolePerm> Permissions { get; set; }
        [JsonIgnore]
        public ICollection<Log> Logs { get; set; }
    }
}
