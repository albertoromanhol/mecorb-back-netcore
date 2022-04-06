using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MecOrb.CrossCutting.Assemblies
{
    [ExcludeFromCodeCoverage]
    public class AssemblyUtil
    {
        public static IEnumerable<Assembly> GetCurrentAssemblies()
        {            
            return new Assembly[]
            {
                Assembly.Load("MecOrb.Api"),
                Assembly.Load("MecOrb.Application"),
                Assembly.Load("MecOrb.Domain"),
                Assembly.Load("MecOrb.Domain.Core"),
                Assembly.Load("MecOrb.Infrastructure"),
                Assembly.Load("MecOrb.CrossCutting")
            };
        }
    }
}
