using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csaba.Entity {
    public class StockItem : BaseEntity {
        public Product Product { get; set; }
        public int Count { get; set; }
        public Stock Stock {get; set;}
    }
}