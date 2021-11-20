using System.Collections.Generic;

namespace CMDB.Domain.Entities
{
    public class Docking : Device
    {
        public Docking()
        {
            Keys = new List<Kensington>();
        }
        public virtual List<Kensington> Keys { get; set; }
    }
}