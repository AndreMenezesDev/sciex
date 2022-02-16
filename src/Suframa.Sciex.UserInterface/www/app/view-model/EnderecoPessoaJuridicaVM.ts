import { manterEnderecoVM } from './ManterEnderecoVM';

export class enderecoPessoaJuridicaVM {
	endereco: manterEnderecoVM;
	idPessoaJuridica?: number;
	idCep: number;
	idUnidadeCadastradora?: number;
	numeroEndereco: string;
	complemento: string;
	pontoReferencia: string;
	telefone?: number;
	ramal?: number;
	email: string;
}
