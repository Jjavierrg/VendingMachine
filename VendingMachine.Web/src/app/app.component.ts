import { Component, OnInit } from '@angular/core';
import { ApiClient } from './core/api/api.client';
import { SignalRService } from './core/services/signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  public credit: number = 0;

  constructor(
    private apiClient: ApiClient,
    public signalRService: SignalRService
  ) {
    this.apiClient
      .creditGET()
      .subscribe((credit) => (this.credit = credit?.credit ?? 0));
  }

  public ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addDisplayDataListener();
  }
}
