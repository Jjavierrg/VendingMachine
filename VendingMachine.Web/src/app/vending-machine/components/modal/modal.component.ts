import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
})
export class ModalComponent {
  @Input() showCancelButton: boolean = false;
  @Input() title: string = '';
  @Input() visible: boolean = false;

  @Output() public accept = new EventEmitter<void>();
  @Output() public cancel = new EventEmitter<void>();
  @Output() public visibleChange = new EventEmitter<boolean>();

  public onAccept(): void {
    this.visible = false;
    this.visibleChange.emit(false);
    this.accept.emit();
  }

  public onCancel(): void {
    this.visible = false;
    this.visibleChange.emit(false);
    this.cancel.emit();
  }
}
