using Suframa.Sciex.DataAccess.Database;
using System;

namespace Suframa.Sciex.DataAccess
{
	public interface ICommandRepositorySciex<T> where T : BaseEntity
	{
		void Apagar(int id);

		void Apagar(long id);

		void ApagarAssinc(int id);

		void Salvar(T entity, bool forcedInclude);

		void Salvar(T entity);

		Int64 Salvar(string SQL);
	}
}