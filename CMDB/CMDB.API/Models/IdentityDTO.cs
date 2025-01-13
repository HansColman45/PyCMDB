using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.API.Models
{
    public class IdentityDTO : ModelDTO
    {
        public int IdenId { get; set; }
        public string Name { get; set; }
        [NotMapped]
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
        [EmailAddress]
        public string EMail { get; set; }
        [Required(ErrorMessage = "Please fill in a UserID")]
        public string UserID { get; set; }
        [Required(ErrorMessage = "Please fill in a Company")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Please select a Language")]
        public LanguageDTO Language { get; set; }
        [Required(ErrorMessage = "Please select a Type")]
        public TypeDTO Type { get; set; }
        public virtual ICollection<IdenAccountDTO> Accounts { get; set; }
        public virtual ICollection<DeviceDTO> Devices { get; set; }
        public virtual ICollection<MobileDTO> Mobiles { get; set; }
        public virtual ICollection<SubscriptionDTO> Subscriptions { get; set; }

        public IdentityDTO()
        {
            Accounts = new List<IdenAccountDTO>();
            Devices = new List<DeviceDTO>();
            Mobiles = new List<MobileDTO>();
            Subscriptions = new List<SubscriptionDTO>();
        }
    }
}