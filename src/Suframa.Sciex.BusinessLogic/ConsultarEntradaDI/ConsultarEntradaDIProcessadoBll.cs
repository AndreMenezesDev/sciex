using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class ConsultarEntradaDIProcessadoBll : IConsultarEntradaDIProcessadoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		
		public ConsultarEntradaDIProcessadoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public PagedItems<DIEntradaVM> ListarPaginado(DIEntradaVM pagedFilter)
		{
			try
			{
				var listaDIEntrada = _uowSciex.QueryStackSciex.DiEntrada.ListarPaginado<DIEntradaVM>(p =>
				(
				   (
					   pagedFilter.SituacaoLeitura == 0 &&
					   p.IdDiArquivoEntrada == pagedFilter.Id
				   ) ||
				   (
					   p.Situacao == pagedFilter.SituacaoLeitura &&
					   p.IdDiArquivoEntrada == pagedFilter.Id

				   )
				), pagedFilter);

				if (listaDIEntrada.Items.Count > 0)
				{
					foreach (var item in listaDIEntrada.Items)
					{
						// Pesquisar o nome da razão social da empresa
						var Empresa = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(s => s.Cnpj == item.Cnpj);

						if (Empresa != null)
						{
							item.NomeEmpresa = Empresa.RazaoSocial;
						}
					}
				}

				return listaDIEntrada;
			}
			catch
			{

				return new PagedItems<DIEntradaVM>();
			}
			
		}
	}
}
