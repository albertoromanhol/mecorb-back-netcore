using MecOrb.Domain.ValueObjects;
using Xunit;

namespace MecOrb.Tests.ValueObjects
{
    public class EmailTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("joao")]
        [InlineData("@teste")]
        [InlineData("@teste.com")]
        [InlineData("joaoteste.com.br")]
        public void CriarEmail_EmailInvalido_Test(string endereco)
        {
            var email = new Email(endereco);

            Assert.True(email.Invalid);
            Assert.Contains(email.Notifications, n => n.Property == nameof(Email.Endereco));
        }

        [Theory]
        [InlineData("joao@email.com")]
        [InlineData("joao.souza@email.com")]
        [InlineData("joao.silva@microsoft.com.br")]
        public void CriarEmail_EmailValido_Test(string endereco)
        {
            var email = new Email(endereco);

            Assert.True(email.Valid);
        }
    }
}
