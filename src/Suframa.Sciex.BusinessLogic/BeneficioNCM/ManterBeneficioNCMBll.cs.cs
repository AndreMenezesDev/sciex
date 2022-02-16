using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Mapping;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Suframa.Sciex.BusinessLogic { 
	public class ManterBeneficioNCMBll : IManterBeneficioNCMBll
	{

		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioPssBll _usuarioPss;

		enum EnumAcao
		{
			INCLUSAO = 1,
			ALTERACAO = 2,
			EXCLUSAO = 3
		}
		public ManterBeneficioNCMBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPss)
		{
			_usuarioPss = usuarioPss;
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public PagedItems<TaxaNCMBeneficioVM> ListarPaginado(TaxaNCMBeneficioVM pagedFilter)
		{

			try
			{
				PagedItems<TaxaNCMBeneficioVM> grupoBeneficio;

				if (pagedFilter == null) { return new PagedItems<TaxaNCMBeneficioVM>(); }

				grupoBeneficio = _uowSciex.QueryStackSciex.TaxaNCMBeneficio.ListarPaginado<TaxaNCMBeneficioVM>(o => o.IdTaxaGrupoBeneficio == pagedFilter.IdTaxaGrupoBeneficio, pagedFilter);

				foreach (var item in grupoBeneficio.Items)
				{
					item.DescricaoNCM = _uowSciex.QueryStackSciex.Ncm.Selecionar(o => o.CodigoNCM == item.CodigoNCM).Descricao;
				}


				return grupoBeneficio;

			}
			catch (Exception ex)
			{
				//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");

			}

			return new PagedItems<TaxaNCMBeneficioVM>();
		}

		public TaxaGrupoBeneficioVM Selecionar(int? IdTaxaGrupoBeneficio)
		{
			var grupoBeneficio = new TaxaGrupoBeneficioVM();

			if (!IdTaxaGrupoBeneficio.HasValue) { return new TaxaGrupoBeneficioVM(); }

			var gpBenenficoResult = _uowSciex.QueryStackSciex.TaxaGrupoBeneficio.Selecionar(x => x.IdTaxaGrupoBeneficio == IdTaxaGrupoBeneficio);

			grupoBeneficio = AutoMapper.Mapper.Map<TaxaGrupoBeneficioVM>(gpBenenficoResult);

			return grupoBeneficio;
		}

		public void Deletar(int id)
		{

			var ncm = _uowSciex.QueryStackSciex.TaxaNCMBeneficio.Selecionar(s => s.IdTaxaNCMBeneficio == id);

			if (ncm != null)
			{
				TaxaNCMBeneficioVM model = AutoMapper.Mapper.Map<TaxaNCMBeneficioVM>(ncm);

				_uowSciex.CommandStackSciex.TaxaNCMBeneficio.Apagar(ncm.IdTaxaNCMBeneficio);


				MontarHistoricoDePara(
						registroAnterior: null,
						registro: null,
						modelTela: ref model,
						Acao: (int)EnumAcao.EXCLUSAO
						);

			SalvarHistorico(model);
			}

			_uowSciex.CommandStackSciex.Save();
		}

		public int Salvar(TaxaNCMBeneficioVM taxaGrupoBeneficioVM)
		{
			if (taxaGrupoBeneficioVM == null) { return 0; }

		 	int retorno = this.RegrasSalvar(taxaGrupoBeneficioVM);

			if(retorno == 2) //NCM ja Existe na tabela SCIEX_TAXA_NCM_BENEFICIO
			{
				return retorno;
			} else if(retorno == 0) //ERRO ao Tentar salvar
			{
				return retorno;
			} else //Persiste no Banco
			{
				retorno = 1;
				return retorno;
			}			
			
		}

		public int RegrasSalvar(TaxaNCMBeneficioVM taxaGrupoNCMBeneficioVM)
		{
		
			var CodigoNCM = _uowSciex.QueryStackSciex.Ncm.Selecionar(x => x.IdNcm == taxaGrupoNCMBeneficioVM.IdNcm).CodigoNCM;
			taxaGrupoNCMBeneficioVM.CodigoNCM = CodigoNCM;
			
		    var VerificarRegistroCadastrado = _uowSciex.QueryStackSciex.TaxaNCMBeneficio.Selecionar(x => x.IdTaxaGrupoBeneficio == taxaGrupoNCMBeneficioVM.IdTaxaGrupoBeneficio
																									  && x.CodigoNCM == taxaGrupoNCMBeneficioVM.CodigoNCM);

			if (VerificarRegistroCadastrado != null) { return 2; } //ncm ja cadastrado no banco
			
			// Salva Grupo
			var grupoNCMBeneficioEntity = AutoMapper.Mapper.Map<TaxaNCMBeneficioEntity>(taxaGrupoNCMBeneficioVM);

			if (grupoNCMBeneficioEntity == null) { return 0; }

			_uowSciex.CommandStackSciex.TaxaNCMBeneficio.Salvar(grupoNCMBeneficioEntity);
			_uowSciex.CommandStackSciex.Save();

			taxaGrupoNCMBeneficioVM.IdTaxaNCMBeneficio = grupoNCMBeneficioEntity.IdTaxaNCMBeneficio;

			MontarHistoricoDePara(
						registroAnterior: null,
						registro: null,
						modelTela: ref taxaGrupoNCMBeneficioVM,
						Acao: (int)EnumAcao.INCLUSAO
						);

			SalvarHistorico(taxaGrupoNCMBeneficioVM);

			return 1;
		}

		private void SalvarHistorico(TaxaNCMBeneficioVM modelTela)
		{
			var usuario = _usuarioPss.ObterUsuarioLogado();

			var idAplicacao = _uowSciex.QueryStackSciex.AuditoriaAplicacao.Selecionar(q => q.CodigoAplicacao == 2).IdAuditoriaAplicacao;

			AuditoriaEntity audit = new AuditoriaEntity()
			{
				IdAuditoriaAplicacao = idAplicacao,
				CpfCnpjResponsavel = usuario.usuarioLogadoCpfCnpj.CnpjCpfUnformat(),
				DataHoraAcao = DateTime.Now,
				Justificativa = string.IsNullOrEmpty(modelTela.Justificativa) ? "-" : modelTela.Justificativa,
				NomeResponsavel = usuario.usuarioLogadoNome,
				DescricaoAcao = string.IsNullOrEmpty(modelTela.DescricaoAlteracoes) ? "-" : modelTela.DescricaoAlteracoes,
				IdReferencia = (long)modelTela.IdTaxaGrupoBeneficio,
				TipoAcao = (byte)modelTela.TipoAcao
			};

			_uowSciex.CommandStackSciex.Auditoria.Salvar(audit);
			_uowSciex.CommandStackSciex.Save();
		}

		private void MontarHistoricoDePara(TaxaNCMBeneficioEntity registroAnterior, TaxaNCMBeneficioEntity registro, ref TaxaNCMBeneficioVM modelTela, int Acao)
		{
			modelTela.TipoAcao = Acao;
			switch (Acao)
			{
				case 1:
					modelTela.DescricaoAlteracoes = $@"INCLUSÃO: Inserido a NCM código: [{modelTela.CodigoNCM}] para o benefício ID:[{modelTela.IdTaxaGrupoBeneficio}]";
					break;

				//case 2:
				//	modelTela.DescricaoAlteracoes = string.Format($@"ALTERAÇÃO: alterando registro: [{registroAnterior.IdTaxaGrupoBeneficio}] , Campos afetados: ", Environment.NewLine);

				//	if (!string.Equals(registroAnterior.Descricao, registro.Descricao))
				//	{
				//		modelTela.DescricaoAlteracoes += string.Format($@"[TGB_DS] [DE:{registroAnterior.Descricao}  PARA:{registro.Descricao}]; ", Environment.NewLine);
				//	}

				//	if (registroAnterior.TipoBeneficio != registro.TipoBeneficio)
				//	{
				//		modelTela.DescricaoAlteracoes += string.Format($@"[TGB_TP_BENEFICIO] [DE:{registroAnterior.TipoBeneficio}  PARA:{registro.TipoBeneficio}]; ", Environment.NewLine);
				//	}
				//	if (registroAnterior.ValorPercentualReducao != registro.ValorPercentualReducao)
				//	{
				//		modelTela.DescricaoAlteracoes += $@"[TGB_VL_PERC_REDUCAO] [DE:{registroAnterior.ValorPercentualReducao}  PARA:{registro.ValorPercentualReducao}];";
				//	}
				//	if (registroAnterior.StatusBeneficio != registro.StatusBeneficio)
				//	{
				//		modelTela.DescricaoAlteracoes += $@"[TGB_ST] [DE:{registroAnterior.StatusBeneficio}  PARA:{registro.StatusBeneficio}];";
				//	}
				//	if (!string.Equals(registroAnterior.DescricaoAmparoLegal, registro.DescricaoAmparoLegal))
				//	{
				//		modelTela.DescricaoAlteracoes += $@"[TGB_ST] [DE:{registroAnterior.StatusBeneficio}  PARA:{registro.StatusBeneficio}];";
				//	}

				//	break;

				case 3:
					modelTela.DescricaoAlteracoes = $@"EXCLUSÃO: Removendo a NCM código: [{modelTela.CodigoNCM}] do benefício ID:[{modelTela.IdTaxaGrupoBeneficio}]";
					break;

				default:
					break;
			}

		}

		public PagedItems<TaxaEmpresaAtuacaoVM> ListarEmpresaPDI(TaxaEmpresaAtuacaoVM parametros)
		{																					

			var viewSagat = _uowSciex.QueryStackSciex.ListarPaginadoSql<TaxaEmpresaAtuacaoVM>(this.MontaQueryEmpresaPDI(), parametros);
			
			return viewSagat;
		}

		public string MontaQueryEmpresaPDI()
		{
			string sql = " select " +
						 " a.TEA_NU_CNPJ as CNPJ, " +
						 " b.imp_ds_razao_social as RazaoSocial " +
						 " from VW_SAGAT_EMPRESA_ATUACAO a, VW_SCIEX_IMPORTADOR b " +
						 " where a.TEA_NU_CNPJ = b.imp_nu_cnpj ";
			return sql;
		}
	}
}

