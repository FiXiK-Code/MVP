using Microsoft.AspNetCore.SignalR;

namespace MVP.SignalR
{
    public class MyHub : Hub
    {
        public string GetConnetcionId() => Context.ConnectionId;
    }
}
