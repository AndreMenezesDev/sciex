using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class PaisBll : IPaisBll
	{
		private readonly IUnitOfWork _uow;

		public PaisBll(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public IEnumerable<PaisVM> Listar(PaisVM paisVM = null)
		{
			if (paisVM == null) { paisVM = new PaisVM(); }
			var lista = _uow.QueryStack.Pais
				.Listar(x => (string.IsNullOrEmpty(paisVM.Descricao) || x.Descricao == paisVM.Descricao) &&
				(!paisVM.IdPais.HasValue || x.IdPais == paisVM.IdPais));

			return AutoMapper.Mapper.Map<IEnumerable<PaisVM>>(lista);
		}

		public IEnumerable<object> ListarPaises(PaisVM paisVM)
		{

			if (paisVM.Descricao == null && paisVM.Id == null)
			{
				return new List<object>();
			}

			var pais = _uow.QueryStack.Pais
				.Listar().Where(
					o =>
						(paisVM.Descricao == null || (o.Descricao.ToLower().Contains(paisVM.Descricao.ToLower()) || (o.CodigoPais != null && o.CodigoPais.ToString().Contains(paisVM.Descricao.ToString()))))
					&&
						(paisVM.Id == null || o.CodigoPais == paisVM.Id.Value.ToString("D3"))
					&&  o.CodigoPais != null
					)
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.CodigoPais == null ? "" : s.CodigoPais,
						text = (s.CodigoPais != null ? ("00" + s.CodigoPais.ToString()).Slice(3) : "") + " | " + s.Descricao
					});


			return pais;
		}
		

		public string ListarDescricaoPais(string codigoPais)
		{
			var pais = _uow.QueryStack.Pais
				.Listar(x => codigoPais != "" && x.CodigoPais == codigoPais)
				.ToList().FirstOrDefault();


				return pais == null ? "" : pais.Descricao;
		
		}
	}
}