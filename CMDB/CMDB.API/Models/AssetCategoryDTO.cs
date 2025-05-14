namespace CMDB.API.Models
{
    /// <summary>
    /// The AssetCategoryDTO class represents a Data Transfer Object (DTO) for asset categories.
    /// </summary>
    public class AssetCategoryDTO: ModelDTO
    {
        /// <summary>
        /// The unique identifier for the asset category.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Category name of the asset.
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// The prefix for the asset category.
        /// </summary>
        public string Prefix { get; set; }
    }
}
