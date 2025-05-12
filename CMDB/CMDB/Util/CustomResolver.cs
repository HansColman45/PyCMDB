using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CMDB.Util
{
    /// <summary>
    /// Custom JSON resolver to ignore virtual properties
    /// </summary>
    public class CustomResolver : DefaultContractResolver
    {
        private readonly List<string> _namesOfVirtualPropsToKeep = new List<string>(new String[] { });
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomResolver() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="namesOfVirtualPropsToKeep"></param>
        public CustomResolver(IEnumerable<string> namesOfVirtualPropsToKeep)
        {
            this._namesOfVirtualPropsToKeep = namesOfVirtualPropsToKeep.Select(x => x.ToLower()).ToList();
        }
        /// <summary>
        /// Creates a property
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);
            var propInfo = member as PropertyInfo;
            if (propInfo != null)
            {
                if (propInfo.GetMethod.IsVirtual && !propInfo.GetMethod.IsFinal
                    && !_namesOfVirtualPropsToKeep.Contains(propInfo.Name.ToLower()))
                {
                    prop.ShouldSerialize = obj => false;
                }
            }
            return prop;
        }
    }
}
