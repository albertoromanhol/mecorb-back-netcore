using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Interfaces
{
    public interface IManouverApplication
    {
        ManouverResult Simulate(ManouverConfigModel manouverConfigModel);
    }
}