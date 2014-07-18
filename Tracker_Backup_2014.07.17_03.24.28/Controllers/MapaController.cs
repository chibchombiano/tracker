using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tracker.Models;

namespace Tracker.Controllers
{
    public class MapaController : Controller
    {
        private readonly MarkerRepository _markerRepository;

        public MapaController()
        {
            _markerRepository = new MarkerRepository();
        }

        // GET: Mapa
        public ActionResult Index(ApplicationUser user)
        {
            return View(user);
        }

        [HttpPost]
        public ActionResult GetMarkers(string imei)
        {
            return Json(_markerRepository.GetMarkers(imei));
        }
    }
}