import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ApiClient, apiEndpoint } from './core/api/api.client';
import { VendingMachineModule } from './vending-machine/vending-machine.module';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule, VendingMachineModule],
  providers: [
    ApiClient,
    {
      provide: apiEndpoint,
      useValue: environment.apiEndpoint,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
