using System;

namespace CMDB.Util
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert string to bool
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool GetBool(this string input)
        {
            if (input.Contains("true") || input.Contains("True") || input.Contains("TRUE"))
                return true;
            else
                return false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
