namespace CMDB.Infrastructure
{
    public interface IDatabaseNameFactory
    {
        string Create(string postfix = null);
    }
    public class DatabaseNameFactory : IDatabaseNameFactory
    {
        public string Create(string postfix = null)
        {
            return ("CMDB");
        }
    }
}
