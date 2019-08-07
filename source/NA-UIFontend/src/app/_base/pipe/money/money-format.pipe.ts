import { Pipe } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { SharedCacheService } from 'src/app/_shared/caches/shared-cache.service';

@Pipe({ name: 'money', pure: false })

export class MoneyFormat extends DecimalPipe {
    constructor(private _sharedCache: SharedCacheService) {
        super('pl_PL');
    }
    
    transform(value: any, option = 'currency'): string {        
        const data = this._sharedCache.decimalPlacesConfig;
        const decimal = Math.pow(10, option === 'currency' ? data.currency : data.exchangeRate);
        switch (data.option) {
            case 1:
                value = Math.ceil(value * decimal) / decimal;
                break;
            case 2:
                value = Math.floor(value * decimal) / decimal;
                break;
            default:
                break;
        }
        const format = `0.${data.currency}-${data.currency}`
        return super.transform(value, format);
    }
}