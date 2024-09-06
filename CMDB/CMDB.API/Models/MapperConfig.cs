using AutoMapper;
using CMDB.Domain.Entities;
namespace CMDB.API.Models
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Account, AccountDTO>();
                cfg.CreateMap<AccountType,TypeDTO>();
                cfg.CreateMap<IdentityType,TypeDTO>();
                cfg.CreateMap<Language, LanguageDTO>();
                cfg.CreateMap<Admin, AdminDTO>();
                cfg.CreateMap<Identity, IdentityDTO>();
                cfg.CreateMap<Log, LogDTO>();
                cfg.CreateMap<IdenAccount, IdenAccountDTO>();
            });
            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
