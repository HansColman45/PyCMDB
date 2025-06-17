using System.ComponentModel.DataAnnotations;

namespace CMDB.Domain.DTOs
{
    /// <summary>
    /// The Languages know in the Db
    /// </summary>
    public class LanguageDTO
    {
        /// <summary>
        /// The ISO 639-1 code of the language
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// The description of the language
        /// </summary>
        public required string Description { get; set; }
    }
}