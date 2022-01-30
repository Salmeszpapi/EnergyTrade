using Csaba.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnergyTrade.Models
{
    public class ProductWithUser
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Coffein { get; set; }
        public int Sugar { get; set; }
        public byte[] Image { get; set; }
        public int Username { get; set; }

    }
}