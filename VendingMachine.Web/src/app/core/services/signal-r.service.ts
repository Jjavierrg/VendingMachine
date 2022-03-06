import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection!: HubConnection;

  public startConnection = () => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiEndpoint}/display`)
      .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  public addDisplayDataListener = () => {
    this.hubConnection.on('display', (data) => {
      console.log(data);
    });
  };
}
