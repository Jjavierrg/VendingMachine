import { Component, OnInit } from '@angular/core';
import { CoinWithQuantityDto, ProductSlotDto } from './core/api/api.client';
import { SignalRService } from './core/services/signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  public coin = new CoinWithQuantityDto({ coinValue: 100, quantity: 2 });
  public coin2 = new CoinWithQuantityDto({ coinValue: 50, quantity: 2 });

  public product1: ProductSlotDto = new ProductSlotDto({
    name: 'Zumo Naranja',
    price: 300,
    quantity: 7,
  });
  public product2: ProductSlotDto = new ProductSlotDto({
    name: 'Zumo',
    price: 3000,
    quantity: 7000,
  });
  public product3: ProductSlotDto = new ProductSlotDto({
    name: 'Naranja',
    price: 3,
  });

  constructor(public signalRService: SignalRService) {}

  public ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addDisplayDataListener();
  }
}
