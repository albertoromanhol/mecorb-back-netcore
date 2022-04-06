using Flunt.Validations;
using MecOrb.Domain.Core.Entities;
using MecOrb.Domain.ValueObjects;
using System;

namespace MecOrb.Domain.Entities
{
    public class Exemplo : EntityGuid, IAggregateRoot
    {
        public Exemplo(Nome nome, CPF cpf, Email email)
        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            DataCriacao = DateTime.Now;

            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(Nome, nameof(Nome), "Nome não pode ser nulo")
                .IsNotNull(Cpf, nameof(Cpf), "Cpf não pode ser nulo")
                .IsNotNull(Email, nameof(Email), "Email não pode ser nulo"));

            if (Nome != null)
                AddNotifications(Nome);

            if (Cpf != null)
                AddNotifications(Cpf);

            if (Email != null)
                AddNotifications(Email);
        }

        public Exemplo(Nome nome, CPF cpf, Email email, string segmento)
            : this(nome, cpf, email)
        {
            AlterarSegmento(segmento);
        }


        public Nome Nome { get; private set; }
        public CPF Cpf { get; private set; }
        public Telefone Telefone { get; private set; }
        public Email Email { get; private set; }
        public string Segmento { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public void InformarOuAlterarTelefone(Telefone telefone)
        {
            Telefone = telefone;

            if (Telefone != null)
                AddNotifications(telefone);
        }

        public void AlterarSegmento(string segmento)
        {
            Segmento = segmento;

            if (Segmento == null)
                return;

            AddNotifications(new Contract()
                .Requires()
                .HasLen(Segmento, 6, nameof(Segmento), "Segmento deve conter 6 posições")
                .Matchs(Segmento, "^[a-zA-Z]{3}[0-9]{3}$", nameof(Segmento), "Segmento está fora do padrão de nomenclatura"));
        }
    }
}
