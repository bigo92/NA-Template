import { Pipe, PipeTransform } from '@angular/core';
import * as writtenNumber from 'written-number';
import { LanguageService } from '../../services/language.service';
@Pipe({ name: 'numToWords' })

export class NumToWords implements PipeTransform {
    private lstLanguage = ['en', 'es', 'ar', 'pt', 'fr', 'eo', 'it', 'vi', 'tr', 'uk', 'ru', 'id', 'pl'];
    private lstAnd = [
        { id: 'en', value: 'and' },
        { id: 'vi', value: 'và' },
        { id: 'fr', value: 'et' },
        { id: 'es', value: 'y' },
        { id: 'pt', value: 'e' },
        { id: 'tr', value: 've' },
        { id: 'uk', value: 'і' },
        { id: 'id', value: 'dan' },
        { id: 'ar', value: 'و' },
    ]
    constructor(private languageService: LanguageService) { }
    transform(value: string): string {
        value = value.replace(new RegExp(' ', 'g'), '');
        value = value.replace(',', '.');
        const valueFloat = parseFloat(value);
        let languege = this.languageService.getLanguage();
        if (!this.lstLanguage.includes(languege)) languege = 'en';
        if (languege === 'pl') {
            return this.slownie(valueFloat)
        }
        const option = { lang: languege };
        const values = value.split('.');
        let result = '';
        if (values.length === 1) {
            result = writtenNumber(values[0], option)
        } else {
            const and = this.lstAnd.find(x => x.id === languege);
            result = `${writtenNumber(values[0], option)} złoty ${and ? and.value : this.lstAnd[0].value} ${writtenNumber(values[1], option)} groszy`
        }
        return result;
    }

    slowa(liczba: any, koncowka: any) {

        var jednosci = ['', ' jeden', ' dwa', ' trzy', ' cztery', ' pięć', ' sześć', ' siedem', ' osiem', ' dziewięć'];
        var nascie = ['', ' jedenaście', ' dwanaście', ' trzynaście', ' czternaście', ' piętnaście', ' szesnaście', ' siedemnaście', ' osiemnaście', ' dziewietnaście'];
        var dziesiatki = ['', ' dziesięć', ' dwadzieścia', ' trzydzieści', ' czterdzieści', ' pięćdziesiąt', ' sześćdziesiąt', ' siedemdziesiąt', ' osiemdziesiąt', ' dziewięćdziesiąt'];
        var setki = ['', ' sto', ' dwieście', ' trzysta', ' czterysta', ' pięćset', ' sześćset', ' siedemset', ' osiemset', ' dziewięćset'];
        var grupy = [
            ['', '', ''],
            [' tysiąc', ' tysiące', ' tysięcy'],
            [' milion', ' miliony', ' milionów'],
            [' miliard', ' miliardy', ' miliardów'],
            [' bilion', ' biliony', ' bilionów'],
            [' biliard', ' biliardy', ' biliardów'],
            [' trylion', ' tryliony', ' trylionów']];

        if (!isNaN(liczba)) {

            var wynik = '';
            var znak = '';
            if (liczba == 0)
                wynik = 'zero';
            if (liczba < 0) {
                znak = 'minus';
                liczba = -liczba;
            }

            var g = 0;
            while (liczba > 0) {
                var s = Math.floor((liczba % 1000) / 100);
                var n = 0;
                var d = Math.floor((liczba % 100) / 10);
                var j = Math.floor(liczba % 10);
                if (d == 1 && j > 0) {
                    n = j;
                    d = 0;
                    j = 0;
                }

                var k = 2;
                if (j == 1 && s + d + n == 0)
                    k = 0;
                if (j == 2 || j == 3 || j == 4)
                    k = 1;
                if (s + d + n + j > 0) {
                    if (g == 0)
                        wynik = wynik + koncowka[k]
                    wynik = setki[s] + dziesiatki[d] + nascie[n] + jednosci[j] + grupy[g][k] + wynik;
                }

                g++;
                liczba = Math.floor(liczba / 1000);
            }

            return (znak + wynik).trim();
        } else {
            return ('Podano nieprawidłową wartość!');
        }
    }

    slownie(kwota: any) {

        var zlotowki = [' złoty', ' złote', ' złotych'];
        var grosze = [' grosz', ' grosze', ' groszy'];
        var wynik = '';

        var zloty = Math.floor(kwota);
        var grosz = Math.round((kwota - zloty) * 100);

        if (!isNaN(kwota)) {
            wynik = this.slowa(zloty, zlotowki);
            if (grosz != 0)
                wynik = wynik + ' i ' + this.slowa(grosz, grosze);

            return wynik.trim();

        } else {
            return ('Podano nieprawidłową wartość!');
        }

    }

}