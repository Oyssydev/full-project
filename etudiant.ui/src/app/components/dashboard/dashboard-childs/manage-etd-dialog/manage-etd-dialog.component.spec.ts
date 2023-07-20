import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageEtdDialogComponent } from './manage-etd-dialog.component';

describe('ManageEtdDialogComponent', () => {
  let component: ManageEtdDialogComponent;
  let fixture: ComponentFixture<ManageEtdDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ManageEtdDialogComponent]
    });
    fixture = TestBed.createComponent(ManageEtdDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
