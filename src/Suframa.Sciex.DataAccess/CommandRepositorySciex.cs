using Suframa.Sciex.DataAccess.Database;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Suframa.Sciex.DataAccess
{
    public class CommandRepositorySciex<T> : ICommandRepositorySciex<T> where T : BaseEntity
    {
        private readonly IDatabaseContextSciex _dbContextSciex;

        internal CommandRepositorySciex(IDatabaseContextSciex dbContextSciex)
        {
			_dbContextSciex = dbContextSciex;
        }

        private void Adicionar(T entity)
        {
			_dbContextSciex.Set<T>().Add(entity);
		}

        private void Atualizar(T entity)
        {

			var e = _dbContextSciex.Entry(entity);
				_dbContextSciex.Entry(entity).State = EntityState.Modified;
	

        }

        public void Apagar(int id)
        {
            var entityTrackeable = _dbContextSciex.Set<T>().Find(id);
            if (entityTrackeable == null) { return; }
			_dbContextSciex.Set<T>().Remove(entityTrackeable);
        }

		public void Apagar(long id)
		{
			var entityTrackeable = _dbContextSciex.Set<T>().Find(id);
			if (entityTrackeable == null) { return; }
			_dbContextSciex.Set<T>().Remove(entityTrackeable);
		}

		public async void ApagarAssinc(int id)
        {
            var entityTrackeable = await _dbContextSciex.Set<T>().FindAsync(id);
			_dbContextSciex.Set<T>().Remove(entityTrackeable);
        }

        public void Salvar(T entity, bool forcedInclude)
        {
            var props = typeof(T)
                .GetProperties()
                .Where(prop =>
                    Attribute.IsDefined(prop,
                        typeof(System.ComponentModel.DataAnnotations.KeyAttribute)));

			if ((props.First().GetValue(entity).GetType().Name == "Int64" ? (long)props.First().GetValue(entity) : (int)props.First().GetValue(entity)) == 0 || forcedInclude)
            {
				this.Adicionar(entity);

			}
            else
            {
                this.Atualizar(entity);
            }
        }

		public void Salvar(T entity)
		{
			var props = typeof(T)
				.GetProperties()
				.Where(prop =>
					Attribute.IsDefined(prop,
						typeof(System.ComponentModel.DataAnnotations.KeyAttribute)));

			if ((props.First().GetValue(entity).GetType().Name == "Int64" ? (long)props.First().GetValue(entity) : (int)props.First().GetValue(entity)) == 0)
			{
				this.Adicionar(entity);

			}
			else
			{
				this.Atualizar(entity);
			}
		}

		public Int64 Salvar(string SQL)
		{
			Int64 newProdID = 0;

			string connString =  System.Configuration.ConfigurationManager.ConnectionStrings["databasecontextsciex"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connString))
			{
				SqlCommand cmd = new SqlCommand(SQL, conn);
				try
				{
					conn.Open();
					newProdID = (Int64)cmd.ExecuteScalar();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			return (Int64)newProdID;
		}
	}
}