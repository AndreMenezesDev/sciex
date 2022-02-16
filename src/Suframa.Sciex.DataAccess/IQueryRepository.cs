using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess
{
    public interface IQueryRepository<T> where T : BaseEntity
    {
        int Contar(Expression<Func<T, bool>> predicate);

        Task<int> ContarAssinc(Expression<Func<T, bool>> predicate);

        bool Existe(Expression<Func<T, bool>> predicate);

        List<T> Listar();

        List<U> Listar<U>();

        List<U> Listar<U>(Expression<Func<T, bool>> predicate);

        //List<T> Listar(Expression<Func<T, bool>> predicate);
        List<T> Listar(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        Task<List<T>> ListarAssinc();

        Task<List<T>> ListarAssinc(Expression<Func<T, bool>> predicate);

        IList<TResult> ListarGrafo<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> where);

        PagedItems<T> ListarPaginado(Expression<Func<T, bool>> predicate, PagedOptions pagedFilter);

        PagedItems<U> ListarPaginado<U>(Expression<Func<T, bool>> predicate, PagedOptions pagedFilter);

        PagedItems<TResult> ListarPaginadoGrafo<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> where, PagedOptions pagedFilter);

        T Selecionar(Expression<Func<T, bool>> predicate);

        U Selecionar<U>(Expression<Func<T, bool>> predicate);

        Task<T> SelecionarAssinc(Expression<Func<T, bool>> predicate);

        Task<U> SelecionarAssinc<U>(Expression<Func<T, bool>> predicate);

        TResult SelecionarGrafo<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> where);

        T SelecionarUltimo<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order);
    }
}