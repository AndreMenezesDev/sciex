using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Suframa.Sciex.BusinessLogic
{
	public class AladiBll : IAladiBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;

		//[DllImport(@"C:\Users\ctis\Documents\Delphi\ErroDll\ErroDll.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		//static extern void ChamaErro([MarshalAs(UnmanagedType.AnsiBStr)]string Erro);


		public AladiBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();

		
		}
	

		public IEnumerable<AladiVM> Listar(AladiVM aladiVM)
		{
			var aladi = _uowSciex.QueryStackSciex.Aladi.Listar<AladiVM>();
			return AutoMapper.Mapper.Map<IEnumerable<AladiVM>>(aladi);
		}

		public IEnumerable<object> ListarChave(AladiVM aladiVM)
{
			if (aladiVM.Descricao == null && aladiVM.Id == null)
			{
				return new List<object>();
			}
			try
			{
				string descricao = aladiVM.Descricao;
				if (descricao == null) throw new ArgumentException("Descricao nula");

				int valor = Convert.ToInt32(aladiVM.Descricao);
				aladiVM.Descricao = valor.ToString();

				var lista = _uowSciex.QueryStackSciex.Aladi
					.Listar().Where(o =>
							(aladiVM.Descricao == null || (o.Descricao.ToLower().Contains(aladiVM.Descricao.ToLower())) || o.Codigo.ToString().Contains(aladiVM.Descricao.ToLower()))
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdAladi,
							text = s.Codigo.ToString("D3") + " | " + s.Descricao
						}).Where(o => o.text.Contains(descricao))
						;
				return lista;
			}
			catch
			{
				var lista = _uowSciex.QueryStackSciex.Aladi
					.Listar().Where(o =>
							(aladiVM.Descricao == null || (o.Descricao.ToLower().Contains(aladiVM.Descricao.ToLower())))
						&&
							(aladiVM.Id == null || o.IdAladi == aladiVM.Id)
						)
					.OrderBy(o => o.Descricao)
					.Select(
						s => new
						{
							id = s.IdAladi,
							text = s.Codigo.ToString("D3") + " | " + s.Descricao
						});
				return lista;
			}

		}

		public PagedItems<AladiVM> ListarPaginado(AladiVM pagedFilter)
		{
	
			try
			{

				//var a = Convert.ToInt32("1") / Convert.ToInt32("0");

				if (pagedFilter == null) { return new PagedItems<AladiVM>(); }


				var aladi = _uowSciex.QueryStackSciex.Aladi.ListarPaginado<AladiVM>(o =>
					(
						(
							pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.Descricao) ||
							o.Descricao.Contains(pagedFilter.Descricao)
						)
					),
					pagedFilter);

				return aladi;
			}
			catch (Exception ex )
			{
				//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");
			
			}

			return new PagedItems<AladiVM>();
			
		}

		public void RegrasSalvar(AladiVM aladi)
		{
			if (aladi == null) { return; }

			// Salva Aladi
			var aladiEntity = AutoMapper.Mapper.Map<AladiEntity>(aladi);

			if (aladiEntity == null) { return; }

			if (aladi.IdAladi.HasValue)
			{
				aladiEntity = _uowSciex.QueryStackSciex.Aladi.Selecionar(x => x.IdAladi == aladi.IdAladi);

				aladiEntity = AutoMapper.Mapper.Map(aladi, aladiEntity);
			}

			_uowSciex.CommandStackSciex.Aladi.Salvar(aladiEntity);
		}

		public void Salvar(AladiVM aladiVM)
		{
			RegrasSalvar(aladiVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public AladiVM Selecionar(int? idAladi)
		{
			var aladiVM = new AladiVM();

			if (!idAladi.HasValue) { return aladiVM; }

			var aladi = _uowSciex.QueryStackSciex.Aladi.Selecionar(x => x.IdAladi == idAladi);

			if (aladi == null){

				_validation._aladiExcluirValidation.ValidateAndThrow(new AladiDto
				{
					ExisteRegistro = false
				});
			}

			aladiVM = AutoMapper.Mapper.Map<AladiVM>(aladi);

			return aladiVM;
		}

		public void Deletar(int id)
		{	

			var aladi = _uowSciex.QueryStackSciex.Aladi.Selecionar(s => s.IdAladi == id);

			if (aladi != null)
			{

				_validation._aladiExisteRelacionamentoValidation.ValidateAndThrow(new AladiDto
				{
					TotalEncontradoAladi = aladi.Parametros.Count,

				});

				_validation._aladiExisteRelacionamentoValidation.ValidateAndThrow(new AladiDto
				{
					TotalEncontradoAladi = aladi.PliMercadoria.Count,

				});

			}
			else
			{
				_validation._aladiExcluirValidation.ValidateAndThrow(new AladiDto
				{
					ExisteRegistro = false
				});
			}


			if (aladi != null)
			{
				_uowSciex.CommandStackSciex.Aladi.Apagar(aladi.IdAladi);
			}

			_uowSciex.CommandStackSciex.Save();

		
		}
	}
}