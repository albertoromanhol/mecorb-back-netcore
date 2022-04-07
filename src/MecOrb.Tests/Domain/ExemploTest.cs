using MecOrb.Domain.ValueObjects;
using System;
using Xunit;

namespace MecOrb.Tests.Domain
{
    public class ExemploTest
    {
        [Fact]
        public void CriarExemplo_ExemploInvalido_Test()
        {
            var exemplo = new Exemplo(null, null, null);

            Assert.True(exemplo.Invalid);
            Assert.Contains(exemplo.Notifications, n => n.Property == nameof(Exemplo.Nome));
            Assert.Contains(exemplo.Notifications, n => n.Property == nameof(Exemplo.Cpf));
            Assert.Contains(exemplo.Notifications, n => n.Property == nameof(Exemplo.Email));
        }

        [Fact]
        public void CriarExemplo_ExemploValido_Test()
        {
            var exemplo = new Exemplo(
                new Nome("João", "Silva"),
                new CPF("12345678911"),
                new Email("joao@localiza.com"));

            Assert.True(exemplo.Valid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abc4567")]
        [InlineData("123456")]
        [InlineData("abcdef")]
        [InlineData("abcd12")]
        [InlineData("ab1234")]
        public void CriarExemplo_SegmentoInvalido_Test(string segmento)
        {
            var exemplo = new Exemplo(
                new Nome("João", "Silva"),
                new CPF("12345678911"),
                new Email("joao@localiza.com"),
                segmento);

            Assert.True(exemplo.Invalid);
            Assert.Contains(exemplo.Notifications, n => n.Property == nameof(Exemplo.Segmento));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abc4567")]
        [InlineData("123456")]
        [InlineData("abcdef")]
        [InlineData("abcd12")]
        [InlineData("ab1234")]
        public void AlterarSegmento_Invalido_Test(string segmento)
        {
            var exemplo = new Exemplo(
                new Nome("João", "Silva"),
                new CPF("12345678911"),
                new Email("joao@localiza.com"));

            exemplo.AlterarSegmento(segmento);

            Assert.True(exemplo.Invalid);
            Assert.Contains(exemplo.Notifications, n => n.Property == nameof(Exemplo.Segmento));
        }

        [Theory]
        [InlineData("abc123")]
        [InlineData("XYZ789")]
        [InlineData(null)]
        public void AlterarSegmento_Valido_Test(string segmento)
        {
            var exemplo = new Exemplo(
                new Nome("João", "Silva"),
                new CPF("12345678911"),
                new Email("joao@localiza.com"));

            exemplo.AlterarSegmento(segmento);

            Assert.True(exemplo.Valid);
        }

        [Theory]
        [InlineData(31, "988882233")]
        [InlineData(11, "999992211")]
        public void InformarOuAlterarTelefone_Valido_Test(int ddd, string numero)
        {
            var exemplo = new Exemplo(
                new Nome("João", "Silva"),
                new CPF("12345678911"),
                new Email("joao@localiza.com"));

            exemplo.InformarOuAlterarTelefone(new Telefone(ddd, numero));

            Assert.True(exemplo.Valid);
        }

        [Fact]
        public void InformarOuAlterarTelefone_NuloValido_Test()
        {
            var exemplo = new Exemplo(
                new Nome("João", "Silva"),
                new CPF("12345678911"),
                new Email("joao@localiza.com"));

            exemplo.InformarOuAlterarTelefone(null);

            Assert.True(exemplo.Valid);
        }

        [Theory]
        [InlineData(0, "9888821AA")]
        [InlineData(31, null)]
        public void InformarOuAlterarTelefone_Invalido_Test(int ddd, string numero)
        {
            var exemplo = new Exemplo(
                new Nome("João", "Silva"),
                new CPF("12345678911"),
                new Email("joao@localiza.com"));

            exemplo.InformarOuAlterarTelefone(new Telefone(ddd, numero));

            Assert.True(exemplo.Invalid);
            Assert.Contains(exemplo.Notifications, n => n.Property == nameof(Exemplo.Telefone.Numero));
        }

        [Fact]
        public void CriarExemplo_DataCriacao_Test()
        {
            var exemplo = new Exemplo(
                new Nome("João", "Silva"),
                new CPF("12345678911"),
                new Email("joao@localiza.com"));

            Assert.Equal(DateTime.Today, exemplo.DataCriacao.Date);
        }
    }
}
