import { Component, Input, input } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { TournamentDto } from '../tp-model/TpModel';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TournamentPlayerListComponent } from "../tournament-player-list/tournament-player-list.component";

@Component({
  selector: 'app-tournament-details-homepage',
  standalone: true,
  imports: [MatTabsModule, CommonModule, MatCardModule, TournamentPlayerListComponent],
  templateUrl: './tournament-details-homepage.component.html',
  styleUrl: './tournament-details-homepage.component.scss'
})
export class TournamentDetailsHomepageComponent {
  @Input() public tournamentId?: string;
}
