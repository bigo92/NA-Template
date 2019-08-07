import { Injectable } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationError } from '@angular/router';
declare var $;
@Injectable({
  providedIn: 'root'
})
export class LoadingScreenService {

  private sub: any;
  private minTime: number;
  private startTime: Date;
  private status: boolean = false;
  private guildId: string;
  public maxTime: number;
  public count: number;
  constructor(
    private router: Router
  ) { }

  start(time: any = 100, max: any = 1000) {
    this.minTime = time;
    this.maxTime = max;
    this.sub = this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.openLoading();
      }
    });
  }

  end() {
    this.sub.unsubscribe();
  }

  awaitComponent(count: number = 1) {
    this.count = count;
  }

  loadDone() {
    this.count--;
    if (this.count > 0) { // block await component load done
      return;
    }
    setTimeout(() => {
      let dateNow = new Date();
      let spanTime = 0.3;//this.ex.getDiffDate(dateNow, this.startTime).time;
      if (spanTime > this.minTime) {
        $('#loading-page').fadeOut();
      } else {
        $('#loading-page').delay(this.minTime - spanTime).fadeOut();
      }
    }, 50);
  }

  openLoading() {
    this.startTime = new Date();
    $('#loading-page').show();
    this.status = true;
    this.guildId = this.startTime.toISOString();
    this.closeloading(this.guildId);
    this.count = 1;
  }

  private closeloading(id) {
    let guiId = id;
    setTimeout(() => {
      if (guiId === this.guildId && this.status === true) {
        $('#loading-page').fadeOut();
      }
    }, this.maxTime);
  }
}
