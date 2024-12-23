import { Component } from '@angular/core';
import { TournamentPlannerService } from '../tournament-planner.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';

import {
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
import {MatListModule} from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { TournamentDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { TournamentCardComponent } from "../tournament-card/tournament-card.component";

@Component({
  selector: 'app-tournament-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule,
    MatInputModule, MatListModule, MatCardModule, MatIconModule, MatButtonModule, MatTabsModule, TournamentCardComponent],
  templateUrl: './tournament-list.component.html',
  styleUrl: './tournament-list.component.scss'
})
export class TournamentListComponent {
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
