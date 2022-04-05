using Analytics.MaquinaCW.Domain.Entities;
using System.Collections.Generic;

namespace Analytics.MaquinaCW.Domain.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> ListAllByUser(int userId);
    }
}
