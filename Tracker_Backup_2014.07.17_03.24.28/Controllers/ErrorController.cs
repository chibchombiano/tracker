using Microsoft.AspNet.SignalR;
using SignalRChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tracker.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {
                
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}