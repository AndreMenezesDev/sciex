import { Component, Input, OnInit, ElementRef, OnChanges, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { ArrayService } from '../../services/array.service';
import { ApplicationService } from '../../services/application.service';

@Component({
    selector: 'app-drop-list',
    templateUrl: './drop-list.component.html',
    providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: DropListComponent, multi: true }]
})

export class DropListComponent implements ControlValueAccessor, OnInit, OnChanges {
    @Input() isDisabled = false;
    @Input() podeCarregar = true;
    @Input() isRequired = false;
    @Input() lazy = false;
    @Input() loadWhenParameterEmpty: boolean;
    @Input() parametro: any;
    @Input() parametroChave: string;
    @Input() placeholder: string;
	@Input() servico: string;
	@Input() isVisibleFieldCod: boolean = false;
    @Input() selecionarPrimeiroRegistro: boolean = true;

    list: any = new Array<any>();
    listAutoComplete: any = new Array<any>();
    model: any;
	required: any;

	@ViewChild("select") select;

    constructor(
        private applicationService: ApplicationService,
        private arrayService: ArrayService,
        private elm: ElementRef,
	) {
		this.placeholder = this.placeholder || 'Informe o código ou a descrição';
    }

    ngOnInit() {
        this.required = this.elm.nativeElement.attributes['required'];

        // Se for especificado parametro chave
        if (!this.parametroChave) {
            this.loadWhenParameterEmpty = true; // carregar quando a propriedade parametro estiver vazia
            this.lazy = false; // ativa automaticamente o serviço de carregar dados
        } else {
            this.loadWhenParameterEmpty = false; // não carrega os ítens quando a propriedade parametro estiver vazia
            this.lazy = true; // mantem o componente preguiçoso até a propriedade parametro ser modificada ou o métodos listItems ser acionado
        }

        if (!this.lazy) {
            this.listItems();
        }
    }

    getCount() {
        return this.list ? this.list.length : 0;
    }

    getIsDisable() {
        return this.isDisabled || !this.list || this.list.length == 0;
    }

    reset() {
        this.list = null;
        this.listAutoComplete = null;
        this.model = '';
    }

    listItemsObservable() {
        return Observable.create(observer => {
            this.callService(this.urlService(), observer);
        });
    }

    urlService() {
        let url = `${this.servico}DropDown/`;

        if (this.parametroChave && this.parametro) {
            url = url + '?' + this.parametroChave + '=' + this.parametro;
        } else {
            if (!this.parametroChave && this.parametro) {
                url = url + this.parametro;
            }
        }

        return url;
    }

	callService(url, observer?) {
        return this.applicationService.get(url).subscribe(result => {
            this.list = result;
            this.listAutoComplete = this.list;

            // Selecionar o primeiro ítem quando retornar somente 1 linha
            if (this.list.length == 1) {
                this.model = this.list[0].id;
                this.onChange(this.list[0].id);
            }else{
                if(this.selecionarPrimeiroRegistro){
                    this.model = this.list[0].id;
                    this.onChange(this.list[0].id);
                }
            }

            if (observer) {
                observer.next(result);
                observer.complete();
            }
        });
    }
    listItems() {
        return this.callService(this.urlService());
    }

    removeItem(item: string) {
        this.arrayService.remove(this.list, 'id', item);

        if (this.list.length == 1) {
            this.model = this.list[0].id;
            this.onChange(this.list[0].id);
        }
    }

    addItem(item: any) {
        this.arrayService.add(this.list, 'id', item, false);
    }

    getAllItems() {
        return this.list;
    }

    getSelectedKey() { return this.model; }

    getSelectedItem() {
        const empty = { value: this.getSelectedKey() };

        if (!this.list) { return empty; }

        const item = this.list.filter(x => x.id == this.getSelectedKey());

        if (!item) { return empty; }

        return item[0];
    }

    getSelectedValue() {
        if (this.getSelectedItem()) {
            return this.getSelectedItem().value;
        }
    }

    writeValue(obj: any): void {
        if (!obj) { obj = ''; }
        this.model = obj;
    }

	ngOnChanges(changes: any) {

        if (changes.parametro && this.podeCarregar) {
            if (!changes.parametro.firstChange) {
                if (changes.parametro.currentValue || this.loadWhenParameterEmpty) {
                    this.listItems();
                }
            }
        }
    }

    onChange(value) {
        this.writeValue(value);
    }

    registerOnChange(fn: any): void { this.onChange = fn; }

    registerOnTouched(fn: any): void { }

    setDisabledState(isDisabled: boolean): void { }

    onInput(value) {
        if (!value) {
            this.list = this.listAutoComplete;
            return;
        }

        if (this.list && this.list.length > 1) {
            this.list = this.listAutoComplete.filter(function (el) {
                return el.value.toLowerCase().indexOf(value.toLowerCase()) != -1;
            });
        }
	}

	onClear() {
		this.select.nativeElement.value = "";
	}

	public clear() {
		this.onClear();
	}
}
