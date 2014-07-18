using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tracker.Util
{
    public class ProjectRepository
    {
        private CloudTable table;

        public ProjectRepository()
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=hefesoft;AccountKey=dodn17DT7hBi3lXrWlvXihLS9J7xuItHLIpWLBZn2QEMdBHm02Lqxr055rNCpP5z3FhfcjjX3MhPy1Npk3VF3Q==";

            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(connectionString);

            var client = storageAccount.CreateCloudTableClient();
            this.table = client.GetTableReference("tracker");
            this.table.CreateIfNotExists();
          }

        public void Insert(Tracker project)
        {
            //project.Id = Guid.NewGuid();

            dynamic entity = new ElasticTableEntity();
            entity.PartitionKey = project.imei;
            entity.RowKey = DateTime.Now.Ticks;

            entity.Document = JsonConvert.SerializeObject(project,
                Newtonsoft.Json.Formatting.Indented);

            // Additional fields for querying (indexes)
            entity.imei = project.imei;
            entity.direccion = project.direccion;
            entity.lat = project.lat;
            entity.longitud = project.longitud;

            this.table.Execute(TableOperation.Insert(entity));
        }

        public IEnumerable<Tracker> List(string partitionKey)
        {
            var query = new TableQuery<ElasticTableEntity>()                
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, partitionKey));

            dynamic entities = table.ExecuteQuery(query).ToList();
            foreach (var entity in entities)
            {
                var document = (string)entity.Document.StringValue;
                yield return JsonConvert.DeserializeObject<Tracker>(document);
            }
        }

        public IEnumerable<Tracker> ListWithTasks(string partitionKey)
        {
            var query = new TableQuery<ElasticTableEntity>()
                .Select(new[] { "Document" })
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey",
                        QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForInt("TotalTasks",
                        QueryComparisons.GreaterThan, 0)));

            dynamic entities = table.ExecuteQuery(query).ToList();
            foreach (var entity in entities)
            {
                var document = (string)entity.Document.StringValue;
                yield return JsonConvert.DeserializeObject<Tracker>(document);
            }
        }


        public List<Tracker> listadoPuntos(string partitionKey)
        {
            TableQuery<Tracker> query1 = new TableQuery<Tracker>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            List<Tracker> lst = new List<Tracker>();

            // Print the fields for each customer.
            foreach (Tracker entity in table.ExecuteQuery(query1))
            {
                lst.Add(new Tracker() 
                { 
                    direccion = entity.direccion,
                    imei = entity.imei,
                    longitud = entity.longitud,
                    tick = entity.RowKey,
                    RowKey = entity.RowKey,
                    PartitionKey = entity.PartitionKey
                });
            }

            return lst;
        }

        public List<Tracker> listadoPuntos(string partitionKey, string rowKey)
        {
            TableQuery<Tracker> query1 = new TableQuery<Tracker>().Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey",
                        QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey",
                        QueryComparisons.Equal, rowKey)));

            List<Tracker> lst = new List<Tracker>();

            // Print the fields for each customer.
            foreach (Tracker entity in table.ExecuteQuery(query1))
            {
                lst.Add(new Tracker()
                {
                    direccion = entity.direccion,
                    imei = entity.imei,
                    longitud = entity.longitud,
                    tick = entity.RowKey,
                    RowKey = entity.RowKey,
                    PartitionKey = entity.PartitionKey
                });
            }

            return lst;
        }

        public List<Tracker> listadoPuntos(string partitionKey, string anio, string mes, string dia, string hora = "-1")
        {

            string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            string Anio = TableQuery.GenerateFilterCondition("anio_servidor", QueryComparisons.Equal, anio);
            string Mes = TableQuery.GenerateFilterCondition("mes_servidor", QueryComparisons.Equal, mes);
            string Dia = TableQuery.GenerateFilterCondition("dia_servidor", QueryComparisons.Equal, dia);
            string Hora;            

            
            //// OR 
            string combinedFilter = string.Format("({0}) {1} ({2}) {3} ({4}) {5} ({6})", pkFilter, TableOperators.And, Anio, TableOperators.And, Mes, TableOperators.And, Dia);

            if (!hora.Equals("-1"))
            {
                Hora = TableQuery.GenerateFilterCondition("hora_servidor", QueryComparisons.Equal, hora);
                combinedFilter += string.Format(" {0} ({1})", TableOperators.And, Hora);
            }

            TableQuery<Tracker> query = new TableQuery<Tracker>().Where(combinedFilter);






            List<Tracker> lst = new List<Tracker>();

            // Print the fields for each customer.
            foreach (Tracker entity in table.ExecuteQuery(query))
            {
                lst.Add(new Tracker()
                {
                    direccion = entity.direccion,
                    imei = entity.imei,
                    longitud = entity.longitud,
                    lat = entity.lat,
                    tick = entity.RowKey,
                    RowKey = entity.RowKey,
                    PartitionKey = entity.PartitionKey,
                    hora_servidor = entity.hora_servidor,
                    minuto_servidor = entity.minuto_servidor,
                    segundo_servidor = entity.segundo_servidor,
                });
            }

            return lst;
        }

        public Tracker Load(string partitionKey, string rowKey)
        {
            var tracker = new Tracker();
            var query = new TableQuery<ElasticTableEntity>()
                //.Select(new[] { "Document" })
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey",
                        QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey",
                        QueryComparisons.Equal, rowKey)));

            dynamic entity = table.ExecuteQuery(query).SingleOrDefault();
            if (entity != null)
            {

                tracker.direccion = entity.Properties.direccion;
                return tracker;
            }

            return null;
        }

        public void Update(Tracker project)
        {
            dynamic entity = new ElasticTableEntity();
            entity.PartitionKey = project.imei;
            //entity.RowKey = project.Id.ToString();
            entity.ETag = "*";

            entity.Document = JsonConvert.SerializeObject(project,
                Newtonsoft.Json.Formatting.Indented);

            // Additional fields for querying (indexes)
            entity.imei = project.imei;
            entity.direccion = project.direccion;
            entity.lat = project.lat;
            entity.longitud = project.longitud;

            this.table.Execute(TableOperation.Replace(entity));
        }

        public void Delete(Tracker project)
        {
            dynamic entity = new ElasticTableEntity();
            entity.PartitionKey = project.imei;
            entity.RowKey = project.tick;
            entity.ETag = "*";

            this.table.Execute(TableOperation.Delete(entity));
        }

        public void Delete(string partitionKey, string rowKey)
        {
            dynamic entity = new ElasticTableEntity();
            entity.PartitionKey = partitionKey;
            entity.RowKey = rowKey;
            entity.ETag = "*";

            this.table.Execute(TableOperation.Delete(entity));
        }
    }
}