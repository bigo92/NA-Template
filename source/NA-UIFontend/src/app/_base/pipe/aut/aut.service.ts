import { AppConfigService } from './../../services/app-config.service';
import { Injectable } from '@angular/core';
import { HttpService } from '../../services/http.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AutService {
  private url = `${AppConfigService.settings.apiServer.physical}/api/Account`;
  private data = new BehaviorSubject<any>(null);
  constructor(private http: HttpService) {}

  public getData() {
    return this.data.asObservable();
  }

  public getClaims(): Promise<any> {
    return new Promise(result => {
      let sub = this.data.asObservable().subscribe(x => {
        result(x);
        //sub.unsubscribe();
      });
    });
  }

  public destroyData() {
    this.data.next(null);
  }

  public async initData() {
    let url = `${this.url}/GetClaims`;
    let rs = await this.http.getApiAsync<any>(url, null);
    if (rs.success) {
      this.data.next(rs.data);
    } else {
      this.data.next(null);
    }
  }

  public CheckRole(role: string) {
    let result = this.data.value.lstRole.find(x => x.normalizedName === role);
    return result ? true : false;
  }

  public checkClaim(lstClaim: any[]) {
    let isHide = false;
    let lst = this.data.value.lstClaim;
    for (let index = 0; index < lst.length; index++) {
      const element = lst[index];
      if (lstClaim.includes(element)) {
        isHide = true;
        return isHide;
      }
      isHide = false;
    }
    return isHide;
  }
}
