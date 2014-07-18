using Microsoft.AspNet.SignalR;
using SignalRChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tracker.Controllers
{
    
    public class SignalController : Controller
    {
        [HttpGet]
        // GET: Signal
        public string Index(string imei, string latitude, string longitude)
        {

            string valor = latitude + ',' + longitude;


            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.addNewMessageToPage(imei, valor);
            return "";
        }
    }
}