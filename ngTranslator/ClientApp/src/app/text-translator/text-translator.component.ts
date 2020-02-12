import { Component, OnInit } from '@angular/core';
import { TranslatorService } from '../services/translator.service';
import { AvailableLanguage } from '../models/availablelanguage';
import { TranslationResult } from '../models/translationresult';

@Component({
  selector: 'app-text-translator',
  templateUrl: './text-translator.component.html',
  styleUrls: ['./text-translator.component.css']
})
export class TextTranslatorComponent implements OnInit {

  loading = false;
  availbleLanguage: AvailableLanguage[];
  outputLanguage: string;
  inputText: string;
  translationResult: TranslationResult;

  constructor(private translatorService: TranslatorService) { }

  ngOnInit() {
    this.translatorService.getAvailableLanguage().subscribe(
      (result: AvailableLanguage[]) => this.availbleLanguage = result
    );
  }

  GetTranslation() {
    if (this.outputLanguage != null) {
      this.loading = true;
      this.translatorService.getTransaltedText(this.inputText, this.outputLanguage).subscribe
        ((result: TranslationResult) => {
          this.translationResult = result;
          this.loading = false;
        });
    }
  }

  setTargetlanguage(selectedItem) {
    this.outputLanguage = selectedItem.target.value;
  }
}
