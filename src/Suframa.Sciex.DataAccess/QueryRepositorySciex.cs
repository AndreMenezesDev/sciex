using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.Extension;
using Suframa.Sciex.DataAccess.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess
{
	public class QueryRepositorySciex<T> : IQueryRepositorySciex<T> where T : BaseEntity
	{
		private readonly IDatabaseContextSciex _dbContextSciex;

		internal QueryRepositorySciex(IDatabaseContextSciex dbContextSciex)
		{
			_dbContextSciex = dbContextSciex;
		}

		//// https://github.com/kahanu/System.Linq.Dynamic
		//// https://weblogs.asp.net/scottgu/dynamic-linq-part-1-using-the-linq-dynamic-query-library
		//private IQueryable<T> Ordenar(IQueryable<T> collection, string sortBy, bool reverse = false)
		//{
		//    return System.Linq.Dynamic.DynamicQueryable.OrderBy<T>(collection, sortBy, reverse);
		//}

		public int Contar(Expression<Func<T, bool>> predicate)
		{
			return _dbContextSciex
				.Set<T>()
				.Where(predicate)
				.AsNoTracking()
				.Count();
		}

		public async Task<int> ContarAssinc(Expression<Func<T, bool>> predicate)
		{
			return await _dbContextSciex
				.Set<T>()
				.Where(predicate)
				.AsNoTracking()
				.CountAsync();
		}

		public bool Existe(Expression<Func<T, bool>> predicate)
		{
			return _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.Any(predicate);
		}

		public T Find(object id)
		{
			return _dbContextSciex
				.Set<T>()
				.Find(id);
		}

		public List<T> Listar()
		{
			return _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.ToList();
		}

		public List<U> Listar<U>()
		{
			var entities = this.Listar();
			return AutoMapper.Mapper.Map<List<U>>(entities);
		}

		//public List<T> Listar(Expression<Func<T, bool>> predicate)
		//{
		//	return _dbContext
		//		.Set<T>()
		//		.Where(predicate)
		//		.AsNoTracking()
		//		.ToList();
		//}

		public List<U> Listar<U>(Expression<Func<T, bool>> predicate)
		{
			return AutoMapper.Mapper.Map<List<U>>(this.Listar(predicate));
		}

		/// <summary>
		/// https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="orderBy"></param>
		/// <param name="includeProperties"></param>
		/// <returns></returns>
		public List<T> Listar(Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeProperties = "")
		{
			IQueryable<T> query = _dbContextSciex.Set<T>();

			if (filter != null)
			{
				query = query.AsNoTracking().Where(filter);
			}

			foreach (var includeProperty in includeProperties.Split
				(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}

			if (orderBy != null)
			{
				return orderBy(query)
					.AsNoTracking()
					.ToList();
			}
			else
			{
				return query
					.AsNoTracking()
					.ToList();
			}
		}

		public async Task<List<T>> ListarAssinc()
		{
			return await _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<List<T>> ListarAssinc(Expression<Func<T, bool>> predicate)
		{
			return await _dbContextSciex
				.Set<T>()
				.Where(predicate)
				.AsNoTracking()
				.ToListAsync();
		}

		public IList<TResult> ListarGrafo<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> where)
		{
			return _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.Select(select)
				.Where(where)
				.ToList();
		}

		public PagedItems<T> ListarPaginado(Expression<Func<T, bool>> predicate, PagedOptions pagedFilter)
		{
			if (string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.SortManny == null)
			{
				var props = typeof(T)
					.GetProperties()
					.Where(prop =>
						Attribute.IsDefined(prop,
							typeof(System.ComponentModel.DataAnnotations.KeyAttribute)));

				pagedFilter.Sort = props.First().Name;
			}

			PagedItems<T> paged = new PagedItems<T>();

			var query = _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.Where(predicate)
				.AsQueryable();

			paged.Total = query.Count();

			if (!string.IsNullOrEmpty(pagedFilter.Sort))
			{
				query = LinqExtension.OrderBy(query, pagedFilter.Sort, pagedFilter.Reverse);
			}
			else
			{
				if (pagedFilter.SortManny != null)
				{
					var list = pagedFilter.SortManny.ToList();
					for (int i = 0; i < list.Count; i++)
					{
						if (i == 0)
						{
							query = LinqExtension.OrderBy(query, list[i].Sort, list[i].Reverse);
						}
						else
						{
							query = LinqExtension.ThenBy(query, list[i].Sort, list[i].Reverse);
						}
					}
				}
			}

			var skip = (pagedFilter.Page.Value * pagedFilter.Size.Value) - pagedFilter.Size.Value;
			query = query.Skip(skip < 0 ? 0 : skip);
			query = query.Take(pagedFilter.Size.Value);

			paged.Items = query.ToList();
			return paged;
		}

		public PagedItems<U> ListarPaginado<U>(Expression<Func<T, bool>> predicate, PagedOptions pagedFilter)
		{
			try
			{
				var paged = this.ListarPaginado(predicate, pagedFilter);

				var a = AutoMapper.Mapper.Map<IList<U>>(paged.Items);

				PagedItems<U> pagedMapped = new PagedItems<U>
				{
					Total = paged.Total,
					Items = AutoMapper.Mapper.Map<IList<U>>(paged.Items)
				};

				return pagedMapped;
			}
			catch (Exception ex )
			{

				throw;
			}
			
		}

		public PagedItems<TResult> ListarPaginadoGrafo<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> where, PagedOptions pagedFilter)
		{
			if (string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.SortManny == null)
			{
				var props = typeof(T)
					.GetProperties()
					.Where(prop =>
						Attribute.IsDefined(prop,
							typeof(System.ComponentModel.DataAnnotations.KeyAttribute)));

				pagedFilter.Sort = props.First().Name;
			}

			PagedItems<TResult> paged = new PagedItems<TResult>();

			var query = _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.Select(select)
				.Where(where)
				.AsQueryable();

			paged.Total = query.Count();

			if (!string.IsNullOrEmpty(pagedFilter.Sort))
			{
				query = LinqExtension.OrderBy(query, pagedFilter.Sort, pagedFilter.Reverse);
			}
			else
			{
				if (pagedFilter.SortManny != null)
				{
					var list = pagedFilter.SortManny.ToList();

					for (int i = 0; i < list.Count; i++)
					{
						if (i == 0)
						{
							query = LinqExtension.OrderBy(query, list[i].Sort, list[i].Reverse);
						}
						else
						{
							query = LinqExtension.ThenBy(query, list[i].Sort, list[i].Reverse);
						}
					}
				}
			}

			var skip = (pagedFilter.Page.Value * pagedFilter.Size.Value) - pagedFilter.Size.Value;
			query = query.Skip(skip);
			query = query.Take(pagedFilter.Size.Value);

			paged.Items = query.ToList();
			return paged;
		}

		public PagedItems<T1> ListarPaginadoSql<T1>(string sql, PagedOptions pagedFilter)
		{
			throw new NotImplementedException();
		}

		public T Selecionar(Expression<Func<T, bool>> predicate)
		{
			return _dbContextSciex
				.Set<T>()	
				.AsNoTracking()
				.SingleOrDefault(predicate);
		}

		public U Selecionar<U>(Expression<Func<T, bool>> predicate)
		{
			var entity = _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.SingleOrDefault(predicate);

			return AutoMapper.Mapper.Map<U>(entity);
		}

		public async Task<T> SelecionarAssinc(Expression<Func<T, bool>> predicate)
		{
			return await _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.SingleOrDefaultAsync(predicate);
		}

		public async Task<U> SelecionarAssinc<U>(Expression<Func<T, bool>> predicate)
		{
			var entity = await _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.SingleOrDefaultAsync(predicate);

			return AutoMapper.Mapper.Map<U>(entity);
		}

		public TResult SelecionarGrafo<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> where)
		{
			return _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.Select(select)
				.Where(where)
				.SingleOrDefault();
		}

		public T SelecionarUltimo<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> order)
		{
			return _dbContextSciex
				.Set<T>()
				.AsNoTracking()
				.OrderByDescending(order)
				.FirstOrDefault(predicate);
		}
	}
}