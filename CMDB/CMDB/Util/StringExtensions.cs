namespace CMDB.Util
{
    public static class StringExtensions
    {
        public static bool GetBool(this string input)
        {
            if(input.Contains("true") || input.Contains("True") || input.Contains("TRUE"))
                return true;
            else
                return false;
        }
    }
}
