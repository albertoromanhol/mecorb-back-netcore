using Flunt.Notifications;

namespace MecOrb.Domain.Core.ValueObjects
{
    public abstract class ValueObject : Notifiable
    {
        public ValueObject GetCopy()
        {
            return MemberwiseClone() as ValueObject;
        }
    }
}
