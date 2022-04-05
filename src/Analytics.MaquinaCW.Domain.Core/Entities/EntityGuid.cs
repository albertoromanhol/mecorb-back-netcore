using Flunt.Notifications;
using System;

namespace Analytics.MaquinaCW.Domain.Core.Entities
{
    public abstract class EntityGuid : Notifiable
    {
        private Guid _id;
        public virtual Guid Id
        {
            get => _id;
            protected set => _id = value;
        }

        protected EntityGuid() => Id = Guid.NewGuid();
    }
}
