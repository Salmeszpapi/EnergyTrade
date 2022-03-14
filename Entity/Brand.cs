using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csaba.Entity {
    public class Brand : BaseEntity {
        public string Name { get; set; }
        public bool Checked { get; set; }
        public byte[] Image { get; set; }
    }
}