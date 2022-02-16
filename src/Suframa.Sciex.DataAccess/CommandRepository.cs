using Suframa.Sciex.DataAccess.Database;
using System;
using System.Data.Entity;
using System.Linq;

namespace Suframa.Sciex.DataAccess
{
    public class CommandRepository<T> : ICommandRepository<T> where T : BaseEntity
    {
        private readonly IDatabaseContext _dbContext;

        internal CommandRepository(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void Adicionar(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        private void Atualizar(T entity)
        {
            var e = _dbContext.Entry(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Apagar(int id)
        {
            var entityTrackeable = _dbContext.Set<T>().Find(id);
            if (entityTrackeable == null) { return; }
            _dbContext.Set<T>().Remove(entityTrackeable);
        }

        public async void ApagarAssinc(int id)
        {
            var entityTrackeable = await _dbContext.Set<T>().FindAsync(id);
            _dbContext.Set<T>().Remove(entityTrackeable);
        }

        public void Salvar(T entity)
        {
            var props = typeof(T)
                .GetProperties()
                .Where(prop =>
                    Attribute.IsDefined(prop,
                        typeof(System.ComponentModel.DataAnnotations.KeyAttribute)));

            if ((int)props.First().GetValue(entity) == 0)
            {
                this.Adicionar(entity);
            }
            else
            {
                this.Atualizar(entity);
            }
        }
    }
}