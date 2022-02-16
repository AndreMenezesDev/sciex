using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class SolicitacaoBll : ISolicitacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		public SolicitacaoBll(
			IUnitOfWorkSciex uowSciex,
			IUsuarioLogado IUsuarioLogado,
			IUsuarioInformacoesBll usuarioInformacoesBll,
			IViewImportadorBll IViewImportadorBll
			)
		{
			_uowSciex =uowSciex;

			_IUsuarioLogado = IUsuarioLogado;
	        
			_usuarioInformacoesBll= _usuarioInformacoesBll;
			
			_IViewImportadorBll =IViewImportadorBll;




		}




	}
}
