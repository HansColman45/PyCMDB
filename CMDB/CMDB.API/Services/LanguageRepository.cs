using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface ILanguageRepository
    {
        Task<List<LanguageDTO>> GetAll();
        Task<LanguageDTO?> GetByCode(string code);
    }
    public class LanguageRepository : GenericRepository, ILanguageRepository
    {
        public LanguageRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<List<LanguageDTO>> GetAll()
        {
            var languages =  await _context.Languages.AsNoTracking()
                .Select(x => ConvertLanguage(x))
                .ToListAsync();
            return languages;
        }
        public async Task<LanguageDTO?> GetByCode(string code)
        {
            var language =  await _context.Languages.AsNoTracking()
                .Where(x => x.Code == code).AsNoTracking()
                .Select(x => ConvertLanguage(x))
                .FirstOrDefaultAsync();
            return language;
        }

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
