using MecOrb.Domain.Entities;
using System.Collections.Generic;

namespace MecOrb.Domain.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> ListAllByUser(int userId);
    }
}
