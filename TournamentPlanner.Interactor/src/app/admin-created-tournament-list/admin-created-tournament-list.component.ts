import { Component } from '@angular/core';
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

@Component({
  selector: 'app-admin-created-tournament-list',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatListModule, TournamentCardComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './admin-created-tournament-list.component.html',
  styleUrl: './admin-created-tournament-list.component.scss'
})
export class AdminCreatedTournamentListComponent {

  nameInput = new FormControl();
  public tournaments: TournamentDto[] = [];

  constructor(private tp: TournamentPlannerService) {
  }





  public data$ = this.nameInput.valueChanges.pipe(
    startWith(''),
    debounceTime(500),
    distinctUntilChanged(),
    switchMap((name) => this.tp.getTournament(name)),
    map((tournaments) =>
      tournaments.sort(
        (a, b) =>
          new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
      )
    ),
    map((tournaments) =>
      tournaments.filter((tor) => {
        if (tor.endDate) {
          return new Date(tor.endDate) >= new Date();
        }
        return true;
      })
    )
  );
}
