import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CoinWithQuantityDto } from '../api/api.client';
import { DisplayData } from '../models/displaydata.model';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection!: HubConnection;
  private displaySubject = new Subject<DisplayData>();
  private returnSubject = new Subject<CoinWithQuantityDto[]>();

  public startConnection = () => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiEndpoint}/api/hub`)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  public getDisplayObservable(): Observable<DisplayData> {
    return this.displaySubject.asObservable();
  }

  public getReturnObservable(): Observable<CoinWithQuantityDto[]> {
    return this.returnSubject.asObservable();
  }

  public addDisplayDataListener = () => {
    this.hubConnection.on('display', (data) => {
      const displayData = JSON.parse(data) as DisplayData;
      this.displaySubject.next(displayData);
    });
  };

  public addReturnDataListener = () => {
    this.hubConnection.on('returned', (data) => {
      const money = (JSON.parse(data) as any[]) ?? [];
      const returnCoins = money.map(
        (x) =>
          new CoinWithQuantityDto({
            coinValue: +x['CoinValue'],
            quantity: +x['Quantity'],
          })
      );
      this.returnSubject.next(returnCoins);
    });
  };
}
