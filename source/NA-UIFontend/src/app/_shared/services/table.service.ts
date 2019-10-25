import { AppConfigService } from './../../_base/services/app-config.service';
import { Injectable } from '@angular/core';
import { HttpService } from '../../_base/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class TableService {  
  private url = `${
    AppConfigService.settings.apiServer.physical
  }/api`;
  constructor(private http: HttpService) {}

  public Setting(tag: string) {
    let url = `assets/settings/${tag}.json`;
    return this.http.getApiAsync<any>(url);
  }

  public getData(params: any = null, pushState: boolean = false) {
    let url = `${this.url}`;
    if (params.where != null && typeof params.where !== 'string') {
      params.where = JSON.stringify(params.where);
    } 
    return this.http.getApiAsync<any[]>(url, params);
  }

  public initAdd(params: any = null) {
    let url = `${this.url}/PostForm`;
    return this.http.getApiAsync<any>(url, params);
  }

  public Add(params: any = null) {
    let url = `${this.url}`;
    return this.http.postApiAsync<any>(url, params);
  }

  public initEdit(params: any) {
    let url = `${this.url}/PutForm`;
    return this.http.getApiAsync<any>(url, params);
  }

  public Edit(id: number, params: any = null) {
    let url = `${this.url}?id=${id}`;
    return this.http.putApiAsync<any>(url, params);
  }

  public Patch(id: number, params: any = null) {
    let url = `${this.url}?id=${id}`;
    return this.http.patchApiAsync<any>(url, params);
  }

  public FindOne(params: any) {
    let url = `${this.url}/FindOne`;
    return this.http.getApiAsync<any>(url, params);
  }

  public Delete(params: any) {
    let url = `${this.url}/Delete`;
    return this.http.deleteApiAsync<any>(url, params);
  }

  public UploadFile(data: any) {
    const url = `${this.url}/upload`;
    return this.http.uploadApiAsync<any[]>(url, data);
  }
}
