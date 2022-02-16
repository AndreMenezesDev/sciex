let opcaoInscricao = 'Cadastro de pessoa física ou jurídica para fruição de incentivos fiscais.';
let opcaoCredenciamento = 'Cadastro de pessoa física ou jurídica, tais como: prepostos, consultores, auditorias independentes, empresas, remetentes, transportadores.';
let opcaoPessoaJuridica = '•    Aprovar projetos empresariais de qualquer natureza a serem implementados no âmbito da Zona Franca de Manaus, nas Áreas de Livre Comércio a ela vinculadas e demais áreas da Amazônia Ocidental; <br /> <br />' + '•    Promover a entrada de mercadorias nacionais ou estrangeiras na Zona Franca de Manaus, nas Áreas de Livre Comércio a ela vinculadas e demais áreas da Amazônia Ocidental; <br /> <br />' + '•    Promitentes compradoras ou concessionárias de lotes de terras no âmbito do Distrito Agropecuário, para efeito de implementação e desenvolvimento de projetos agropecuários aprovados pela Suframa, nos termos de regulamentação específica.';
let opcaoPessoaFisica = '•    Promitentes compradoras ou concessionárias de lotes de terras no âmbito do Distrito Agropecuário, para efeito de implementação e desenvolvimento de projetos agropecuários aprovados pela Suframa, nos termos de regulamentação específica.';
let opcaoCredenciamentoJuridica = 'Como auditorias independentes, empresas remetentes e transportadores.'
let opcaoCredenciamentoFisica = 'Como prepostos, consultores e transportadores.';
let opcaoCredenciamentoJuridicaTransportador = 'Responsáveis pelo transporte das mercadorias para as áreas incentivadas.';
let opcaoCredenciamentoJuridicaAuditores = 'Responsáveis pelos serviços de auditoria de processos produtivos básicos e de sistemas de qualidade.';
let opcaoCredenciamentoJuridicaRemetente = 'Fornecedoras das mercadorias destinadas às áreas incentivadas.';
let opcaoCredenciamentoFisicaPreposto = 'Incumbidas de atos procedimentais relativos ao internamento de mercadorias nas áreas incentivadas.';
let opcaoCredenciamentoFisicaConsultores = 'Responsáveis pela apresentação e acompanhamento de projetos empresariais de qualquer natureza.';
let opcaoCredenciamentoFisicaTransportador = 'Responsáveis pelo transporte das mercadorias para as áreas incentivadas.';

$(document).on("mouseenter mouseleave", "a", function () {
	let opcao = '';

	if ($('#descricaoInscricao').length > 0 && $('#descricaoInscricao').is(':hover')) {
		opcao = opcaoInscricao;
	}

	if ($('#descricaoCredenciamento').length > 0 && $('#descricaoCredenciamento').is(':hover')) {
		opcao = opcaoCredenciamento;
	}

	if ($('#inscricaoJuridica').length > 0 && $('#inscricaoJuridica').is(':hover')) {
		opcao = opcaoPessoaJuridica;
	}

	if ($('#inscricaoFisica').length > 0 && $('#inscricaoFisica').is(':hover')) {
		opcao = opcaoPessoaFisica;
	}

	if ($('#credenciamentoJuridica').length > 0 && $('#credenciamentoJuridica').is(':hover')) {
		opcao = opcaoCredenciamentoJuridica;
	}

	if ($('#credenciamentoFisica').length > 0 && $('#credenciamentoFisica').is(':hover')) {
		opcao = opcaoCredenciamentoFisica
	}

	if ($('#credenciamentoTransportadoresPJ').length > 0 && $('#credenciamentoTransportadoresPJ').is(':hover')) {
		opcao = opcaoCredenciamentoJuridicaTransportador;
	}

	if ($('#credenciamentoAuditoresPJ').length > 0 && $('#credenciamentoAuditoresPJ').is(':hover')) {
		opcao = opcaoCredenciamentoJuridicaAuditores;
	}

	if ($('#credenciamentoRemetentePJ').length > 0 && $('#credenciamentoRemetentePJ').is(':hover')) {
		opcao = opcaoCredenciamentoJuridicaRemetente;
	}

	if ($('#credenciamentoPrepostosPF').length > 0 && $('#credenciamentoPrepostosPF').is(':hover')) {
		opcao = opcaoCredenciamentoFisicaPreposto;
	}

	if ($('#credenciamentoConsultoresPF').length > 0 && $('#credenciamentoConsultoresPF').is(':hover')) {
		opcao = opcaoCredenciamentoFisicaConsultores;
	}

	if ($('#credenciamentoTranportadoresPF').length > 0 && $('#credenciamentoTranportadoresPF').is(':hover')) {
		opcao = opcaoCredenciamentoFisicaTransportador;
	}

	$('#descricao').html(opcao);
});

//Função de contagem regressiva de caracteres do text box de cancelamento
$(document).on("keyup", ".char-textarea", function (event) {
	verificarTextAreaMaxLength(this, event);
});

function verificarTextAreaMaxLength(textBox, e) {
	var maxLength = parseInt($(textBox).data("length"));

	if (!verificarCaracteresEspeciais(e)) {
		if (textBox.value.length > maxLength - 1) textBox.value = textBox.value.substring(0, maxLength);
	}
	$(".char-count").html(maxLength - textBox.value.length);

	return true;
}
function verificarCaracteresEspeciais(e) {
	if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40) {
		return false;
	}

	return true;
}
