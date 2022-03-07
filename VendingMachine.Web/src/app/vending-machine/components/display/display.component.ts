import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-display',
  templateUrl: './display.component.html',
  styleUrls: ['./display.component.css'],
})
export class DisplayComponent {
  @Input() public status: string = '';
  @Input() public credit: number = 0;

  constructor() {}
}
