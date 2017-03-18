using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace AllOdds.hubs
{
    public class OddsHub : Hub
    {
        public void notifyUpdate()
        {            
            Clients.All.notifyUpdate();
        }
    }
}