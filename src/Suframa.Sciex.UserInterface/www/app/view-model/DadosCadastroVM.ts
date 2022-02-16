import { identificacaoPessoaFisicaVM } from './IdentificacaoPessoaFisicaVM';
import { identificacaoPessoaJuridicaVM } from './IdentificacaoPessoaJuridicaVM';
import { atividadesPessoaJuridicaVM } from './AtividadesPessoaJuridicaVM';
import { quadrosAdministradoresVM } from './QuadrosAdministradoresVM';
import { quadrosSocietariosVM } from './QuadrosSocietariosVM';
import { quadroPessoalVM } from './QuadroPessoalVM';
import { dadosSolicitanteVM } from './DadosSolicitanteVM';
import { documentosComprobatoriosVM } from './DocumentosComprobatoriosVM';
import { resumoVM } from './ResumoVM';
import { campoSistemaVM } from './CampoSistemaVM';
import { pendenciaCadastralVM } from './PendenciaCadastralVM';
import { protocoloVM } from './ProtocoloVM';
import { pedidoCorrecaoVM } from './PedidoCorrecaoVM';

export class dadosCadastroVM {
	atividadesPessoaJuridica: atividadesPessoaJuridicaVM;
	camposSistema: campoSistemaVM[];
	dadosSolicitante: dadosSolicitanteVM;
	documentosComprobatorios: documentosComprobatoriosVM;
	identificacaoPessoaFisica: identificacaoPessoaFisicaVM;
	identificacaoPessoaJuridica: identificacaoPessoaJuridicaVM;
	idProtocolo?: number;
	isCredenciamento: boolean;
	isInscricaoCadastral: boolean;
	isInscricaoCadastralPessoaJuridica: boolean;
	pendenciasCadastrais: pendenciaCadastralVM[];
	pedidoCorrecao: pedidoCorrecaoVM[];
	protocolo: protocoloVM;
	quadroPessoal: quadroPessoalVM[];
	quadrosAdministradores: quadrosAdministradoresVM;
	quadrosSocietarios: quadrosSocietariosVM;
	resumo: resumoVM;
	tipoDadoCadastro: number;
}
