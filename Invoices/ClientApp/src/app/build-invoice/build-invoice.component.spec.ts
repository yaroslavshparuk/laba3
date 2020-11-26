import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BuildInvoiceComponent } from './build-invoice.component';

describe('BuildInvoiceComponent', () => {
  let component: BuildInvoiceComponent;
  let fixture: ComponentFixture<BuildInvoiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BuildInvoiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BuildInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
