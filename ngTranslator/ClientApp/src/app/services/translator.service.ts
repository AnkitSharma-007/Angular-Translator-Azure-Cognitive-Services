import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TranslatorService {

  baseURL: string;

  constructor(private http: HttpClient) {
    this.baseURL = '/api/Translation';
  }

  getAvailableLanguage() {
    return this.http.get(this.baseURL)
      .pipe(response => {
        return response;
      });
  }

  getTransaltedText(textToTranslate: string, targetLanguage: string) {
    return this.http.post(this.baseURL + `/${textToTranslate}/${targetLanguage}`, {})
      .pipe(response => {
        return response;
      });
  }
}
