using Csaba.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnergyTrade.Models
{
    public class TradeAgent
    {
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public DateTime Date { get; set; }
        public List<Product> SenderItems { get; set; }
        public List<Product> ReceiverItems { get; set; }
        public int IsComplete { get; set; }
    }
}