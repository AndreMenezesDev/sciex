import { identificacaoPessoaFisicaVM } from './IdentificacaoPessoaFisicaVM';
import { identificacaoPessoaJuridicaVM } from './IdentificacaoPessoaJuridicaVM';

export class inscricaoCadastralVM {
	ano: number;
	codigo: number;
	cpfCnpj: string;
	data: Date;
	idInscricaoCadastral: number;
	idPessoaFisica: number;
	idPessoaJuridica: number;
	idSituacaoInscricao: number;
	idTipoIncentivo: number;
	idUnidadeCadastradora: number;
	nomeRazaoSocial: string;
	pessoaFisica: identificacaoPessoaFisicaVM;
	pessoaJuridica: identificacaoPessoaJuridicaVM;
}
