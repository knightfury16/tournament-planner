import { Component, Input, input } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { TournamentDto } from '../tp-model/TpModel';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tournament-details-homepage',
  standalone: true,
  imports: [MatTabsModule, CommonModule],
  templateUrl: './tournament-details-homepage.component.html',
  styleUrl: './tournament-details-homepage.component.scss'
})
export class TournamentDetailsHomepageComponent {
  @Input() public tournamentId?: string;
}
