using Flunt.Notifications;

namespace MecOrb.Domain.Core.Entities
{
    public abstract class Entity : Notifiable
    {
        public int Id { get; set; }        
    }
}
