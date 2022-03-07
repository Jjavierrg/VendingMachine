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
  public products: ProductSlotDto[] = [];
  public insertCoins: CoinWithQuantityDto[] = [];
  public returnedCoins: CoinWithQuantityDto[] = [];
  public displayData: DisplayData = { statusText: '', creditInfo: 0 };

  public insertCoinsModalVisible: boolean = false;
  public isReturnCoinsModalVisible: boolean = false;

  constructor(
    public signalRService: SignalRService,
    public vendingService: VendingService
  ) {
    this.loadInitialData();
  }

  public ngOnInit(): void {
    this.subscribeToSignalR();
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

  public onProductSelection(slot: ProductSlotDto): void {
    this.vendingService.selectProduct(slot).subscribe((x) => {
      const product = this.products.find((p) => p.id === x.product?.id);
      if (!!product) {
        product.quantity = x.product?.quantity;
      }
    });
  }

  private loadInitialData(): void {
    this.vendingService.getProducts().subscribe((x) => (this.products = x));
    this.vendingService
      .getUserCredit()
      .subscribe((x) => (this.displayData.creditInfo = x));

    this.vendingService
      .getAvailableCoins()
      .subscribe((x) => (this.insertCoins = x));
  }

  private subscribeToSignalR(): void {
    this.signalRService.startConnection();
    this.signalRService.addDisplayDataListener();
    this.signalRService.addReturnDataListener();

    this.subscribeToDisplayData();
    this.subscribeToReturnData();
  }

  private subscribeToDisplayData(): void {
    this.signalRService.getDisplayObservable().subscribe((x) => {
      if (x.creditInfo === null || x.creditInfo === undefined) {
        x.creditInfo = this.displayData?.creditInfo;
      }

      if (x.statusText === null || x.statusText === undefined) {
        x.statusText = this.displayData?.statusText;
      }

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
