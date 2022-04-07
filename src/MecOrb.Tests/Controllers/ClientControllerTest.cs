using MecOrb.Application.Models;
using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using MecOrb.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MecOrb.Tests.Controllers
{
    [Collection("Mapper")]
    public class ClientControllerTest
    {
        private readonly MapperFixture _mapperFixture;
        private Mock<IPlanetRepository> ClienteRepository;

        public ClientControllerTest(MapperFixture mapperFixture)
        {
            _mapperFixture = mapperFixture;
            ClienteRepository = new Mock<IPlanetRepository>();
        }

        [Theory]
        [InlineData(1)]
        public void ListClients_ByUser_Test(int id)
        {
            ClienteRepository.Setup(m => m.ListAllByUser(id)).Returns(new List<Planet>() { new Planet("Sanye", 1, true), new Planet("Sanye teste", 5, true) });
            var controller = CreateClientController();
            var result = controller.List(id);

            Assert.IsType<OkObjectResult>(result);

            var objectResult = GetOkObject<IEnumerable<PlanetModel>>(result);
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
