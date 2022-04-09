using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csaba.Entity {
    public class Trade : BaseEntity {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public List<int> SenderItems { get; set; }
        public List<int> ReceiverItems { get; set; }
        public DateTime Date { get; set; }
        public byte[] Image { get; set; }
    }
}