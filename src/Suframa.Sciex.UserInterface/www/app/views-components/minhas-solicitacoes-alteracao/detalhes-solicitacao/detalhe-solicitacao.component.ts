import { Component, OnInit, Injectable, ViewChild, AfterContentInit } from '@angular/core';
import { PagedItems } from '../../../view-model/PagedItems';
import { ModalService } from '../../../shared/services/modal.service';
import { MessagesService } from '../../../shared/services/messages.service';
import { ApplicationService } from '../../../shared/services/application.service';
import { ValidationService } from '../../../shared/services/validation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthGuard } from '../../../shared/guards/auth-guard.service';
import { PRCSolicDetalheVM } from '../../../view-model/PRCSolicDetalheVM';
import { DetalheMinhaSolicitacaoAlteracaoVM } from '../../../view-model/DetalheMinhaSolicitacaoAlteracaoVM';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
	selector: 'app-detalhe-solicitacao',
	templateUrl: './detalhe-solicitacao.component.html'
})

@Injectable()
export class DetalheSolicitacaoComponent implements OnInit {
    
	grid: any = { sort: {} };
	model : DetalheMinhaSolicitacaoAlteracaoVM = new DetalheMinhaSolicitacaoAlteracaoVM();
	parametros: PRCSolicDetalheVM = new PRCSolicDetalheVM();
	servico = 'SolicitacoesAlteracaoDetalhe';	
	formPai = this;
	idSolicitacaoAlteracao : number;
	routeMinhasSolicitacoes : string;

	constructor(
		private applicationService: ApplicationService,
		private validationService: ValidationService,
		private modal: ModalService,
		private msg: MessagesService,
		private router: Router,
		private route: ActivatedRoute,
		private authguard: AuthGuard,
	) {
	 this.routeMinhasSolicitacoes = sessionStorage.getItem("routeMinhasSolicitacoes");		
	 this.idSolicitacaoAlteracao = Number(this.route.snapshot.url[this.route.snapshot.url.length - 1].path);			
	}

	ngOnInit(): void {
		this.buscarInfo();
	}

	onChangeSort($event) {
		this.grid.sort = $event;
	}

	onChangeSize($event) {
		this.grid.size = $event;
	}

	onChangePage($event) {
		this.grid.page = $event;
		this.listar();
	}

	buscarInfo(){
		this.applicationService.get<DetalheMinhaSolicitacaoAlteracaoVM>(this.servico, this.idSolicitacaoAlteracao)
			.subscribe((result:DetalheMinhaSolicitacaoAlteracaoVM) => {
			this.model = result;
			this.listar();
		});
	}

	listar(){
		
		this.parametros.servico  = this.servico;
		this.parametros.page = this.grid.page;
		this.parametros.size = this.grid.size;
		this.parametros.sort = this.grid.sort.field || "Id";
		this.parametros.reverse = this.grid.sort.reverse;
		this.parametros.idSolicitacaoAlteracao = this.idSolicitacaoAlteracao;

			this.applicationService.get(this.servico, this.parametros).subscribe((result:PagedItems) => {
				if (result.total > 0){
					console.log(result)
					this.grid.lista = result.items;
					this.grid.total = result.total;
					this.setarDadosExportacao();
				} else {
					this.grid = { sort: {} };
					this.parametros.exportarListagem = false;
				}
			});
	}

	voltar(){
		let arrayUrl = sessionStorage.getItem("arrayUrl");
		let obj = JSON.parse(arrayUrl)
		let url = obj[obj.length - 2]
		obj.pop()
		sessionStorage.removeItem("arrayUrl")
		sessionStorage.setItem("arrayUrl", JSON.stringify(obj))
		
		this.router.navigate([url]);
	}

	setarDadosExportacao() {
		this.parametros.exportarListagem = true;

		this.parametros.titulo = "DETALHE MINHA SOLICITAÇÕES DE ALTERAÇÃO"

		this.parametros.width = { 0: { columnWidth: 120 }, 
								  1: { columnWidth: 120 }, 
								  2: { columnWidth: 120 }, 
								  3: { columnWidth: 120 }, 
								  4: { columnWidth: 120 },
								  5: { columnWidth: 90 } ,
								  6: { columnWidth: 90 } };
		
								  this.parametros.servico = this.servico;		

		this.parametros.columns = [
			"Insumo", 
			"Detalhe", 
			"Status",
			"Descrição Insumo", 
			"Tipo de Alteração",
			"DE",
			"PARA"
		];

		this.parametros.fields = [
			"codigoInsumo", 
			"codigoDetalheInsumo", 
			"descricaoStatus", 
			"descricaoInsumo", 
			"descricaoTipoAlteracao",
			"descricaoDe",
			"descricaoPara"
		];
	}

}

