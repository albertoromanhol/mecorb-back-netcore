﻿using Analytics.MaquinaCW.Domain.Core.Entities;

namespace Analytics.MaquinaCW.Domain.Entities
{
    public class Client : Entity, IAggregateRoot
    {
        public Client(string name, int? codeMetric, bool active)
        {
            Name = name;
            CodeMetric = codeMetric;
            Active = active;            
        }
        public string Name { get; private set; }
        public int? CodeMetric { get; private set; }
        public bool Active { get; private set; }
    }
}
