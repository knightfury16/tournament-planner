import { Component, Input } from '@angular/core';
import { TournamentDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
  selector: 'app-tournament-matches-list',
  standalone: true,
  imports: [MatCardModule, MatTabsModule],
  templateUrl: './tournament-matches-list.component.html',
  styleUrl: './tournament-matches-list.component.scss'
})
export class TournamentMatchesListComponent {
  @Input({required: true}) public tournamentId?: number;

  public myCounter = ["a","b","c"];

}
