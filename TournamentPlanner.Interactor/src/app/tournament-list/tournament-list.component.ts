import { Component, inject } from '@angular/core';
import { TournamentPlannerService } from '../tournament-planner.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';

import {
  BehaviorSubject,
} from 'rxjs';
import { RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { GameTypeSupported, TournamentDto, TournamentSearchCategory, TournamentStatus } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { TournamentCardComponent } from "../tournament-card/tournament-card.component";
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatNativeDateModule } from '@angular/material/core';
import { trimAllSpace } from '../../Shared/Utility/stringUtility';
import { LoadingService } from '../../Shared/loading.service';
import { TournamentSearchComponent, TournamentSearchCriteria } from '../tournament-search/tournament-search.component';

@Component({
  selector: 'app-tournament-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule,
    MatInputModule, MatListModule, MatCardModule, MatIconModule, MatButtonModule, MatNativeDateModule,
    MatTabsModule, TournamentCardComponent, MatDatepickerModule, MatSelectModule, MatProgressSpinner, MatExpansionModule, TournamentSearchComponent],
  templateUrl: './tournament-list.component.html',
  styleUrl: './tournament-list.component.scss'
})
export class TournamentListComponent {
  public readonly tournamentSearchCategory = TournamentSearchCategory;
  public readonly tournamentStatus = TournamentStatus;
  public readonly gameTypeSupported = GameTypeSupported;
  public loadingService = inject(LoadingService);
  searchForm: FormGroup;
  tournaments$ = new BehaviorSubject<TournamentDto[]>([]);

  constructor(
    private fb: FormBuilder,
    private tp: TournamentPlannerService
  ) {
    this.searchForm = this.fb.group({
      name: [''],
      searchCategory: [this.tournamentSearchCategory.All],
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
    this.loadingService.show();
    this.tp.getTournament().subscribe({
      next: (tournaments) => {
        this.tournaments$.next(this.processTournaments(tournaments));
      },
      error: (error) => {
        console.error('Error fetching tournaments:', error);
      },
      complete: () => {
        this.loadingService.hide();
      }
    });
  }

  private processTournaments(tournaments: TournamentDto[]): TournamentDto[] {
    const isAdvancedSearchActive = this.getIsAdvanceSearchActive();


    return tournaments
      .filter((tournament) => {
        // Always exclude draft tournaments
        if (tournament.status === TournamentStatus.Draft) {
          return false;
        }

        // If advanced search is not active, only show active/upcoming tournaments
        if (!isAdvancedSearchActive) {
          return tournament.endDate ? new Date(tournament.endDate) >= new Date() : true;
        }

        // Additional filtering can be added based on search category if needed
        return true;
      })
      .sort((a, b) => new Date(b.createdAt!).getTime() - new Date(a.createdAt!).getTime());
  }

  getIsAdvanceSearchActive() {
    const isAdvancedSearchActive = this.searchForm.get('searchCategory')!.dirty ||
      this.searchForm.get('status')!.dirty ||
      this.searchForm.get('gameType')!.dirty ||
      this.searchForm.get('startDate')!.dirty ||
      this.searchForm.get('endDate')!.dirty;
    return isAdvancedSearchActive;
  }


  onSearch(criteria: TournamentSearchCriteria) {
    this.loadingService.show();

    this.tp.getTournament(
      criteria.name,
      criteria.searchCategory,
      criteria.status,
      criteria.gameType,
      criteria.startDate,
      criteria.endDate
    ).subscribe({
      next: (tournaments) => {
        this.tournaments$.next(this.processTournaments(tournaments));
      },
      error: (error) => {
        console.error('Error fetching tournaments:', error);
      },
      complete: () => {
        this.loadingService.hide();
      }
    });
  }

  public curatedTournamentStatus(): string[] {
    return [this.tournamentStatus.RegistrationOpen, this.tournamentStatus.Ongoing, this.tournamentStatus.RegistrationClosed, this.tournamentStatus.Completed];
  }

  public onClear() {
    this.loadInitialTournaments();
  }
}
