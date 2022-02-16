using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
	public class StatusProtocoloGrupoDto
	{
		public static List<int> Distribuicao
		{
			get
			{
				return new List<int> {
					(int) EnumStatusProtocolo.AguardandoConferenciaDocumental,
					(int) EnumStatusProtocolo.AguardandoReanalise,
					(int) EnumStatusProtocolo.AguardandoVisitaTecnica,
					(int) EnumStatusProtocolo.ComPendencia,
					(int) EnumStatusProtocolo.EmAnalise,
					(int) EnumStatusProtocolo.PagamentoRecebido,
					(int) EnumStatusProtocolo.ProntoParaAnalise
				};
			}
		}

		public static List<int> IndeferidoAguardandoRecurso
		{
			get
			{
				return new List<int> {
					(int) EnumStatusProtocolo.IndeferidoAguardandoRecurso
				};
			}
		}

		public static List<int> ProntoParaAnalise
		{
			get
			{
				return new List<int> {
					(int)EnumStatusProtocolo.PagamentoRecebido,
					(int)EnumStatusProtocolo.ProntoParaAnalise
				};
			}
		}

		public static List<int> ProtocoloAberto
		{
			get
			{
				return new List<int> {
					(int) EnumStatusProtocolo.Cancelado,
					(int) EnumStatusProtocolo.Deferido,
					(int) EnumStatusProtocolo.Indeferido
				};
			}
		}

		public static List<int> StatusAnalise
		{
			get
			{
				return new List<int> {
					(int)EnumStatusProtocolo.EmAnalise,
					(int)EnumStatusProtocolo.ComPendencia,
					(int)EnumStatusProtocolo.AguardandoAnalise,
					(int)EnumStatusProtocolo.AguardandoReanalise,
					(int)EnumStatusProtocolo.AguardandoVisitaTecnica,
					(int)EnumStatusProtocolo.AguardandoConferenciaDocumental
				};
			}
		}

		public static List<int> StatusAnaliseGrupo
		{
			get
			{
				return new List<int>
				{
					(int) EnumStatusProtocolo.EmAnalise,
					(int) EnumStatusProtocolo.ComPendencia,
					(int) EnumStatusProtocolo.AguardandoAnalise,
					(int) EnumStatusProtocolo.AguardandoReanalise,
					(int) EnumStatusProtocolo.AguardandoVisitaTecnica,
					(int) EnumStatusProtocolo.RecursoEmAnalise
				};
			}
		}

		public static List<int> StatusAnaliseUsuario
		{
			get
			{
				return new List<int> {
					(int)EnumStatusProtocolo.AguardandoAnalise,
					(int)EnumStatusProtocolo.EmAnalise,
					(int)EnumStatusProtocolo.ComPendencia,
					(int)EnumStatusProtocolo.AguardandoConferenciaDocumental,
					(int)EnumStatusProtocolo.AguardandoReanalise,
					(int)EnumStatusProtocolo.AguardandoVisitaTecnica
				};
			}
		}

		public static List<int> StatusProtocoloPapelCoordenadorGeral
		{
			get
			{
				return new List<int> {
						(int) EnumStatusProtocolo.RecursoEmAnalise
					};
			}
		}

		public static List<int> StatusProtocoloPapelSuperintendenteAdjunto
		{
			get
			{
				return new List<int> {
						(int)EnumStatusProtocolo.EmJulgamento
					};
			}
		}

		public static List<int> StatusProtocoloPapelTecnico
		{
			get
			{
				return new List<int> {
						(int)EnumStatusProtocolo.AguardandoAnalise,
						(int)EnumStatusProtocolo.AguardandoConferenciaDocumental,
						(int)EnumStatusProtocolo.AguardandoReanalise,
						(int)EnumStatusProtocolo.AguardandoVisitaTecnica,
						(int)EnumStatusProtocolo.ComPendencia,
						(int)EnumStatusProtocolo.EmAnalise
					};
			}
		}
	}
}