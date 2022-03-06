import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProductSlotDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-slot',
  templateUrl: './slot.component.html',
  styleUrls: ['./slot.component.css'],
})
export class SlotComponent {
  @Input() public product?: ProductSlotDto;
  @Output() public productSelection = new EventEmitter<ProductSlotDto>();

  public getLogoLetter(product?: ProductSlotDto): string {
    if (!product?.name) return '';

    var firstLetters = product.name
      .split(' ')
      .filter((x) => x)
      .map((x) => x[0]);

    return firstLetters.join('');
  }

  public onProductSelection(product: ProductSlotDto) {
    if (!product?.quantity) {
      return;
    }

    this.productSelection.emit(product);
  }
}
