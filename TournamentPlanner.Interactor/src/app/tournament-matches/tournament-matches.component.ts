import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { TournamentPlannerService } from '../tournament-planner.service';
import { CommonModule } from '@angular/common';
import { map, Observable } from 'rxjs';
import { Match } from '../tp-model/TpModel';

@Component({
  selector: 'app-tournament-matches',
  standalone: true,
  imports: [MatTableModule, CommonModule],
  templateUrl: './tournament-matches.component.html',
  styleUrl: './tournament-matches.component.scss',
})
export class TournamentMatchesComponent implements OnInit {
  @Input() public tournamentId?: number;

  displayedColumns: string[] = [
    'firstPlayerName',
    'secondPlayerName',
    'winner',
    'gameScheduled',
  ];
  dataSource = new MatTableDataSource<Match>();

  constructor(private tp: TournamentPlannerService) {}
  ngOnInit(): void {
    console.log('From matches component', this.tournamentId);
    // asserting not null here
    this.tp
      .getMatches(this.tournamentId!)
      .pipe(
        map((matches) => matches ?? []) // Ensure it always returns an array
      )
      .subscribe((matches) => {
        this.dataSource.data = matches;
      });
  }
}
