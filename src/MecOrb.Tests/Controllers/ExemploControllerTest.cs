using MecOrb.Application;
using MecOrb.Domain.ValueObjects;
using MecOrb.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MecOrb.Tests.Controllers
{
    [Collection("Mapper")]
    public class ExemploControllerTest
    {
        private readonly MapperFixture _mapperFixture;
        private Mock<IExemploRepository> exemploRepository;

        public ExemploControllerTest(MapperFixture mapperFixture)
        {
            _mapperFixture = mapperFixture;
            exemploRepository = new Mock<IExemploRepository>();
        }

        [Theory]
        [InlineData("f8a0db6b-dabf-4f97-9b1c-8cf08b930466")]
        [InlineData("6fdc66ad-649f-4f3c-9806-6409e8ca4e47")]
        public void ObterDadosExemplo_ExemplosExistentes_Test(string id)
        {
            exemploRepository.Setup(m => m.ObterPorId(It.IsAny<Guid>())).Returns(new Exemplo(new Nome("sANYE", "cAROLINE"), new CPF("11111111111"), new Email("teste@teste.com")));
            var controller = CreateExemploController();
            var result = controller.Get(Guid.Parse(id));

            Assert.IsType<OkObjectResult>(result);
            exemploRepository.Verify(m => m.ObterPorId(It.IsAny<Guid>()), Times.Once);
            exemploRepository.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("42f41603-5269-4c0d-9ce2-afa8d293240b")]
        [InlineData("3dd64d9c-aaef-4a98-9b2c-5f6d7fc28ead")]
        public void ObterDadosExemplo_ExemplosInexistentes_Test(string id)
        {
            var controller = CreateExemploController();
            var result = controller.Get(Guid.Parse(id));

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ListarExemplos_ExemplosCadastrados_Test()
        {
            exemploRepository.Setup(m => m.ListarTodos()).Returns(new List<Exemplo>() { new Exemplo(new Nome("sANYE", "cAROLINE"), new CPF("11111111111"), new Email("teste@teste.com")), new Exemplo(new Nome("sANYE", "cAROLINE"), new CPF("11111111111"), new Email("teste@teste.com")) });
            var controller = CreateExemploController();
            var result = controller.List();

            Assert.IsType<OkObjectResult>(result);

            var objectResult = GetOkObject<IEnumerable<ExemploModel>>(result);
            Assert.Equal(2, objectResult.Count());
        }

        [Fact]
        public void InserirExemplo_ExemploInvalido_Test()
        {
            var model = new ExemploModel()
            {
                Nome = "Jo�o",
                Sobrenome = "Silva"
            };

            exemploRepository.Setup(m => m.Incluir(It.IsAny<Exemplo>()));
            var controller = CreateExemploController();
            var result = controller.Post(model);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void InserirExemplo_ExemploValido_Test()
        {
            var model = new ExemploModel()
            {
                Nome = "Jo�o",
                Sobrenome = "Silva",
                Cpf = "12345678910",
                Ddd = 11,
                Telefone = "999009900",
                Email = "joao@gmail.com",
                Segmento = "aqw187"
            };

            exemploRepository.Setup(m => m.Incluir(It.IsAny<Exemplo>()));
            var controller = CreateExemploController();
            var result = controller.Post(model);

            Assert.IsType<CreatedResult>(result);
        }

        private ExemploController CreateExemploController()
        {
            var exemploApplication = new PlanetApplication(_mapperFixture.Mapper, exemploRepository.Object);

            return new ExemploController(_mapperFixture.Mapper, exemploApplication, exemploRepository.Object);
        }

        private T GetOkObject<T>(IActionResult result)
        {
            var okObjectResult = (OkObjectResult)result;
            return (T)okObjectResult.Value;
        }
    }
}
