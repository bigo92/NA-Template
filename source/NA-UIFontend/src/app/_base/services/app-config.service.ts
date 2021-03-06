import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IAppConfig } from '../models/IAppConfig';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  static settings: IAppConfig;

  constructor(private http: HttpClient) {}

  load() {
    const jsonFile = `assets/appsetting.json`;
    return new Promise<void>((resolve, reject) => {
      this.http
        .get(jsonFile)
        .toPromise()
        .then((response: IAppConfig) => {
          AppConfigService.settings = <IAppConfig>response;

          console.log('Config Loaded');
          console.log(AppConfigService.settings);
          resolve();
        })
        .catch((response: any) => {
          reject(`Could not load the config file`);
        });
    });
  }
}


export function loadConfigurations(configService: AppConfigService) {
  return () => configService.load();
}