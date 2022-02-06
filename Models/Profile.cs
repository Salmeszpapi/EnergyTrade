using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnergyTrade.Models
{
    public class Profile
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string password { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime DataJoined { get; set; }
        public bool OwnProfile { get; set; }

    }

}