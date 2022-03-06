import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuantityCoinComponent } from './components/quantity-coin/quantity-coin.component';
import { FormsModule } from '@angular/forms';
import { DisplayComponent } from './components/display/display.component';
import { SlotComponent } from './components/slot/slot.component';

@NgModule({
  declarations: [QuantityCoinComponent, DisplayComponent, SlotComponent],
  imports: [CommonModule, FormsModule],
  exports: [QuantityCoinComponent, DisplayComponent, SlotComponent],
})
export class VendingMachineModule {}
