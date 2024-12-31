import { Component } from '@angular/core';
import { TournamentPlannerService } from '../tournament-planner.service';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';

import {
  BehaviorSubject,
  debounceTime,
  distinctUntilChanged,
  firstValueFrom,
  map,
  startWith,
  switchMap,
} from 'rxjs';
import { RouterModule } from '@angular/router';
import { MatFormField, MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { TournamentDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { TournamentCardComponent } from "../tournament-card/tournament-card.component";
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-tournament-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule,
    MatInputModule, MatListModule, MatCardModule, MatIconModule, MatButtonModule,MatNativeDateModule, 
    MatTabsModule, TournamentCardComponent, MatDatepickerModule, MatSelectModule, MatProgressSpinner, MatExpansionModule],
  templateUrl: './tournament-list.component.html',
  styleUrl: './tournament-list.component.scss'
})
export class TournamentListComponent {
  searchForm: FormGroup;
  tournaments$ = new BehaviorSubject<TournamentDto[]>([]);
  loading = false;

  constructor(
    private fb: FormBuilder,
    private tp: TournamentPlannerService
  ) {
    this.searchForm = this.fb.group({
      name: [''],
      searchCategory: [''],
      status: [''],
      gameType: [''],
      startDate: [null],
      endDate: [null]
    });
  }

  ngOnInit() {
    this.loadInitialTournaments();
  }

  loadInitialTournaments() {
    this.loading = true;
    this.tp.getTournament().subscribe({
      next: (tournaments) => {
        this.tournaments$.next(this.processTournaments(tournaments));
      },
      error: (error) => {
        console.error('Error fetching tournaments:', error);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  private processTournaments(tournaments: TournamentDto[]): TournamentDto[] {
    return tournaments
      .sort((a, b) => new Date(b.createdAt!).getTime() - new Date(a.createdAt!).getTime())
      .filter((tor) => {
        if (tor.endDate) {
          return new Date(tor.endDate) >= new Date();
        }
        return true;
      });
  }

  onSearch() {
    this.loading = true;
    const formValues = this.searchForm.value;
    
    const startDate = formValues.startDate ? new Date(formValues.startDate).toISOString() : '';
    const endDate = formValues.endDate ? new Date(formValues.endDate).toISOString() : '';

    this.tp.getTournament(
      formValues.name,
      formValues.searchCategory,
      formValues.status,
      formValues.gameType,
      startDate,
      endDate
    ).subscribe({
      next: (tournaments) => {
        this.tournaments$.next(this.processTournaments(tournaments));
      },
      error: (error) => {
        console.error('Error fetching tournaments:', error);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  clearSearch() {
    this.searchForm.reset();
    this.loadInitialTournaments();
  }
}
