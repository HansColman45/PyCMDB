namespace CMDB.API.Models
{
    public class AssetTypeDTO: ModelDTO
    {
        public int TypeID { get; set; }
        public required string Vendor { get; set; }
        public required string Type { get; set; }
    }
}
