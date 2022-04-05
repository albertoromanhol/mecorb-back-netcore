using Flunt.Notifications;

namespace Analytics.MaquinaCW.Domain.Core.Entities
{
    public abstract class Entity : Notifiable
    {
        public int Id { get; set; }        
    }
}
