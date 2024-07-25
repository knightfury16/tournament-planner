import { Component } from '@angular/core';
import { TournamentPlannerService } from '../tournament-planner.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  debounceTime,
  distinctUntilChanged,
  map,
  startWith,
  switchMap,
} from 'rxjs';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-tournament-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './tournament-list.component.html',
  styleUrl: './tournament-list.component.scss'
})
export class TournamentListComponent {
  nameInput = new FormControl();

  constructor(private tp: TournamentPlannerService){}


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
