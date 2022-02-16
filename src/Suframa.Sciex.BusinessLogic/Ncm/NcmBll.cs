using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.BusinessLogic.Pss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Suframa.Sciex.BusinessLogic
{
	public class NcmBll : INcmBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioPssBll _usuarioPss;

		public NcmBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPss)
		{
			_uowSciex = uowSciex;
			_validation = new Validation();
			_usuarioPss = usuarioPss;
		}

		public IEnumerable<NcmVM> Listar(NcmVM ncmVM)
		{
			var ncm = _uowSciex.QueryStackSciex.Ncm.Listar<NcmVM>();
			return AutoMapper.Mapper.Map<IEnumerable<NcmVM>>(ncm);
		}

		public IEnumerable<object> ListarChave(NcmVM ncmVM)
		{

			if (ncmVM.Id == null && ncmVM.Descricao == null)
			{
				return new List<object>();
			}

			var pais = _uowSciex.QueryStackSciex.Ncm
				.Listar().Where(o => 
									(
										ncmVM.Descricao == null 
										|| o.Descricao.ToLower().Contains(ncmVM.Descricao.ToLower())
										|| (o.CodigoNCM != null && o.CodigoNCM.ToString().Contains(ncmVM.Descricao.ToString()))
									)
									&& (ncmVM.Id == null || o.IdNcm.Equals(ncmVM.Id))
					)
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
					    id = s.IdNcm,
						text = s.CodigoNCM + " | " + s.Descricao
					});

			return pais;
		}

		public IEnumerable<object> ListarChave(ViewNcmVM viewNcmVM)
		{

			if (viewNcmVM.Descricao == null && viewNcmVM.Id == 0)
			{
				return new List<object>();
			}

			var ncm = _uowSciex.QueryStackSciex.ViewNcm
				.Listar().Where(o =>
						(viewNcmVM.Descricao == null || (o.Descricao.ToLower().Contains(viewNcmVM.Descricao.ToLower()) || o.CodigoNCM.ToString().Contains(viewNcmVM.Descricao.ToString())))
					&&
						(viewNcmVM.Id == 0 || o.CodigoNCM == viewNcmVM.Id.ToString())
					)			
				.OrderBy(o => o.Descricao)
				.Select(
					s => new
					{
						id = s.CodigoNCM,
						text = s.CodigoNCM + " - " + s.Descricao
					});

			return ncm;
		}

		public PagedItems<NcmVM> ListarPaginado(NcmVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<NcmVM>(); }

			var ncm = _uowSciex.QueryStackSciex.Ncm.ListarPaginado<NcmVM>(o =>
				(
					(
						string.IsNullOrEmpty(pagedFilter.CodigoNCM) ||
						o.CodigoNCM == pagedFilter.CodigoNCM
					) &&
					(
						string.IsNullOrEmpty(pagedFilter.Descricao) ||
						o.Descricao.Contains(pagedFilter.Descricao)
					) &&
					(
						pagedFilter.CheckboxSomenteAmazoniaOcidental == false ||
						o.IsAmazoniaOcidental == 1
					)&&
					(
						pagedFilter.Status == 2 || o.Status == pagedFilter.Status
					)
				),
				pagedFilter);

			foreach (var item in ncm.Items)
			{
				item.IsAmazoniaOcidentalString = item.IsAmazoniaOcidental == 1
													? "Sim"
													: "Não";
			}

			return ncm;
		}

		public bool ValidarRegrarSalvar(string codigoNCM)
		{
			
			var ncmEntity = _uowSciex.QueryStackSciex.Ncm.Selecionar(x => x.CodigoNCM == codigoNCM);

			if (ncmEntity != null)
				return true;

			return false;
		}

		public string ajusteCodigo(string codigo)
		{
			foreach( var num in codigo)
			{
				codigo = codigo.Replace(".", "");
			}

			return codigo;
		}

		public NcmVM RegrasSalvar(NcmVM ncm)
		{
			if (ncm == null) { return null; }

			if (ValidarRegrarSalvar(ajusteCodigo(ncm.CodigoNCM)) && ncm.isEditStatus != 1 && ncm.AcaoTela !=2)
			{
				ncm.MensagemErro = "Registro já existe";
				return ncm;
			}
				ncm.MensagemErro = null;

			ncm.CodigoNCM = ajusteCodigo(ncm.CodigoNCM);

			var ncmEntity = AutoMapper.Mapper.Map<NcmEntity>(ncm);

			if (ncmEntity == null) { return null; }

			if (ncm.IdNcm.HasValue)
			{
				var ncmEntityAnterior = _uowSciex.QueryStackSciex.Ncm.Selecionar(x => x.IdNcm == ncm.IdNcm);

				if (ncmEntityAnterior != null && ncm.AcaoTela == 2)
				{
					MontarHistoricoDeParaNcm(ncmEntityAnterior, ncmEntity, ref ncm);
				}				

				ncmEntity = AutoMapper.Mapper.Map(ncm, ncmEntity);
			}			

			_uowSciex.CommandStackSciex.Ncm.Salvar(ncmEntity);
			_uowSciex.CommandStackSciex.Save();

			ncm.IdNcm = ncmEntity.IdNcm;

			if (ncm.AcaoTela == 1)
			{
				MontarHistoricoDeParaNcm(ncmEntityAnterior: null, ncmEntity: null, ncm: ref ncm);
			}

			SalvarHistorico(ncm);

			return ncm;
		}

		private void MontarHistoricoDeParaNcm(NcmEntity ncmEntityAnterior, NcmEntity ncmEntity, ref NcmVM ncm)
		{
			switch (ncm.AcaoTela)
			{
				case 1:
					ncm.DescricaoAlteracoes = $@"INCLUSÃO: Inserido o registro: [{ncm.IdNcm}]";
					break;

				case 2:
					ncm.DescricaoAlteracoes = string.Format($@"ALTERAÇÃO: alterando registro: [{ncmEntityAnterior.IdNcm}] , Campos afetados: ", Environment.NewLine);

					if (!string.Equals(ncmEntityAnterior.Descricao, ncmEntity.Descricao))
					{
					ncm.DescricaoAlteracoes += string.Format($@"[NCM_DS] [DE:{ncmEntityAnterior.Descricao}  PARA:{ncmEntity.Descricao}]; ",Environment.NewLine);
					}

					if (ncmEntityAnterior.IsAmazoniaOcidental != ncmEntity.IsAmazoniaOcidental)
					{
					ncm.DescricaoAlteracoes += string.Format($@"[NCM_ST_AMAZ_OCIDENTAL] [DE:{ncmEntityAnterior.IsAmazoniaOcidental}  PARA:{ncmEntity.IsAmazoniaOcidental}]; ",Environment.NewLine);
					}
					if (ncmEntityAnterior.Status != ncmEntity.Status)
					{
						ncm.DescricaoAlteracoes += $@"[NCM_ST] [DE:{ncmEntityAnterior.Status}  PARA:{ncmEntity.Status}];";
					}
					
					break;

				case 3:
					ncm.DescricaoAlteracoes = $@"EXCLUSÃO: Excluindo o registro: [{ncm.IdNcm}]";
					break;

				default:
					break;
			}

			

		}

		private void SalvarHistorico(NcmVM ncm)
		{
			var usuario = _usuarioPss.ObterUsuarioLogado();

			var idAplicacao = _uowSciex.QueryStackSciex.AuditoriaAplicacao.Selecionar(q => q.CodigoAplicacao == 1).IdAuditoriaAplicacao;

			AuditoriaEntity audit = new AuditoriaEntity()
			{
				IdAuditoriaAplicacao = idAplicacao,
				CpfCnpjResponsavel = usuario.usuarioLogadoCpfCnpj.CnpjCpfUnformat(),
				DataHoraAcao = DateTime.Now,
				Justificativa = string.IsNullOrEmpty(ncm.Justificativa) && ncm.isEditStatus != 1 ? "-" : ncm.Justificativa,
				NomeResponsavel = usuario.usuarioLogadoNome,
				DescricaoAcao = string.IsNullOrEmpty(ncm.DescricaoAlteracoes) ? "-" : ncm.DescricaoAlteracoes,
				IdReferencia = (long)ncm.IdNcm,
				TipoAcao = (byte)ncm.AcaoTela
			};

			_uowSciex.CommandStackSciex.Auditoria.Salvar(audit);
			_uowSciex.CommandStackSciex.Save();
		}

		public NcmVM Salvar(NcmVM ncmVM)
		{
			ncmVM = RegrasSalvar(ncmVM);
			_uowSciex.CommandStackSciex.Save();
			return ncmVM;
		}

		public NcmVM Selecionar(int? idNcm)
		{
			var ncmVM = new NcmVM();

			if (!idNcm.HasValue) { return ncmVM; }

			var ncm = _uowSciex.QueryStackSciex.Ncm.Selecionar(x => x.IdNcm == idNcm);

			if (ncm == null)
			{
				_validation._ncmExcluirValidation.ValidateAndThrow(new NcmDto
				{
					ExisteRegistro = false
				});
			}

			ncmVM = AutoMapper.Mapper.Map<NcmVM>(ncm);

			return ncmVM;
		}

		public NcmVM Selecionar(NcmVM ncmVM)
		{
			if (ncmVM == null)
				return new NcmVM();

			var ncm = _uowSciex.QueryStackSciex.Ncm.Selecionar(x => (String.IsNullOrEmpty(ncmVM.CodigoNCM) || x.CodigoNCM.ToLower().Equals(ncmVM.CodigoNCM.ToLower())) && (String.IsNullOrEmpty(ncmVM.Descricao) || x.Descricao.ToLower().Equals(ncmVM.Descricao.ToLower())));

			if (ncm == null)
			{
				return null;
			}

			ncmVM = AutoMapper.Mapper.Map<NcmVM>(ncm);

			return ncmVM;
		}

		public void Deletar(int id)
		{
			var ncm = _uowSciex.QueryStackSciex.Ncm.Selecionar(s => s.IdNcm == id);

			if (ncm != null)
			{				
				_uowSciex.CommandStackSciex.Ncm.Apagar(ncm.IdNcm);
			}

			_uowSciex.CommandStackSciex.Save();
		}
	}
}