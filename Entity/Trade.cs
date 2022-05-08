using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csaba.Entity {
    public class Trade : BaseEntity {
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string SenderItems { get; set; }
        public string ReceiverItems { get; set; }
        public DateTime Date { get; set; }
        public int IsCompleted { get; set; }
    }
}