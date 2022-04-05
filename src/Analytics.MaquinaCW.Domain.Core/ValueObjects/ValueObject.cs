using Flunt.Notifications;

namespace Analytics.MaquinaCW.Domain.Core.ValueObjects
{
    public abstract class ValueObject : Notifiable
    {
        public ValueObject GetCopy()
        {
            return MemberwiseClone() as ValueObject;
        }
    }
}
