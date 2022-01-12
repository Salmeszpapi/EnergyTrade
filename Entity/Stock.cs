using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csaba.Entity {
    public class Stock : BaseEntity {
        public User User { get; set; }
        public List<StockItem> StockItems { get; set; }

    }
}