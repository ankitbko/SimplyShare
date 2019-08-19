using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplyShare.Tracker.Models
{
    public class CosmosOption
    {
        public string MongoConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
