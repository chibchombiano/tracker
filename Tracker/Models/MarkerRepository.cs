using System;
using System.Collections.Generic;
using System.Globalization;
using Tracker.Util;

namespace Tracker.Models
{
    public class MarkerRepository
    {

        public IList<GoogleMarker> GetMarkers(string imei)
        {
            var repo = new ProjectRepository();
            var result = repo.listadoPuntos(imei, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Hour.ToString());
            var googleMarkers = new List<GoogleMarker>();

            foreach (var item in result)
            {
                googleMarkers.Add(new GoogleMarker() 
                { 
                    InfoWindow = item.direccion,
                    Latitude = double.Parse(item.lat, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(item.longitud, CultureInfo.InvariantCulture),
                    SiteName = item.direccion
                
                });
            }

            return googleMarkers;
        }

        internal object GetMarkers(string imei, DateTime fecha)
        {
            var repo = new ProjectRepository();
            var result = repo.listadoPuntos(imei, fecha.Year.ToString(), fecha.Month.ToString(), fecha.Day.ToString());
            var googleMarkers = new List<GoogleMarker>();

            foreach (var item in result)
            {
                googleMarkers.Add(new GoogleMarker()
                {
                    InfoWindow = item.direccion,
                    Latitude = double.Parse(item.lat, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(item.longitud, CultureInfo.InvariantCulture),
                    SiteName = item.direccion

                });
            }

            return googleMarkers;
        }
    }
}