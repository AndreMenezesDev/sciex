using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureAgendaAtendimento(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgendaAtendimentoEntity>()
                .Property(e => e.CpfCnpj)
                .IsUnicode(false);

            modelBuilder.Entity<AgendaAtendimentoEntity>()
                .Property(e => e.NomeRazaoSocial)
                .IsUnicode(false);
        }
    }
}