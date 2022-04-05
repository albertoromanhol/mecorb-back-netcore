using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Analytics.MaquinaCW.CrossCutting.Assemblies
{
    [ExcludeFromCodeCoverage]
    public class AssemblyUtil
    {
        public static IEnumerable<Assembly> GetCurrentAssemblies()
        {            
            return new Assembly[]
            {
                Assembly.Load("Analytics.MaquinaCW.Api"),
                Assembly.Load("Analytics.MaquinaCW.Application"),
                Assembly.Load("Analytics.MaquinaCW.Domain"),
                Assembly.Load("Analytics.MaquinaCW.Domain.Core"),
                Assembly.Load("Analytics.MaquinaCW.Infrastructure"),
                Assembly.Load("Analytics.MaquinaCW.CrossCutting")
            };
        }
    }
}
