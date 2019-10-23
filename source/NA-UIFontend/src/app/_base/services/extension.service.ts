import { Injectable } from '@angular/core';
import { AppConfigService } from './app-config.service';

@Injectable({
    providedIn: 'root'
})
export class ExtensionService {

    constructor() { }

    public logDebug(mess?: any, ...params: any[]) {
        if (AppConfigService.settings.deploy === 'debugger') {
            console.debug(mess, params);
        }
    }
}