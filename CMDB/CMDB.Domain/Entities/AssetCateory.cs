using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMDB.Domain.Entities
{
    [Table("category")]
    public class AssetCategory : Model
    {
        public AssetCategory()
        {
            Types = new List<AssetType>();
            Laptops = new List<Laptop>();
            Desktops = new List<Desktop>();
            Dockings = new List<Docking>();
            Kensingtons = new List<Kensington>();
            Screens = new List<Screen>();
            Mobiles = new List<Mobile>();
            Tokens = new List<Token>();
            Subscriptions = new List<Subscription>();
            SubscriptionTypes = new List<SubscriptionType>();
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill in a category")]
        public string Category { get; set; }
        public string Prefix { get; set; }

        public virtual ICollection<AssetType> Types { get; set; }
        public ICollection<Laptop> Laptops { get; set; }
        public ICollection<Desktop> Desktops { get; set; }
        public ICollection<Docking> Dockings { get; set; }
        public ICollection<Kensington> Kensingtons { get; set; }
        public ICollection<Screen> Screens { get; set; }
        public ICollection<Mobile> Mobiles { get; set; }
        public ICollection<Token> Tokens { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<SubscriptionType> SubscriptionTypes { get; set; }
    }
}