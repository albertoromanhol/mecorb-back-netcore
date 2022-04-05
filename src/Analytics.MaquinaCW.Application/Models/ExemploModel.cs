using System;

namespace Analytics.MaquinaCW.Application.Models
{
    public class ExemploModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cpf { get; set; }
        public int? Ddd { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Segmento { get; set; }
    }
}
