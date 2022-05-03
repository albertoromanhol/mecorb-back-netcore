using System;

namespace MecOrb.Application
{
    class CollisionException : Exception
    {
        public CollisionException(string body1, string body2) : base($"Colisão entre os corpos {body1} e {body2}") { }
    }
}