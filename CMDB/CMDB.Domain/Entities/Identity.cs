using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    [Table("identity")]
    public class Identity : Model
    {
        public Identity()
        {
            Devices = new List<Device>();
            Mobiles = new List<Mobile>();
            Accounts = new List<IdenAccount>();
            Subscriptions = new List<Subscription>();
        }

        [Key]
        public int IdenId { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please fill in a Lastname")]
        public string LastName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return "";
                else
                    if (Name == "Stock")
                        return Name;
                    else
                        return Name.Split(',')[1].Trim();
            }
            set => Name = FirstName + ", " + value;
        }
        [NotMapped]
        [Required(ErrorMessage = "Please fill in a firstname")]
        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return "";
                else
                    if (Name == "Stock")
                        return Name;
                    else
                        return Name.Split(',')[0];
            }
            set => Name = value + ", " + LastName;
        }
        [Required(ErrorMessage = "Please fill in a E-Mail address")]
        [EmailAddress]
        public string EMail { get; set; }
        [Required(ErrorMessage = "Please fill in a UserID")]
        public string UserID { get; set; }
        [Required(ErrorMessage = "Please fill in a Company")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Please select a Language")]
        public Language Language { get; set; }
        [Required(ErrorMessage = "Please select a Type")]
        public IdentityType Type { get; set; }

        public int? TypeId { get; set; }
        public string LanguageCode { get; set; }
        public ICollection<Device> Devices { get; set; }
        public ICollection<Mobile> Mobiles { get; set; }
        public virtual ICollection<IdenAccount> Accounts { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}