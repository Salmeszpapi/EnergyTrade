using Csaba.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnergyTrade.Models
{
    public class ProductWithUser
    {
        public Brand Brand { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Coffein { get; set; }
        public int Sugar { get; set; }
        public string Image { get; set; }
        public int UserID { get; set; }
        public List<Brand> Brands { get; set; }
        public int ProductID { get; set; }

    }
}