using System;

namespace MecOrb.Application.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CodeMetric { get; set; }
        public bool Active { get; set; }
        
    }
}
