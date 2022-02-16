using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Mapping;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Suframa.Sciex.BusinessLogic { 
	public class ManterBeneficioBll : IManterBeneficioBll
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

		public ManterBeneficioBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPss)
		{
			_usuarioPss = usuarioPss;
			_uowSciex = uowSciex;
			_validation = new Validation();
		}

		public void Salvar(TaxaGrupoBeneficioVM taxaGrupoBeneficioVM)
		{
			if (taxaGrupoBeneficioVM == null){return;}

			this.RegrasSalvar(taxaGrupoBeneficioVM);

			_uowSciex.CommandStackSciex.Save();
		}

		public void RegrasSalvar(TaxaGrupoBeneficioVM taxaGrupoBeneficioVM)
		{
			if (taxaGrupoBeneficioVM == null) { return; }
			
			// Salva Grupo
			var grupoBeneficioEntity = AutoMapper.Mapper.Map<TaxaGrupoBeneficioEntity>(taxaGrupoBeneficioVM);
						
			if (grupoBeneficioEntity == null) { return; }

			if (taxaGrupoBeneficioVM.IdTaxaGrupoBeneficio.HasValue)
			{
				var grupoBeneficioEntityAnterior = _uowSciex.QueryStackSciex.TaxaGrupoBeneficio.Selecionar(x => x.IdTaxaGrupoBeneficio == taxaGrupoBeneficioVM.IdTaxaGrupoBeneficio);

				MontarHistoricoDePara(
						registroAnterior: grupoBeneficioEntityAnterior, 
						registro: grupoBeneficioEntity, 
						modelTela: ref taxaGrupoBeneficioVM, 
						Acao: (int)EnumAcao.ALTERACAO
						);

				grupoBeneficioEntity = AutoMapper.Mapper.Map(taxaGrupoBeneficioVM, grupoBeneficioEntity);

				_uowSciex.CommandStackSciex.TaxaGrupoBeneficio.Salvar(grupoBeneficioEntity);
				_uowSciex.CommandStackSciex.Save();
			}
			else
			{
				var UltimoCodigoGrupoBeneficio = _uowSciex.QueryStackSciex.ListarPaginadoSql<TaxaGrupoBeneficioVM>("Select Max(TGB_CO) as Codigo " +
																												  " from SCIEX_TAXA_GRUPO_BENEFICIO", taxaGrupoBeneficioVM).Items.FirstOrDefault();
				UltimoCodigoGrupoBeneficio.Codigo++;

				grupoBeneficioEntity.Codigo = Convert.ToInt16(UltimoCodigoGrupoBeneficio.Codigo);				

				_uowSciex.CommandStackSciex.TaxaGrupoBeneficio.Salvar(grupoBeneficioEntity);
				_uowSciex.CommandStackSciex.Save();

				taxaGrupoBeneficioVM.IdTaxaGrupoBeneficio = grupoBeneficioEntity.IdTaxaGrupoBeneficio;

				MontarHistoricoDePara(
						registroAnterior: null,
						registro: null,
						modelTela: ref taxaGrupoBeneficioVM,
						Acao: (int)EnumAcao.INCLUSAO
						);
			}

			SalvarHistorico(taxaGrupoBeneficioVM);
			
		}

		private void SalvarHistorico(TaxaGrupoBeneficioVM modelTela)
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

		private void MontarHistoricoDePara(TaxaGrupoBeneficioEntity registroAnterior, TaxaGrupoBeneficioEntity registro, ref TaxaGrupoBeneficioVM modelTela, int Acao)
		{
			modelTela.TipoAcao = Acao;
			switch (Acao)
			{
				case 1:					
					modelTela.DescricaoAlteracoes = $@"INCLUSÃO: Inserido o registro: [{modelTela.IdTaxaGrupoBeneficio}]";
					break;

				case 2:
					modelTela.DescricaoAlteracoes = string.Format($@"ALTERAÇÃO: alterando registro: [{registroAnterior.IdTaxaGrupoBeneficio}] , Campos afetados: ", Environment.NewLine);

					if (!string.Equals(registroAnterior.Descricao, registro.Descricao))
					{
						modelTela.DescricaoAlteracoes += string.Format($@"[TGB_DS] [DE:{registroAnterior.Descricao}  PARA:{registro.Descricao}]; ", Environment.NewLine);
					}

					if (registroAnterior.TipoBeneficio != registro.TipoBeneficio)
					{
						modelTela.DescricaoAlteracoes += string.Format($@"[TGB_TP_BENEFICIO] [DE:{registroAnterior.TipoBeneficio}  PARA:{registro.TipoBeneficio}]; ", Environment.NewLine);
					}
					if (registroAnterior.ValorPercentualReducao != registro.ValorPercentualReducao)
					{
						modelTela.DescricaoAlteracoes += $@"[TGB_VL_PERC_REDUCAO] [DE:{registroAnterior.ValorPercentualReducao}  PARA:{registro.ValorPercentualReducao}];";
					}
					if (registroAnterior.StatusBeneficio != registro.StatusBeneficio)
					{
						modelTela.DescricaoAlteracoes += $@"[TGB_ST] [DE:{registroAnterior.StatusBeneficio}  PARA:{registro.StatusBeneficio}];";
					}
					if (!string.Equals(registroAnterior.DescricaoAmparoLegal,registro.DescricaoAmparoLegal))
					{
						modelTela.DescricaoAlteracoes += $@"[TGB_ST] [DE:{registroAnterior.StatusBeneficio}  PARA:{registro.StatusBeneficio}];";
					}

					break;

				case 3:
					modelTela.DescricaoAlteracoes = $@"EXCLUSÃO: Excluindo o registro: [{modelTela.IdTaxaGrupoBeneficio}]";
					break;

				default:
					break;
			}



		}

		public PagedItems<TaxaGrupoBeneficioVM> ListarPaginado(TaxaGrupoBeneficioVM pagedFilter)
		{

			try
			{
				PagedItems<TaxaGrupoBeneficioVM> grupoBeneficio;

				if (pagedFilter == null) { return new PagedItems<TaxaGrupoBeneficioVM>(); }

				if (pagedFilter.Codigo == 0) { pagedFilter.Codigo = -1; }

				#region Tipo de Benefício == TODOS && Situacao == TODOS

				if (pagedFilter.StatusBeneficio == 99 && pagedFilter.TipoBeneficio == 99) //AMBOS TODOS
				{

					grupoBeneficio = _uowSciex.QueryStackSciex.TaxaGrupoBeneficio.ListarPaginado<TaxaGrupoBeneficioVM>(o =>
					(
						(
							pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
						) && (
							string.IsNullOrEmpty(pagedFilter.Descricao) ||
							string.Equals(o.Descricao, pagedFilter.Descricao)
						) 
					),
					pagedFilter);

					foreach (var item in grupoBeneficio.Items)
					{
						var Percent = item.ValorPercentualReducao;
						item.ValorPercentualReducao = (decimal.Floor(Percent * 100));
						item.PercentualConcatenado = item.ValorPercentualReducao + "%";

						if (item.TipoBeneficio == 1) { item.TipoBeneficioConcatenado = "Isenção"; }
						if (item.TipoBeneficio == 2) { item.TipoBeneficioConcatenado = "Redução"; }
						if (item.TipoBeneficio == 3) { item.TipoBeneficioConcatenado = "Suspensão"; }
						if (item.TipoBeneficio == 0) { item.TipoBeneficioConcatenado = "Nenhum"; }

						item.DataConcatenada = item.DataCadastro.ToString("dd/MM/yyyy");

					}

						return grupoBeneficio;
				}

				#endregion

				#region Tipo de Benefício != TODOS && Situacao == TODOS

				if (pagedFilter.StatusBeneficio == 99 && pagedFilter.TipoBeneficio != 99) 
				{

					grupoBeneficio = _uowSciex.QueryStackSciex.TaxaGrupoBeneficio.ListarPaginado<TaxaGrupoBeneficioVM>(o =>
					(
						(
							pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
						) && (
							string.IsNullOrEmpty(pagedFilter.Descricao) ||
							o.Descricao.Contains(pagedFilter.Descricao)
						) &&
						(							
							o.TipoBeneficio == pagedFilter.TipoBeneficio
						)
					),
					pagedFilter);

					foreach (var item in grupoBeneficio.Items)
					{
						var Percent = item.ValorPercentualReducao;
						var PercentualConcatenado = (decimal.Floor(Percent * 100));
						item.PercentualConcatenado = PercentualConcatenado + "%";

						if (item.TipoBeneficio == 1) { item.TipoBeneficioConcatenado = "Isenção"; }
						if (item.TipoBeneficio == 2) { item.TipoBeneficioConcatenado = "Redução"; }
						if (item.TipoBeneficio == 3) { item.TipoBeneficioConcatenado = "Suspensão"; }
						if (item.TipoBeneficio == 0) { item.TipoBeneficioConcatenado = "Nenhum"; }

						item.DataConcatenada = item.DataCadastro.ToString("dd/MM/yyyy");
					}

					return grupoBeneficio;
				}

				#endregion

				#region Tipo de Benefício == TODOS && Situacao != TODOS

				if (pagedFilter.StatusBeneficio != 99 && pagedFilter.TipoBeneficio == 99)
				{

					grupoBeneficio = _uowSciex.QueryStackSciex.TaxaGrupoBeneficio.ListarPaginado<TaxaGrupoBeneficioVM>(o =>
					(
						(
							pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
						) && (
							string.IsNullOrEmpty(pagedFilter.Descricao) ||
							o.Descricao.Contains(pagedFilter.Descricao)
						) && (
							o.StatusBeneficio == pagedFilter.StatusBeneficio
						)
					),
					pagedFilter);

					foreach (var item in grupoBeneficio.Items)
					{
						var Percent = item.ValorPercentualReducao;
						var PercentualConcatenado = (decimal.Floor(Percent * 100));
						item.PercentualConcatenado = PercentualConcatenado + "%";

						if (item.TipoBeneficio == 1) { item.TipoBeneficioConcatenado = "Isenção"; }
						if (item.TipoBeneficio == 2) { item.TipoBeneficioConcatenado = "Redução"; }
						if (item.TipoBeneficio == 3) { item.TipoBeneficioConcatenado = "Suspensão"; }
						if (item.TipoBeneficio == 0) { item.TipoBeneficioConcatenado = "Nenhum"; }

						item.DataConcatenada = item.DataCadastro.ToString("dd/MM/yyyy");
					}

					return grupoBeneficio;

					return grupoBeneficio;
				}

				#endregion

				#region Tipo de Benefício != TODOS && Situacao != TODOS

				if (pagedFilter.StatusBeneficio != 99 && pagedFilter.TipoBeneficio != 99)
				{
					grupoBeneficio = _uowSciex.QueryStackSciex.TaxaGrupoBeneficio.ListarPaginado<TaxaGrupoBeneficioVM>(o =>
					(
						(
							pagedFilter.Codigo == -1 || o.Codigo == pagedFilter.Codigo
						) && (
							string.IsNullOrEmpty(pagedFilter.Descricao) ||
							o.Descricao.Contains(pagedFilter.Descricao)
						) && (
							o.StatusBeneficio == pagedFilter.StatusBeneficio
						) && (
							o.TipoBeneficio == pagedFilter.TipoBeneficio
						)
					),
					pagedFilter);

					foreach (var item in grupoBeneficio.Items)
					{

						var Percent = item.ValorPercentualReducao;
						var PercentualConcatenado = (decimal.Floor(Percent * 100));
						item.PercentualConcatenado = PercentualConcatenado + "%";

						if (item.TipoBeneficio==1) { item.TipoBeneficioConcatenado = "Isenção";  }
						if (item.TipoBeneficio == 2) { item.TipoBeneficioConcatenado = "Redução"; }
						if (item.TipoBeneficio == 3) { item.TipoBeneficioConcatenado = "Suspensão"; }
						if (item.TipoBeneficio == 0) { item.TipoBeneficioConcatenado = "Nenhum"; }

						item.DataConcatenada = item.DataCadastro.ToString("dd/MM/yyyy");
					}

					return grupoBeneficio;
				}
				
				#endregion
			}
			catch (Exception ex)
			{
				//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");

			}

			return new PagedItems<TaxaGrupoBeneficioVM>();

		}

		public TaxaGrupoBeneficioVM Selecionar(int? codigoGpBenefiico)
		{
			var grupoBeneficio = new TaxaGrupoBeneficioVM();

			if (!codigoGpBenefiico.HasValue) { return new TaxaGrupoBeneficioVM(); }

			var gpBenenficoResult = _uowSciex.QueryStackSciex.TaxaGrupoBeneficio.Selecionar(x => x.Codigo == codigoGpBenefiico);


			grupoBeneficio = AutoMapper.Mapper.Map<TaxaGrupoBeneficioVM>(gpBenenficoResult);
			var Percent = grupoBeneficio.ValorPercentualReducao;
			var PercentualConcatenado = (decimal.Floor(Percent * 100));
			grupoBeneficio.ValorPercentualReducao = PercentualConcatenado;
			grupoBeneficio.PercentualConcatenado = PercentualConcatenado + "%";

			return grupoBeneficio;
		}


	}
}

