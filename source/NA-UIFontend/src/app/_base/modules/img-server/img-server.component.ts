import { AppConfigService } from './../../services/app-config.service';
import {
  Component,
  OnInit,
  ViewEncapsulation,
  Input,
  ElementRef,
  AfterViewInit,
  OnChanges
} from '@angular/core';

declare var $;
@Component({
  selector: 'img-server',
  templateUrl: './img-server.component.html',
  styleUrls: ['./img-server.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ImgServerComponent implements OnInit, OnChanges, AfterViewInit {
  @Input('src') src: any;
  @Input('alt') alt: any;
  @Input('class') class: any;
  public isLoad: boolean = false;
  public srcloading: any;
  constructor(private el: ElementRef) {}

  ngOnInit() {
    this.startLoading();
  }

  ngOnChanges() {
    if (this.isLoad) {
      this.isLoad = false;
      $(this.el.nativeElement).addClass(this.class);
      this.startLoading();
      this.endLoading();
    }
  }

  ngAfterViewInit() {
    this.endLoading();
  }

  startLoading() {
    this.srcloading = `${
      AppConfigService.settings.apiServer.physical
    }/api/file/dowload?url=${this.src}&w=${10}&h=${10}`;
  }

  endLoading() {
    let outside = this;
    setTimeout(() => {
      let img = outside.el.nativeElement;
      let w = $(img).width();
      let h = $(img).height();

      this.src = `${
        AppConfigService.settings.apiServer.physical
      }/api/file/dowload?url=${outside.src}&w=${w}&h=${h}`;
      $(outside.el.nativeElement).removeClass(outside.class);
      outside.isLoad = true;
    }, 0);
  }
}
