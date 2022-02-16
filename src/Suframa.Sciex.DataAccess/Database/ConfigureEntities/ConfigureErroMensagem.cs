using Suframa.Sciex.DataAccess.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
	public partial class DatabaseContextSciex : DbContext
	{
		private static void ConfigureErroMensagem(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ErroMensagemEntity>()
				.HasMany(e => e.ErroProcessamento)
				.WithRequired(e => e.ErroMensagem)
				.WillCascadeOnDelete(false);
		}
	}
}