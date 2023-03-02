using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.SignalR
{
    public class MyHub : Hub
    {
        public void PullMassage()
        {
            Clients.All.SendAsync("Тестовое сообщение!");
        }
    }
}
