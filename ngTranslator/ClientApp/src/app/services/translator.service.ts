import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TranslatorService {

  apiUrl = '';

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.apiUrl = baseUrl + 'api/Translation';
  }

  getAvailableLanguage() {
    return this.http.get(this.apiUrl)
      .pipe(map(response => {
        return response;
      }));
  }

  getTransaltedText(textToTranslate: string, targetLanguage: string) {
    return this.http.post(this.apiUrl + `/${textToTranslate}/${targetLanguage}`, {})
      .pipe(map(response => {
        return response;
      }));
  }
}
