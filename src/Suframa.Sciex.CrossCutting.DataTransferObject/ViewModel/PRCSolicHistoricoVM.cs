﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCSolicHistoricoVM : PRCStatusVM
	{
		public PRCSolicitacaoAlteracaoVM SolicitacaoAlteracao { get; set; }

	}

}
