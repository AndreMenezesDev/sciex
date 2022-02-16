using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class AliArquivoEnvioArquivoEnvioBll : IAliArquivoEnvioBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;		
		private readonly IUsuarioLogado _usuarioLogado;			

		public AliArquivoEnvioArquivoEnvioBll(IUnitOfWorkSciex uowSciex,
			IUsuarioLogado usuarioLogadoBll)
		{
			_uowSciex = uowSciex;
			_usuarioLogado = usuarioLogadoBll;				
		}

		public IEnumerable<AliArquivoEnvioVM> Listar(AliArquivoEnvioVM AliArquivoEnvioVM)
		{
			var AliArquivoEnvio = _uowSciex.QueryStackSciex.AliArquivoEnvio.Listar<AliArquivoEnvioVM>();
			return AutoMapper.Mapper.Map<IEnumerable<AliArquivoEnvioVM>>(AliArquivoEnvio);
		}
			
		public AliArquivoEnvioVM RegrasSalvar(AliArquivoEnvioVM AliArquivoEnvioVM)
		{
			var entityAliArquivoEnvio = AutoMapper.Mapper.Map<AliArquivoEnvioEntity>(AliArquivoEnvioVM);
			_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(entityAliArquivoEnvio);
			_uowSciex.CommandStackSciex.Save();

			var _AliArquivoEnvioVM = AutoMapper.Mapper.Map<AliArquivoEnvioVM>(entityAliArquivoEnvio);
			return _AliArquivoEnvioVM;
		}		

		public AliArquivoEnvioVM Selecionar(long? idAliArquivoEnvio)
		{
			var AliArquivoEnvioVM = new AliArquivoEnvioVM();
			if (!idAliArquivoEnvio.HasValue) { return AliArquivoEnvioVM; }

			var AliArquivoEnvio = _uowSciex.QueryStackSciex.AliArquivoEnvio.Selecionar(x => x.IdAliArquivoEnvio == idAliArquivoEnvio);
			if (AliArquivoEnvio == null) { return AliArquivoEnvioVM; }

			AliArquivoEnvioVM = AutoMapper.Mapper.Map<AliArquivoEnvioVM>(AliArquivoEnvio);
			return AliArquivoEnvioVM;
		}

		public void Deletar(long id)
		{
			var AliArquivoEnvio = _uowSciex.QueryStackSciex.AliArquivoEnvio.Selecionar(s => s.IdAliArquivoEnvio == id);
			if (AliArquivoEnvio != null)
			{
				_uowSciex.CommandStackSciex.AliArquivoEnvio.Apagar(AliArquivoEnvio.IdAliArquivoEnvio);
			}
		}

		public PagedItems<AliArquivoEnvioVM> ListarPaginado(AliArquivoEnvioVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<AliArquivoEnvioVM>(); }

			if (pagedFilter.NumeroAli == -1)
			{
				var dataInicio = pagedFilter.DataEnvioInicial == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataEnvioInicial.Value.Year, pagedFilter.DataEnvioInicial.Value.Month, pagedFilter.DataEnvioInicial.Value.Day);
				var dataFim = pagedFilter.DataEnvioFinal == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataEnvioFinal.Value.Year, pagedFilter.DataEnvioFinal.Value.Month, pagedFilter.DataEnvioFinal.Value.Day, 23, 59, 59);

				var aliArquivoEnvio = _uowSciex.QueryStackSciex.AliArquivoEnvio.ListarPaginado<AliArquivoEnvioVM>(o =>
					(
					   (pagedFilter.DataEnvioInicial == null && pagedFilter.DataEnvioFinal == null) || (o.DataGeracao >= dataInicio.Date && o.DataGeracao <= dataFim.Date)
					   &&
					   (pagedFilter.CodigoStatusEnvioSiscomex == 0) || (pagedFilter.CodigoStatusEnvioSiscomex == o.CodigoStatusEnvioSiscomex)
					),
					pagedFilter);

				return aliArquivoEnvio;
			}
			else
			{

				var aliArquivoEnvio = _uowSciex.QueryStackSciex.AliArquivoEnvio.ListarPaginado<AliArquivoEnvioVM>(o =>
					(					
					   (pagedFilter.NumeroAli == 0 ||  o.AliEntradaArquivo.Any(x => x.PliMercadoria.Ali.NumeroAli == pagedFilter.NumeroAli))
					),
					pagedFilter);

				return aliArquivoEnvio;
			}
			
		}
	}
}
