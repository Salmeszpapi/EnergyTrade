using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnergyTrade.Models
{
    public class myHub :Hub
    {
        public void test(string message)
        {
            Clients.All.Test(message);
        }
    }
}