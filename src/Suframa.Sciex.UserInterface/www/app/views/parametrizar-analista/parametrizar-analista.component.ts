import { Component, OnInit, Injectable } from '@angular/core';
import { PagedItems } from '../../view-model/PagedItems';
import { ModalService } from '../../shared/services/modal.service';
import { MessagesService } from '../../shared/services/messages.service';
import { ApplicationService } from '../../shared/services/application.service';
import { Router } from '@angular/router';



@Component({
    selector: 'app-parametrizar-analista',
    templateUrl: './parametrizar-analista.component.html'
})

@Injectable()
export class ParametrizarAnalistaComponent implements OnInit {
    grid: any = { sort: {} };
    parametros: any = {};
    ocultarFiltro: boolean = false;
    ocultarGrid: boolean = true;
    servicoParametrizarAnalistaGrid = 'ParametroAnalista1Grid';
    result: boolean = false;
    

    constructor(
        private applicationService: ApplicationService,
        private modal: ModalService,
		private msg: MessagesService,
		private router: Router
	) {
		if (localStorage.getItem(this.router.url) == null && localStorage.length > 0) {
			localStorage.clear();
		}
	}

    ngOnInit(): void {
        this.listar();
    }

    buscar() {        
		this.grid.page = 1;
		this.listar();
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

    limpar() {
        this.parametros = {};
        this.limpar();
    }

    verificarValor(valor, x) {

        for (var i = 0; i < valor.length; i++) {
            if (x == valor[i]) {

                this.result = true;
            }
        }
        var v = this.result;
        this.result = false;

        return v;
    }

    montarListaStatus(valor) {
        // adicionar status pli a listas

        var listaStatus = [
            { idStatusPLI: 0, descricao: 'Novo', checked: this.verificarValor(valor, 0) },
            { idStatusPLI: 1, descricao: 'Em Análise', checked: this.verificarValor(valor, 1) },
            { idStatusPLI: 2, descricao: 'Pendente', checked: this.verificarValor(valor, 2) }
        ];

        return Object.keys(listaStatus).map(function (key) { return listaStatus[key]; });
    }

    listar() {
        this.parametros.page = this.grid.page;
        this.parametros.size = this.grid.size;
        this.parametros.sort = this.grid.sort.field;
        this.parametros.reverse = this.grid.sort.reverse;

        this.applicationService.get(this.servicoParametrizarAnalistaGrid, this.parametros).subscribe((result: PagedItems) => {
            this.grid.lista = result.items;
            this.grid.total = result.total;

            this.ocultarGrid = true;

			if ((!this.grid.lista || this.grid.lista.length == 0)) {                
                this.modal.alerta(this.msg.NENHUM_REGISTRO_ENCONTRADO);
			}
            else {
                for (var i = 0; i < result.items.length; i++) {
                   
                    
                    if (result.items[i].descricaoAnaliseVisualBloqueioFim != null
                        && result.items[i].descricaoAnaliseVisualBloqueioFim != "")
                    {
                        var checkedStatusAnaliseVisual = result.items[i].descricaoAnaliseVisualBloqueioFim.split(',');
                        result.items[i].listaStatusAnaliseVisual = this.montarListaStatus(checkedStatusAnaliseVisual);
                    }
                    else {
                        result.items[i].listaStatusAnaliseVisual = this.montarListaStatus('');
                    }

                    if (result.items[i].descricaoAnaliseLoteListagemBloqueioFim != null
                        && result.items[i].descricaoAnaliseLoteListagemBloqueioFim != "")
                    {
                        var checkedStatusAnaliseListagem = result.items[i].descricaoAnaliseLoteListagemBloqueioFim.split(',');
                        result.items[i].listaStatusAnaliseListagem = this.montarListaStatus(checkedStatusAnaliseListagem);        
                    }
                    else
                    {
                        result.items[i].listaStatusAnaliseListagem = this.montarListaStatus('');        
                    }
                }                            
                this.ocultarGrid = false;
            }
        });
    }

    

}
