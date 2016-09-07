using System;
using KenBot.EikonCi;
using Microsoft.AspNet.SignalR;

namespace Ken
{
    public class EikonCi
    {
        private readonly IHubContext _context;

        public static readonly Lazy<EikonCi> Instance = new Lazy<EikonCi>(() => new EikonCi(GlobalHost.ConnectionManager.GetHubContext<EikonCiInteraction>()));

        public EikonCi(IHubContext context)
        {
            _context = context;
        }

        public void SendMessage(string name, string message)
        {
            _context.Clients.All.Send(name, message);
        }
    }
}