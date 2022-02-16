using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class ParametrosBll : IParametrosBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioLogado _usuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private string CNPJ { get; set; }

		public ParametrosBll(IUnitOfWorkSciex uowSciex,
			IUsuarioLogado usuarioLogado, IUsuarioInformacoesBll usuarioInformacoesBll, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioLogado = usuarioLogado;
			_validation = new Validation();
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_usuarioPssBll = usuarioPssBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ();
		}

		public IEnumerable<ParametrosVM> Listar(ParametrosVM parametrosVM)
		{
			var parametros = _uowSciex.QueryStackSciex.Parametros.Listar<ParametrosVM>();
			return AutoMapper.Mapper.Map<IEnumerable<ParametrosVM>>(parametros);
		}

		public IEnumerable<object> ListarChave(ParametrosVM parametrosVM)
		{
			if (parametrosVM.Descricao == null && parametrosVM.Id == null)
			{
				return new List<object>();
			}
			try
			{
				string descricao = parametrosVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(parametrosVM.Descricao);
				parametrosVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.Parametros
					.Listar().Where(o =>
							(parametrosVM.Descricao == null || (o.Descricao.ToLower().Contains(parametrosVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(parametrosVM.Descricao.ToLower()))
						&&
							(parametrosVM.Id == null || o.IdParametro == parametrosVM.Id)
						&&
							(parametrosVM.CPNJImportador == null || o.CPNJImportador == parametrosVM.CPNJImportador.CnpjCpfUnformat())
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdParametro,
							text = s.Codigo.ToString("D3") + " | " + s.Descricao
						}).Where(o => o.text.Contains(descricao))
						;
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.Parametros
					.Listar().Where(o =>
							(parametrosVM.Descricao == null || (o.Descricao.ToLower().Contains(parametrosVM.Descricao.ToLower())))
						&&
							(parametrosVM.Id == null || o.IdParametro == parametrosVM.Id)
						&&
							(parametrosVM.CPNJImportador == null || o.CPNJImportador == parametrosVM.CPNJImportador.CnpjCpfUnformat())
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdParametro,
							text = s.Codigo.ToString("D3") + " | " + s.Descricao
						});
				return lista;
			}
		}

		public PagedItems<ParametrosVM> ListarPaginado(ParametrosVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<ParametrosVM>(); }
			var usuarioLogado = _usuarioPssBll.ObterUsuarioLogado();

			var parametros = _uowSciex.QueryStackSciex.Parametros.ListarPaginado<ParametrosVM>(o =>
				(
					//(
					//	pagedFilter.IdParametro == -1 || o.IdParametro == pagedFilter.IdParametro
					//) &&
					(
						pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					) &&
					(
						usuarioLogado.Perfis.Contains(EnumPerfil.Coordenador) || o.CPNJImportador == usuarioLogado.usuCpfCnpjEmpresaOuLogado.Replace(".","").Replace("-","").Replace("/","")
					)
				),
				pagedFilter);

			return parametros;
		}

		public void RegrasSalvar(ParametrosVM parametro)
		{
			var Cnpj = (this.CNPJ).CnpjUnformat();
			if (parametro == null) { return; }

			if (parametro.Codigo == 0 || parametro.Codigo == null)
			{
				var fabricante = _uowSciex.QueryStackSciex.Parametros.Listar(o => o.CPNJImportador == Cnpj);

				if (fabricante.Count == 0)
				{
					parametro.Codigo = 1;
				}
				else
				{
					var codigo = fabricante.LastOrDefault().Codigo;
					parametro.Codigo = 0;
					parametro.Codigo = codigo + 1;
				}
			}

			var parametrosEntity = AutoMapper.Mapper.Map<ParametrosEntity>(parametro);

			if (parametrosEntity == null) { return; }		
			if (parametro.IdParametro.HasValue)
			{
				parametrosEntity = _uowSciex.QueryStackSciex.Parametros.Selecionar(x => x.IdParametro == parametro.IdParametro);

				parametrosEntity = AutoMapper.Mapper.Map(parametro, parametrosEntity);
			}

			parametrosEntity.CPNJImportador = parametro.CPNJImportador.CnpjCpfUnformat();

			_uowSciex.CommandStackSciex.Parametros.Salvar(parametrosEntity);
		}

		public void Salvar(ParametrosVM parametrosVM)
		{
			RegrasSalvar(parametrosVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public ParametrosVM Selecionar(int? idParametros)
		{
			var parametrosVM = new ParametrosVM();

			if (!idParametros.HasValue) { return parametrosVM; }

			var parametros = _uowSciex.QueryStackSciex.Parametros.Selecionar(x => x.IdParametro == idParametros);

			if (parametros == null) { return parametrosVM; }

			parametrosVM = AutoMapper.Mapper.Map<ParametrosVM>(parametros);

			return parametrosVM;
		}

		public void Deletar(int id)
		{
			var parametro = _uowSciex.QueryStackSciex.Parametros.Selecionar(s => s.IdParametro == id);

			if (parametro != null)
			{

				_validation._parametroExisteRelacionamentoValidation.ValidateAndThrow(new ParametrosDto
				{
					TotalEncontradoParametros = parametro.IdAladi > 0 ? 1 : 0

				});

			}
			else
			{
				_validation._parametroExcluirValidation.ValidateAndThrow(new ParametrosDto
				{
					ExisteRegistro = false
				});
			}


			if (parametro != null)
			{
				_uowSciex.CommandStackSciex.Parametros.Apagar(parametro.IdParametro);
			}

			_uowSciex.CommandStackSciex.Save();


		}
	}

}