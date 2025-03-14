import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatNativeDateModule } from '@angular/material/core';
import { GameTypeSupported, TournamentSearchCategory, TournamentStatus } from '../tp-model/TpModel';
import { trimAllSpace } from '../../Shared/Utility/stringUtility';

export interface TournamentSearchCriteria {
  name: string;
  searchCategory: string;
  status: string;
  gameType: string;
  startDate: string;
  endDate: string;
}

@Component({
  selector: 'app-tournament-search',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatSelectModule,
    MatExpansionModule,
    MatNativeDateModule
  ],
  templateUrl: './tournament-search.component.html',
  styleUrl: './tournament-search.component.scss'
})
export class TournamentSearchComponent {
  @Input() initialSearchCategory: string = TournamentSearchCategory.All;
  @Input() statusOptions: string[] = [];
  @Output() search = new EventEmitter<TournamentSearchCriteria>();
  @Output() clear = new EventEmitter<void>();

  public readonly tournamentSearchCategory = TournamentSearchCategory;
  public readonly tournamentStatus = TournamentStatus;
  public readonly gameTypeSupported = GameTypeSupported;

  searchForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.searchForm = this.fb.group({
      name: [''],
      searchCategory: [this.initialSearchCategory],
      status: [''],
      gameType: [''],
      startDate: [null],
      endDate: [null]
    });
  }

  ngOnInit() {
    // Initialize form with input values if needed
    this.searchForm.get('searchCategory')?.setValue(this.initialSearchCategory);
  }

  onSearch() {
    const formValues = this.searchForm.value;
    var status = formValues.status != null ? trimAllSpace(formValues.status) : '';

    const searchCriteria: TournamentSearchCriteria = {
      name: formValues.name || '',
      searchCategory: formValues.searchCategory || TournamentSearchCategory.All,
      status: status,
      gameType: formValues.gameType || '',
      startDate: formValues.startDate ? formValues.startDate.toISOString() : '',
      endDate: formValues.endDate ? formValues.endDate.toISOString() : ''
    };

    this.search.emit(searchCriteria);
  }

  clearSearch() {
    this.searchForm.reset();
    this.searchForm.get('searchCategory')?.setValue(this.initialSearchCategory);
    this.clear.emit();
  }

}
