import { Component, inject, OnInit, signal } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { TournamentCardComponent } from '../tournament-card/tournament-card.component';
import { CommonModule } from '@angular/common';
import { map } from 'rxjs';
import { TournamentDto } from '../tp-model/TpModel';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { switchMap } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { TournamentPlannerService } from '../tournament-planner.service';
import { startWith } from 'rxjs';
import { MatInputModule } from '@angular/material/input';
import { AdminTournamentService } from '../../Shared/admin-tournament.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-created-tournament-list',
  standalone: true,
  imports: [MatFormFieldModule, MatCardModule, MatListModule, MatButtonModule, RouterModule,
    TournamentCardComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './admin-created-tournament-list.component.html',
  styleUrl: './admin-created-tournament-list.component.scss'
})
export class AdminCreatedTournamentListComponent implements OnInit {

  nameInput = new FormControl();
  private _adminTPService = inject(AdminTournamentService);
  public tournaments = signal<TournamentDto[] | undefined>(undefined);

  async ngOnInit() {
    try {
      var response = await this._adminTPService.getAdminTournaments();
      this.tournaments.set(response);
    } catch (error) {
      console.log(error)
    }
  }
}
