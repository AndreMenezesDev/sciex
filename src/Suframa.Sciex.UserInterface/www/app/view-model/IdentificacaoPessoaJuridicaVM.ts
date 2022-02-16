import { pessoaJuridicaInscricaoEstadualVM } from './PessoaJuridicaInscricaoEstadualVM';
import { manterEnderecoVM } from './ManterEnderecoVM';
import { documentoComprobatorioVM } from './DocumentoComprobatorioVM';
import { FiltroCadastroPessoaJuridicaVM } from './FiltroCadastroPessoaJuridicaVM';

export class identificacaoPessoaJuridicaVM {
	cnpj: string;
	codigo: string;
	complemento: string;
	dataAlteracao?: Date;
	dataInclusao?: Date;
	dataRegistro?: Date;
	documentosComprobatorios: documentoComprobatorioVM[];
	email: string;
	endereco: manterEnderecoVM;
	filtroCadastroPessoaJuridica: FiltroCadastroPessoaJuridicaVM;
	idCep: number;
	idPessoaJuridica: number;
	idPorteEmpresa?: number;
	idRequerimento?: number;
	idTipoRequerimento: number;
	idUnidadeCadastradora: number;
	isCredenciamento: boolean;
	isCredenciamentoTransportador: boolean;
	isEntidadeEmpresarial: boolean;
	isQuadroSocietario: boolean;
	modalidadeTransportador: number;
	nomeFantasia: string;
	numeroEndereco: string;
	numeroInscricaoMunicipal: string;
	numeroRegistroConstituicao: string;
	pessoaJuridicaInscricaoEstadual: pessoaJuridicaInscricaoEstadualVM[];
	pontoReferencia: string;
	ramal?: number;
	razaoSocial: string;
	statusOptanteSimples?: boolean;
	telefone?: number;
	valorCapitalSocial?: number;
}
