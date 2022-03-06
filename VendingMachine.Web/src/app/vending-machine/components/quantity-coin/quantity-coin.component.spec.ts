import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuantityCoinComponent } from './quantity-coin.component';

describe('QuantityCoinComponent', () => {
  let component: QuantityCoinComponent;
  let fixture: ComponentFixture<QuantityCoinComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [QuantityCoinComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuantityCoinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
