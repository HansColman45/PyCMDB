using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// The interface of the language repo
    /// </summary>
    public interface ILanguageRepository
    {
        /// <summary>
        /// This will return a list of all know Languages
        /// </summary>
        /// <returns>List of <see cref="LanguageDTO"/></returns>
        Task<List<LanguageDTO>> GetAll();
        /// <summary>
        /// This will return a language by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns><see cref="LanguageDTO"/></returns>
        Task<LanguageDTO> GetByCode(string code);
    }
    /// <summary>
    /// The class of the Language Repo
    /// </summary>
    public class LanguageRepository : GenericRepository, ILanguageRepository
    {
        private LanguageRepository()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public LanguageRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc/>
        public async Task<List<LanguageDTO>> GetAll()
        {
            var languages =  await _context.Languages.AsNoTracking()
                .Select(x => ConvertLanguage(x))
                .ToListAsync();
            return languages;
        }
        /// <inheritdoc/>
        public async Task<LanguageDTO> GetByCode(string code)
        {
            var language =  await _context.Languages.AsNoTracking()
                .Where(x => x.Code == code).AsNoTracking()
                .Select(x => ConvertLanguage(x))
                .FirstOrDefaultAsync();
            return language;
        }
        /// <summary>
        /// This will convert the Language to a DTO
        /// </summary>
        /// <param name="laguage"><see cref="Language"/></param>
        /// <returns><see cref="LanguageDTO"/></returns>
        public static LanguageDTO ConvertLanguage(Language laguage)
        {
            return new()
            {
                Code = laguage.Code,
                Description = laguage.Description
            };
        }
    }
}
