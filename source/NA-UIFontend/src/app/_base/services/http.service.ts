import { ErrorModel } from '../models/errorModel';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { HttpModel } from '../models/httpModel';
import { HistoryService } from './history.service';

declare var $: any;

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(
    private http: HttpClient,
    private router: Router,
    private ht: HistoryService
  ) { }

  // getApi
  getApiAsync<T>(url: string, params: any = null) {
    let result: Promise<HttpModel<T>>;
    let lstError: ErrorModel[];

    result = this.http.get(url, {
      headers: this.getHeaders(),
      responseType: 'json',
      params: params
    }).toPromise()
      .then(response => {
        let data = JSON.parse(response.toString());
        if (data.paging) {
          return new HttpModel<T>(true,null,data.data, data.paging);
        }
        return new HttpModel<T>(true,null,data, null);
      })
      .catch(err => {
        try {
          lstError = this.handleError(err);
          return new HttpModel<T>(false,lstError,null,null);
        } catch (error) {
          return new HttpModel<T>(false,[],err,null);
        }
        
      });
    return result;
  }


  // postApi
  postApiAsync<T>(url: string, body: any) {
    let result: Promise<HttpModel<T>>;
    let lstError: ErrorModel[];
    result = this.http.post(url, body, { headers: this.postHeaders() })
      .toPromise()
      .then(response => {
        let data = JSON.parse(response.toString());
        if (data.paging) {
          return new HttpModel<T>(true,null,data.data, data.paging);
        }
        return new HttpModel<T>(true,null,data, null);
      })
      .catch(err => {
        try {
          lstError = this.handleError(err);
          return new HttpModel<T>(false,lstError,null,null);
        } catch (error) {
          return new HttpModel<T>(false,[],err,null);
        }
        
      });
    return result;
  }

  // patch
  patchApiAsync<T>(url: string, body: any) {
    let result: Promise<HttpModel<T>>;
    let lstError: ErrorModel[];
    result = this.http.patch(url, body, { headers: this.postHeaders() })
      .toPromise()
      .then(response => {
        let data = JSON.parse(response.toString());
        if (data.paging) {
          return new HttpModel<T>(true,null,data.data, data.paging);
        }
        return new HttpModel<T>(true,null,data, null);
      })
      .catch(err => {
        try {
          lstError = this.handleError(err);
          return new HttpModel<T>(false,lstError,null,null);
        } catch (error) {
          return new HttpModel<T>(false,[],err,null);
        }
        
      });
    return result;
  }

  putApiAsync<T>(url: string, body: any) {
    let result: Promise<HttpModel<T>>;
    let lstError: ErrorModel[];
    result = this.http.put(url, body, { headers: this.postHeaders() })
      .toPromise()
      .then(response => {
        let data = JSON.parse(response.toString());
        if (data.paging) {
          return new HttpModel<T>(true,null,data.data, data.paging);
        }
        return new HttpModel<T>(true,null,data, null);
      })
      .catch(err => {
        try {
          lstError = this.handleError(err);
          return new HttpModel<T>(false,lstError,null,null);
        } catch (error) {
          return new HttpModel<T>(false,[],err,null);
        }
        
      });
    return result;
  }

  // upload
  uploadApiAsync<T>(url: string, form: any) {
    let result: Promise<HttpModel<T>>;
    let lstError: ErrorModel[];
    result = this.http.post(url, form, { headers: this.uploadHeaders() })
      .toPromise()
      .then(response => {
        let data = JSON.parse(response.toString());
        if (data.paging) {
          return new HttpModel<T>(true,null,data.data, data.paging);
        }
        return new HttpModel<T>(true,null,data, null);
      })
      .catch(err => {
        try {
          lstError = this.handleError(err);
          return new HttpModel<T>(false,lstError,null,null);
        } catch (error) {
          return new HttpModel<T>(false,[],err,null);
        }
        
      });
    return result;
  }


  deleteApiAsync<T>(url: string, params: any, showErrorDialog: boolean = false, pushState: boolean = false) {
    let result: Promise<HttpModel<T>>;
    let lstError: ErrorModel[];
    // Add params to url
    if (params != null) {
      url += '?' + this.jsonToSearch(params);
      if (pushState) {
        this.pushState(params);
      }
    }
    result = this.http.delete(url, { headers: this.getHeaders() })
      .toPromise()
      .then(response => {
        let data = JSON.parse(response.toString());
        if (data.paging) {
          return new HttpModel<T>(true,null,data.data, data.paging);
        }
        return new HttpModel<T>(true,null,data, null);
      })
      .catch(err => {
        try {
          lstError = this.handleError(err);
          return new HttpModel<T>(false,lstError,null,null);
        } catch (error) {
          return new HttpModel<T>(false,[],err,null);
        }
        
      });
    return result;
  }

  // Handle
  private redirectError(error: any) {
    if (error != null) {
      // logic
      try {
        if (error.status == 401) {
          this.router.navigate(['/auth/login']);
        }
        if (error.status == 403) {
          this.router.navigate(['/auth/login']);
        }
      } catch (error) {
        error = [{ key: '', error: 'Error System' }]
      }
    }
  }

  private handleError(error: Response | any) {
    // In a real world app, you might use a remote logging infrastructure
    this.redirectError(error);
    return error;
  }
  // Headers
  private getHeaders() {
    const headers = new HttpHeaders();
    if (localStorage.getItem('Authorization')) {
      headers.append('Authorization', localStorage.getItem('Authorization'));
    }
    if (localStorage.getItem('Language')) {
      headers.append('Language', localStorage.getItem('Language'));
    }
    return headers;
  }

  private postHeaders() {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json; charset=utf-8');
    // headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=utf-8');
    if (localStorage.getItem('Authorization')) {
      headers.append('Authorization', localStorage.getItem('Authorization'));
    }
    if (localStorage.getItem('Language')) {
      headers.append('Language', localStorage.getItem('Language'));
    }
    return headers;
  }

  private uploadHeaders() {
    const headers = new HttpHeaders();
    if (localStorage.getItem('Authorization')) {
      headers.append('Authorization', localStorage.getItem('Authorization'));
    }
    if (localStorage.getItem('Language')) {
      headers.append('Language', localStorage.getItem('Language'));
    }
    return headers;
  }

  private resetHeaders() {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=utf-8');
    if (localStorage.getItem('Language')) {
      headers.append('Language', localStorage.getItem('Language'));
    }
    return headers;
  }

  // Function Extention
  public jsonToSearch(obj: any) {
    const str = Object.keys(obj).map(function (key) {
      let value = obj[key];
      if (value == null) { value = ''; }
      return key + '=' + encodeURI(value);
    }).join('&');
    return str;
  }

  public SearchTojson(search: string) {
    if (search == null || search === '') { return null; }
    search = search.replace('?', '');
    return JSON.parse('{"' + decodeURI(search).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}');
  }

  public pushState(obj: any) {
    const urlCurent = (window.location.pathname + window.location.search);
    const urlTo = window.location.pathname + '?' + this.jsonToSearch(obj);
    if (urlCurent !== urlTo) {
      this.ht.pushState(urlTo);
    }
  }
}
