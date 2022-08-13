using System.Security.Cryptography;
using System.Text;

namespace CMDB.Infrastructure
{
    public interface IPasswordHasher
    {
        string EncryptPassword(string password);
    }
    public class PasswordHasher : IPasswordHasher
    {
        public string EncryptPassword(string password)
        {
            MD5 md5 = MD5.Create();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
