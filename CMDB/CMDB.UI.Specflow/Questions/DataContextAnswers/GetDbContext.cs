using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.UI.Specflow.Questions.DataContextAnswers
{
    public class GetDbContext : Question<CMDBContext>
    {
        public override CMDBContext PerformAs(IPerformer actor)
        {
            string connectionstring = "Server=.;Database=CMDB;User Id=sa;Password=Gr7k6VKW92dteZ5n;encrypt=false;";
            var options = new DbContextOptionsBuilder<CMDBContext>()
                .UseSqlServer(connectionstring)
                .EnableSensitiveDataLogging()
                .Options;
            return new CMDBContext(options);
        }
    }
}
