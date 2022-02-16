import { Component, ViewChild } from '@angular/core';


@Component({
	selector: 'app-modal-amparo-legal-beneficio',
	templateUrl: './modal-amparo-legal-beneficio.component.html',
})

export class ModalAmparoLegalBeneficio {

	@ViewChild('appModalAmparoLegalBeneficio') appModalAmparoLegalBeneficio;
	@ViewChild('appModalAmparoLegalBeneficioBackground') appModalAmparoLegalBeneficioBackground;

    texto : string;
	percentual : any;
	descricao : string;
	tipobeneficio : string;
	codigo: any;

	public abrir(descricaoAmparoLegal, percentual, descricao, tipoBeneficio, codigo) {

		if(tipoBeneficio == '1'){
			this.tipobeneficio = "Isenção";
		} else if(tipoBeneficio == '2'){
			this.tipobeneficio = "Redução";
		} else if(tipoBeneficio == '3'){
			this.tipobeneficio = "Suspensão";
		} else if(tipoBeneficio == '0'){
			this.tipobeneficio = "Nenhum";
		} else{
			this.tipobeneficio = "--";
		}

		codigo ? this.codigo = codigo : this.codigo = '--';
		descricaoAmparoLegal ? this.texto = descricaoAmparoLegal : this.texto = '--';
		percentual ? this.percentual = percentual : this.percentual = '--';
		descricao ? this.descricao = descricao : this.descricao = '--';

		this.appModalAmparoLegalBeneficio.nativeElement.style.display = 'block';
		this.appModalAmparoLegalBeneficioBackground.nativeElement.style.display = 'block';

	}

	public fechar() {
		this.appModalAmparoLegalBeneficio.nativeElement.style.display = 'none';
		this.appModalAmparoLegalBeneficioBackground.nativeElement.style.display = 'none';
	}

}
