using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tracker.Models;
using Tracker.Util;
using Microsoft.AspNet.Identity;
using leeksnet.AspNet.Identity.TableStorage;
using System.Globalization;

namespace Tracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly MarkerRepository _markerRepository;
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public HomeController() : this(
                new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>("DefaultEndpointsProtocol=https;AccountName=hefesoft;AccountKey=dodn17DT7hBi3lXrWlvXihLS9J7xuItHLIpWLBZn2QEMdBHm02Lqxr055rNCpP5z3FhfcjjX3MhPy1Npk3VF3Q==",
                id => id.GetHashCode().ToString(CultureInfo.InvariantCulture)
                )))
        {
            _markerRepository = new MarkerRepository();            
        }

        public HomeController(UserManager<ApplicationUser> userManager)
        {

            UserManager = userManager;
        }

        public ActionResult Index()
        {

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            if (User.Identity.IsAuthenticated)
            {

                 var user = UserManager.FindByName(User.Identity.Name);
                 return RedirectToAction("Index", "Mapa", user);
            }

            return View();

            
        }

        [HttpGet]
        public ActionResult Sync()
        {
            return View(_markerRepository.GetMarkers("355227042112508"));
        }

        [HttpGet]
        public ActionResult Async()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMarkersAsync(string imei)
        {

             return Json(_markerRepository.GetMarkers(imei));
        }
    }
}
