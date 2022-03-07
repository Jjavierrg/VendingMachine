import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CoinWithQuantityDto, ProductSlotDto } from './core/api/api.client';
import { DisplayData } from './core/models/displaydata.model';
import { SignalRService } from './core/services/signal-r.service';
import { VendingService } from './core/services/vending.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  public products$: Observable<ProductSlotDto[]>;
  public insertCoins: CoinWithQuantityDto[] = [];
  public returnedCoins: CoinWithQuantityDto[] = [];
  public displayData: DisplayData = { StatusText: '', CreditInfo: 0 };

  public insertCoinsModalVisible: boolean = false;
  public isReturnCoinsModalVisible: boolean = false;

  constructor(
    public signalRService: SignalRService,
    public vendingService: VendingService
  ) {
    this.subscribeToDisplayData();
    this.subscribeToReturnData();

    this.products$ = this.vendingService.getProducts();
    this.vendingService
      .getUserCredit()
      .subscribe((x) => (this.displayData.CreditInfo = x));

    this.vendingService
      .getAvailableCoins()
      .subscribe((x) => (this.insertCoins = x));
  }

  public ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addDisplayDataListener();
    this.signalRService.addReturnDataListener();
  }

  public showInsertCoins() {
    this.insertCoins.forEach((x) => (x.quantity = 0));
    this.insertCoinsModalVisible = true;
  }

  public insertCoinsToMachine(): void {
    this.vendingService.insertCoins(this.insertCoins);
  }

  public returnCoins(): void {
    this.vendingService.returnCoins();
  }

  private subscribeToDisplayData(): void {
    this.signalRService.getDisplayObservable().subscribe((x) => {
      this.displayData = x;
    });
  }

  private subscribeToReturnData(): void {
    this.signalRService.getReturnObservable().subscribe((x) => {
      this.returnedCoins = x;
      this.isReturnCoinsModalVisible = true;
    });
  }
}
