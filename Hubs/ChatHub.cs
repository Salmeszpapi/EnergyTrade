using Csaba.Entity;
using Microsoft.AspNet.SignalR;
using SSM.Common.Services.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EnergyTrade.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            EnergyContext db = new EnergyContext();
            var Message = db.Messages.ToList();
            Message messages = new Message();
            messages.UserName = name;
            messages.Text = message;
            messages.Time = DateTime.Now;
            db.Messages.Add(messages);
            db.SaveChanges();
            Clients.All.addNewMessageToPage(name,message);
        }
    }
}