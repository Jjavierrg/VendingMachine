import { Component, OnInit } from '@angular/core';
import { CoinWithQuantityDto } from './core/api/api.client';
import { SignalRService } from './core/services/signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  public coin = new CoinWithQuantityDto({ coinValue: 100, quantity: 2 });
  public coin2 = new CoinWithQuantityDto({ coinValue: 50, quantity: 2 });
  constructor(public signalRService: SignalRService) {}

  public ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addDisplayDataListener();
  }
}
