﻿using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ListarDetalheInsumoPorIdInsumoController : ApiController
	{
		private readonly IPEDetalheInsumoBll _bll;

		public ListarDetalheInsumoPorIdInsumoController(IPEDetalheInsumoBll bll)
		{
			_bll = bll;
		}
		
		//public DadosDetalhesInsumosVM Get(int id)
		//{
		//	return _bll.ListarDetalhePorIdInsumo(id);
		//}

		public DadosDetalhesInsumosVM Get([FromUri] SalvarDetalheVM vm)
		{
			return _bll.ListarDetalhePorIdInsumo(vm);
		}

		//public LEInsumoVM Put([FromBody]LEInsumoVM vm)
		//{
		//	vm = _bll.Salvar(vm);
		//	return vm;
		//}

		//public void Delete(int id)
		//{
		//	_bll.Deletar(id);
		//}
	}
}