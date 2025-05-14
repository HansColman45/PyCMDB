using System.ComponentModel.DataAnnotations;

namespace CMDB.API.Models
{
    /// <summary>
    /// The AdminDTO class is used to represent an admin in the system.
    /// </summary>
    public class AdminDTO : ModelDTO
    {
        /// <summary>
        /// The Id of the admin
        /// </summary>
        public int AdminId { get; set; }
        /// <summary>
        /// The linked AccountId of the admin
        /// </summary>
        public int? AccountId { get; set; }
        /// <summary>
        /// The level of the admin number between 0 and 9
        /// </summary>
        [Required(ErrorMessage = "Please select an level")]
        public int Level { get; set; }
        /// <summary>
        /// The hasshed password of the admin
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// The date the password was set
        /// </summary>
        public DateTime DateSet { get; set; }
        /// <summary>
        /// The linked Account of the admin
        /// </summary>
        public AccountDTO Account { get; set; }
    }
}
