using AutoMapper;
using Analytics.MaquinaCW.Domain.Entities;
using Analytics.MaquinaCW.Domain.Repositories;
using Analytics.MaquinaCW.Infrastructure.DbModels;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Analytics.MaquinaCW.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IMapper _mapper;
        private readonly IDbConnection _connection;

        public ClientRepository(IMapper mapper, IDbConnection dbConnection)
        {
            _mapper = mapper;
            _connection = dbConnection;
        }

        public IEnumerable<Client> ListAllByUser(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@userId", userId, DbType.Int32);
            IEnumerable<ClientDbModel> clients = _connection.Query<ClientDbModel>(SQL_CLIENTS_BY_USER, parameters);
            
            return _mapper.Map<IEnumerable<ClientDbModel>, IEnumerable<Client>>(clients);
        }

        private const string SQL_CLIENTS_BY_USER = "SELECT c.id as Id  " +
                                                   "     , c.nome as Name " +
                                                   "     , c.codigo_cliente_metric as CodeMetric " +
                                                   "     , c.ativo as Active " +
                                                   "FROM Cliente c " +
                                                   "inner join UsuarioCliente uc on c.Id = uc.cliente_id " +
                                                   "where uc.usuario_id = @userId ";        
    }    
}