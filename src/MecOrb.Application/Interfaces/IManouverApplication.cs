using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Interfaces
{
    public interface IManouverApplication
    {
        ManouverResult Hohmann(ManouverConfigModel manouverConfigModel);
        ManouverResult BiElliptic(ManouverConfigModel manouverConfigModel);
    }
}