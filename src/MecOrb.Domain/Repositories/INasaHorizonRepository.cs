using MecOrb.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MecOrb.Domain.Repositories
{
    public interface INasaHorizonRepository
    {
        Task<Dictionary<string, VectorXYZ>> GetEphemerities(int bodyId, long? simulationDate = null);
    }
}
