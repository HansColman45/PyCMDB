namespace CMDB.API.Models
{
    public class AssetTypeDTO: ModelDTO
    {
        public int TypeID { get; set; }
        public string Vendor { get; set; }
        public string Type { get; set; }
        public AssetCategoryDTO AssetCategory { get; set; }
        public int? CategoryId { get; set; }

        public override string ToString()
        {
            return Vendor + " " + Type;
        }
    }
}
