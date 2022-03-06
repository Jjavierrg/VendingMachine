import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuantityCoinComponent } from './components/quantity-coin/quantity-coin.component';
import { FormsModule } from '@angular/forms';
import { DisplayComponent } from './components/display/display.component';

@NgModule({
  declarations: [QuantityCoinComponent, DisplayComponent],
  imports: [CommonModule, FormsModule],
  exports: [QuantityCoinComponent, DisplayComponent],
})
export class VendingMachineModule {}
