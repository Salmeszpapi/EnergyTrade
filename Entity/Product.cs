using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csaba.Entity {
    public class Product : BaseEntity {
        public Brand Brand { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Coffein { get; set; }
        public int Sugar { get; set; }
        public byte[] Image { get; set; }
        public int Seen { get; set; }
    }
}