using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tracker.Util
{
    public class Tracker :  TableEntity
    {     
        public string direccion { get; set; }

        public string imei { get; set; }

        public string lat { get; set; }

        public string longitud { get; set; }

        public string tick { get; set; }

        public string anio_servidor { get; set; }

        public string mes_servidor { get; set; }

        public string dia_servidor { get; set; }

        public string hora_servidor { get; set; }

        public string minuto_servidor { get; set; }

        public string segundo_servidor { get; set; }
     
        //public List<Task> Tasks { get; set; }
    }

    public class Task
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}