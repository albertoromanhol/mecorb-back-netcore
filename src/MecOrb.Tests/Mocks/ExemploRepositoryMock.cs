using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using MecOrb.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MecOrb.Tests.Mocks
{
    public class ExemploRepositoryMock : IExemploRepository
    {
        public void Incluir(Exemplo exemplo)
        {
        }

        public Exemplo ObterPorId(Guid id)
        {
            return Exemplos.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Exemplo> ListarTodos()
        {
            return Exemplos;
        }

        private static IEnumerable<Exemplo> Exemplos
        {
            get
            {
                var exemplo1 = new Exemplo(
                        new Nome("João", "Silva"),
                        new CPF("12345678910"),
                        new Email("joaosilva@gmail.com"),
                        "abc123");

                var exemplo2 = new Exemplo(
                        new Nome("Maria", "Silva"),
                        new CPF("01987654321"),
                        new Email("mariasilva@gmail.com"),
                        "xpz789");

                typeof(Exemplo).GetProperty("Id").SetValue(exemplo1, Guid.Parse("f8a0db6b-dabf-4f97-9b1c-8cf08b930466"));
                typeof(Exemplo).GetProperty("DataCriacao").SetValue(exemplo1, new DateTime(2011, 2, 2));

                typeof(Exemplo).GetProperty("Id").SetValue(exemplo2, Guid.Parse("6fdc66ad-649f-4f3c-9806-6409e8ca4e47"));
                typeof(Exemplo).GetProperty("DataCriacao").SetValue(exemplo2, new DateTime(2010, 1, 1));

                return new Exemplo[]
                {
                        exemplo1, exemplo2
                };
            }
        }
    }
}
