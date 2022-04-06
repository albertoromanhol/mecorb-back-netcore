using Microsoft.AspNetCore.Mvc;
using MecOrb.Api.Controllers;
using MecOrb.Application;
using MecOrb.Application.Models;
using MecOrb.Tests.Fixtures;
using MecOrb.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using MecOrb.Domain.Repositories;
using MecOrb.Domain.Entities;
using MecOrb.Domain.ValueObjects;

namespace MecOrb.Tests.Controllers
{
    [Collection("Mapper")]
    public class ClientControllerTest
    {
        private readonly MapperFixture _mapperFixture;
        private Mock<IClientRepository> ClienteRepository;

        public ClientControllerTest(MapperFixture mapperFixture)
        {
            _mapperFixture = mapperFixture;
            ClienteRepository = new Mock<IClientRepository>();
        }

        [Theory]
        [InlineData(1)]
        public void ListClients_ByUser_Test(int id)
        {
            ClienteRepository.Setup(m => m.ListAllByUser(id)).Returns(new List<Client>() { new Client("Sanye", 1, true), new Client("Sanye teste", 5, true) });
            var controller = CreateClientController();
            var result = controller.List(id);

            Assert.IsType<OkObjectResult>(result);
            
            var objectResult = GetOkObject<IEnumerable<ClientModel>>(result);
            Assert.Equal(2, objectResult.Count());
        }      

        private ClientController CreateClientController()
        {                        
            
            return new ClientController(_mapperFixture.Mapper, ClienteRepository.Object);
        }

        private T GetOkObject<T>(IActionResult result)
        {
            var okObjectResult = (OkObjectResult)result;
            return (T)okObjectResult.Value;
        }
    }
}
