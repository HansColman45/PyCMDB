namespace CMDB.API.Models
{
    public class AssetCategoryDTO: ModelDTO
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string? Prefix { get; set; }
    }
}
