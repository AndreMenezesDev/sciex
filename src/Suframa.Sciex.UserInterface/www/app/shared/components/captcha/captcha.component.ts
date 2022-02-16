import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { ReCaptchaComponent } from 'angular2-recaptcha';
import { InformationService } from '../../services/information.service';

@Component({
	selector: 'app-captcha',
	templateUrl: './captcha.component.html',
})
export class CaptchaComponent {
	public sitekey: string;
	private bypass = false;

	@Input() token: any;
	@Output() tokenChange = new EventEmitter<any>();
	@ViewChild(ReCaptchaComponent) captcha: ReCaptchaComponent;

	constructor(private informationService: InformationService) {
		const informacao = this.informationService.get();

		this.sitekey = informacao.appSettings.captchA_SITE_KEY;
		this.bypass = informacao.appSettings.bypasS_CAPTCHA;
	}

	public captchaResponse($event) {
		this.token = $event;
		this.tokenChange.emit($event);
	}

	public reset() {
		this.captcha.reset();
	}

	public isValidToken() {
		if (this.bypass) {
			return true;
		}

		return (this.token) ? true : false;
	}
}
