import { Component } from '@angular/core';
import { TournamentPlannerService } from '../tournament-planner.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { debounceTime, distinctUntilChanged, startWith, switchMap } from 'rxjs';

@Component({
  selector: 'app-tournament-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
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
    switchMap(name => this.tp.getTournament(name))
  );



}
