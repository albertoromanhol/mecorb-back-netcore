﻿using MecOrb.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MecOrb.Domain.Repositories
{
    public interface IExemploRepository
    {
        void Incluir(Exemplo exemplo);
        Exemplo ObterPorId(Guid id);
        IEnumerable<Exemplo> ListarTodos();
    }
}