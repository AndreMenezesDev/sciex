import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConverterService } from './converter.service';
import { environment } from '../../../environments/environment';
import { Http, Headers } from '@angular/http';

@Injectable()
export class ApplicationService {
	token: string;
	authenticated: boolean;

	constructor(
		private converterService: ConverterService,
		private http: HttpClient,
	) { }

	createAuthorizationHeader(): HttpHeaders {
		if (sessionStorage.getItem('token') && !this.token) {
			this.token = sessionStorage.getItem('token');
		}

		if (this.token) {
			const headers = new HttpHeaders({'Authorization':'Bearer ' + this.token});
			sessionStorage.setItem('token', this.token);
			return headers;
		} else {
			return new HttpHeaders();
		}
    }

    withCredentials() {
        return !environment.developmentMode;
    }

	removeToken() {
		this.token = null;
		sessionStorage.removeItem('token');
		//localStorage.removeItem('token');
	}

	setToken(token: string) {
		this.token = token;
		sessionStorage.setItem('token', this.token);
		//localStorage.setItem('token', this.token);
	}

	get<T>(service: string, data?: any) {
        let url = environment.serviceUrl + service;

        if (data) {        
			if (data !== Object(data)) {
				url += '/' + data;
			} else {
				url += '?' + this.converterService.objectToQueryString(data);
            }
            
		}

		return this.http.get<T>(url, {
            withCredentials: this.withCredentials(),
			headers: this.createAuthorizationHeader()
		});
	}

	put<T>(service: string, data?: any) {
		const url = environment.serviceUrl + service;

		return this.http.put<T>(url, data, {
            withCredentials: this.withCredentials(),
			headers: this.createAuthorizationHeader()
		});
	}

	post<T>(service: string, data?: any) {
		const url = environment.serviceUrl + service;

		return this.http.post<T>(url, data, {
		withCredentials: this.withCredentials(),
			headers: this.createAuthorizationHeader()
		});
	}
	
	logout<T>() {
		let url = environment.psslogout;

		return this.http.get<T>(url, {
            withCredentials: this.withCredentials(),
			headers: this.createAuthorizationHeader()
		});
	}

	delete(service: string, data?: any) {
		const url = environment.serviceUrl + service + '/' + data;

		return this.http.delete(url, {
            withCredentials: this.withCredentials(),
			headers: this.createAuthorizationHeader()
		});
	}

	deletar<T>(service: string, data?: any) {
		const url = environment.serviceUrl + service + '/' + data;

		return this.http.delete<T>(url, {
            withCredentials: this.withCredentials(),
			headers: this.createAuthorizationHeader()
		});
	}
}
