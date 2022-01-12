using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csaba.Entity {
    public class User : BaseEntity {
        public string Name { get; set; }
        public string Password { get; set; }

    }
}