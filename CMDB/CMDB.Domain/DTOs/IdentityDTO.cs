using CMDB.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// The DTO file for Identity <see cref="Identity"/>"/>
    /// </summary>
    public class IdentityDTO : ModelDTO
    {
        /// <summary>
        /// The primary key for the Identity
        /// </summary>
        public int IdenId { get; set; }
        /// <summary>
        /// The name of the Identity
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Lastname of the Identity
        /// </summary>
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
        /// <summary>
        /// The Firstname of the Identity
        /// </summary>
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
        /// <summary>
        /// The Email address of the Identity
        /// </summary>
        [EmailAddress]
        public string EMail { get; set; }
        /// <summary>
        /// The UserID of the Identity
        /// </summary>
        [Required(ErrorMessage = "Please fill in a UserID")]
        public string UserID { get; set; }
        /// <summary>
        /// The Company of the Identity
        /// </summary>
        [Required(ErrorMessage = "Please fill in a Company")]
        public string Company { get; set; }
        /// <summary>
        /// The Language of the Identity
        /// </summary>
        [Required(ErrorMessage = "Please select a Language")]
        public LanguageDTO Language { get; set; }
        /// <summary>
        /// The Type of the Identity
        /// </summary>
        [Required(ErrorMessage = "Please select a Type")]
        public TypeDTO Type { get; set; }
        /// <summary>
        /// The linked accounts
        /// </summary>
        public virtual ICollection<IdenAccountDTO> Accounts { get; set; }
        /// <summary>
        /// The linked devices
        /// </summary>
        public virtual ICollection<DeviceDTO> Devices { get; set; }
        /// <summary>
        /// The linked mobiles
        /// </summary>
        public virtual ICollection<MobileDTO> Mobiles { get; set; }
        /// <summary>
        /// The linked subscriptions
        /// </summary>
        public virtual ICollection<SubscriptionDTO> Subscriptions { get; set; }
        /// <summary>
        /// constructor for the IdentityDTO
        /// </summary>
        public IdentityDTO()
        {
            Accounts = new List<IdenAccountDTO>();
            Devices = new List<DeviceDTO>();
            Mobiles = new List<MobileDTO>();
            Subscriptions = new List<SubscriptionDTO>();
        }
    }
}