using MecOrb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MecOrb.Domain.Repositories
{
    public interface INasaHorizonRepository
    {
        Task<Dictionary<string, Vector3>> GetEphemerities(int bodyId, DateTime? simulationDate = null);
    }
}
