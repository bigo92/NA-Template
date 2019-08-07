import { Pipe } from '@angular/core';
import { DatePipe } from '@angular/common';
import { SharedCacheService } from 'src/app/_shared/caches/shared-cache.service';

@Pipe({ name: 'dateFormat' })

export class DateFormat extends DatePipe {
    constructor(private _sharedCache: SharedCacheService) {
        super('pl_PL');
    }

    transform(value: any, option = 'date'): string {
        const data = this._sharedCache.currencyConfig;
        const format = option === 'date' ? 'dd/MM/yyyy' : 'dd/MM/yyyy HH:mm';
        const timezone = data.timezone.split(' ')[0];
        return super.transform(value, format, timezone);
    }
}