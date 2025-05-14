namespace CMDB.API.Models
{
    /// <summary>
    /// The DTO for AssetType
    /// </summary>
    public class AssetTypeDTO: ModelDTO
    {
        /// <summary>
        /// The Id of the AssetType
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// The vendor of the AssetType
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// The type of the AssetType
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The AssetCategory of the AssetType
        /// </summary>
        public AssetCategoryDTO AssetCategory { get; set; }
        /// <summary>
        /// The Id of the AssetCategory of the AssetType
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// The description of the AssetType
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Vendor + " " + Type;
        }
    }
}
