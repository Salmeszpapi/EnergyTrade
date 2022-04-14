using EnergyTrade.Models;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;

namespace EnergyTrade.Hubs
{
    public class CounterHub : Hub
    {
        private static int counter = 0;
        private List<SimpleUser> Users;

        public override System.Threading.Tasks.Task OnConnected()
        {
            counter = counter + 1;
            Clients.All.UpdateCount(counter);
            string username = Context.User.Identity.Name;
            //find group based on username
            GetUSers();
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            counter = counter + 1;
            Clients.All.UpdateCount(counter);
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            counter = counter - 1;
            Clients.All.UpdateCount(counter);
            return base.OnDisconnected(stopCalled);
        }
        public void GetUSers()
        {

        }
    }
}