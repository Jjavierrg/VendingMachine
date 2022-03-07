import { Component, Input } from '@angular/core';
import { CoinWithQuantityDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-quantity-coin',
  templateUrl: './quantity-coin.component.html',
  styleUrls: ['./quantity-coin.component.css'],
})
export class QuantityCoinComponent {
  @Input() public coin?: CoinWithQuantityDto;
  @Input() public readonly: boolean = false;

  constructor() {}

  public getCoinDescription(coin?: CoinWithQuantityDto): string {
    if (!coin?.coinValue) return '';

    var isEuro = coin.coinValue >= 100;
    return `${isEuro ? coin.coinValue / 100 : coin.coinValue} ${
      isEuro ? 'Eur' : 'Cts'
    }`;
  }

  public onCoinClick(): void {
    if (!this.coin || this.readonly) {
      return;
    }

    this.coin.quantity = (this.coin.quantity ?? 0) + 1;
  }
}
