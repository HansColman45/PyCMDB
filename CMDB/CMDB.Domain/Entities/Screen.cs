using System.Collections.Generic;

namespace CMDB.Domain.Entities
{
    public class Screen : Device
    {
        public Screen()
        {
            Keys = new List<Kensington>();
        }
        public virtual List<Kensington> Keys { get; set; }
    }
}