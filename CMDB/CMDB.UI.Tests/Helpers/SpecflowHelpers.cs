using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Helpers
{
    public class Identity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public string Language { get; set; }
    }
    public class Account
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Application { get; set; }
    }
    public class Device
    {
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
    }
    public class Laptop : Device
    {
        public string MAC { get; set; }
        public string RAM { get; set; }
    }
    public class Desktop : Device
    {
        public string MAC { get; set; }
        public string RAM { get; set; }
    }
    public class DockingStation : Device 
    {
    }
}
