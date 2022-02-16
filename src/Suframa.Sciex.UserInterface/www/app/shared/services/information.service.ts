import { Injectable } from '@angular/core';
import { ApplicationService } from './application.service';

@Injectable()
export class InformationService {
	information: any;

	constructor(private applicationService: ApplicationService) { }

	getFromServer() {
		this.applicationService.get('Informacao')
			.subscribe(result => {
				this.information = result;
			});
	}

	get() {
		return this.information;
	}
}
