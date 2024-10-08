namespace CMDB.API.Models
{
    public class AssetCategoryDTO: ModelDTO
    {
        public int Id { get; set; }
        public required string Category { get; set; }
        public required string Prefix { get; set; }
    }
}
