using AutoMapper;
using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using MecOrb.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MecOrb.Infrastructure.Repositories
{
    public class ExemploRepository : IExemploRepository
    {
        private readonly IMapper _mapper;

        public ExemploRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Incluir(Exemplo exemplo)
        {
            // Persistir no banco
        }

        public IEnumerable<Exemplo> ListarTodos()
        {
            var exemplos = RepoFake.Exemplos; // Recuperar do banco

            return _mapper.Map<IEnumerable<ExemploDbModel>, IEnumerable<Exemplo>>(exemplos);
        }

        public Exemplo ObterPorId(Guid id)
        {
            var exemplo = RepoFake.Exemplos.Where(x => x.Id == id).FirstOrDefault(); // Recuperar do banco

            return _mapper.Map<ExemploDbModel, Exemplo>(exemplo);
        }

        private class RepoFake
        {
            public static IEnumerable<ExemploDbModel> Exemplos
            {
                get
                {
                    return new ExemploDbModel[]
                    {
                        new ExemploDbModel()
                        {
                            Id = Guid.Parse("b5d028d5-1c11-40de-88df-832c0c0f36b9"),
                            Nome = "João",
                            Sobrenome = "Silva",
                            Email = "joaosilva@gmail.com",
                            Cpf = "12345678910",
                            Segmento = "abc123",
                            DataCriacao = new DateTime(2011, 2, 2)
                        },

                        new ExemploDbModel()
                        {
                            Id = Guid.Parse("340eefcb-c0b2-4c66-aa45-3a594e349dac"),
                            Nome = "Maria",
                            Sobrenome = "Silva",
                            Email = "mariasilva@gmail.com",
                            Cpf = "01987654321",
                            Ddd = 21,
                            Telefone = "999881122",
                            Segmento = $"xpz789",
                            DataCriacao = new DateTime(2010, 1, 1)
                        }
                    };
                }
            }
        }
    }    
}