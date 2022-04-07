using MecOrb.Domain.Core.Entities;

namespace MecOrb.Domain.Entities
{
    public class Simulation : Entity, IAggregateRoot
    {
        public string Result { get; set; }
    }
}
