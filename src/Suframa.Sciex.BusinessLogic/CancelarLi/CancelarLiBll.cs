using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Ftp;
using Suframa.Sciex.CrossCutting.Texto;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class CancelarLiBll : ICancelarLiBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IPliBll _pliBll;
		private readonly IAliBll _aliBll;
		private readonly IAliHistoricoBll _aliHistoricoBll;
		private string CNPJ { get; set; }

		public CancelarLiBll(
			IUnitOfWorkSciex uowSciex,
			IUnitOfWork uowCadsuf,
			IUsuarioLogado usuarioLogado,
			IViewImportadorBll viewImportadorBll,
			IComplementarPLIBll complementarPLIBll,
			IUsuarioInformacoesBll usuarioInformacoesBll,
			IAliBll aliBll,
			IPliBll pliBll,
			IAliHistoricoBll aliHistoricoBll,
			IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;
			_IUsuarioLogado = usuarioLogado;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_pliBll = pliBll;
			_aliBll = aliBll;
			_aliHistoricoBll = aliHistoricoBll;
			this.CNPJ = "";
		}

		public IEnumerable<LiVM> Listar()
		{
			var li = _uowSciex.QueryStackSciex.Li.Listar<AliVM>();
			return AutoMapper.Mapper.Map<IEnumerable<LiVM>>(li);
		}

		public IEnumerable<object> ListarChave(LiVM liVM)
		{
			var lista = _uowSciex.QueryStackSciex.Li
				.Listar().Where(o =>
						(liVM.IdPliMercadoria == -1 || o.IdPliMercadoria == liVM.IdPliMercadoria)
					)
				.OrderBy(o => o.DataCadastro)
				.Select(
					s => new
					{
						id = s.NumeroLi,
						text = s.DataCadastro
					});
			return lista;
		}

		public PagedItems<LiVM> ListarPaginado(LiVM pagedFilter)
		{
			string cpfcnpf = _usuarioInformacoesBll.ObterCNPJ().Replace(".", "").Replace("/", "").Replace("-", "");
			var dataInicio = pagedFilter.DataCadastro == DateTime.MinValue ? DateTime.MinValue : new DateTime(pagedFilter.DataCadastro.Year, pagedFilter.DataCadastro.Month, pagedFilter.DataCadastro.Day);			
			var dataFim = pagedFilter.DataCancelamento == null ? DateTime.MinValue : new DateTime(pagedFilter.DataCancelamento.Value.Year, pagedFilter.DataCancelamento.Value.Month, pagedFilter.DataCancelamento.Value.Day, 23, 59, 59);
			var li = new PagedItems<LiVM>();

			if (pagedFilter == null || this.CNPJ == null) { return new PagedItems<LiVM>(); }

			if (dataInicio != DateTime.MinValue && dataFim != DateTime.MinValue)
			{
				if (pagedFilter.Sort == "numeroNCM")
				{
					pagedFilter.Sort = "";

					li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								pagedFilter.AnoPli == 0 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
							)
							&&
							(
								pagedFilter.NumeroLi == -1 || o.NumeroLi == pagedFilter.NumeroLi
							)
							&&
							(
								(pagedFilter.Status == 0
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA ||
									o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO ||
									o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
									&&
									pagedFilter.StatusAli == 0
									&&
									(
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA ||
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO ||
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR
									)

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCadastro >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCadastro <= dataFim
									)
								)
								||
								(pagedFilter.Status == 1
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCadastro >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCadastro <= dataFim
									)
								)
								||
								(pagedFilter.Status == 2
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCancelamento <= dataFim
									)
								)
								||
								(pagedFilter.Status == 3
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento <= dataFim
									)
								)
								||
								(pagedFilter.Status == 4
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento <= dataFim
									)
								)
							)
							&&
							(
								o.PliMercadoria.Pli.Cnpj == cpfcnpf
								||
								o.PliMercadoria.Pli.NumeroResponsavelRegistro == cpfcnpf
							)
						),
						pagedFilter);

					if (pagedFilter.Reverse)
						li.Items = li.Items.OrderByDescending(o => o.NumeroNCM).ToList();
					else
						li.Items = li.Items.OrderBy(o => o.NumeroNCM).ToList();

				}
				else if (pagedFilter.Sort == "descricaoNCMMercadoria")
				{
					pagedFilter.Sort = "";

					li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								pagedFilter.AnoPli == 0 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
							)
							&&
							(
								pagedFilter.NumeroLi == -1 || o.NumeroLi == pagedFilter.NumeroLi
							)
							&&
							(
								(pagedFilter.Status == 0
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA ||
									o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO ||
									o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
									&&
									pagedFilter.StatusAli == 0
									&&
									(
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA ||
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO ||
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR
									)

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCadastro >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCadastro <= dataFim
									)
								)
								||
								(pagedFilter.Status == 1
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCadastro >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCadastro <= dataFim
									)
								)
								||
								(pagedFilter.Status == 2
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCancelamento <= dataFim
									)
								)
								||
								(pagedFilter.Status == 3
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento <= dataFim
									)
								)
								||
								(pagedFilter.Status == 4
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento <= dataFim
									)
								)
							)
							&&
							(
								o.PliMercadoria.Pli.Cnpj == cpfcnpf
								||
								o.PliMercadoria.Pli.NumeroResponsavelRegistro == cpfcnpf
							)
						),
						pagedFilter);

					if (pagedFilter.Reverse)
					{
						li.Items = li.Items.OrderByDescending(o => o.DescricaoNCMMercadoria).ToList();
					}
					else
						li.Items = li.Items.OrderBy(o => o.DescricaoNCMMercadoria).ToList();

				}
				else if (pagedFilter.Sort == "numeroReferencia")
				{
					pagedFilter.Sort = "";

					li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								pagedFilter.AnoPli == 0 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
							)
							&&
							(
								pagedFilter.NumeroLi == -1 || o.NumeroLi == pagedFilter.NumeroLi
							)
							&&
							(
								(pagedFilter.Status == 0
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA ||
									o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO ||
									o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
									&&
									pagedFilter.StatusAli == 0
									&&
									(
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA ||
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO ||
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR
									)

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCadastro >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCadastro <= dataFim
									)
								)
								||
								(pagedFilter.Status == 1
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCadastro >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCadastro <= dataFim
									)
								)
								||
								(pagedFilter.Status == 2
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCancelamento <= dataFim
									)
								)
								||
								(pagedFilter.Status == 3
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento <= dataFim
									)
								)
								||
								(pagedFilter.Status == 4
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento <= dataFim
									)
								)
							)
							&&
							(
								o.PliMercadoria.Pli.Cnpj == cpfcnpf
								||
								o.PliMercadoria.Pli.NumeroResponsavelRegistro == cpfcnpf
							)
						),
						pagedFilter);

					if (pagedFilter.Reverse)
					{
						li.Items = li.Items.OrderByDescending(o => o.NumeroReferencia).ToList();
					}
					else
						li.Items = li.Items.OrderBy(o => o.NumeroReferencia).ToList();

				}
				else
				{
					li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								pagedFilter.AnoPli == 0 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
							)
							&&
							(
								pagedFilter.NumeroLi == -1 || o.NumeroLi == pagedFilter.NumeroLi
							)
							&&
							(
								(pagedFilter.Status == 0
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA ||
									o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO ||
									o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
									&&
									pagedFilter.StatusAli == 0
									&&
									(
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA ||
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO ||
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR
									)

								)
								&&
									(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCadastro >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCadastro <= dataFim
									)
								)
								||
								(pagedFilter.Status == 1
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA
								)
								&&
								(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCadastro >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCadastro <= dataFim
									)
								)
								||
								(pagedFilter.Status == 2
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR

								)
								&&
								(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.DataCancelamento <= dataFim
									)
								)
								||
								(pagedFilter.Status == 3
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_DEFERIDA
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

								)
								&&
								(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento <= dataFim
									)
								)
								||
								(pagedFilter.Status == 4
								&&
								(
									o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO
									&&
									o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

								)
								&&
								(
										string.IsNullOrEmpty(dataInicio.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento >= dataInicio
									)
									&&
									(
										string.IsNullOrEmpty(dataFim.ToString()) ||
										o.PliMercadoria.Ali.DataCancelamento <= dataFim
									)
								)
							)
							&&
							(
								o.PliMercadoria.Pli.Cnpj == cpfcnpf
								||
								o.PliMercadoria.Pli.NumeroResponsavelRegistro == cpfcnpf
							)

						),
						pagedFilter);
				} 
			}
			else
			{
				if (pagedFilter.Sort == "numeroNCM")
				{
					pagedFilter.Sort = "";

					li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								pagedFilter.AnoPli == 0 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
							)
							&&
							(
								pagedFilter.NumeroLi == -1 || o.NumeroLi == pagedFilter.NumeroLi
							)
							&&
							(
								(pagedFilter.Status == 0
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA ||
										o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO ||
										o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
										&&
										pagedFilter.StatusAli == 0
										&&
										(
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA ||
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO ||
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR
										)

									)
								
								)
								||
								(pagedFilter.Status == 1
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA

									)
								
								)
								||
								(pagedFilter.Status == 2
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR

									)
								
								)
								||
								(pagedFilter.Status == 3
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

									)
							
								)
								||
								(pagedFilter.Status == 4
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

									)
								
								)
							)
							&&
							(
								o.PliMercadoria.Pli.Cnpj == cpfcnpf
								||
								o.PliMercadoria.Pli.NumeroResponsavelRegistro == cpfcnpf
							)
						),
						pagedFilter);

					if (pagedFilter.Reverse)
						li.Items = li.Items.OrderByDescending(o => o.NumeroNCM).ToList();
					else
						li.Items = li.Items.OrderBy(o => o.NumeroNCM).ToList();

				}
				else if (pagedFilter.Sort == "descricaoNCMMercadoria")
				{
					pagedFilter.Sort = "";

					li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								pagedFilter.AnoPli == 0 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
							)
							&&
							(
								pagedFilter.NumeroLi == -1 || o.NumeroLi == pagedFilter.NumeroLi
							)
							&&
							(
								(pagedFilter.Status == 0
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA ||
										o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO ||
										o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
										&&
										pagedFilter.StatusAli == 0
										&&
										(
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA ||
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO ||
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR
										)

								)
								
								)
								||
								(pagedFilter.Status == 1
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA

									)
								
								)
								||
								(pagedFilter.Status == 2
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR

									)
								
								)
								||
								(pagedFilter.Status == 3
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

									)
								
								)
								||
								(pagedFilter.Status == 4
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

									)
								
								)
							)
							&&
							(
								o.PliMercadoria.Pli.Cnpj == cpfcnpf
								||
								o.PliMercadoria.Pli.NumeroResponsavelRegistro == cpfcnpf
							)
						),
						pagedFilter);

					if (pagedFilter.Reverse)
					{
						li.Items = li.Items.OrderByDescending(o => o.DescricaoNCMMercadoria).ToList();
					}
					else
						li.Items = li.Items.OrderBy(o => o.DescricaoNCMMercadoria).ToList();

				}
				else if (pagedFilter.Sort == "numeroReferencia")
				{
					pagedFilter.Sort = "";

					li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								pagedFilter.AnoPli == 0 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
							)
							&&
							(
								pagedFilter.NumeroLi == -1 || o.NumeroLi == pagedFilter.NumeroLi
							)
							&&
							(
								(pagedFilter.Status == 0
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA ||
										o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO ||
										o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
										&&
										pagedFilter.StatusAli == 0
										&&
										(
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA ||
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO ||
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR
										)

									)
								
								)
								||
								(pagedFilter.Status == 1
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA

									)
								
								)
								||
								(pagedFilter.Status == 2
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR

									)
								
								)
								||
								(pagedFilter.Status == 3
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

									)
								
								)
								||
								(pagedFilter.Status == 4
								&&
									(
										o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

									)
								
								)
							)
							&&
							(
								o.PliMercadoria.Pli.Cnpj == cpfcnpf
								||
								o.PliMercadoria.Pli.NumeroResponsavelRegistro == cpfcnpf
							)
						),
						pagedFilter);

					if (pagedFilter.Reverse)
					{
						li.Items = li.Items.OrderByDescending(o => o.NumeroReferencia).ToList();
					}
					else
						li.Items = li.Items.OrderBy(o => o.NumeroReferencia).ToList();

				}
				else
				{
					li = _uowSciex.QueryStackSciex.Li.ListarPaginado<LiVM>(o =>
						(
							(
								pagedFilter.NumeroPli == -1 || o.PliMercadoria.Pli.NumeroPli == pagedFilter.NumeroPli
							)
							&&
							(
								pagedFilter.AnoPli == 0 || o.PliMercadoria.Pli.Ano == pagedFilter.AnoPli
							)
							&&
							(
								pagedFilter.NumeroLi == -1 || o.NumeroLi == pagedFilter.NumeroLi
							)
							&&
							(
								(pagedFilter.Status == 0
								&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA ||
										o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO ||
										o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
										&&
										pagedFilter.StatusAli == 0
										&&
										(
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA ||
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO ||
											o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR
										)

									)
								
								)
								||
								(pagedFilter.Status == 1
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_DEFERIDA
									)
								
								)
								||
								(pagedFilter.Status == 2
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_CANCELADA_PELO_IMPORTADOR
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.LI_CANCELADA_PELO_IMPORTADOR

									)
								
								)
								||
								(pagedFilter.Status == 3
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_DEFERIDA
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

									)
								
								)
								||
								(pagedFilter.Status == 4
									&&
									(
										o.Status == (byte)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO
										&&
										o.PliMercadoria.Ali.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO

									)
								)
							)
							&&
							(
								o.PliMercadoria.Pli.Cnpj == cpfcnpf
								||
								o.PliMercadoria.Pli.NumeroResponsavelRegistro == cpfcnpf
							)

						),
						pagedFilter);
				}
			}

			foreach (var item in li.Items)
			{
				if (string.Equals(item.DescricaoStatusAcao, "LI DEFERIDA"))
				{
					item.DataStatusFormatada = item.DataCadastro.ToShortDateString();
				}
				else if (string.Equals(item.DescricaoStatusAcao, "LI CANCELADA PELO IMPORTADOR"))
				{
					item.DataStatusFormatada = item.DataCancelamento.Value.ToShortDateString();
				}
				else if ((string.Equals(item.DescricaoStatusAcao, "LI ENVIADA AO SISCOMEX PARA CANCELAMENTO")) || string.Equals(item.DescricaoStatusAcao, "LI SOLICITADA PARA CANCELAMENTO"))
				{
					var regALI = _uowSciex.QueryStackSciex.Ali.Selecionar(q => q.IdPliMercadoria == item.IdPliMercadoria);
					item.DataStatusFormatada = regALI != null
											? regALI.DataCancelamento.Value.ToShortDateString()
											: "-";
				}
				else
				{
					item.DataStatusFormatada = "-";
				}

				var idMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(q => q.IdPliMercadoria == item.IdPliMercadoria).IdPLI;
				var numeroReferencia = _uowSciex.QueryStackSciex.Pli.Selecionar(q => q.IdPLI == idMercadoria).NumeroLIReferencia;
				item.IdPLI = _uowSciex.QueryStackSciex.Pli.Selecionar(q => q.IdPLI == idMercadoria).IdPLI;

				if (numeroReferencia != null)
				{
					if (!String.IsNullOrEmpty(numeroReferencia.Trim()))
						item.NumeroReferencia = numeroReferencia.Trim();
					else
					{
						item.NumeroReferencia = "-";
					}
				}
				else
				{
					item.NumeroReferencia = "-";
				}

				if (numeroReferencia != null)
				{
					if (!String.IsNullOrEmpty(numeroReferencia.Trim()))
					{
						var numLi = Int64.Parse(numeroReferencia);

						var idLi = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numLi).IdPliMercadoria;
						var NumeroAli = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == idLi).NumeroAli;

						var idPLiMercadoriaReferencia = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == idLi).IdPliMercadoria;

						if(idPLiMercadoriaReferencia == null)
						{
							item.IdPliMercadoriaRefrencia = null;
						}
						else
						{
							item.IdPliMercadoriaRefrencia = idPLiMercadoriaReferencia;
						}

					}
				}


			}

			return li;
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}

		public void RegrasSalvar(LiVM liVM)
		{

			foreach (var idPliMercadoria in liVM.ListaSelecionados)
			{
				//Modifica o Status da ALI
				var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == idPliMercadoria);
				pliMercadoria.Ali.Status = (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO;
				pliMercadoria.Ali.DataCancelamento = GetDateTimeNowUtc();
				if (liVM.AtivarLIOriginal)
					pliMercadoria.Ali.NumeroAtivaOrigem = 0;
				else
					pliMercadoria.Ali.NumeroAtivaOrigem = 1;

				_uowSciex.CommandStackSciex.Ali.Salvar(pliMercadoria.Ali);

				AliHistoricoEntity aliHistorico = new AliHistoricoEntity();
				aliHistorico.IdPliMercadoria = idPliMercadoria;
				aliHistorico.StatusAliAnterior = (byte)EnumAliStatus.ALI_DEFERIDA;
				aliHistorico.StatusLiAnterior = (byte)EnumLiStatus.LI_DEFERIDA;
				aliHistorico.DataOperacao = GetDateTimeNowUtc();
				aliHistorico.CPFCNPJResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.CnpjCpfUnformat();
				aliHistorico.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;
				aliHistorico.Observacao = "LI SOLICITADA PARA CANCELAMENTO";

				_uowSciex.CommandStackSciex.AliHistorico.Salvar(aliHistorico);
				_uowSciex.CommandStackSciex.Save();
			}

		}

		public LiVM Selecionar(long? numeroLI)
		{
			var liVM = new LiVM();
			if (!numeroLI.HasValue) { return liVM; }

			var li = _uowSciex.QueryStackSciex.Li.Selecionar(x => x.NumeroLi == numeroLI);
			if (li == null) { return liVM; }

			liVM = AutoMapper.Mapper.Map<LiVM>(li);
			return liVM;
		}

		public LiVM Visualizar(LiVM liVM)
		{
			var entity = _uowSciex.QueryStackSciex.Li.Selecionar(x => x.NumeroLi == liVM.NumeroLi);
			var retorno = AutoMapper.Mapper.Map<LiVM>(entity);
			return retorno;
		}

		public void Salvar(LiVM liVM)
		{
			RegrasSalvar(liVM);
		}

		public LiVM RegrasGerarCopiaPliParaCancelamento(LiVM liVM)
		{
			_pliBll.CopiarPliParaCancelamentoLi(liVM.ListaSelecionados);
			return new LiVM();
		}

		public void Deletar(int id)
		{
			var li = _uowSciex.QueryStackSciex.Li.Selecionar(s => s.NumeroLi == id);
			if (li != null)
			{
				_uowSciex.CommandStackSciex.Li.Apagar(li.NumeroLi.Value);
			}
		}

		public void LerAquivoLI(string path)
		{
			GerarArquivoSimulacaoLI();

			bool arquivoExistente = false;
			int contador = 1;
			string linha;
			long codigoLiArquivoRetorno = 0;


			string enderecoFTP = "192.168.0.251";
			string pastaFTP = "ArquivoRetornoSISCOMEX";
			string nomeDoArquivo = "SERPRO.FILE";

			var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "FTP envio SISCOMEX");
			var configuracoesUsuario = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.Descricao == "Usuario FTP SISCOMEX").FirstOrDefault();
			var configuracoesSenha = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.Descricao == "Senha FTP SISCOMEX").FirstOrDefault();

			//Arquivo de processamento da LI
			string localAqruivoFTP = configuracoesFTP.Valor;
			string usuario = configuracoesUsuario.Valor;
			string senha = configuracoesSenha.Valor;

			StreamReader file = null;

			#region REGRA DE NEGOCIO 03

			if (Ftp.VerificarSeExisteArquivo(localAqruivoFTP, usuario, senha))
			{
				arquivoExistente = false;
				file =
					new StreamReader(
						 new MemoryStream(Ftp.ReceberArquivo(localAqruivoFTP, usuario, senha)),
						 Encoding.Default)
					;
			}
			else
			{
				var registro = _uowSciex.QueryStackSciex.LiArquivoRetorno.Listar(o => o.StatusLeituraArquivo == 0).OrderBy(o => o.DataRecepcaoArquivo).FirstOrDefault();
				if (registro.LiArquivo.ArquivoLIRetorno != null)
				{
					arquivoExistente = true;
					file =
						new StreamReader(
							 new MemoryStream(registro.LiArquivo.ArquivoLIRetorno), Encoding.Default)
						;

					codigoLiArquivoRetorno = registro.IdLiArquivoRetorno;
				}
			}

			#endregion

			#region REGRA DE NEGOCIO 01

			if (!arquivoExistente)
			{
				byte[] arquivo = Ftp.ReceberArquivo(localAqruivoFTP, usuario, senha);
				LiArquivoRetornoEntity _liArquivoRetornoEntity = new LiArquivoRetornoEntity();

				_liArquivoRetornoEntity.NomeArquivo = nomeDoArquivo;
				_liArquivoRetornoEntity.StatusLeituraArquivo = 0;
				_liArquivoRetornoEntity.DataRecepcaoArquivo = GetDateTimeNowUtc();
				_liArquivoRetornoEntity.QuantidadeErroLI = 0;
				_liArquivoRetornoEntity.QuantidadeLI = 0;
				_liArquivoRetornoEntity.QuantidadeLIDeferida = 0;
				_liArquivoRetornoEntity.QuantidadeLIIndeferida = 0;
				_liArquivoRetornoEntity.TipoArquivoLI = (byte)EnumLiTipoArquivo.LI_ARQUIVO_NORMAL;

				_liArquivoRetornoEntity.LiArquivo = new LiArquivoEntity();
				_liArquivoRetornoEntity.LiArquivo.ArquivoLIRetorno = arquivo;

				_uowSciex.CommandStackSciex.LiArquivoRetorno.Salvar(_liArquivoRetornoEntity);
				_uowSciex.CommandStackSciex.Save();

				codigoLiArquivoRetorno = _liArquivoRetornoEntity.IdLiArquivoRetorno;

				ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.IdListaServico = 12; // SALVAR ARQUIVO DE LI-NORMAL
				_controleExecucaoServicoVM.MemoObjetoEnvio = localAqruivoFTP;
				_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";

				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();

				//exclui o arquivo do FTP
				Ftp.DeleteFile(localAqruivoFTP, usuario, senha);
			}

			#endregion

			//gerar 
			if (file != null)
			{
				StringBuilder SQLExecutar = new StringBuilder();

				SQLExecutar.AppendLine(
				"USE dbd_sciex " +
				"DECLARE @CONTROLE_SERVICO_COD INT " +

				"BEGIN TRY " +
				"BEGIN TRANSACTION; " +

				" INSERT INTO SCIEX_CONTROLE_EXEC_SERVICO " +
				"(CES_DH_EXECUCAO_INICIO, CES_ME_OBJETO_ENVIO, CES_ST_EXECUCAO, CES_NU_CPF_CNPJ_INTERESSADO, CES_NO_CPF_CNPJ_INTERESSADO, LSE_ID)" +
				"VALUES (" +
				" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , '" + localAqruivoFTP + "',0,'04407029000143','Administrador do Sistema – SUFRAMA', 14" +
				") " +

				"SET @CONTROLE_SERVICO_COD = @@identity"

				);

				int qtdLI = 0;
				int qtdLIDeferida = 0;
				int qtdLIIndeferida = 0;
				int qtdErros = 0;

				while ((linha = file.ReadLine()) != null)
				{
					//cabeçalho
					if (contador == 1)
					{
						Console.WriteLine(linha);
					}
					else
					{
						if (linha.Length > 1)
						{
							qtdLI = qtdLI + 1;

							string tipoRegistro = linha.Substring(0, 2);

							string numeroALI = string.Empty;
							string numeroLIProtocolda = string.Empty;
							string numeroLI = string.Empty;
							string codigoStatusDiagnostico = string.Empty;
							string dataGeracaoDiagnostico = string.Empty;
							string mensagemErro = string.Empty;

							#region REGRA DE NEGOCIO 04
							switch (tipoRegistro)
							{
								case "01": //LI GERADA COM SUCESSO
									{
										qtdLIDeferida = qtdLIDeferida + 1;

										numeroALI = linha.Substring(2, 15).Trim();
										numeroLIProtocolda = linha.Substring(17, 10);
										numeroLI = linha.Substring(17, 10);
										dataGeracaoDiagnostico = linha.Substring(37, 8);

										long numeroALIPesquisa = Convert.ToInt64(numeroALI);
										var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);
										var listaLI = _uowSciex.QueryStackSciex.Li.Listar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

										string ano = dataGeracaoDiagnostico.Substring(0, 4);
										string mes = dataGeracaoDiagnostico.Substring(4, 2);
										string dia = dataGeracaoDiagnostico.Substring(6, 2);

										#region REGRA DE NEGOCIO 05

										if (listaLI.Count > 0)
										{
											//Insere Tabela de li_historico
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI_HISTORICO_ERRO " +
												"(" +
												"	LI_NU," +
												"	LI_NU_LI_PROTOCOLADA," +
												"	LI_DT_GERACAO," +
												"	LI_DS_MENSAGEM," +
												"	LI_NU_DIAGNOSTICO_ERRO," +
												"	LI_ST," +
												"	LHE_TP_ERRO," +
												"	LI_DH_CADASTRO," +
												"	LI_DH_CANCELAMENTO" +
												")" +
												"" +
												"SELECT " +
												"	LI_NU, " +
												"	LI_NU_LI_PROTOCOLADA, " +
												"	LI_DT_GERACAO, " +
												"	LI_DS_MENSAGEM, " +
												"	LI_NU_DIAGNOSTICO_ERRO, " +
												"	LI_ST, " +
												"	1, " +
												"	CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
												"	LI_DH_CANCELAMENTO " +
												"FROM SCIEX_LI WHERE LI_NU = " + listaLI[0].NumeroLi.ToString()
												);

											//Atualiza LI
											SQLExecutar.AppendLine(
												"UPDATE SCIEX_LI SET " +
													"LAR_ID = " + codigoLiArquivoRetorno.ToString() +
													",LI_NU = " + numeroLI +
													",LI_ST = 1" +
													",LI_TP = 1" +
													",LI_DH_CADASTRO = '" + GetDateTimeNowUtc().ToString() + "'" +
													",LI_NU_PROTOCOLADA = " + numeroLIProtocolda +
													",LI_DT_GERACAO = '" + ano + "-" + mes + "-" + dia + "'" +
													",LI_DS_MENSAGEM = NULL " +
													",LI_NU_DIAGNOSTICO_ERRO = NULL " +
													",LI_DH_CANCELAMENTO = NULL " +
													"FROM SCIEX_LI WHERE LI_NU = " + numeroLI
												);
										}
										else
										{
											//INSERIR A LI
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI " +
												"( PME_ID," +
												"  LAR_ID, " +
												"  LI_NU, " +
												"  LI_ST, " +
												"  LI_TP, " +
												"  LI_DH_CADASTRO, " +
												"  LI_NU_LI_PROTOCOLADA, " +
												"  LI_DT_GERACAO) " +
												"VALUES (" +
													ali.IdPliMercadoria.ToString() + "," +
													codigoLiArquivoRetorno.ToString() + "," +
													numeroLI + "," +
													"1," +
													"1," +
													"'" + GetDateTimeNowUtc().ToString() + "'," +
													numeroLIProtocolda + "," +
													"'" + ano + "-" + mes + "-" + dia + "'" +
												") ");

											//atualizar a ALI
											SQLExecutar.AppendLine(
												" UPDATE SCIEX_ALI SET " +
												 "  ALI_ST = " + (int)EnumAliStatus.ALI_DEFERIDA +
												"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
												" WHERE ALI_NU = " + ali.NumeroAli.ToString()
												);

										}

										#endregion

										break;
									}
								case "99": //LI COM ERRO
									{
										qtdLIIndeferida = qtdLIIndeferida + 1;

										numeroALI = linha.Substring(2, 15).Trim();
										numeroLIProtocolda = linha.Substring(17, 10);
										codigoStatusDiagnostico = linha.Substring(27, 1);
										dataGeracaoDiagnostico = linha.Substring(28, 8);
										mensagemErro = linha.Substring(36, linha.Length - 36);

										long numeroALIPesquisa = Convert.ToInt64(numeroALI);
										var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);
										var listaLI = _uowSciex.QueryStackSciex.Li.Listar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

										string ano = dataGeracaoDiagnostico.Substring(0, 4);
										string mes = dataGeracaoDiagnostico.Substring(4, 2);
										string dia = dataGeracaoDiagnostico.Substring(6, 2);

										#region REGRA DE NEGOCIO¨06

										if (listaLI.Count > 0)
										{
											//Insere Tabela de li_historico
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI_HISTORICO_ERRO " +
												"(" +
												"	LI_NU," +
												"	LI_NU_LI_PROTOCOLADA," +
												"	LI_DT_GERACAO," +
												"	LI_DS_MENSAGEM," +
												"	LI_NU_DIAGNOSTICO_ERRO," +
												"	LI_ST," +
												"	LHE_TP_ERRO," +
												"	LI_DH_CADASTRO," +
												"	LI_DH_CANCELAMENTO" +
												")" +
												"" +
												"SELECT " +
												"	LI_NU, " +
												"	LI_NU_LI_PROTOCOLADA, " +
												"	LI_DT_GERACAO, " +
												"	LI_DS_MENSAGEM, " +
												"	LI_NU_DIAGNOSTICO_ERRO, " +
												"	LI_ST, " +
												"	1, " +
												"	CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
												"	LI_DH_CANCELAMENTO " +
												"FROM SCIEX_LI WHERE LI_NU = " + listaLI[0].NumeroLi.ToString()
												);

											//Atualiza LI
											SQLExecutar.AppendLine(
												"UPDATE SCIEX_LI SET " +
													"LI_NU = NULL, " +
													",LI_NU_PROTOCOLADA = " + numeroLIProtocolda +
													",LI_DT_GERACAO = '" + ano + "-" + mes + "-" + dia + "'" +
													",LI_DS_MENSAGEM =  '" + mensagemErro + "'" +
													",LI_NU_DIAGNOSTICO_ERRO = NULL " +
													",LI_DH_CANCELAMENTO = NULL " +
													"FROM SCIEX_LI WHERE LI_NU = " + numeroLI
												);
										}
										else
										{
											if (!mensagemErro.Contains("LICENCIAMENTO COM ERRO – NAO PROCESSADO"))
											{
												//INSERIR A LI
												SQLExecutar.AppendLine(
													"INSERT INTO SCIEX_LI " +
													"( PME_ID," +
													"  LAR_ID, " +
													"  LI_NU_LI_PROTOCOLADA, " +
													"  LI_NU_DIAGNOSTICO_ERRO, " +
													"  LI_DS_MENSAGEM, " +
													"  LI_ST, " +
													"  LI_TP, " +
													"  LI_DH_CADASTRO, " +
													"  LI_DT_GERACAO) " +
													"VALUES (" +
														ali.IdPliMercadoria.ToString() + "," +
														codigoLiArquivoRetorno.ToString() + "," +
														numeroLIProtocolda + "," +
														codigoStatusDiagnostico + "," +
														"'" + mensagemErro + "'," +
														"2," +
														"1," +
														"'" + GetDateTimeNowUtc().ToString() + "'," +
														"'" + ano + "-" + mes + "-" + dia + "'" +
													") ");

												//atualizar a ALI
												SQLExecutar.AppendLine(
													" UPDATE SCIEX_ALI SET " +
													 "  ALI_ST = " + (int)EnumAliStatus.ALI_INDEFERIDA_PELO_SISCOMEX +
													"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
													" WHERE ALI_NU = " + ali.NumeroAli.ToString()
													);
											}
											else
											{
												//INSERIR A LI
												SQLExecutar.AppendLine(
													"INSERT INTO SCIEX_LI " +
													"( PME_ID," +
													"  LAR_ID, " +
													"  LI_NU_LI_PROTOCOLADA, " +
													"  LI_NU_DIAGNOSTICO_ERRO, " +
													"  LI_DS_MENSAGEM, " +
													"  LI_ST, " +
													"  LI_TP, " +
													"  LI_DH_CADASTRO, " +
													"  LI_DT_GERACAO) " +
													"VALUES (" +
														ali.IdPliMercadoria.ToString() + "," +
														codigoLiArquivoRetorno.ToString() + "," +
														numeroLIProtocolda + "," +
														codigoStatusDiagnostico + "," +
														"'" + mensagemErro + "'," +
														"2," +
														"1," +
														"'" + GetDateTimeNowUtc().ToString() + "'," +
														"'" + ano + "-" + mes + "-" + dia + "'" +
													") ");

												//atualizar a ALI
												SQLExecutar.AppendLine(
													" UPDATE SCIEX_ALI SET " +
													"  ALI_ST = " + (int)EnumAliStatus.ALI_GERADA +
													"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
													" WHERE ALI_NU = " + ali.NumeroAli.ToString()
													);
											}
										}

										#endregion

										break;
									}
								case "98": //ERRO DE ESTRUTURA DO ARQUIVO
									{
										qtdErros = qtdErros + 1;

										numeroALI = linha.Substring(2, 15).Trim();
										mensagemErro = linha.Substring(17, linha.Length - 17);

										long numeroALIPesquisa = Convert.ToInt64(numeroALI);
										var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);
										var listaLI = _uowSciex.QueryStackSciex.Li.Listar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

										#region REGRA DE NEGOCIO 07

										if (listaLI.Count > 0)
										{
											//Insere Tabela de li_historico
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI_HISTORICO_ERRO " +
												"(" +
												"	LI_NU," +
												"	LI_NU_LI_PROTOCOLADA," +
												"	LI_DT_GERACAO," +
												"	LI_DS_MENSAGEM," +
												"	LI_NU_DIAGNOSTICO_ERRO," +
												"	LI_ST," +
												"	LHE_TP_ERRO," +
												"	LI_DH_CADASTRO," +
												"	LI_DH_CANCELAMENTO" +
												")" +
												"" +
												"SELECT " +
												"	LI_NU, " +
												"	LI_NU_LI_PROTOCOLADA, " +
												"	LI_DT_GERACAO, " +
												"	LI_DS_MENSAGEM, " +
												"	LI_NU_DIAGNOSTICO_ERRO, " +
												"	LI_ST, " +
												"	1, " +
												"	CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
												"	LI_DH_CANCELAMENTO " +
												"FROM SCIEX_LI WHERE LI_NU = " + listaLI[0].NumeroLi.ToString()
												);

											//Atualiza LI
											SQLExecutar.AppendLine(
												"UPDATE SCIEX_LI SET " +
													"LI_NU = NULL, " +
													",LI_NU_PROTOCOLADA = NULL " +
													",LI_DS_MENSAGEM =  '" + mensagemErro + "'" +
													",LI_NU_DIAGNOSTICO_ERRO = NULL " +
													",LI_DH_CANCELAMENTO = NULL " +
													",LI_DH_CANCELAMENTO = NULL " +
													",LI_ST = 2 " +
													"FROM SCIEX_LI WHERE LI_NU = " + numeroLI
												);

											//atualizar a ALI
											SQLExecutar.AppendLine(
												" UPDATE SCIEX_ALI SET " +
													"  ALI_ST = " + (int)EnumAliStatus.ALI_GERADA +
												"  , ALI_DH_RESPOSTA_SISCOMEX = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) " +
												" WHERE ALI_NU = " + ali.NumeroAli.ToString()
												);
										}
										else
										{
											//INSERIR A LI
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI " +
												"( PME_ID," +
												"  LAR_ID, " +
												"  LI_DS_MENSAGEM, " +
												"  LI_ST, " +
												"  LI_TP, " +
												"  LI_DH_CADASTRO) " +
												"VALUES (" +
													ali.IdPliMercadoria.ToString() + "," +
													codigoLiArquivoRetorno.ToString() + "," +
													"'" + mensagemErro + "'," +
													"2," +
													"1," +
													"'" + GetDateTimeNowUtc().ToString() + "'," +
												") ");

											//atualizar a ALI
											SQLExecutar.AppendLine(
												" UPDATE SCIEX_ALI SET " +
													"  ALI_ST = " + (int)EnumAliStatus.ALI_GERADA +
												"  ,ALI_DH_RESPOSTA_SISCOMEX = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00'))" +
												" WHERE ALI_NU = " + ali.NumeroAli.ToString()
												);
										}

										#endregion

										break;
									}
								default:
									break;
							}
							#endregion
						}
					}
					contador++;
				}
				file.Close();

				#region REGRA DE NEGOCIO 08
				SQLExecutar.AppendLine(
					"UPDATE SCIEX_LI_ARQUIVO_RETORNO SET " +
					   "LAR_QT_LI = " + qtdLI.ToString() +
					   ",LAR_QT_LI_DEFERIDA = " + qtdLIDeferida.ToString() +
					   ",LAR_QT_LI_INDEFERIDA = " + qtdLIIndeferida.ToString() +
					   ",LAR_QT_LI_ERRO = " + qtdErros.ToString() +
					"WHERE LAR_ID =" + codigoLiArquivoRetorno
				);
				#endregion

				SQLExecutar.AppendLine(
				"					UPDATE SCIEX_CONTROLE_EXEC_SERVICO " +
				"					SET " +
				"						CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), " +
				"						CES_ST_EXECUCAO = 2, " +
				"						CES_ME_OBJETO_RETORNO = 'Arquivo de LI (" + codigoLiArquivoRetorno.ToString() + " lido com sucesso' " +
				"					WHERE CES_ID = @CONTROLE_SERVICO_COD " +
					"COMMIT; " +
				"END TRY   " +
				"BEGIN CATCH " +
				"	IF @@TRANCOUNT > 0 " +
				"	BEGIN " +
				"		ROLLBACK " +
				"		SELECT " +
				"			 ERROR_NUMBER() AS ErrorNumber " +
				"			, ERROR_SEVERITY() AS ErrorSeverity " +
				"			, ERROR_STATE() AS ErrorState " +
				"			, ERROR_PROCEDURE() AS ErrorProcedure " +
				"			, ERROR_LINE() AS ErrorLine " +
				"			, ERROR_MESSAGE() AS ErrorMessage; " +
				"				BEGIN TRANSACTION " +
				"					UPDATE SCIEX_CONTROLE_EXEC_SERVICO " +
				"					SET " +
				"						CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), " +
				"						CES_ST_EXECUCAO = 2, " +
				"						CES_ME_OBJETO_RETORNO = ERROR_MESSAGE() " +
				"					WHERE CES_ID = @CONTROLE_SERVICO_COD " +
				"				COMMIT " +
				"	END " +
				"END CATCH ");

				string sql = SQLExecutar.ToString();
				_uowSciex.CommandStackSciex.Salvar(sql);
			}


			//arquivo de cancelamento da LI
			nomeDoArquivo = "ARQCANC.FILE";
			localAqruivoFTP = @"ftp://" + enderecoFTP + "/" + pastaFTP + "/" + nomeDoArquivo;

			if (Ftp.VerificarSeExisteArquivo(localAqruivoFTP, usuario, senha))
			{
				//TO-DO
			}
		}

		public void GerarArquivoSimulacaoLI()
		{
			StringBuilder arquivo = new StringBuilder();

			arquivo.Append("24");
			arquivo.Append("04407029000143");

			string dataInicio = "01/01/" + GetDateTimeNowUtc().Year.ToString();
			string dataAtual = GetDateTimeNowUtc().Day.ToString() + "/" + GetDateTimeNowUtc().Month.ToString() + "/" + GetDateTimeNowUtc().Year.ToString();
			TimeSpan date = Convert.ToDateTime(dataAtual) - Convert.ToDateTime(dataInicio);
			arquivo.Append((date.Days + 1).ToString("D3"));

			arquivo.Append(GetDateTimeNowUtc().Hour.ToString("D2") + GetDateTimeNowUtc().Minute.ToString("D2") + GetDateTimeNowUtc().Second.ToString("D2"));
			arquivo.Append("02");
			arquivo.Append("02");
			arquivo.Append("02");
			arquivo.Append("850");
			arquivo.AppendLine();

			List<AliEntity> listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.Status == (int)EnumAliStatus.ALI_ENVIADA_AO_SISCOMEX);

			long codigoLIProtocolada = 2459230332;
			long codigoLI = 2459230332;

			int contador = 1;
			foreach (AliEntity item in listaALI)
			{
				if (contador % 6 == 0)
				{
					codigoLIProtocolada++;
					arquivo.Append("99");
					arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
					arquivo.Append(codigoLIProtocolada.ToString("D10"));
					arquivo.Append("2");
					arquivo.Append(GetDateTimeNowUtc().Year.ToString("D4") + GetDateTimeNowUtc().Month.ToString("D2") + GetDateTimeNowUtc().Day.ToString("D2"));
					arquivo.Append("NUMERO DO DESTAQUE DA NCM PARA ANUENCIA - NAO INFORMADO");
				}
				else

				if (contador % 10 == 0)
				{
					codigoLIProtocolada++;
					arquivo.Append("99");
					arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
					arquivo.Append(codigoLIProtocolada.ToString("D10"));
					arquivo.Append("2");
					arquivo.Append(GetDateTimeNowUtc().Year.ToString("D4") + GetDateTimeNowUtc().Month.ToString("D2") + GetDateTimeNowUtc().Day.ToString("D2"));
					arquivo.Append("DECLARACAO NAO ACEITA - USUARIO NAO CADASTRADO COMO REPRESENTANTE LEGAL DO IMPORTADOR PARA AS ATIVIDADES DE IMPORTACAO");
				}
				else
				{
					arquivo.Append("01");
					arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
					arquivo.Append(codigoLIProtocolada.ToString("D10"));
					arquivo.Append(codigoLI.ToString("D10"));
					arquivo.Append(GetDateTimeNowUtc().Year.ToString("D4") + GetDateTimeNowUtc().Month.ToString("D2") + GetDateTimeNowUtc().Day.ToString("D2"));
				}

				arquivo.AppendLine();

				codigoLIProtocolada++;
				codigoLI++;
				contador++;
			}

			string fileName = @"c:\temp\ARQCANC.FILE";

			if (!Ftp.VerificarSeExisteArquivo(@"ftp://192.168.0.251/ArquivoRetornoSISCOMEX/SERPRO.FILE", "ctis", "ctis"))
			{
				string mensagem = Ftp.EnviarArquivo(@"ftp://192.168.0.251/ArquivoRetornoSISCOMEX/SERPRO.FILE", "ctis", "ctis", arquivo.ToString());

				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}

				using (StreamWriter sw = File.CreateText(fileName))
				{
					sw.Write(arquivo.ToString());
				}
			}

		}

	}
}
