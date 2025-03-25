import { Component, inject, OnInit, signal } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { TournamentCardComponent } from '../tournament-card/tournament-card.component';
import { CommonModule } from '@angular/common';
import { TournamentDto, TournamentSearchCategory, TournamentStatus } from '../tp-model/TpModel';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AdminTournamentService } from '../../Shared/admin-tournament.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { TournamentSearchComponent, TournamentSearchCriteria } from '../tournament-search/tournament-search.component';
import { LoadingService } from '../../Shared/loading.service';

@Component({
  selector: 'app-admin-created-tournament-list',
  standalone: true,
  imports: [MatFormFieldModule, MatCardModule, MatListModule, MatButtonModule, RouterModule,
    TournamentCardComponent, CommonModule, ReactiveFormsModule, TournamentSearchComponent],
  templateUrl: './admin-created-tournament-list.component.html',
  styleUrl: './admin-created-tournament-list.component.scss'
})
export class AdminCreatedTournamentListComponent implements OnInit {

  nameInput = new FormControl();
  private _adminTPService = inject(AdminTournamentService);
  public readonly tournamentSearchCategory = TournamentSearchCategory;
  public readonly tournamentStatus = TournamentStatus;
  public tournaments = signal<TournamentDto[] | undefined>(undefined);
  public loadingService = inject(LoadingService);
  public tp = inject(AdminTournamentService);

  async ngOnInit() {
    try {
      var response = await this._adminTPService.getAdminTournaments();
      this.tournaments.set(
        response.sort((a, b) => new Date(b.createdAt!).getTime() - new Date(a.createdAt!).getTime()));
    } catch (error) {
      console.log(error)
    }
  }


  async onSearch(criteria: TournamentSearchCriteria) {
    this.loadingService.show();

    try {
      var tournaments = await this.tp.getAdminTournaments(
        criteria.name,
        criteria.searchCategory,
        criteria.status,
        criteria.gameType,
        criteria.startDate,
        criteria.endDate
      );
      this.setTournaments(tournaments);
    } catch (error) {
      console.error('Error fetching tournaments:', error);
    } finally {
      this.loadingService.hide();
    }

  }

  async loadInitialTournaments() {
    this.loadingService.show();
    try {
      var tournaments = await this.tp.getAdminTournaments();
      this.setTournaments(tournaments);
    }
    catch (error: any) {
      console.error('Error fetching tournaments:', error);

    } finally {
      this.loadingService.hide();
    }
  }

  public setTournaments(tournaments: TournamentDto[]) {
    this.tournaments.set(tournaments.sort((a, b) => new Date(b.createdAt!).getTime() - new Date(a.createdAt!).getTime()));
  }

  public onClear() {
    this.loadInitialTournaments();
  }
}
