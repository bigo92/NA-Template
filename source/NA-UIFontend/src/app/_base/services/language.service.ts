import { Injectable, EventEmitter } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TableModelService } from 'src/app/_shared/services/table-model.service';

@Injectable()
export class LanguageService {

    private langData: any[] = [];
    private listLanguage: any[] = [];
    private langDefault: string = 'en';
    private onChange = new EventEmitter<any>();
    constructor(
        private translate: TranslateService,
        private _tableModelService: TableModelService,
    ) { }

    public async start() {
        var lang: string[] = [];
        await this.getLanguages();
        this.langData.forEach(x => {
            lang.push(x.value);
        });
        this.translate.addLangs(lang);
        this.translate.setDefaultLang(this.langDefault);
        this.translate.use(this.langDefault);

        try {
            if (localStorage.getItem('Language')) {
                this.translate.use(localStorage.getItem('Language'));
                this.langDefault = localStorage.getItem('Language');
            } else {
                localStorage.setItem('Language', this.langDefault);
            }
            // const browserLang = translate.getBrowserLang();
            // translate.use(browserLang.match(/en|vi/) ? browserLang : 'vi');
        } catch (error) {
            localStorage.setItem('Language', this.langDefault);
        }
    }

    private async getLanguages() {
        let rs = await this._tableModelService.FindOne({ tag: 'languages' });
        if (rs.success) {
            let language = rs.data.data.setting.find(x => x.keyName === 'languageId');
            if (language) {
                this.listLanguage = language.dataSource;
                this.listLanguage.forEach(x => {
                    this.langData.push({
                        txt: x.name,
                        value: x.id
                    });
                });
            }
        }
    }

    public getData() {
        return this.langData;
    }

    public getChange() {
        return this.onChange;
    }

    public getLanguage() {
        if (localStorage.getItem('Language')) {
            return localStorage.getItem('Language');
        }
        return this.langDefault;
    }

    public setLanguage(value: string) {
        var index = this.langData.findIndex(x => x.value == value);
        if (index != -1) {
            localStorage.setItem('Language', value);
            this.translate.use(this.langData[index].value);
            this.onChange.emit(this.langData[index])
        }
    }
}
