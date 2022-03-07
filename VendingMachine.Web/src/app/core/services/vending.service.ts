import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ApiClient,
  CoinWithQuantityDto,
  ProductSlotDto,
  SaleDto,
  SlotOrderDto,
} from '../api/api.client';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class VendingService {
  constructor(private apiClient: ApiClient) {}

  public getProducts(): Observable<ProductSlotDto[]> {
    return this.apiClient.productsAll();
  }

  public getUserCredit(): Observable<number> {
    return this.apiClient
      .creditGET()
      .pipe(map((credit) => credit?.credit ?? 0));
  }

  public getAvailableCoins(): Observable<CoinWithQuantityDto[]> {
    return this.apiClient.coins();
  }

  public insertCoins(coins?: CoinWithQuantityDto[]): void {
    this.apiClient.creditPOST(coins).subscribe();
  }

  public returnCoins(): void {
    this.apiClient.return().subscribe();
  }

  public selectProduct(product: ProductSlotDto): Observable<SaleDto> {
    const order = new SlotOrderDto({ quantity: 1, slotNumber: product.id });
    return this.apiClient.order(order);
  }
}
