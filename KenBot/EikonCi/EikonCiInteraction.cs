using Microsoft.AspNet.SignalR;

namespace KenBot.EikonCi
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EikonCiInteraction : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }
    }
}